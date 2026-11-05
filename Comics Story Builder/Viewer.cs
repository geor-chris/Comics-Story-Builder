using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Comics_Story_Builder
{
    public partial class Viewer : Form
    {
        // Μεταβλητές
        private string imageFolderPath;
        private string[] imagefiles;
        private int currentIndex = 0;
        private PictureBox pictureBox;

        // Νέες μεταβλητές για τα κουμπιά
        private Button btnNext;
        private Button btnPrevious;

        public Viewer(string path)
        {
            InitializeComponent();

            //Αποθηκεύουμε το μονοπάτι
            imageFolderPath = path;

            // Ρυθμίσεις εμφάνισης
            Text = "Photo Viewer (Manual)";
            Size = new Size(800, 800);

            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BackColor = Color.Transparent;
            this.Controls.Add(pictureBox);

            

            
            btnNext = new Button();
            btnNext.Text = ">>";
            btnNext.Size = new Size(60, 50);
            //Τοποθέτηση στα δεξιά στη μέση
            btnNext.Location = new Point(this.ClientSize.Width - 70, this.ClientSize.Height / 2 - 25);
            btnNext.Anchor = AnchorStyles.Right; //Για να μένει δεξιά αν αλλάξει μέγεθος το παράθυρο
            btnNext.Click += BtnNext_Click;
            this.Controls.Add(btnNext);
            btnNext.BringToFront(); //Να είναι πάνω από την εικόνα

           
            btnPrevious = new Button();
            btnPrevious.Text = "<<";
            btnPrevious.Size = new Size(60, 50);
            //Τοποθέτηση στα αριστερά στη μέση
            btnPrevious.Location = new Point(10, this.ClientSize.Height / 2 - 25);
            btnPrevious.Anchor = AnchorStyles.Left; //Για να μένει αριστερά
            btnPrevious.Click += BtnPrevious_Click;
            this.Controls.Add(btnPrevious);
            btnPrevious.BringToFront(); //Να είναι πάνω από την εικόνα
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            LoadImages();

            if (imagefiles.Length > 0)
            {
                displayImage(currentIndex);
            }
            else
            {
                MessageBox.Show("Δεν βρέθηκαν εικόνες στον φάκελο!");
                this.Close(); 
            }
        }

        private void LoadImages()
        {
            if (!Directory.Exists(imageFolderPath))
            {
                imagefiles = new string[0];
                return;
            }
            imagefiles = Directory.GetFiles(imageFolderPath, "*.png");

        }

        
        private void BtnNext_Click(object sender, EventArgs e) //Event για το κουμπί ΕΠΟΜΕΝΟ
        { 
            if (imagefiles.Length == 0) return;

            currentIndex++;
            
            if (currentIndex >= imagefiles.Length) // Αν φτάσουμε στο τέλος, γυρνάμε στην αρχή (Loop)
            {
                currentIndex = 0;
            }
            displayImage(currentIndex);
        }

        
        private void BtnPrevious_Click(object sender, EventArgs e) //Event για το κουμπί ΠΡΟΗΓΟΥΜΕΝΟ
        {
            //Έλεγχος αν υπάρχουν εικόνες
            if (imagefiles.Length == 0) return;

            
            if (currentIndex == 0) //Έλεγχος αν είμαστε ήδη στην πρώτη εικόνα (δείκτης 0)
            {
                MessageBox.Show("Είστε στην αρχή του comic!");
                return; //Σταματάμε εδώ, δεν κάνουμε τίποτα άλλο
            }

            
            currentIndex--; //Αν δεν είμαστε στην αρχή, μειώνουμε τον δείκτη και δείχνουμε την προηγούμενη
            displayImage(currentIndex);
        }

        private void displayImage(int index)
        {
            if (index >= 0 && index < imagefiles.Length)
            {
                
                if (pictureBox.Image != null)
                {
                    pictureBox.Image.Dispose();
                }

                string imagePath = imagefiles[index];

               
                
                using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read)) //Χρησιμοποιούμε using για να κλείσει το Stream αμέσως μόλις διαβαστεί
                {
                    
                    pictureBox.Image = new Bitmap(stream); //Δημιουργούμε ένα αντίγραφο (Bitmap) από το stream
                }
                

                this.Text = "Viewer - " + Path.GetFileName(imagePath) +
                            " " + (index + 1) + "/" + imagefiles.Length;
            }
        }

       
        protected override void OnResize(EventArgs e)  //Φροντίζουμε τα κουμπιά να μένουν στη σωστή θέση αν γίνει Resize
        {
            base.OnResize(e);
            if (btnNext != null && btnPrevious != null)
            {
                btnNext.Location = new Point(this.ClientSize.Width - 70, this.ClientSize.Height / 2 - 25);
                btnPrevious.Location = new Point(10, this.ClientSize.Height / 2 - 25);
            }
        }
    }
}