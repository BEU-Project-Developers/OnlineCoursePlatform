using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prabesh_Academy.Modules.Authentication;
using Svg;

namespace Prabesh_Academy.Modules.Views
{
    public partial class Course_Home : UserControl
    {
        private Main mainFormInstance;
        private Stack<NavigationState> _navigationStack = new Stack<NavigationState>();
        private string ApiBaseUrl = ConfigurationManager.ConnectionStrings["ApiBaseUrl"].ConnectionString;
        string JWTtoken = TokenManager.JWTToken;

        public Course_Home(Main mainFormArg)
        {
            mainFormInstance = mainFormArg;
            InitializeComponent();
            LoadBackButtonImage();

            this.Dock = DockStyle.Fill;
            if (mainFormInstance != null)
            {
                mainFormInstance.Controls.Clear();
                mainFormInstance.Controls.Add(this);
            }
            LoadData();

        }
        private void LoadBackButtonImage()
        {
            string svgContent = @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" fill=""orange""><path d=""M10 20A10 10 0 1 0 0 10a10 10 0 0 0 10 10zm1.289-15.7 1.422 1.4-4.3 4.344 4.289 4.245-1.4 1.422-5.714-5.648z""/></svg>";
            this.back_button.BackgroundImage = SvgToBitmap(svgContent);
            this.back_button.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private Bitmap SvgToBitmap(string svgContent)
        {
            using (var ms = new MemoryStream())
            {
                return SvgDocument.FromSvg<SvgDocument>(svgContent).Draw();
            }
        }

        private async Task LoadData()
        {
            await LoadEducationalLevels();
        }

        private async Task LoadEducationalLevels()
        {
            _navigationStack.Clear();
            _navigationStack.Push(new NavigationState { LevelId = null, GroupId = null, CourseId = null, SubjectId = null, ParentId = null });
            await LoadDataFromApi();
        }

        private async Task LoadGroupsForLevel(int levelId)
        {
            _navigationStack.Push(new NavigationState { LevelId = levelId, GroupId = null, CourseId = null, SubjectId = null, ParentId = null });
            await LoadDataFromApi(levelId: levelId);
        }

        private async Task LoadCoursesForGroup(int groupId)
        {
            int? levelId = _navigationStack.Peek().LevelId;
            _navigationStack.Push(new NavigationState { LevelId = levelId, GroupId = groupId, CourseId = null, SubjectId = null, ParentId = null });
            await LoadDataFromApi(levelId: levelId, groupId: groupId);
        }

        private async Task ShowCourseDetails(int courseId)
        {
            int? levelId = _navigationStack.Peek().LevelId;
            int? groupId = _navigationStack.Peek().GroupId;
            _navigationStack.Push(new NavigationState { LevelId = levelId, GroupId = groupId, CourseId = courseId, SubjectId = null, ParentId = null });
            await LoadDataFromApi(levelId: levelId, groupId: groupId, courseId: courseId);
        }

        private async Task ShowSubjectDetails(int subjectId)
        {
            int? levelId = _navigationStack.Peek().LevelId;
            int? groupId = _navigationStack.Peek().GroupId;
            int? courseId = _navigationStack.Peek().CourseId;
            _navigationStack.Push(new NavigationState { LevelId = levelId, GroupId = groupId, CourseId = courseId, SubjectId = subjectId, ParentId = null });
            await LoadDataFromApi(levelId: levelId, groupId: groupId, courseId: courseId, subjectId: subjectId);
        }

        private async Task LoadContentChildren(int subjectId, int? parentId)
        {
            int? levelId = _navigationStack.Peek().LevelId;
            int? groupId = _navigationStack.Peek().GroupId;
            int? courseId = _navigationStack.Peek().CourseId;
            _navigationStack.Push(new NavigationState { LevelId = levelId, GroupId = groupId, CourseId = courseId, SubjectId = subjectId, ParentId = parentId });
            await LoadDataFromApi(levelId: levelId, groupId: groupId, courseId: courseId, subjectId: subjectId, parentId: parentId);
        }

        private async Task LoadDataFromApi(int? levelId = null, int? groupId = null, int? courseId = null, int? subjectId = null, int? parentId = null)
        {
            flowLayoutPanel1.Controls.Clear();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);

                    string url = $"{ApiBaseUrl}/allAvailableCourses?";
                    if (levelId.HasValue) url += $"level_id={levelId}&";
                    if (groupId.HasValue) url += $"group_id={groupId}&";
                    if (courseId.HasValue) url += $"course_id={courseId}&";
                    if (subjectId.HasValue) url += $"subject_id={subjectId}&";
                    if (parentId.HasValue) url += $"parent_id={parentId}&";

                    if (url.EndsWith("&"))
                    {
                        url = url.TrimEnd('&');
                    }

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    if (result is JArray jArrayResult)
                    {
                        // Handle lists of items (levels, groups, courses, subjects)
                        foreach (var item in jArrayResult)
                        {
                            string type = item["type"]?.ToString().ToLower() ?? (subjectId.HasValue ? "content" : "");
                            string description = item["description"]?.ToString();
                            AddCard((int)item["id"], (string)item["name"], (string)item["svg"], (int)item["progress"], type, description);
                        }
                    }
                    else if (result is JObject jObjectResult)
                    {
                        if (subjectId.HasValue && jObjectResult.ContainsKey("contents"))
                        {
                            DisplayContentHierarchy(JsonConvert.DeserializeObject<List<dynamic>>(jObjectResult["contents"].ToString()));
                        }
                        else if (jObjectResult.ContainsKey("type") && jObjectResult["type"] != null && jObjectResult["type"].ToString() == "subject_id")
                        {
                            await ShowSubjectDetails((int)jObjectResult["data"]);
                        }
                        else if (jObjectResult.ContainsKey("contents")) // Check for direct content
                        {
                            DisplayContentHierarchy(JsonConvert.DeserializeObject<List<dynamic>>(jObjectResult["contents"].ToString()));
                        }
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

        private void DisplayContentHierarchy(List<dynamic> contentList)
        {
            foreach (var content in contentList)
            {
                DisplayContentCard(content);
            }

        }

        private void DisplayContentCard(dynamic content)
        {
            // Create the main card panel
            Panel contentCard = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 90) / 3, 120),
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = content.content_id,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            contentCard.Cursor = Cursors.Hand; // Set cursor for the entire card

            // SVG display area
            PictureBox svgPictureBox = new PictureBox
            {
                Size = new Size(contentCard.Width / 4, contentCard.Height - 20), // SVG takes 25% width
                Location = new Point(10, 10),
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Render the SVG data
            try
            {
                using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes((string)content.svg)))
                {
                    svgPictureBox.Image = SvgDocument.Open<SvgDocument>(stream).Draw();
                }
            }
            catch
            {
                svgPictureBox.BackColor = Color.Red; // Error background if SVG fails
            }

