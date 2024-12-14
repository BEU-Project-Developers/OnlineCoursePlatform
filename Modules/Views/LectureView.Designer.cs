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
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill, // Fill entire LectureView space
                Orientation = Orientation.Vertical,
                SplitterDistance = 600, // Adjust split distance for initial sizing
                IsSplitterFixed = false // Allow user to resize panels
            };

            // Player Panel (Left Panel)
            playerPanel = new Panel
            {
                Dock = DockStyle.Fill, // Fill the left panel
                BackColor = Color.Black // Placeholder background
            };

            // Placeholder for Media Player
            playerPlaceholder = new Label
            {
                Text = "Media Player",
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 18, FontStyle.Bold)
            };
            playerPanel.Controls.Add(playerPlaceholder);

            // Playlist Panel (Right Panel)
            playlistPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, // Fill the right panel
                AutoScroll = true, // Enable scrolling for large playlists
                ColumnCount = 1,
                BackColor = Color.White
            };
            playlistPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Add Panels to SplitContainer
            splitContainer.Panel1.Controls.Add(playerPanel);
            splitContainer.Panel2.Controls.Add(playlistPanel);

            // Add SplitContainer to the Form
            Controls.Add(splitContainer);
        }
        #endregion

        private SplitContainer splitContainer;
        private Panel playerPanel;
        private TableLayoutPanel playlistPanel;
        private Label playerPlaceholder;
    }
}