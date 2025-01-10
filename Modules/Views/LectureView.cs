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
using System.Windows.Forms.Integration;
using System.Runtime.InteropServices;


namespace Prabesh_Academy.Modules.Views
{
    public partial class LectureView : System.Windows.Forms.UserControl
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

        private Panel videoPanel;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        private async void ShowLectureContent(int lectureId, string lectureLink, string content)
        {
            playerPanel.Controls.Clear();

            if (!string.IsNullOrEmpty(lectureLink))
            {
                try
                {
                    if (Uri.IsWellFormedUriString(lectureLink, UriKind.Absolute))
                    {
                        // Create video panel
                        videoPanel = new Panel
                        {
                            Dock = DockStyle.Fill,
                            BackColor = Color.Black
                        };
                        playerPanel.Controls.Add(videoPanel);

                        // Launch the default media player in a new process
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = "mswindowsmusic:";  // For Windows 11 Media Player
                        process.StartInfo.Arguments = "http://127.0.0.1:5000/stream?file_path=./assets/hehe.webm";
                        //process.StartInfo.Arguments = $"?playlisttype=video&url={Uri.EscapeDataString(lectureLink)}";

                        process.StartInfo.UseShellExecute = true;

                        try
                        {
                            process.Start();
                        }
                        catch
                        {
                            // Fallback to legacy method if modern method fails
                            process.StartInfo.FileName = "wmplayer.exe";
                            process.StartInfo.Arguments = $"\"{lectureLink}\"";
                            process.Start();
                        }

                        // Optional: Set the media player window as a child of your form
                        await Task.Delay(1000); // Give the player time to start
                        if (!process.HasExited && process.MainWindowHandle != IntPtr.Zero)
                        {
                            SetParent(process.MainWindowHandle, videoPanel.Handle);
                        }
                    }
                    else
                    {
                        ShowMessage("Invalid video link provided.");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Error playing video: {ex.Message}");
                }
            }
            else if (!string.IsNullOrEmpty(content))
            {
                ShowMessage(content);
            }
        }

        private void ShowMessage(string message)
        {
            var label = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true
            };
            playerPanel.Controls.Add(label);
        }

    }

}