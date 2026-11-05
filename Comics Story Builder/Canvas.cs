using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO; 
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;






namespace Comics_Story_Builder
{

    public partial class Canvas : Form
    {
        Size screen;

        DoubleBufferedPanel panel1 = new DoubleBufferedPanel(); // Το κύριο Panel όπου θα τοποθετούμε τα PictureBox με τους χαρακτήρες, τα bubbles και το background.
                                                                // Είναι custom για να υποστηρίζει double buffering και να μην τρεμοπαίζει όταν ζωγραφίζουμε πάνω του.

        bool click = false; // Flag για να ξέρουμε πότε κάνουμε κλικ για να προσθέσουμε εικόνα και πότε απλά επιλέγουμε
        
        
        Point createpoint;  // Η θέση που θα δημιουργηθεί η νέα εικόνα (υπολογίζεται στο Panel1Click)
        PictureBox imgDraw, imgSelected; // imgDraw είναι το προσωρινό PictureBox που ακολουθεί τον κέρσορα όταν κάνουμε κλικ για να προσθέσουμε μια εικόνα.
                                         // imgSelected είναι το PictureBox που έχει επιλεγεί (κόκκινο πλαίσιο).
        
        
        
        int created = 0;  // Μετρητής για να δίνουμε μοναδικά ονόματα στα dynamically δημιουργούμενα PictureBox (προαιρετικό, αλλά βοηθάει στο debugging)
        bool resize = false; 
      
        int selection; // 1=superheroes, 2=bubbles, 3=backgrounds
        

        Panel pics = new Panel(); 
        bool moving = false;
        Point loc = new Point(); // Για να κρατάμε τη θέση του ποντικιού όταν κάνουμε drag για μετακίνηση
        List<Image> superheroes = new List<Image>(); // Λίστες για να κρατάμε τις εικόνες που φορτώνουμε από τους φακέλους.
                                                     // Θα χρησιμοποιούνται για να δημιουργούμε τα PictureBox όταν ο χρήστης κάνει κλικ για να προσθέσει μια εικόνα.
        List<Image> backgrounds = new List<Image>();
        List<Image> bubbles = new List<Image>();
        List<int> panelcount = new List<int>();

        // ΑΥΤΗ ΕΙΝΑΙ Η ΚΥΡΙΑ ΛΙΣΤΑ ΠΟΥ ΘΑ ΕΧΕΙ ΟΛΕΣ ΤΙΣ ΣΕΛΙΔΕΣ
        List<List<AssetData>> allPages = new List<List<AssetData>>(); // Κάθε στοιχείο της allPages είναι μια λίστα AssetData που αντιπροσωπεύει τα αντικείμενα μιας σελίδας.
                                                                      // Όταν δημιουργούμε νέα σελίδα, προσθέτουμε μια νέα λίστα στην allPages. Όταν αλλάζουμε σελίδα, φορτώνουμε την αντίστοιχη λίστα από την allPages στη canvasObjects για να εμφανιστεί στην οθόνη.
       
        List<AssetData> canvasObjects = new List<AssetData>(); // Αυτή η λίστα θα περιέχει τα αντικείμενα της τρέχουσας σελίδας που εμφανίζεται στο panel1.
                                                               // Όταν κάνουμε αλλαγή σελίδας, αυτή η λίστα θα ενημερώνεται με τα δεδομένα της νέας σελίδας από την allPages.

        int viewingIndex = 0;

        string path = "/images/";
        readonly string FileName;
        int counter;
        bool load;


        int panels = 1;
        public Canvas(string FileName, bool load = false)
        {
            InitializeComponent();
            LoadImagesFromFolder("Superheroes", superheroes);
            LoadImagesFromFolder("Backgrounds", backgrounds);
            LoadImagesFromFolder("Bubbles", bubbles);

            this.load = load;

            menuStrip1.Dock = DockStyle.Top;


            this.Paint += Paint1;
            this.DoubleBuffered = true;

            this.FileName = FileName;

            if (load == true) // ΑΝ ΕΙΝΑΙ ΥΠΑΡΧΟΝ COMIC: Φορτώνουμε τα δεδομένα από το .b αρχείο
            {
                LoadProjectBinary();
                // Αφού φορτώσουμε, πάμε στην τελευταία σελίδα ή στην πρώτη
                if (allPages.Count > 0) {
                    viewingIndex = 0;
                    canvasObjects = allPages[0];
                    counter=allPages.Count;
                    panels = panelcount[0];
                    panel1.Controls.Clear();
                    panel1.BackgroundImage = null;
                    ReconstructCanvas();
                
                }
            }
            else
            {
                // ΑΝ ΕΙΝΑΙ ΝΕΟ COMIC: Δημιουργούμε την πρώτη κενή σελίδα
                allPages.Add(new List<AssetData>());
                canvasObjects = allPages[0]; // Συνδέουμε την τρέχουσα σελίδα με την πρώτη της λίστας
            }
        }
    
        

