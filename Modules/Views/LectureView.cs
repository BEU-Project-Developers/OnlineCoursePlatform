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

            // Dispose of or stop any existing content in the panel
            foreach (Control control in playerPanel.Controls)
            {
                control.Dispose(); // Dispose any active controls
            }
            playerPanel.Controls.Clear(); // Clear controls only after disposing


            if (!string.IsNullOrEmpty(lectureLink))
            {
                try
                {
                    if (Uri.IsWellFormedUriString(lectureLink, UriKind.Absolute))
                    {
                        // Create a WebView2 panel to display the demo HTML content
                        var webView2Panel = new Panel
                        {
                            Dock = DockStyle.Fill,
                            BackColor = Color.White
                        };
                        playerPanel.Controls.Add(webView2Panel);

                        // Create WebView2 control
                        var webView2 = new Microsoft.Web.WebView2.WinForms.WebView2
                        {
                            Dock = DockStyle.Fill
                        };

                        // Initialize WebView2
                        webView2Panel.Controls.Add(webView2);

                        // Ensure WebView2 is initialized
                        await webView2.EnsureCoreWebView2Async();

                        // Set the source URL of the WebView2 to your HTML file path
                        //MessageBox.Show(Application.StartupPath);
                        //string htmlFilePath = Path.Combine(Application.StartupPath, "Modules","WebPlayer", "player.html");
                        string htmlFilePath = "C:\\Users\\prabe\\Documents\\Class\\3rd Year\\Modern Programmin Language - 1\\Project\\Prabesh Academy\\Modules\\WebPlayer\\player.html";
                        // Make sure the paths are correctly set to the files
                        string parameters = $"?videoUrl={Uri.EscapeDataString(lectureLink)}";

                        if (File.Exists(htmlFilePath))
                        {
                            webView2.Source = new Uri($"file:///{htmlFilePath}{parameters}");
                        }
                        else
                        {
                            MessageBox.Show("HTML file not found!");
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