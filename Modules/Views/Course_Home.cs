using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Prabesh_Academy.Modules.Authentication;
using Svg;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Prabesh_Academy.Modules.Views
{
    public partial class Course_Home : UserControl
    {
        private Main mainFormInstance;
        private readonly string _apiBaseUrl;
        private readonly string _jwtToken;
        private Stack<NavigationState> _navigationHistory = new Stack<NavigationState>();
        private NavigationState _currentState = new NavigationState();

        public Course_Home(Main mainFormArg)
        {
            mainFormInstance = mainFormArg;
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            _apiBaseUrl = ConfigurationManager.ConnectionStrings["ApiBaseUrl"]?.ConnectionString;
            _jwtToken = TokenManager.JWTToken;

            LoadBackButtonImage();
            LoadAvailableCourses();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (mainFormInstance != null)
            {
                mainFormInstance.Controls.Clear();
                mainFormInstance.Controls.Add(this);
            }
        }

        private void LoadBackButtonImage()
        {
            // Consider using embedded resources for icons instead of string literals
            string backSvgContent = @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" fill=""orange""><path d=""M10 20A10 10 0 1 0 0 10a10 10 0 0 0 10 10zm1.289-15.7 1.422 1.4-4.3 4.344 4.289 4.245-1.4 1.422-5.714-5.648z""/></svg>";
            back_button.BackgroundImage = SvgToBitmap(backSvgContent);
            back_button.BackgroundImageLayout = ImageLayout.Stretch;
            back_button.Visible = false;

            string homeSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" xml:space=""preserve"" width=""2048"" height=""2048"" style=""shape-rendering:geometricPrecision;text-rendering:geometricPrecision;image-rendering:optimizeQuality;fill-rule:evenodd;clip-rule:evenodd""><defs><style>.fil6{fill:#616161}.fil7{fill:#689f38}.fil8{fill:#8bc34a}.fil1,.fil4{fill:#616161;fill-rule:nonzero}.fil1{fill:#d50000}</style></defs><g id=""Layer_x0020_1""><g id=""_361372472""><path style=""fill:#bdbdbd;fill-rule:nonzero"" d=""M1611 858v905h-125v-688h-376v688H424V793h1187z""/><path class=""fil1"" d=""m249 800 775-544 761 544z""/><path style=""fill:#c60404;fill-rule:nonzero"" d=""m249 800 775-544v544z""/><path style=""fill:#a1887f;fill-rule:nonzero"" d=""M986 1075H611v375h375z""/><path class=""fil4"" d=""M976 1085H620v356h356v-356zm-365-20h385v395H601v-395h10z""/><path class=""fil1"" d=""M1036 1066H562v31h474z""/><path style=""fill:#a1887f"" d=""M1110 1106v657h376v-688h-376z""/><path class=""fil4"" d=""M1120 1097v646h356v-667h-355v21zm-20 625v-666h395v707h-396v-41z""/><path class=""fil6"" d=""M494 1741h1140v51H494z""/><path class=""fil7"" d=""M442 1689h-92c-28 0-51 23-51 51v51h195v-51c0-28-23-51-51-51zM674 1730h-56c-17 0-31 14-31 31v31h117v-31c0-17-14-31-31-31z""/><path class=""fil8"" d=""M575 1657H454c-37 0-67 30-67 67v62c0 3 2 5 5 5h250v-67c0-37-30-67-67-67z""/><path class=""fil7"" d=""M1594 1657h44c37 0 67 30 67 67v32c0 19-16 35-35 35h-143v-67c0-37 30-67 67-67z""/><path class=""fil8"" d=""M1655 1689h92c28 0 51 23 51 51v51h-195v-51c0-28 23-51 51-51z""/><path style=""fill:#757575"" d=""M1101 1763h402v29h-402z""/><circle class=""fil6"" cx=""1184"" cy=""1408"" r=""32""/></g><path style=""fill:none"" d=""M0 0h2048v2048H0z""/></g></svg>";
            home_button.BackgroundImage = SvgToBitmap(homeSvg);
            home_button.BackgroundImageLayout = ImageLayout.Stretch;
            home_button.Visible = false;

            string refreshSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" xml:space=""preserve"" width=""2048"" height=""2048"" style=""shape-rendering:geometricPrecision;text-rendering:geometricPrecision;image-rendering:optimizeQuality;fill-rule:evenodd;clip-rule:evenodd""><defs><style>.fil0{fill:none}</style></defs><g id=""Layer_x0020_1""><path class=""fil0"" d=""M0 0h2048v2048H0z""/><path class=""fil0"" d=""M255.999 255.999h1536v1536h-1536z""/><path class=""fil0"" d=""M255.999 256h1536v1536h-1536z""/><path d=""M673.842 674.712c169.51-169.716 428.821-193.899 624.831-73.916-66.458 65.674-142.934 142.486-142.934 142.486-53.806 64.347 6.448 101.463 37.116 100.397H1689.367c18.8 0 34.219-15.418 34.219-34.48v-492.44c2.318-44.817-47.134-88.605-98.36-38.37 0 0-84.456 82.42-142.673 140.114-298.464-217.915-719.041-193.19-988.706 76.213-157.38 157.38-230.491 365.988-221.726 572.018h254.694c-9.027-140.636 39.433-284.355 147.026-392.022z"" style=""fill:#00897b""/><path d=""M1521.18 981.213c9.027 140.896-39.434 284.56-147.026 392.228-169.772 169.454-428.821 193.712-624.831 73.915 66.458-65.673 142.952-142.41 142.952-142.41 53.525-64.421-6.729-101.463-37.135-100.416H358.628c-19.063 0-34.48 15.418-34.48 34.219v492.439c-2.057 45.022 47.394 88.605 98.36 38.369 0 0 84.455-82.158 142.934-140.112 298.483 218.176 719.041 193.45 988.444-76.214 157.381-157.101 230.753-365.97 221.988-572.017H1521.18z"" style=""fill:#4db6ac""/></g></svg>";
            refresh_button.BackgroundImage = SvgToBitmap(refreshSvg);
            refresh_button.BackgroundImageLayout = ImageLayout.Stretch;

            string logoutSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 512 512"" xml:space=""preserve""><path fill=""#FFB0AA"" d=""m31 256 122 100v-50h122V206H153v-50z""/><path fill=""#6E83B7"" d=""M481 502H129V386h48v68h256V58H177v68h-48V10h352z""/></svg>";
            logout_button.BackgroundImage = SvgToBitmap(logoutSvg);
            logout_button.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private Bitmap SvgToBitmap(string svgContent)
        {
            using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(svgContent)))
            {
                return SvgDocument.Open<SvgDocument>(ms).Draw();
            }
        }

        private async void home_button_Click(object sender, EventArgs e)
        {
            _navigationHistory.Clear();
            _currentState = new NavigationState(); // Reset to initial state
            await LoadAvailableCourses();
        }

        private async void refresh_button_Click(object sender, EventArgs e)
        {
            await LoadAvailableCourses(_currentState.LevelId, _currentState.GroupId, _currentState.CourseId, _currentState.SubjectId, _currentState.ParentId);
        }

        private async void logout_button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout and relaunch?", "Logout and Relaunch Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                TokenManager.JWTToken = null;
                TokenManager.UpdateToken(null);

                if (mainFormInstance != null && !mainFormInstance.IsDisposed)
                {
                    mainFormInstance.Close();
                }
                Main newMainForm = new Main();
                newMainForm.Show();
            }
        }

        private async Task LoadAvailableCourses(int? levelId = null, int? groupId = null, int? courseId = null, int? subjectId = null, int? parentId = null)
        {
            if (string.IsNullOrEmpty(_apiBaseUrl))
            {
                MessageBox.Show("API Base URL is not configured.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(_jwtToken))
            {
                MessageBox.Show("JWT Token is missing. Please login.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            flowLayoutPanel1.Controls.Clear();
            _currentState.LevelId = levelId;
            _currentState.GroupId = groupId;
            _currentState.CourseId = courseId;
            _currentState.SubjectId = subjectId;
            _currentState.ParentId = parentId;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            string url = $"{_apiBaseUrl}/allAvailableCourses";
            var queryParams = new Dictionary<string, string>();

            if (levelId.HasValue) queryParams.Add("level_id", levelId.Value.ToString());
            if (groupId.HasValue && levelId.HasValue) queryParams.Add("group_id", groupId.Value.ToString());
            if (courseId.HasValue && groupId.HasValue && levelId.HasValue) queryParams.Add("course_id", courseId.Value.ToString());
            if (subjectId.HasValue && courseId.HasValue && groupId.HasValue && levelId.HasValue) queryParams.Add("subject_id", subjectId.Value.ToString());
            if (parentId.HasValue && subjectId.HasValue && courseId.HasValue && groupId.HasValue && levelId.HasValue) queryParams.Add("parent_id", parentId.Value.ToString());

            if (queryParams.Any())
            {
                url += "?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<CourseItem>>(jsonString);

                if (items != null)
                {
                    // Optimization: If only one item is returned, directly navigate deeper
                    if (items.Count == 1) //
                    {
                        //if (items.Count == 1 && levelId.HasValue == parentId.HasValue) // Check to prevent infinite loops, but it was useless maybe.

                            var item = items[0];
                        switch (item.type.ToLower())
                        {
                            case "level":
                                await LoadAvailableCourses(levelId: item.id);
                                break;
                            case "group":
                                await LoadAvailableCourses(levelId: _currentState.LevelId, groupId: item.id);
                                break;
                            case "course":
                                await LoadAvailableCourses(levelId: _currentState.LevelId, groupId: _currentState.GroupId, courseId: item.id);
                                break;
                            case "subject":
                                await LoadAvailableCourses(levelId: _currentState.LevelId, groupId: _currentState.GroupId, courseId: _currentState.CourseId, subjectId: item.id);
                                break;
                                // Consider if similar direct navigation is needed for content
                        }
                    }
                    else
                    {
                        foreach (var item in items)
                        {
                            AddCard(item.id, item.name, item.svg, item.progress, item.type, item.description, item.containsChildren);
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error communicating with the API: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing API response: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            back_button.Visible = _navigationHistory.Count > 0;
            home_button.Visible = back_button.Visible;
        }

        private void AddCard(int id, string name, string svgData, int progressPercent, string type, string description = null, bool containsChildren = true)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width > 0 ? (flowLayoutPanel1.ClientSize.Width - 60) / 3 : 200), 120),
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = new CardInfo { Type = type.ToLower(), Id = id, ContainsChildren = containsChildren },
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Cursor = Cursors.Hand;

            PictureBox svgPictureBox = new PictureBox
            {
                Size = new Size(card.Width / 4, card.Height - 20),
                Location = new Point(10, 10),
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            try
            {
                if (!string.IsNullOrWhiteSpace(svgData))
                {
                    using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(svgData)))
                    {
                        svgPictureBox.Image = SvgDocument.Open<SvgDocument>(stream).Draw();
                    }
                }
                else
                {
                    svgPictureBox.BackColor = Color.Gray;
                }
            }
            catch
            {
                svgPictureBox.BackColor = Color.Red;
            }
            card.Controls.Add(svgPictureBox);

            Panel textPanel = new Panel
            {
                Location = new Point(svgPictureBox.Width + 20, 10),
                Size = new Size(card.Width - svgPictureBox.Width - 30, card.Height - 20),
                BackColor = Color.Transparent
            };

            Label lblName = new Label
            {
                Text = $"{type.ToUpper()}: {name}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = false,
                MaximumSize = new Size(textPanel.Width, 0),
                Size = new Size(textPanel.Width, 40),
                Location = new Point(0, 0)
            };
            textPanel.Controls.Add(lblName);

            Point whereIsProgress = new Point(0, lblName.Bottom + 10);
            if (!string.IsNullOrWhiteSpace(description))
            {
                Label lblDescription = new Label
                {
                    Text = description,
                    Font = new Font("Arial", 9),
                    AutoSize = false,
                    MaximumSize = new Size(textPanel.Width, 0),
                    Size = new Size(textPanel.Width, 40),
                    Location = new Point(0, lblName.Bottom + 10)
                };
                textPanel.Controls.Add(lblDescription);
                whereIsProgress = new Point(0, lblDescription.Bottom + 10);
            }

            ProgressBar progressBar = new ProgressBar
            {
                Value = progressPercent,
                Maximum = 100,
                Minimum = 0,
                Size = new Size(textPanel.Width, 20),
                Location = whereIsProgress
            };
            textPanel.Controls.Add(progressBar);
            card.Controls.Add(textPanel);

            card.Click += Card_Click;
            card.MouseEnter += ContentCard_MouseHover;
            card.MouseLeave += ContentCard_MouseLeave;

            flowLayoutPanel1.Controls.Add(card);
        }

        private async void Card_Click(object sender, EventArgs e)
        {
            if (sender is Panel card && card.Tag is CardInfo tagInfo)
            {
                string type = tagInfo.Type;
                int id = tagInfo.Id;

                // Before navigating, push the current state
                _navigationHistory.Push((NavigationState)_currentState.Clone());

                switch (type)
                {
                    case "level":
                        await LoadAvailableCourses(levelId: id);
                        break;
                    case "group":
                        await LoadAvailableCourses(levelId: _currentState.LevelId, groupId: id);
                        break;
                    case "course":
                        await LoadAvailableCourses(levelId: _currentState.LevelId, groupId: _currentState.GroupId, courseId: id);
                        break;
                    case "subject":
                        await LoadAvailableCourses(_currentState.LevelId, _currentState.GroupId, _currentState.CourseId, subjectId: id);
                        break;
                    case "content":
                        if (!tagInfo.ContainsChildren)
                        {
                            if (_currentState.SubjectId.HasValue)
                            {
                                LectureView lectureView = new LectureView(mainFormInstance, id) { Dock = DockStyle.Fill };
                                mainFormInstance.Controls.Clear();
                                mainFormInstance.Controls.Add(lectureView);
                            }
                            else
                            {
                                MessageBox.Show("Subject ID is missing to load the lecture.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            await LoadContentChildren(_currentState.SubjectId.Value, id);
                        }
                        break;
                }
            }
        }

        private async Task ShowSubjectDetails(int subjectId)
        {
            await LoadAvailableCourses(_currentState.LevelId, _currentState.GroupId, _currentState.CourseId, subjectId: subjectId);
        }

        private async Task LoadContentChildren(int subjectId, int? parentId)
        {
            if (string.IsNullOrEmpty(_apiBaseUrl) || string.IsNullOrEmpty(_jwtToken)) return;

            flowLayoutPanel1.Controls.Clear();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            string url = $"{_apiBaseUrl}/allAvailableCourses?level_id={_currentState.LevelId}&group_id={_currentState.GroupId}&course_id={_currentState.CourseId}&subject_id={subjectId}";
            if (parentId.HasValue)
            {
                url += $"&parent_id={parentId.Value}";
            }

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var contentList = JsonConvert.DeserializeObject<List<CourseItem>>(jsonString);

                if (contentList != null)
                {
                    DisplayContentHierarchy(contentList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayContentHierarchy(List<CourseItem> contentList)
        {
            foreach (var content in contentList)
            {
                AddCard(content.id, content.name, content.svg, content.progress, content.type, containsChildren: content.containsChildren);
            }
        }

        private class CardInfo
        {
            public string Type { get; set; }
            public int Id { get; set; }
            public bool ContainsChildren { get; set; }
        }

        private void ContentCard_MouseHover(object sender, EventArgs e)
        {
            if (sender is Panel card)
            {
                card.BackColor = Color.CornflowerBlue;
            }
        }

        private void ContentCard_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Panel card)
            {
                card.BackColor = Color.LightGray;
            }
        }

        private async void BackButton_Click(object sender, EventArgs e)
        {
            if (_navigationHistory.Count > 0)
            {
                var previousState = _navigationHistory.Pop();
                await LoadAvailableCourses(
                    previousState.LevelId,
                    previousState.GroupId,
                    previousState.CourseId,
                    previousState.SubjectId,
                    previousState.ParentId
                );
            }
        }
    }

    public class CourseItem
    {
        public string type { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string svg { get; set; }
        public int progress { get; set; }
        public string description { get; set; }
        public int? parent_id { get; set; }
        public bool containsChildren { get; set; }
    }

    public class NavigationState : ICloneable
    {
        public int? LevelId { get; set; }
        public int? GroupId { get; set; }
        public int? CourseId { get; set; }
        public int? SubjectId { get; set; }
        public int? ParentId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}