        private void Canvas_Load(object sender, EventArgs e)
        {

            screen = new Size(this.ClientSize.Width, this.ClientSize.Height);

            int nwX = this.ClientSize.Width / 4;

            // Τοποθέτηση του Panel ανάμεσα στις γραμμές
            panel1.Location = new Point(nwX, menuStrip1.Height);
            panel1.Size = new Size(this.ClientSize.Width * 7 / 12, this.ClientSize.Height-menuStrip1.Height);
            panel1.BackColor = Color.White;

            

            this.Controls.Add(panel1);
            this.panel1.Paint += PaintPanels;
            this.panel1.Paint += imgSelectPaint;
            this.panel1.MouseClick += Panel1Click;
            //panel1.SendToBack();
            //menuStrip1.BringToFront();

            pics.Location = new Point(0, menuStrip1.Height);
            pics.Size = new Size(this.ClientSize.Width / 4 - 10, this.ClientSize.Height - menuStrip1.Height);
            this.Controls.Add(pics);


            Button clear = new Button();
            clear.Size = new Size(200, 50);
            clear.Location = new Point(this.ClientSize.Width * 11 / 12 - clear.Width / 2, 50);
            clear.Text = "Clear Screen";
            clear.Name = "Clear";
            clear.Font = new Font("Impact", 20);
            clear.BackColor = Color.LightSalmon;
            this.Controls.Add(clear);
            clear.BringToFront();
            clear.MouseClick += clearClick;

            Button mirror = new Button();
            mirror.Size = new Size(200, 50);
            mirror.Location = new Point(this.ClientSize.Width * 11 / 12 - mirror.Width / 2, 150);
            mirror.Text = "Mirror image";
            mirror.Name = "mirror";
            mirror.BackColor = Color.LightSteelBlue;
            mirror.Font = new Font("Impact", 20);
            this.Controls.Add(mirror);
            mirror.BringToFront();
            mirror.MouseClick += mirrorImage;

            Button newPage = new Button();
            newPage.Size = new Size(200, 50);
            newPage.Location = new Point(this.ClientSize.Width * 11 / 12 - newPage.Width / 2, 250);
            newPage.Text = "New page";
            newPage.Name = "New Page";
            newPage.BackColor = Color.LightSkyBlue;
            newPage.Font = new Font("Impact", 20);
            this.Controls.Add(newPage);
            newPage.BringToFront();
            newPage.Click += NewPageCreate;

            Button save = new Button();
            save.Size = new Size(200, 50);
            save.Location = new Point(this.ClientSize.Width * 11 / 12 - save.Width / 2, 350);
            save.Text = "Save page";
            save.Name = "Save";
            save.BackColor = Color.LightGreen;
            save.Font = new Font("Impact", 20);
            this.Controls.Add(save);
            save.BringToFront();
            save.Click += createBitmap;



            Button previous = new Button();
            previous.Size = new Size(80, 50);
            previous.Location = new Point(this.ClientSize.Width * 21 / 24 , this.ClientSize.Height -200);
            previous.Font = new Font(Font.FontFamily, 30);
            previous.Text = "⏪";
            previous.Name = "Previous";
            this.Controls.Add(previous);
            previous.BringToFront();
            previous.Click += btnPrevious_Click;


            Button next = new Button();
            next.Size = new Size(80, 50);
            next.Location = new Point(this.ClientSize.Width * 21 / 24 + next.Width, this.ClientSize.Height - 200);
            next.Font = new Font(Font.FontFamily, 30);
            next.Text = "⏩";
            next.Name = "Next";
            this.Controls.Add(next);
            next.BringToFront();
            next.Click += btnNext_Click;

            Button displaycomic = new Button();
            displaycomic.Size = new Size(200, 50);
            displaycomic.Location = new Point(this.ClientSize.Width * 11 / 12 - displaycomic.Width / 2, 450);
            displaycomic.Text = "Display comic";
            displaycomic.Name = "disp";
            displaycomic.BackColor = Color.LightGray;
            displaycomic.Font = new Font("Impact", 20);
            this.Controls.Add(displaycomic);
            displaycomic.BringToFront();







            displaycomic.Click += (s, ev) => // Όταν πατάμε το κουμπί "Display comic", ανοίγει το Viewer με το φάκελο του τρέχοντος comic
            {
                string comicPath = Path.Combine(Application.StartupPath, "comics", FileName);

                    Viewer viewerForm = new Viewer(comicPath);                  
                    viewerForm.WindowState = FormWindowState.Maximized;

                    this.Hide();


                    viewerForm.ShowDialog();

                    
                    this.Show();
    

            };




        }

        private void btnPrevious_Click(object sender, EventArgs e) // Όταν πατάμε το κουμπί "Previous", πηγαίνουμε στην προηγούμενη σελίδα αν υπάρχει
        {
            if (viewingIndex > 0)
            {
                LoadPageData(viewingIndex-1);
            }
            else
            {
                MessageBox.Show("Αρχή του comic!");
            }
        }

        private void btnNext_Click(object sender, EventArgs e) // Όταν πατάμε το κουμπί "Next", πηγαίνουμε στην επόμενη σελίδα αν υπάρχει
        {
            if (viewingIndex < allPages.Count - 1)
            {
                LoadPageData(viewingIndex + 1);
            }
            else
            {
                MessageBox.Show("Τέλος του comic! Πατήστε 'New Page' για προσθήκη.");
            }
        }

        // Φορτώνει τις εικόνες από τον φάκελο (Superheroes, Backgrounds ή Bubbles) και τις αποθηκεύει στη λίστα που της δίνουμε (superheroes, backgrounds ή bubbles αντίστοιχα)
        private void LoadImagesFromFolder(string folderName, List<Image> targetList) 
        {
            string fullPath = Path.Combine(Application.StartupPath + path, folderName);
            if (Directory.Exists(fullPath))
            {
                foreach (var filePath in Directory.GetFiles(fullPath))
                {
                    Image img = Image.FromFile(filePath);
                    img.Tag = filePath; // ΕΔΩ αποθηκεύουμε το μονοπάτι μέσα στην ίδια την εικόνα
                    targetList.Add(img);
                }
            }
        }

