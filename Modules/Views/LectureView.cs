using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Prabesh_Academy.Modules.Authentication;

namespace Prabesh_Academy.Modules.Views
{
    public partial class LectureView : UserControl
    {
        private int contentId;
        private string ApiBaseUrl = ConfigurationManager.ConnectionStrings["ApiBaseUrl"].ConnectionString;
        string JWTtoken = TokenManager.JWTToken;

        public LectureView(Main mainFormInstance, int contentId)
        {
            if (mainFormInstance != null)
                mainFormInstance.Controls.Clear();

            this.Dock = DockStyle.Fill;
            this.contentId = contentId;
            //MessageBox.Show($"{ contentId}");
            InitializeComponent();
            LoadLectures();
        }
        private async void LoadLectures()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);
                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/lectures/{contentId}");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<dynamic> lectures = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);

                    //MessageBox.Show(jsonResponse.ToString());

                    // Clear existing rows in the playlist
                    playlistPanel.Controls.Clear();
                    playlistPanel.RowCount = 0;

                    // Populate playlist with lecture titles
                    foreach (var lecture in lectures)
                    {
                        int rowIndex = playlistPanel.RowCount++;
                        playlistPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                        // Create a label for each lecture title
                        Label lectureLabel = new Label
                        {
                            Text = (string)lecture.title,
                            Dock = DockStyle.Top,
                            Font = new Font("Arial", 12),
                            Padding = new Padding(10),
                            Tag = (int)lecture.lecture_id, // Store lecture_id for future use
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
                            ShowLectureContent(lectureId, (string)lecture.lecture_link, (string)lecture.content);

                        };

                        // Add the label to the playlist panel
                        playlistPanel.Controls.Add(lectureLabel, 0, rowIndex);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Http error : {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void ShowLectureContent(int lectureId, string lectureLink, string content)
        {
            //  Load the lecture content into a panel and add it to form
            playerPanel.Controls.Clear();
            //if not a url then just add text
            if (!string.IsNullOrEmpty(lectureLink))
            {
                try
                {
                    WebBrowser webBrowser = new WebBrowser
                    {
                        Dock = DockStyle.Fill,
                        Url = new Uri(lectureLink)
                    };
                    playerPanel.Controls.Add(webBrowser);
                }
                catch (Exception)
                {
                    Label contentLabel = new Label
                    {
                        Text = content,
                        Dock = DockStyle.Fill,
                        Font = new Font("Arial", 10),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    playerPanel.Controls.Add(contentLabel);
                }
            }
            else
            {
                Label contentLabel = new Label
                {
                    Text = content,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 10),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                playerPanel.Controls.Add(contentLabel);
            }
        }


    }
}