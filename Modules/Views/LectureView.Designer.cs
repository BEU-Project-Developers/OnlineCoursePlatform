using System.Windows.Forms;

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
                // Leave the component empty, no controls added
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
    }
}