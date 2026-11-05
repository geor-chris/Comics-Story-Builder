using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comics_Story_Builder
{
    public partial class Home : Form
    {
        bool loadpr;
        public Home()
        {


            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {



            Controls.Remove(button1);
            Controls.Remove(button2);
            Controls.Remove(button4);
            label1.Visible = true;
            richTextBox1.Visible = true;
            button3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0)
            {

                Canvas canvas = new Canvas(richTextBox1.Text);

                canvas.FormClosed += (ss, args) => this.Close();

                string folderName = richTextBox1.Text;
                string path = Path.Combine(Application.StartupPath, "comics", folderName);

                try
                {
                    // 2. Έλεγχος αν ο φάκελος υπάρχει ήδη
                    if (!Directory.Exists(path))
                    {
                        // 3. Δημιουργία του φακέλου
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        MessageBox.Show("Ο φάκελος υπάρχει ήδη!");
                        richTextBox1.Clear();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // Διαχείριση σφαλμάτων (π.χ. δικαιώματα πρόσβασης)
                    MessageBox.Show("Προέκυψε σφάλμα: " + ex.Message);
                }

                this.Hide();
                canvas.Show();
            }
            else
            {
                MessageBox.Show("Please enter a comic name.");
            }




        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 20)
            {
                richTextBox1.Text = richTextBox1.Text.Substring(0, 20);
                richTextBox1.SelectionStart = richTextBox1.Text.Length; // Keep cursor at end
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            loadpr = true;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Ρύθμιση του αρχικού φακέλου (προαιρετικά στον φάκελο comics)
                string initialPath = Path.Combine(Application.StartupPath, "comics");
                if (Directory.Exists(initialPath))
                {
                    openFileDialog.InitialDirectory = initialPath;
                }

                // Φίλτρο για να βλέπει μόνο αρχεία .b
                openFileDialog.Filter = "Comic Project Files (*.b)|*.b";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 1. Παίρνουμε το πλήρες μονοπάτι (π.χ. C:\...\comics\Batman\Batman.b)
                    string fullPath = openFileDialog.FileName;

                    // 2. Παίρνουμε ΜΟΝΟ το όνομα (π.χ. Batman) για να το δώσουμε στο Canvas
                    string projectName = Path.GetFileNameWithoutExtension(fullPath);

                    // 3. Περνάμε το όνομα ΚΑΙ την τιμή 'true' που σημαίνει ότι θέλουμε να φορτώσουμε
                    Canvas canvas = new Canvas(projectName, true);

                    canvas.FormClosed += (ss, args) => this.Close();

                    this.Hide();
                    canvas.Show();
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // 1. Ρύθμιση του αρχικού φακέλου
                string initialPath = Path.Combine(Application.StartupPath, "comics");
                if (Directory.Exists(initialPath))
                {
                    folderDialog.SelectedPath = initialPath;
                }

                // Optional: Show a description text inside the dialog
                folderDialog.Description = "Select a Comic Folder";
                folderDialog.ShowNewFolderButton = false; // Disable creating new folders if you want

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string fullPath = folderDialog.SelectedPath; // e.g. C:\comics\Batman

                    // Pass the FULL PATH, not just the name
                    Viewer viewer = new Viewer(fullPath);

                    viewer.FormClosed += (ss, args) => this.Close(); // or this.Show();
                    this.Hide();
                    viewer.Show();
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
