using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using Prabesh_Academy.Modules.Authentication;
using System.Web;

namespace Prabesh_Academy.Modules.Views
{
    public partial class LectureView : UserControl
    {
        private int contentId;
        private string ApiBaseUrl = ConfigurationManager.ConnectionStrings["ApiBaseUrl"].ConnectionString;
        string JWTtoken = TokenManager.JWTToken;
        private WebView2 webView2;
        private Main mainforminst;

        public LectureView(Main mainFormInstance, int contentId)
        {
            if (mainFormInstance != null)
            {
                mainforminst = mainFormInstance;
                mainFormInstance.Controls.Clear();
            }

            this.Dock = DockStyle.Fill;
            this.contentId = contentId;
            InitializeComponent();

            // Initialize WebView2
            webView2 = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webView2);

            // Initialize WebView2 and then load lectures
            webView2.CoreWebView2InitializationCompleted += WebView2_CoreWebView2InitializationCompleted;
            InitializeWebView2Async();
        }


        private async void InitializeWebView2Async()
        {
            await webView2.EnsureCoreWebView2Async();
        }

        private async void WebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                await LoadLectures();
            }
            else
            {
                MessageBox.Show($"WebView2 initialization failed: {e.InitializationException?.Message}");
            }
        }



        private async Task LoadLectures()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);
                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/lectures/{contentId}");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Path to the HTML file
                    string htmlFilePath = "C:\\Users\\prabe\\Documents\\Class\\3rd Year\\Modern Programmin Language - 1\\Project\\Prabesh Academy\\Modules\\WebPlayer\\player.html";

                    // Serialize the JSON response
                    string jsonString = jsonResponse;

                    // Make sure the paths are correctly set to the files
                    string parameters = $"?lecture_list={Uri.EscapeDataString(jsonString)}";

                    //MessageBox.Show(parameters);
                    if (File.Exists(htmlFilePath))
                    {
                        webView2.Source = new Uri($"file:///{htmlFilePath}{parameters}");
                    }
                    else
                    {
                        MessageBox.Show("HTML file not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}