        // Αυτή η μέθοδος διαβάζει το .b αρχείο και γεμίζει την allPages με τις λίστες AssetData για κάθε σελίδα, καθώς και την panelcount με τον αριθμό των panels για κάθε σελίδα.
        private void LoadProjectBinary() 
        {
            string bFilePath = Path.Combine(Application.StartupPath, "comics", FileName, FileName + ".b"); // Το μονοπάτι για το .b αρχείο (πρέπει να είναι μέσα στον φάκελο του comic, με το ίδιο όνομα)

            if (!File.Exists(bFilePath))
            {
                MessageBox.Show("Το αρχείο αποθήκευσης δεν βρέθηκε.");
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(bFilePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    // Φορτώνουμε όλη τη λίστα από το αρχείο
                    allPages = (List<List<AssetData>>)bf.Deserialize(fs);
                    panelcount = (List<int>)bf.Deserialize(fs);
                    //dialogs = (List<RichTextBox>)bf.Deserialize(fs);
                }

                counter = allPages.Count;
                // Αφού γεμίσει η λίστα canvasObjects, πρέπει να τις εμφανίσουμε στο Panel
                //ReconstructCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Σφάλμα κατά τη φόρτωση: " + ex.Message);
            }
        }

        private void ReconstructCanvas() // Αυτή η μέθοδος διαβάζει τα δεδομένα από την canvasObjects (που έχει τα AssetData της τρέχουσας σελίδας)
                                         // και δημιουργεί τα PictureBox και το background στο panel1 για να αναπαραστήσει την κατάσταση της σελίδας.
        {
            panel1.Controls.Clear();
            panel1.BackgroundImage = null;

            //foreach(RichTextBox bb in dialogs)
            //{
            //    panel1.Controls.Add(bb);
            //}

            foreach (AssetData data in canvasObjects)
            {
                if (data.AssetType == "Background")
                {
                    try
                    {
                        panel1.BackgroundImage = Image.FromFile(data.ImagePath);
                        panel1.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    catch { /* Αν δεν βρεθεί η εικόνα, αγνόησέ το για να μην κρασάρει */ }
                    continue;
                }

                

                // Δημιουργία του PictureBox από τα δεδομένα
                PictureBox pb = new PictureBox();
                try
                {
                    pb.Image = Image.FromFile(data.ImagePath);
                }
                catch
                {
                    // Αν δεν βρεθεί η εικόνα, βάλε ένα κενό ή συνέχισε
                    continue;
                }
                
                pb.Location = new Point(data.X, data.Y);
                pb.Size = new Size(data.Width, data.Height);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.BackColor = Color.Transparent;
                pb.Tag = data;
                
                

                pb.MouseDown += (s, ev) =>
                {
                    moving = true;
                    loc = ev.Location;
                };

                //Όταν αφήνεις το κλικ
                pb.MouseUp += (s, ev) =>
                {
                    moving = false;
                };

               

                
                pb.MouseDown += imgMouseDown; // Για το resize flag
                pb.MouseMove += dragAndDrop;  // Η ουσιαστική κίνηση
                pb.MouseMove += imgEdit;      // Για αλλαγή κέρσορα
                pb.MouseUp += imgMouseUp;     // Για το resize flag off
                pb.MouseClick += imgSelect;   // Για επιλογή (κόκκινο πλαίσιο)

                
                pb.MouseClick += (s, ev) => {  // Για να προσθέτουμε κείμενο με δεξί κλικ πάνω στην εικόνα
                    if (ev.Button == MouseButtons.Right) addText(s, ev);
                };

                if (!string.IsNullOrEmpty(data.TextContent) || !string.IsNullOrEmpty(data.TextRtf))  // Αν υπάρχει αποθηκευμένο κείμενο, δημιουργούμε το RichTextBox και το γεμίζουμε με το περιεχόμενο
                {
                    RichTextBox txt = new RichTextBox();
                    txt.Parent = pb;
                    txt.BorderStyle = BorderStyle.None;

                    //ΕΛΕΓΧΟΣ ΚΑΙ ΦΟΡΤΩΣΗ ΘΕΣΗΣ/ΜΕΓΕΘΟΥ
                    // Αν έχουμε σώσει πλάτος > 0, σημαίνει ότι έχουμε αλλάξει το μέγεθος
                    if (data.TxtWidth > 0 && data.TxtHeight > 0)
                    {
                        txt.Location = new Point(data.TxtX, data.TxtY);
                        txt.Size = new Size(data.TxtWidth, data.TxtHeight);
                    }
                    else
                    {
                        //Default τιμές 
                        txt.Size = new Size(pb.Width / 2, pb.Height / 2);
                        txt.Location = new Point(pb.Width / 4, pb.Height / 4);
                    }
                    

                    //Φόρτωση περιεχομένου 
                    if (!string.IsNullOrEmpty(data.TextRtf))
                    {
                        try { txt.Rtf = data.TextRtf; } catch { txt.Text = data.TextContent; }
                    }
                    else
                    {
                        txt.Text = data.TextContent;
                        txt.Font = new Font("Comic Sans MS", 14);
                    }

                    //Events για κίνηση και resize
                    txt.MouseMove += dragAndDrop;
                    txt.MouseDown += (s, ev) => { moving = true; loc = ev.Location; };
                    txt.MouseUp += (s, ev) => { moving = false; };
                    txt.MouseMove += imgEdit;
                    txt.MouseUp += imgMouseUp;
                    txt.MouseDown += imgMouseDown;

                    // Όταν αλλάζει το κείμενο, ενημερώνουμε το AssetData για να σωθεί
                    txt.TextChanged += (s, ev) => { data.TextContent = txt.Text; data.TextRtf = txt.Rtf; };

                    //Και όταν αλλάζει το μέγεθος του κειμένου
                    txt.Resize += (s, ev) => {
                        data.TxtWidth = txt.Width;
                        data.TxtHeight = txt.Height;
                    };
                    // Και όταν τελειώνει η μετακίνηση του κειμένου
                    txt.Move += (s, ev) => {
                        data.TxtX = txt.Location.X;
                        data.TxtY = txt.Location.Y;
                    };

                    pb.Controls.Add(txt);
                    txt.BringToFront();
                }

                panel1.Controls.Add(pb);
            }
            panel1.Invalidate();
        }


        private void SaveProjectBinary() // Αυτή η μέθοδος διασχίζει όλα τα PictureBox στο panel1, βρίσκει τα AssetData τους και ενημερώνει τα δεδομένα στη λίστα canvasObjects.
                                         // Στη συνέχεια, σώζει όλη την allPages (που περιέχει όλες τις σελίδες) και την panelcount (που περιέχει τον αριθμό των panels για κάθε σελίδα) σε ένα .b αρχείο.
        {
            foreach (Control c in panel1.Controls)
            {
                //Ελέγχουμε αν το αντικείμενο είναι PictureBox και αν έχει δεδομένα (Tag)
                if (c is PictureBox pb && pb.Tag is AssetData data)
                {
                    //Ψάχνουμε αν μέσα στο PictureBox υπάρχει RichTextBox (κείμενο)
                    foreach (Control child in pb.Controls)
                    {
                        if (child is RichTextBox rtb)
                        {

                            
                            data.TextContent = rtb.Text; //Αποθηκεύουμε το κείμενο
                            data.TextRtf = rtb.Rtf;

                            //ΑΠΟΘΗΚΕΥΣΗ ΘΕΣΗΣ και ΜΕΓΕΘΟΥΣ
                            data.TxtX = rtb.Location.X;
                            data.TxtY = rtb.Location.Y;
                            data.TxtWidth = rtb.Width;
                            data.TxtHeight = rtb.Height;

                            break;
                        }
                    }
                }
            }

            // Σώζουμε την τρέχουσα κατάσταση στη μνήμη
            if (viewingIndex < allPages.Count)
            {
                allPages[viewingIndex] = new List<AssetData>(canvasObjects);  // Ενημερώνουμε την τρέχουσα σελίδα με τα νέα δεδομένα

                //Σώζουμε και τα panels
                if (viewingIndex < panelcount.Count)
                    panelcount[viewingIndex] = panels;
                else
                    panelcount.Add(panels);
            }

            string bFilePath = Path.Combine(Application.StartupPath, "comics", FileName, FileName + ".b");

            using (FileStream fs = new FileStream(bFilePath, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, allPages);
                bf.Serialize(fs, panelcount);
               bf.Serialize(fs, panelcount);
            }
        }

        private void NewPageCreate(object sender, EventArgs e)  // Όταν πατάμε το κουμπί "New Page", σώζουμε την τρέχουσα σελίδα, 
                                           //δημιουργούμε μια νέα κενή σελίδα, ενημερώνουμε τους δείκτες και καθαρίζουμε το panel για να ξεκινήσουμε τη νέα σελίδα.
        {
            
            if (allPages.Count > viewingIndex)
            {
                allPages[viewingIndex] = new List<AssetData>(canvasObjects);

                
                if (viewingIndex < panelcount.Count)
                    panelcount[viewingIndex] = panels;
                else
                    panelcount.Add(panels);
            }

            createBitmap(sender, e); // Αποθήκευση εικόνας

            //Δημιουργία ΝΕΑΣ σελίδας
            List<AssetData> newPageList = new List<AssetData>();
            allPages.Add(newPageList);

            //Προσθέτουμε την προεπιλογή (1) για τη ΝΕΑ σελίδα στη λίστα panelcount
            panelcount.Add(1);



            //Ενημέρωση δεικτών
            viewingIndex = allPages.Count - 1;
            counter = allPages.Count;

            //Reset για τη νέα σελίδα
            canvasObjects = newPageList;
            panels = 1; //Η νέα σελίδα ξεκινάει πάντα με 1 

            // Καθαρισμός
            panel1.Controls.Clear();
            panel1.BackgroundImage = null;
            imgSelected = null;
            panel1.Invalidate();
            panel1.Update();
        }
        private void LoadPageData(int index) // Αυτή η μέθοδος χρησιμοποιείται για να φορτώσει τα δεδομένα μιας συγκεκριμένης σελίδας
                                             // όταν ο χρήστης κάνει κλικ στα κουμπιά "Previous" ή "Next".
        {
            //Σώζουμε τα τρέχοντα δεδομένα  
            if (viewingIndex < allPages.Count)
            {
                allPages[viewingIndex] = new List<AssetData>(canvasObjects);

                //Σώζουμε τον αριθμό των panels για την σελίδα που αφήνουμε
                if (viewingIndex < panelcount.Count)
                    panelcount[viewingIndex] = panels;
                else
                    panelcount.Add(panels); 
            }

            //Αλλάζουμε σελίδα
            viewingIndex = index;
            canvasObjects = allPages[viewingIndex];

            //Φορτώνουμε τα panels της νέας σελίδας
            
            if (viewingIndex < panelcount.Count)
                panels = panelcount[viewingIndex];
            else
                panels = 1; // Default αν δεν βρεθεί

            //reconstruct ξανά
            panel1.Controls.Clear();
            panel1.BackgroundImage = null;
            ReconstructCanvas();

            
            panel1.Invalidate();
        }


        private void createBitmap(object sender, EventArgs e) // Αυτή η μέθοδος δημιουργεί ένα Bitmap από το panel1 και το αποθηκεύει ως εικόνα.
        {
            
            PictureBox tempSelection = imgSelected; //Κρατάμε προσωρινά σε μια μεταβλητή ποια εικόνα ήταν επιλεγμένη


            imgSelected = null; //Μηδενίζουμε την επιλογή για να φύγει το κόκκινο πλαίσιο

            //Εξαναγκάζουμε το Panel να ξαναζωγραφιστεί ΤΩΡΑ (χωρίς το κόκκινο)
            //Το Invalidate ζητάει ανανέωση, το Update την εκτελεί αμέσως.
            panel1.Invalidate();
            panel1.Update();

            //Δημιουργία του Bitmap (Τώρα το Panel είναι καθαρό)
            Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(bmp, new Rectangle(0, 0, panel1.Width, panel1.Height));

            //Αποθήκευση
            saveImage(bmp);

            //Επαναφέρουμε την επιλογή (για να ξαναδεί ο χρήστης τι είχε επιλέξει)
            imgSelected = tempSelection;
            //panel1.Invalidate(); // Ξαναζωγραφίζουμε το κόκκινο στην οθόνη

            bmp.Dispose();
            SaveProjectBinary();
        }

        private void saveImage(Bitmap bmp)  // Αυτή η μέθοδος αποθηκεύει το Bitmap που δημιουργήθηκε από το panel1 ως αρχείο PNG στον φάκελο του comic, 
                                            // με όνομα που βασίζεται στο FileName και τον counter για να μην σβήνει παλιές εικόνες.
        {


            string directoryPath = Path.Combine(Application.StartupPath, "comics" , FileName);
            string fileNameWithExt = FileName + "_" + counter + ".png";
            string fullPath = Path.Combine(directoryPath, fileNameWithExt);
            try
            {
                bmp.Save(fullPath, ImageFormat.Png); 
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                MessageBox.Show("Σφάλμα: Το αρχείο χρησιμοποιείται ίδη.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void clearClick(object sender, EventArgs e)  // Αυτή η μέθοδος καθαρίζει το panel1 και την canvasObjects για να ξεκινήσουμε με μια κενή σελίδα.
        {
            canvasObjects.Clear();




            panel1.Controls.Clear();
            panel1.BackgroundImage = null;
            imgSelected = null;
            panels = 1;
            panel1.Invalidate();

        }

        private void mirrorImage(object sender, EventArgs e) // Αυτή η μέθοδος κάνει mirror την επιλεγμένη εικόνα (αν υπάρχει) και ενημερώνει την οθόνη.
        {

            if (imgSelected != null)
            {
                imgSelected.Image.RotateFlip(RotateFlipType.RotateNoneFlipX); 
                imgSelected.Invalidate();
                panel1.Invalidate();
            }
            else
            {
                MessageBox.Show("please select an image");
            }
        }


        private void Paint1(object sender, PaintEventArgs e) // Αυτή η μέθοδος ζωγραφίζει τις δύο κάθετες γραμμές που χωρίζουν το panel1 από τα υπόλοιπα 2/4 της οθόνης.
        {
            Graphics g = e.Graphics;

            //Υπολογίζουμε τα σημεία δυναμικά με βάση την ανάλυση της οθόνης
            Point nw = new Point(this.ClientSize.Width / 4, 0);
            Point sw = new Point(this.ClientSize.Width / 4, this.ClientSize.Height);

            Point ne = new Point(this.ClientSize.Width * 5 / 6, 0);
            Point se = new Point(this.ClientSize.Width * 5 / 6, this.ClientSize.Height);

            using (Pen pen = new Pen(Color.Black, 8))
            {
                g.DrawLine(pen, nw, sw);
                g.DrawLine(pen, ne, se);
            }





        }

        private void Panel1Click(object sender, EventArgs e) // Αυτή η μέθοδος χειρίζεται το κλικ στο panel1. Αν το click flag είναι ενεργό, σημαίνει ότι ο χρήστης έχει επιλέξει μια εικόνα από τα διαθέσιμα
                                                             // και τώρα θέλει να την προσθέσει στο panel1.
        {
            if (click)
            {
                
                if (selection == 3)
                {
                    //Αφαιρούμε τυχόν παλιό background από τη λίστα δεδομένων
                    //(Κρατάμε μόνο ένα background ανά σελίδα
                    canvasObjects.RemoveAll(x => x.AssetType == "Background");

                    
                    panel1.BackgroundImageLayout = ImageLayout.Stretch; //Ενημερώνουμε την οθόνη 
                    panel1.BackgroundImage = imgDraw.Image;

                    
                    AssetData bgData = new AssetData //Δημιουργούμε τα δεδομένα για αποθήκευση
                    {
                        X = 0,
                        Y = 0,
                        Width = panel1.Width,
                        Height = panel1.Height,
                        ImagePath = imgDraw.Tag.ToString(),
                        AssetType = "Background"
                    };

                    //Το προσθέτουμε στη λίστα
                    canvasObjects.Add(bgData);

                    //Reset τις μεταβλητές 
                    click = false;
                    return;
                }




                // Υπολογίζουμε τη θέση δημιουργίας της νέας εικόνας με βάση την τρέχουσα θέση του κέρσορα και το μέγεθος της εικόνας, ώστε να εμφανίζεται κάτω από τον κέρσορα.
                createpoint = new Point(Cursor.Position.X - panel1.Location.X - imgDraw.Width / 2, Cursor.Position.Y - panel1.Location.Y - imgDraw.Height * 2 / 3);
                imgDraw.Parent = panel1;
                imgDraw.SizeMode = PictureBoxSizeMode.Zoom;
                imgDraw.Location = createpoint;
                imgDraw.BackColor = Color.Transparent;

                //Events
                imgDraw.MouseDown += (s, ev) => { moving = true; loc = ev.Location; };
                imgDraw.MouseMove += dragAndDrop;
                imgDraw.MouseUp += (s, ev) => { moving = false; };
                imgDraw.MouseMove += imgEdit;
                imgDraw.MouseDown += imgMouseDown;
                imgDraw.MouseClick += imgSelect;

                if (selection == 2) // Αν είναι bubble, προσθέτουμε και το event για να προσθέτουμε κείμενο με δεξί κλικ πάνω του
                {
                    imgDraw.MouseClick += (s, ev) => {
                        if (ev.Button == MouseButtons.Right) addText(s, ev);
                    };
                    

                }

                AssetData newData = new AssetData // Δημιουργούμε τα δεδομένα για την νέα εικόνα που προσθέτουμε, ώστε να μπορούμε να τα αποθηκεύσουμε αργότερα.
                {
                    X = createpoint.X,
                    Y = createpoint.Y,
                    Width = imgDraw.Width,
                    Height = imgDraw.Height,
                    ImagePath = imgDraw.Tag.ToString(),
                    AssetType = "Image"
                };

                canvasObjects.Add(newData); // Το προσθέτουμε στη λίστα των αντικειμένων της σελίδας
                imgDraw.Tag = newData;

                panel1.Controls.Add(imgDraw);
                click = false; // Reset το click flag για να μην προσθέτει συνέχεια εικόνες όταν κάνουμε κλικ στο panel1


            }
            else //
            {
                imgSelected = null;
                panel1.Invalidate();
                //MessageBox.Show("Παρακαλώ επιλέξτε χαρακτήρα");
            }
        }

        private void PaintPanels(object sender, PaintEventArgs e)  // Αυτή η μέθοδος ζωγραφίζει τις γραμμές που χωρίζουν το panel1 σε 2 ή 4 panels, ανάλογα με την επιλογή του χρήστη.
        { 

            Graphics g = e.Graphics;
            Panel p = sender as Panel;
            Pen pen = new Pen(Color.Black, 5);
            switch (panels)
            {
                case 2:
                    {
                        g.DrawLine(pen, 0, p.Height / 2, p.Width, p.Height / 2);
                        break;
                    }
                case 4:
                    {

                        // Γραμμή στη μέση (2/4)
                        g.DrawLine(pen,  p.Width / 2, 0, p.Width / 2, p.Height);

                        g.DrawLine(pen, 0, p.Height / 2, p.Width, p.Height / 2);
                        
                        break;
                    }

            }


        }


        private void dragAndDrop(object sender, MouseEventArgs e) 
        {
            if (moving && !resize)  
            {
                Control ctrl = (Control)sender;

                //Μετακίνηση του αντικειμένου και για Εικόνες και για Κείμενο
                ctrl.Location = new Point(ctrl.Location.X + e.X - loc.X,
                                          ctrl.Location.Y + e.Y - loc.Y);

                panel1.Invalidate();
                panel1.Update();

                //Ενημέρωση δεδομένων ΜΟΝΟ αν είναι Εικόνα που έχει AssetData
                //Χρησιμοποιούμε το 'as' για να δούμε αν το Tag είναι όντως AssetData
                AssetData data = ctrl.Tag as AssetData;

                if (data != null)
                {
                    //Αν είναι εικόνα ενημερώνουμε τη θέση της στη μνήμη
                    data.X = ctrl.Location.X;
                    data.Y = ctrl.Location.Y;
                    data.Width = ctrl.Width;
                    data.Height = ctrl.Height;
                }
                // Αν είναι RichTextBox (data == null), απλά το κουνάμε οπτικά χωρίς να σώζουμε συντεταγμένες X/Y στο AssetData
                // (καθώς το κείμενο είναι παιδί της εικόνας και μετακινείται σχετικά με αυτήν)
            }
        }

        private void imageClick(object sender, MouseEventArgs e) // Αυτή η μέθοδος χειρίζεται το κλικ σε μια εικόνα από τα διαθέσιμα (superheroes, backgrounds ή bubbles). 
                                                                 // Όταν ο χρήστης κάνει κλικ σε μια εικόνα, αυτή αποθηκεύεται στο imgDraw
                                                                 // και το click flag ενεργοποιείται για να περιμένει το επόμενο κλικ στο panel1 για να την προσθέσει.
        {
            imgSelected = null;
            panel1.Invalidate();
            PictureBox temp=(PictureBox)sender;
            imgDraw = new PictureBox();
            imgDraw.Size = temp.Size;
            imgDraw.Image=(Image)temp.Image.Clone();

            imgDraw.Tag = temp.Tag;
            imgDraw.Name = "gen" + created;
            created++; 
            click = true;
            
            
        }

        private void imgEdit(object sender, MouseEventArgs e) // Αυτή η μέθοδος χειρίζεται την κίνηση του ποντικιού πάνω από μια εικόνα στο panel1.
                                                              // Αν το resize flag είναι ενεργό και ο χρήστης κρατάει πατημένο το αριστερό κλικ, τότε αλλάζει το μέγεθος της εικόνας ανάλογα με την κίνηση του ποντικιού.
        {
            Control temp =(Control) sender ;
            
            if (e.Location.X >= temp.Width - 10 && e.Location.Y >= temp.Height - 10) // Αν ο κέρσορας είναι στην κάτω δεξιά γωνία, αλλάζουμε το εικονίδιο για να δείξουμε ότι μπορεί να γίνει resize
            {
                temp.Cursor = Cursors.SizeNWSE; // Αλλαγή κέρσορα για να δείξει ότι μπορεί να γίνει resize
                if (resize && !moving && MouseButtons.Left==e.Button)
                {

                    int newW = Math.Max(e.X, 20);
                    int newH = Math.Max(e.Y, 20);

                    temp.Size = new System.Drawing.Size(e.X, e.Y);
                    foreach (RichTextBox txt in temp.Controls)
                    {
                        txt.Size = new Size(temp.Width / 2, temp.Height / 2);
                        //txt.Size = new System.Drawing.Size(temp.Width - txt.Location.X - temp.Width / 4, temp.Height - txt.Location.Y - temp.Height / 4);
                    }

                    temp.Invalidate();
                    panel1.Invalidate();
                    panel1.Update();


                }
                AssetData data = (AssetData)temp.Tag; 

                if (data != null)
                {  //Όταν τελειώνει το κούνημα ή το resize:
                    data.X = temp.Location.X;
                    data.Y = temp.Location.Y;
                    data.Width = temp.Width;
                    data.Height = temp.Height;
                } 
                    
                    //moving = false;

                
               }
            else
            {
                temp.Cursor = Cursors.Default;
            }
                
            


        }

        private void imgSelect(object sender, MouseEventArgs e) 
        {
            imgSelected = (PictureBox)sender;
            imgSelected.BringToFront();
            
            panel1.Invalidate();
        }



        private void imgMouseDown(object sender, MouseEventArgs e) // Αυτή η μέθοδος χειρίζεται το MouseDown event σε μια εικόνα στο panel1.
                                                                   // Ελέγχει αν το κλικ έγινε στην κάτω δεξιά γωνία της εικόνας για να ενεργοποιήσει το resize flag.
        {
            Control c = (Control)sender;

            //Ενεργοποιούμε το resize ΜΟΝΟ αν το κλικ έγινε στην κάτω δεξιά γωνία
            if (e.X >= c.Width - 15 && e.Y >= c.Height - 15)
            {
                resize = true;
                moving = false; //Σταματάμε την κίνηση (drag) αν κάνουμε resize
            }
            else
            {
                resize = false;
                
            }

            //Επιλογή εικόνας
            if (sender is PictureBox)
            {
                imgSelected = (PictureBox)sender;
                imgSelected.BringToFront();
            }

            panel1.Invalidate();
        }

        private void imgMouseUp(object sender, MouseEventArgs e) // Αυτή η μέθοδος χειρίζεται το MouseUp event σε μια εικόνα στο panel1. Απενεργοποιεί το resize flag όταν αφήνουμε το κλικ.
        {
            resize = false;
            
            //imgSelected = null;
        }

        private void imgSelectPaint(object sender, PaintEventArgs e) // Αυτή η μέθοδος χειρίζεται το Paint event του panel1 για να ζωγραφίσει ένα κόκκινο πλαίσιο γύρω από την επιλεγμένη εικόνα (imgSelected).
        {
            Graphics g = e.Graphics;

            if (imgSelected != null)
            {
                

                    g.DrawRectangle(new Pen(Color.Red, 2), imgSelected.Location.X, imgSelected.Location.Y, imgSelected.Width, imgSelected.Height);

            }
            
           
        }

        private void addText(object sender, MouseEventArgs e) // Αυτή η μέθοδος χειρίζεται το δεξί κλικ πάνω σε μια εικόνα στο panel1
                                                              // για να προσθέσει ένα RichTextBox (για κείμενο) πάνω στην εικόνα που κλικάραμε.
        {
            PictureBox temp = (PictureBox)sender; //temp μεταβλητη για να ξέρουμε πάνω σε ποια εικόνα κάνουμε δεξί κλικ και να προσθέσουμε το RichTextBox εκεί
            if (e.Button == MouseButtons.Right)
            {
                RichTextBox txt = new RichTextBox();
                txt.Parent = temp; // Τοποθετούμε το RichTextBox ως παιδί της εικόνας, ώστε να κινείται μαζί της
                txt.Location = e.Location;
                txt.Text = "";
                txt.Font = new Font("Comic Sans MS", 14);
                txt.Size = new Size(temp.Width-e.X-temp.Width/4,temp.Height-e.Y - temp.Height / 4);
                temp.Controls.Add(txt);
                txt.MouseMove += dragAndDrop;
                txt.MouseDown += (s, ev) =>
                {
                    moving = true;
                    loc = ev.Location;
                    
                };

                //pb.MouseMove += dragAndDrop;
                txt.MouseUp += (s, ev) =>
                {
                    moving = false;
                };

                txt.MouseMove += imgEdit;
                txt.MouseUp += imgMouseUp;
                txt.MouseDown += imgMouseDown;
                //txt.TextChanged += txtChanged;

                //txt.BackColor = Color.Transparent;
                txt.BorderStyle= BorderStyle.None;
                txt.Focus();
                txt.BringToFront();
                //dialogs.Add(txt);
            }

        }

        /*private void txtChanged(object sender, EventArgs e)
        {
            // Το sender είναι το RichTextBox στο οποίο γράφεις
            RichTextBox currentTextBox = (RichTextBox)sender;

            // Ελέγχουμε αν το Parent είναι PictureBox (το συννεφάκι/εικόνα)
            if (currentTextBox.Parent is PictureBox parentImage)
            {
                // Τώρα το parentImage είναι η εικόνα που περιέχει το κείμενο.
                // Αν θέλεις να κάνεις κάτι με τα υπόλοιπα controls μέσα στην εικόνα:
                foreach (Control c in parentImage.Controls)
                {
                    if (c is RichTextBox && c != currentTextBox)
                    {
                        c.Text = currentTextBox.Text;
                    }
                }
            }
        }*/


        /*private PictureBox searchFather(RichTextBox txt)
        {
            Control temp = new Control();
            txt.Parent = temp;
            foreach (Control c in dialogs)
            {
                if (c == temp)
                return (PictureBox)c;
            }
            return null;
        }*/



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void boxToolStripMenuItem_Click(object sender, EventArgs e) // Αυτή η μέθοδος χειρίζεται το κλικ στο μενού για να χωρίσει το panel1 σε 1 panel (χωρίς γραμμές).
        {
            panels = 1;
            
            imgSelected = null; 
            //panel1.BackColor = Color.White; //ugxjyx
            panel1.Invalidate();
        }

        private void bToolStripMenuItem_Click(object sender, EventArgs e) //σε 2
        {
            panels = 2;
            
            imgSelected = null; 
            //panel1.BackColor = Color.White; //kugxiyfx
            panel1.Invalidate();
            //this.Paint += PaintPanels;
        }

        private void boxesToolStripMenuItem_Click(object sender, EventArgs e) //σε 4
        {
            panels = 4;
            imgSelected = null; 
            //panel1.BackColor = Color.White; //jvihc
            panel1.Invalidate();
            //this.Paint += PaintPanels;

        }

        private void superheroesToolStripMenuItem_Click(object sender, EventArgs e)  // Αυτή η μέθοδος χειρίζεται το κλικ στο μενού για να εμφανίσει τις διαθέσιμες εικόνες χαρακτήρων (superheroes) στο panel των εικόνων.
        {
            pics.Controls.Clear();
            displayImages(sender, e,superheroes);
            selection = 1;



        }

        private void bubblesToolStripMenuItem_Click(object sender, EventArgs e) //τα bubbles
        {
            pics.Controls.Clear();


            displayImages(sender, e, bubbles);
            selection = 2;
        }

        private void backgroundsToolStripMenuItem_Click(object sender, EventArgs e) //τα backgrounds
        {

            pics.Controls.Clear();
            displayImages(sender, e, backgrounds);
            selection = 3;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) //κουμπί  save
        {
            SaveProjectBinary();
            Application.Restart();
        }

        private void displayImages(object sender, EventArgs e, List<Image> list) // Αυτή η μέθοδος λαμβάνει μια λίστα εικόνων (ανάλογα με την επιλογή του χρήστη)
                                                                        // και τις εμφανίζει στο panel των εικόνων (pics) για να μπορεί ο χρήστης να τις επιλέξει και να τις προσθέσει στο panel1.
        {
            pics.Controls.Clear();
            int count = 0;
            int startH = 50;


            foreach (var img in list)
            {
                int width = 0;
                count++;
                if (count % 2 == 0)
                {
                    width += screen.Width / 8;
                }
                else if (count > 1)
                {
                    startH += screen.Height / 7;
                }

                PictureBox pb = new PictureBox();
                pb.Size = new Size(screen.Width / 8, screen.Height / 8);
                pb.Image = img;
                pb.Tag = img.Tag;
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Location = new Point(width - pics.Location.X, startH - pics.Location.Y);
                pb.BackColor = Color.Transparent;

                pics.Controls.Add(pb);
                pb.MouseDown += (s, ev) =>
                {
                    moving = true;
                    loc = ev.Location;
                };

                //pb.MouseMove += dragAndDrop;
                pb.MouseUp += (s, ev) =>
                {
                    moving = false;
                };

                pb.MouseClick += imageClick; //ergerger
                pics.Controls.Add(pb);


            }
        }

    }
}
