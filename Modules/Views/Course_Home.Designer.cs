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
            // Initialize the flowLayoutPanel1
            flowLayoutPanel1 = new FlowLayoutPanel
            {
                Name = "flowLayoutPanel1",
                Dock = DockStyle.Fill, // Ensures the FlowLayoutPanel fills the entire form or container
                AutoScroll = true,     // Enables scrolling if content overflows
                Padding = new Padding(10), // Optional: Adds padding around the panel
                WrapContents = true,   // Allows wrapping of content (if needed)
            };

            // Add flowLayoutPanel1 to the form's controls
            Controls.Add(flowLayoutPanel1);

            // Initialize the back button and position it at the bottom-left corner
            back_button = new Button
            {
                Name = "back_button",
                Size = new Size(75, 23),
                Text = "Back",
                Location = new Point(10, this.ClientSize.Height - 40), // Position it at the bottom-left corner
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left // Ensure it stays at the bottom-left on resize
            };

            // Add the click event for the back button
            back_button.Click += BackButton_Click;

            // Add the back button to the form's controls
            Controls.Add(back_button);

            // Set form properties
            Name = "Course_Home";
            Size = new Size(Width, Height); // Use current width and height

            // Perform layout adjustments
            flowLayoutPanel1.ResumeLayout(true);
            ResumeLayout(true);
        }



        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button back_button;

    }
}