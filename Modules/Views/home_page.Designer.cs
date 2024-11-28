using System.Windows.Forms;

namespace Prabesh_Academy.Modules.Views
{
    partial class home_page
    {
        private System.ComponentModel.IContainer components = null;

        // Dispose to clean up resources
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Initialize the UI components
        // Initialize UI components
        private void InitializeComponent()
        {
            this.BackColor = System.Drawing.Color.Black;
            this.Size = new Size(800, 600); // Set the size of the UserControl

            // Back Button
            Button backButton = new Button();
            backButton.Text = "Back";
            backButton.Font = new Font("Comic Sans MS", 10);
            backButton.Size = new Size(60, 30);
            backButton.Location = new Point(10, 10);
            //backButton.Click += BackButton_Click; // Attach click event
            backButton.Visible = false; // Initially hidden

           
 

            // Add controls to the UserControl
        }
    }
}
