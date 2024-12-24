namespace Prabesh_Academy.Modules.Views
{
    partial class Course_Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            pictureBox1 = new PictureBox();
            back_button = new Button();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(back_button);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(800, 450);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(697, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(42, 38);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // back_button
            // 
            // Define the back_button properties
            back_button.Location = new Point(10, back_button.Parent.ClientSize.Height - back_button.Height - 10); // Bottom-left corner
            back_button.Name = "back_button";
            back_button.Size = new Size(100, 35); // Slightly larger size for better appearance
            back_button.TabIndex = 0;
            back_button.Text = "Back";
            back_button.UseVisualStyleBackColor = true;

            // Custom styling
            back_button.FlatStyle = FlatStyle.Flat;
            back_button.FlatAppearance.BorderSize = 0;
            back_button.BackColor = Color.FromArgb(51, 122, 183); // Custom blue color
            back_button.ForeColor = Color.White; // White text color
            back_button.Font = new Font("Arial", 10, FontStyle.Bold);
            back_button.Padding = new Padding(10);

            // Optional: Rounded corners using a custom `Graphics` method or using a `Panel` with `Region` property
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, 20, 20, 180, 90); // Top-left corner
            path.AddArc(back_button.Width - 20, 0, 20, 20, 270, 90); // Top-right corner
            path.AddArc(back_button.Width - 20, back_button.Height - 20, 20, 20, 0, 90); // Bottom-right corner
            path.AddArc(0, back_button.Height - 20, 20, 20, 90, 90); // Bottom-left corner
            path.CloseFigure();
            back_button.Region = new Region(path); // Apply rounded corners

            // Add the button to the form or container

            // 
            // Course_Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pictureBox1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(back_button);

            Name = "Course_Home";
            Size = new Size(800, 450);
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox pictureBox1;
        private Button back_button;
    }
}