            contentCard.Controls.Add(svgPictureBox);

            // Text display and progress panel
            Panel textPanel = new Panel
            {
                Location = new Point(svgPictureBox.Width + 20, 10),
                Size = new Size(contentCard.Width - svgPictureBox.Width - 30, contentCard.Height - 20),
                BackColor = Color.Transparent
            };

            // Title label
            Label contentLabel = new Label
            {
                Text = (string)content.content_name,
                TextAlign = ContentAlignment.TopLeft,
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = false,
                MaximumSize = new Size(textPanel.Width, 40), // Wrap text if needed
                Size = new Size(textPanel.Width, 40),
                Location = new Point(0, 0)
            };
            textPanel.Controls.Add(contentLabel);

            // Progress bar
            ProgressBar progressBar = new ProgressBar
            {
                Value = (int)content.progress,
                Maximum = 100,
                Minimum = 0,
                Size = new Size(textPanel.Width, 20),
                Location = new Point(0, contentLabel.Bottom + 10)
            };
            textPanel.Controls.Add(progressBar);

            contentCard.Controls.Add(textPanel);

            // Click event handler
            contentCard.Click += async (sender, e) =>
            {
                int? contentId = null;
                if (content.content_id != null)
                {
                    contentId = (int)content.content_id;
                }

                if (content.ContainsKey("containsChildren") && content.containsChildren == true)
                {
                    // Load children of the current content
                    await LoadContentChildren(_navigationStack.Peek().SubjectId.Value, contentId);
                }
                else
                {
                    if (contentId.HasValue)
                    {
                        // Navigate to LectureView
                        LectureView lectureView = new LectureView(mainFormInstance, contentId.Value) { Dock = DockStyle.Fill };
                        mainFormInstance.Controls.Clear();
                        mainFormInstance.Controls.Add(lectureView);
                    }
                    else
                    {
                        MessageBox.Show("Invalid Content Id");
                    }
                }
            };

