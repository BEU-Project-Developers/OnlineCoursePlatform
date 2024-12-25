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

namespace Prabesh_Academy.Modules.Views
{
    public partial class Course_Home : UserControl
    {
        private Main mainFormInstance;
        private readonly string _apiBaseUrl;
        private readonly string _jwtToken;
        private Stack<NavigationState> _navigationHistory = new Stack<NavigationState>();
        private int? _currentLevelId;
        private int? _currentGroupId;
        private int? _currentCourseId;
        private int? _currentSubjectId;
        private int? _currentParentId;

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
            string svgContent = @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" fill=""orange""><path d=""M10 20A10 10 0 1 0 0 10a10 10 0 0 0 10 10zm1.289-15.7 1.422 1.4-4.3 4.344 4.289 4.245-1.4 1.422-5.714-5.648z""/></svg>";
            this.back_button.BackgroundImage = SvgToBitmap(svgContent);
            this.back_button.BackgroundImageLayout = ImageLayout.Stretch;
            this.back_button.Visible = false;
        }

        private Bitmap SvgToBitmap(string svgContent)
        {
            using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(svgContent)))
            {
                return SvgDocument.Open<SvgDocument>(ms).Draw();
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
            _currentLevelId = levelId;
            _currentGroupId = groupId;
            _currentCourseId = courseId;
            _currentSubjectId = subjectId;
            _currentParentId = parentId;

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
                    foreach (var item in items)
                    {
                        AddCard(item.id, item.name, item.svg, item.progress, item.type, item.description);
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
        }

        private void AddCard(int id, string name, string svgData, int progressPercent, string type, string description = null)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width > 0 ? (flowLayoutPanel1.ClientSize.Width - 60) / 3 : 200), 120),
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = new CardInfo { Type = type.ToLower(), Id = id },
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
            if (sender is Panel card)
            {
                if (card.Tag is CardInfo tagInfo)
                {
                    string type = tagInfo.Type;
                    int id = tagInfo.Id;

                    _navigationHistory.Push(new NavigationState
                    {
                        LevelId = _currentLevelId,
                        GroupId = _currentGroupId,
                        CourseId = _currentCourseId,
                        SubjectId = _currentSubjectId,
                        ParentId = _currentParentId
                    });

                    switch (type)
                    {
                        case "level":
                            await LoadAvailableCourses(levelId: id);
                            break;
                        case "group":
                            await LoadAvailableCourses(levelId: _currentLevelId, groupId: id);
                            break;
                        case "course":
                            MessageBox.Show("Clicked on course?");
                            await LoadAvailableCourses(levelId: _currentLevelId, groupId: _currentGroupId, courseId: id);
                            break;
                        case "subject":
                            await ShowSubjectDetails(id);
                            break;
                        case "content":
                            await LoadContentChildren(_currentSubjectId.Value, id);
                            break;
                    }
                }
            }
        }

        private async Task ShowSubjectDetails(int subjectId)
        {
            // When showing subject details, we need the context of the current level, group, and course
            await LoadAvailableCourses(_currentLevelId, _currentGroupId, _currentCourseId, subjectId: subjectId);
        }

        private async Task LoadContentChildren(int subjectId, int? parentId)
        {
            if (string.IsNullOrEmpty(_apiBaseUrl) || string.IsNullOrEmpty(_jwtToken)) return;

            flowLayoutPanel1.Controls.Clear();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            // Ensure all necessary parameters are passed
            string url = $"{_apiBaseUrl}/allAvailableCourses?level_id={_currentLevelId}&group_id={_currentGroupId}&course_id={_currentCourseId}&subject_id={subjectId}";
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
                DisplayContentCard(content);
            }
        }

        private void DisplayContentCard(CourseItem content)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width > 0 ? (flowLayoutPanel1.ClientSize.Width - 60) / 3 : 200), 80),
                BackColor = Color.AliceBlue,
                Margin = new Padding(10),
                Tag = new CardInfo { Type = "content", Id = content.id },
                Cursor = Cursors.Hand
            };

            Label nameLabel = new Label
            {
                Text = content.name,
                Font = new Font("Arial", 10),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(nameLabel);

            card.Click += Card_Click;
            flowLayoutPanel1.Controls.Add(card);
        }

        private class CardInfo
        {
            public string Type { get; set; }
            public int Id { get; set; }
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
            // Back button visibility is managed in LoadAvailableCourses method
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
    }

    internal class NavigationState
    {
        public int? LevelId { get; set; }
        public int? GroupId { get; set; }
        public int? CourseId { get; set; }
        public int? SubjectId { get; set; }
        public int? ParentId { get; set; }
    }
}