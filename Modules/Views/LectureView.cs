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
                    MessageBox.Show(jsonResponse);

                    //MessageBox.Show(jsonResponse.ToString());

                    playerPanel.Controls.Clear(); // Clear controls only after disposing
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

                    // Ensure WebView2 is initialized
                    await webView2.EnsureCoreWebView2Async();

                    // Set the source URL of the WebView2 to your HTML file path
                    //MessageBox.Show(Application.StartupPath);
                    //string htmlFilePath = Path.Combine(Application.StartupPath, "Modules","WebPlayer", "player.html");
                    string htmlFilePath = "C:\\Users\\prabe\\Documents\\Class\\3rd Year\\Modern Programmin Language - 1\\Project\\Prabesh Academy\\Modules\\WebPlayer\\player.html";
                    // Make sure the paths are correctly set to the files

                    if (File.Exists(htmlFilePath))
                    {
                        string htmlContent = File.ReadAllText(htmlFilePath);
                        string serializedLectures = JsonConvert.SerializeObject(lectures);
                        string scriptToInject = $"<script>var lectureData = {serializedLectures}; initPlaylist();</script>";
                        htmlContent = htmlContent.Replace("</body>", scriptToInject + "</body>");
                        webView2.Source = new Uri($"file:///{htmlFilePath}");
                    }
                    else
                    {
                        MessageBox.Show("HTML file not found!");
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

     
    }

}