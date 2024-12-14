using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;

namespace Prabesh_Academy.Modules.Views
{
    public partial class LectureView : UserControl
    {

        private int contentId;

        public LectureView(Main mainFormInstance, int contentId)
        {
            //this.FormBorderStyle = FormBorderStyle.None;

            if (mainFormInstance != null)
                mainFormInstance.Controls.Clear();

            this.Dock = DockStyle.Fill; // Ensure the form fills its parent

            this.contentId = contentId; // Initialize contentId

            InitializeComponent(); // Initialize custom layout
            LoadLectures(); // Load the lectures when the form is initialized
        }


        private void LoadLectures()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["pAcademyCourses"].ConnectionString;
            string query = "SELECT * FROM Lectures WHERE content_id = @ContentId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@ContentId", contentId); // Pass the contentId as a parameter
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Clear existing rows in the playlist
                    playlistPanel.Controls.Clear();
                    playlistPanel.RowCount = 0;

                    // Populate playlist with lecture titles
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int rowIndex = playlistPanel.RowCount++;
                        playlistPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                        // Create a label for each lecture title
                        Label lectureLabel = new Label
                        {
                            Text = row["title"].ToString(),
                            Dock = DockStyle.Top,
                            Font = new Font("Arial", 12),
                            Padding = new Padding(10),
                            Tag = row["lecture_id"], // Store lecture_id for future use
                            Cursor = Cursors.Hand,
                            AutoSize = true
                        };

                        // Add hover effect
                        lectureLabel.MouseEnter += (s, e) => { lectureLabel.BackColor = Color.LightGray; };
                        lectureLabel.MouseLeave += (s, e) => { lectureLabel.BackColor = Color.White; };

                        // Add click event (placeholder for future functionality)
                        lectureLabel.Click += (s, e) =>
                        {
                            int lectureId = (int)((Label)s).Tag;
                            MessageBox.Show($"Lecture clicked: ID = {lectureId}");
                        };

                        // Add the label to the playlist panel
                        playlistPanel.Controls.Add(lectureLabel, 0, rowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading lectures: " + ex.Message);
            }
        }
    }
}
