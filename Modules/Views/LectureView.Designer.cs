using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using Svg;

namespace Prabesh_Academy.Modules.Views
{
    partial class LectureView
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
            try
            {
                this.back_button = new System.Windows.Forms.Button();
                this.back_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
                this.back_button.Location = new System.Drawing.Point(10, 10);
                this.back_button.Name = "back_button";
                this.back_button.Size = new System.Drawing.Size(30, 30);
                this.back_button.TabIndex = 1;
                //this.back_button.UseVisualStyleBackColor = true;
                this.back_button.Click += (s, e) =>
                {
                    if (mainforminst != null && !mainforminst.IsDisposed)
                    {
                        webView2.Dispose();
                        mainforminst.Controls.Clear();
                        Course_Home courseHome = new Course_Home(mainforminst); // Recreate Course_Home instance
                        mainforminst.Controls.Add(courseHome);
                    }
                };
                // Convert the SVG, svg already exists there as  into an image
                this.back_button.BackgroundImage = SvgToImage(30,30); // Implement SvgToImage conversion method
                this.back_button.BackgroundImageLayout = ImageLayout.Stretch;

                // Add a click event for the back button

                // Add the back button to the form
                this.Controls.Add(back_button);
            }
            catch (Exception ex)
            {
                // Show an error message
                MessageBox.Show("There was an error initializing the component.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Optionally log the error or handle it further
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #endregion

        // Example method to convert SVG to Image
        private System.Drawing.Image SvgToImage(int width, int height)
        {
            string svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" xml:space=\"preserve\" width=\"655.359\" height=\"655.359\" style=\"shape-rendering:geometricPrecision;text-rendering:geometricPrecision;image-rendering:optimizeQuality;fill-rule:evenodd;clip-rule:evenodd\" viewBox=\"0 0 6.827 6.827\"><path style=\"fill:#424242\" d=\"M0 0h6.827v6.827H0z\"/><path d=\"M.853 3.413a2.56 2.56 0 1 1 5.12 0 2.56 2.56 0 0 1-5.12 0zM4.72 2.8H3.413a.08.08 0 0 1-.08-.08v-.5L2.14 3.413l1.193 1.194v-.5a.08.08 0 0 1 .08-.08H4.72V2.8zm-3.413.613a2.1 2.1 0 0 1 .617-1.49 2.1 2.1 0 0 1 1.49-.616 2.1 2.1 0 0 1 1.489.617 2.1 2.1 0 0 1 .617 1.49 2.1 2.1 0 0 1-.617 1.489 2.1 2.1 0 0 1-1.49.617 2.1 2.1 0 0 1-1.49-.617 2.1 2.1 0 0 1-.616-1.49zm.73-1.376a1.94 1.94 0 0 0-.57 1.376c0 .538.218 1.025.57 1.377.352.352.839.57 1.376.57a1.94 1.94 0 0 0 1.377-.57 1.94 1.94 0 0 0 .57-1.377 1.94 1.94 0 0 0-.57-1.376 1.94 1.94 0 0 0-1.377-.57 1.94 1.94 0 0 0-1.376.57z\" style=\"fill:#fffffe\"/></svg>";
        using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(svg)))
            {
                var svgDoc = Svg.SvgDocument.Open<Svg.SvgDocument>(ms);
                svgDoc.Width = width;
                svgDoc.Height = height;


                return svgDoc.Draw();
            }
        }

        private Button back_button;

    }

}