            contentCard.MouseHover += ContentCard_MouseHover;
            contentCard.MouseLeave += ContentCard_MouseLeave;

            // Add the card to the flow layout panel
            flowLayoutPanel1.Controls.Add(contentCard);
        }
        private void ContentCard_MouseHover(object? sender, EventArgs e)
        {
            if (sender is Panel contentCard)
            {
                contentCard.BackColor = Color.DarkGray; // Example hover effect
            }
        }

        private void ContentCard_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Panel contentCard)
            {
                contentCard.BackColor = Color.LightGray; // Reset background color
            }
        }

        private void AddCard(int id, string name, string svgData, int progressPercent, string type, string description = null)
        {
            // Card panel setup
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 90) / 3, 120),
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = id,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Cursor = Cursors.Hand; // Set cursor for the entire card

            // SVG display area
            PictureBox svgPictureBox = new PictureBox
            {
                Size = new Size(card.Width / 4, card.Height - 20), // SVG takes up 25% of the card width
                Location = new Point(10, 10),
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom // Ensures the SVG fits nicely
            };

            // Render SVG data
            try
            {
                using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(svgData)))
                {
                    svgPictureBox.Image = SvgDocument.Open<SvgDocument>(stream).Draw();
                }
            }
            catch
            {
                svgPictureBox.BackColor = Color.Red; // Show an error background if SVG fails
            }

            card.Controls.Add(svgPictureBox);

            // Text display area (title and progress)
            Panel textPanel = new Panel
            {
                Location = new Point(svgPictureBox.Width + 20, 10),
                Size = new Size(card.Width - svgPictureBox.Width - 30, card.Height - 20),
                BackColor = Color.Transparent
            };

            // Title label
            Label lblName = new Label
            {
                Text = $"{type.ToUpper()}: {name}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = false,
                MaximumSize = new Size(textPanel.Width, 0), // Wrap text
                Size = new Size(textPanel.Width, 40),
                Location = new Point(0, 0)
            };

            textPanel.Controls.Add(lblName);

            Point where_is_progress = new Point(0, lblName.Bottom + 10);

            // Optional description
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
                where_is_progress = new Point(0, lblDescription.Bottom + 10);
            }



            // Progress bar
            ProgressBar progressBar = new ProgressBar
            {
                Value = progressPercent,
                Maximum = 100,
                Minimum = 0,
                Size = new Size(textPanel.Width, 20),
                Location = where_is_progress
            };

            textPanel.Controls.Add(progressBar);

            card.Controls.Add(textPanel);

            // Click event handler for the card
            card.Click += async (sender, e) =>
            {
                switch (type.ToLower())
                {
                    case "level":
                        await LoadGroupsForLevel(id);
                        break;
                    case "group":
                        await LoadCoursesForGroup(id);
                        break;
                    case "course":
                        await ShowCourseDetails(id);
                        break;
                    case "subject":
                        await ShowSubjectDetails(id);
                        break;
                    case "content":
                        await LoadContentChildren(_navigationStack.Peek().SubjectId.Value, id);
                        break;
                }
            };

            card.MouseHover += ContentCard_MouseHover;
            card.MouseLeave += ContentCard_MouseLeave;

            // Add card to the flow layout panel
            flowLayoutPanel1.Controls.Add(card);
        }

        private async void BackButton_Click(object sender, EventArgs e)
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop(); // Remove the current state
                var previousState = _navigationStack.Peek();
                await LoadDataFromNavigationState(previousState);
            }
        }

        private async Task LoadDataFromNavigationState(NavigationState state)
        {
            await LoadDataFromApi(
                levelId: state.LevelId,
                groupId: state.GroupId,
                courseId: state.CourseId,
                subjectId: state.SubjectId,
                parentId: state.ParentId
            );
        }

        private class NavigationState
        {
            public int? LevelId { get; set; }
            public int? GroupId { get; set; }
            public int? CourseId { get; set; }
            public int? SubjectId { get; set; }
            public int? ParentId { get; set; }
        }
    }
}