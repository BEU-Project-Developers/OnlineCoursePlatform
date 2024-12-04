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

        // Initialize UI components
        private void InitializeComponent()
        {
            this.BackColor = System.Drawing.Color.White;

            // Back Button
            Button backButton = new Button();
            backButton.Text = "Back";
            backButton.Font = new Font("Comic Sans MS", 10);
            backButton.Size = new Size(60, 30);
            backButton.Location = new Point(10, 10);
            //backButton.Click += BackButton_Click; // Attach click event
            backButton.Visible = false; // Initially hidden

            this.Controls.Add(backButton);

            // Remove this.Size to allow dynamic resizing
            // this.Size = new Size(800, 600); // Remove this line
        }
    }
}
