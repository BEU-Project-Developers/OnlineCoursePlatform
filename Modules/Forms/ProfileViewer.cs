using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prabesh_Academy.Modules.Forms
{
    public partial class ProfileViewer : Form
    {
        private readonly string _apiBaseUrl; // You'll need to initialize this properly
        private readonly string _jwtToken;   // You'll need to initialize this properly
        private PictureBox profilePictureBox;
        private Label usernameLabel;
        private Label fullNameLabel;
        private Label bioLabel;
        private FlowLayoutPanel detailsPanel;

        public ProfileViewer()
        {
            // Initialize _apiBaseUrl and _jwtToken from your configuration or TokenManager
            _apiBaseUrl = System.Configuration.ConfigurationManager.ConnectionStrings["ApiBaseUrl"]?.ConnectionString ?? "YOUR_API_BASE_URL"; // Replace with your actual base URL
            _jwtToken = Authentication.TokenManager.JWTToken ?? "YOUR_JWT_TOKEN"; // Replace with your actual token retrieval
            InitializeComponent();
        }

        private async void ProfileViewer_Load(object sender, EventArgs e)
        {
            try
            {
                var activeUserData = await GetActiveUserDataAsync();

                if (activeUserData != null)
                {
                    DisplayUserData(activeUserData);
                }
                else
                {
                    MessageBox.Show("No user data retrieved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async Task<UserData> GetActiveUserDataAsync()
        {
            using var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _jwtToken);

            string url = $"{_apiBaseUrl}/activeUser";
            System.Net.Http.HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(responseJson);
                return userData;
            }
            else
            {
                // Handle error cases more gracefully, maybe log the error and show a user-friendly message
                Console.WriteLine($"Error from API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                return null; // Or throw an exception if you want to handle errors in the caller
            }
        }


        private void DisplayUserData(UserData userData)
        {
            if (userData == null) return;

            try
            {
                if (!string.IsNullOrEmpty(userData.ProfilePic))
                {
                    profilePictureBox.Load(userData.ProfilePic);
                }
                else
                {
                    profilePictureBox.Image = SystemIcons.WinLogo.ToBitmap(); // Default user icon
                }
            }
            catch (Exception ex)
            {
                profilePictureBox.Image = SystemIcons.Error.ToBitmap();
                Console.WriteLine($"Error loading profile picture: {ex.Message}");
            }

            usernameLabel.Text = "@" + userData.Username;
            fullNameLabel.Text = $"{userData.FirstName} {userData.LastName}";
            bioLabel.Text = userData.Bio;

            detailsPanel.Controls.Clear();
            AddDetailRow("Email", userData.Email);
            AddDetailRow("Course Level", userData.CourseLevel);
            AddDetailRow("Group Level", userData.GroupLevel);
            AddDetailRow("Subscription", userData.SubscriptionStatus);
            AddDetailRow("User ID", userData.UserID.ToString());
            AddDetailRow("Created At", userData.CreatedAt);
            AddDetailRow("Updated At", userData.UpdatedAt);
            AddDetailRow("Initialized", userData.Initialized.ToString());
            // Add more details as needed
        }

        private void AddDetailRow(string labelText, string valueText)
        {
            Label label = new Label();
            label.Text = $"{labelText}:";
            label.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            label.AutoSize = true;
            label.Margin = new Padding(0, 2, 5, 2); // Adjusted spacing

            Label value = new Label();
            value.Text = valueText;
            value.Font = new Font("Segoe UI", 9);
            value.AutoSize = true;
            value.Margin = new Padding(0, 2, 0, 2); // Adjusted spacing

            FlowLayoutPanel rowPanel = new FlowLayoutPanel();
            rowPanel.AutoSize = true;
            rowPanel.FlowDirection = FlowDirection.LeftToRight;
            rowPanel.Controls.Add(label);
            rowPanel.Controls.Add(value);

            detailsPanel.Controls.Add(rowPanel);
        }


        private void InitializeComponent()
        {
            this.profilePictureBox = new PictureBox();
            this.usernameLabel = new Label();
            this.fullNameLabel = new Label();
            this.bioLabel = new Label();
            this.detailsPanel = new FlowLayoutPanel();
            this.SuspendLayout();

            //
            // profilePictureBox
            //
            this.profilePictureBox.BackColor = Color.LightGray;
            this.profilePictureBox.BorderStyle = BorderStyle.FixedSingle;
            this.profilePictureBox.Location = new Point(25, 25);
            this.profilePictureBox.Name = "profilePictureBox";
            this.profilePictureBox.Size = new Size(150, 150);
            this.profilePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.profilePictureBox.TabIndex = 0;
            this.profilePictureBox.TabStop = false;

            //
            // usernameLabel
            //
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.usernameLabel.ForeColor = Color.DimGray;
            this.usernameLabel.Location = new Point(200, 25);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new Size(110, 25);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "@username";

            //
            // fullNameLabel
            //
            this.fullNameLabel.AutoSize = true;
            this.fullNameLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.fullNameLabel.Location = new Point(200, 55);
            this.fullNameLabel.Name = "fullNameLabel";
            this.fullNameLabel.Size = new Size(138, 32);
            this.fullNameLabel.TabIndex = 2;
            this.fullNameLabel.Text = "Full Name";

            //
            // bioLabel
            //
            this.bioLabel.BorderStyle = BorderStyle.FixedSingle;
            this.bioLabel.Font = new Font("Segoe UI", 10F);
            this.bioLabel.Location = new Point(200, 100);
            this.bioLabel.Name = "bioLabel";
            this.bioLabel.Padding = new Padding(10);
            this.bioLabel.Size = new Size(400, 100); // Adjusted width
            this.bioLabel.TabIndex = 3;
            this.bioLabel.Text = "User bio will be displayed here. A short description about the user.";
            this.bioLabel.TextAlign = ContentAlignment.TopLeft;

            //
            // detailsPanel
            //
            this.detailsPanel.AutoSize = true;
            this.detailsPanel.FlowDirection = FlowDirection.TopDown;
            this.detailsPanel.Location = new Point(25, 220);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new Size(500, 200); // Adjusted size, will grow with content
            this.detailsPanel.TabIndex = 4;

            //
            // ProfileViewer
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke; // Light background color
            this.ClientSize = new Size(700, 500); // Adjusted form size
            this.Controls.Add(this.detailsPanel);
            this.Controls.Add(this.bioLabel);
            this.Controls.Add(this.fullNameLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.profilePictureBox);
            this.Font = new Font("Segoe UI", 9F); // Consistent font for the form
            this.Name = "ProfileViewer";
            this.Padding = new Padding(20); // Form padding for cleaner look
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "User Profile";
            this.Load += new EventHandler(this.ProfileViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.profilePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        public class UserData
        {
            public string Bio { get; set; }
            public string CourseLevel { get; set; }
            public string CreatedAt { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string GroupLevel { get; set; }
            public bool Initialized { get; set; }
            public string LastName { get; set; }
            public string ProfilePic { get; set; }
            public string SubscriptionStatus { get; set; }
            public string UpdatedAt { get; set; }
            public int UserID { get; set; }
            public string Username { get; set; }
        }
    }
}