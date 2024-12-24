using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prabesh_Academy.Modules.Authentication;

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
            this.Dock = DockStyle.Fill;
            if (mainFormInstance != null)
            {
                mainFormInstance.Controls.Clear();
                mainFormInstance.Controls.Add(this);
            }
            LoadData();

            // Subscribe to back button event
            this.back_button.Click += BackButton_Click;

        }

        private async void LoadData()
        {
            await LoadEducationalLevels();
        }

        private async Task LoadEducationalLevels()
        {
            _navigationStack.Clear();
            _navigationStack.Push(new NavigationState { LevelId = null, GroupId = null, CourseId = null, SubjectId = null, ParentId = null });

            flowLayoutPanel1.Controls.Clear();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);

                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/allAvailableCourses");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<dynamic> levels = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);


                    foreach (var level in levels)
                    {
                        AddCard((int)level.id, (string)level.name, (string)level.svg, (int)level.progress, "level");
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

        private async void LoadGroupsForLevel(int levelId)
        {

            _navigationStack.Push(new NavigationState { LevelId = levelId, GroupId = null, CourseId = null, SubjectId = null, ParentId = null });
            flowLayoutPanel1.Controls.Clear();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);

                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/allAvailableCourses?level_id={levelId}");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<dynamic> groups = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);


                    foreach (var group in groups)
                    {
                        AddCard((int)group.id, (string)group.name, (string)group.svg, (int)group.progress, "group");
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

        private async void LoadCoursesForGroup(int groupId)
        {
            _navigationStack.Push(new NavigationState { LevelId = _navigationStack.Peek().LevelId, GroupId = groupId, CourseId = null, SubjectId = null, ParentId = null });
            flowLayoutPanel1.Controls.Clear();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);

                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/allAvailableCourses?level_id={_navigationStack.Peek().LevelId}&group_id={groupId}");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<dynamic> courses = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);


                    foreach (var course in courses)
                    {
                        AddCard((int)course.id, (string)course.name, (string)course.svg, (int)course.progress, "course");
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
        private async void ShowCourseDetails(int courseId)
        {
            flowLayoutPanel1.Controls.Clear();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);

                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/allAvailableCourses?level_id={_navigationStack.Peek().LevelId}&group_id={_navigationStack.Peek().GroupId}&course_id={courseId}");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (result is Newtonsoft.Json.Linq.JObject && result.ContainsKey("type") && result.type == "subject_id")
                    {
                        _navigationStack.Push(new NavigationState { LevelId = _navigationStack.Peek().LevelId, GroupId = _navigationStack.Peek().GroupId, CourseId = courseId, SubjectId = (int)result.data, ParentId = null });
                        if (result.ContainsKey("contents"))
                        {
                            DisplayContentHierarchy(JsonConvert.DeserializeObject<List<dynamic>>(result.contents.ToString()));

                        }
                        else
                        {
                            ShowSubjectDetails((int)result.data);
                        }
                    }
                    else if (result is Newtonsoft.Json.Linq.JArray)
                    {
                        _navigationStack.Push(new NavigationState { LevelId = _navigationStack.Peek().LevelId, GroupId = _navigationStack.Peek().GroupId, CourseId = courseId, SubjectId = null, ParentId = null });

                        List<dynamic> subjects = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);
                        if (subjects.Count == 1)
                        {
                            ShowSubjectDetails((int)subjects[0].id);
                        }
                        else
                        {
                            foreach (var subject in subjects)
                            {
                                AddCard((int)subject.id, (string)subject.name, (string)subject.svg, (int)subject.progress, (string)subject.type, (string)subject.description);
                            }
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
        private async void ShowSubjectDetails(int subjectId)
        {
            _navigationStack.Push(new NavigationState { LevelId = _navigationStack.Peek().LevelId, GroupId = _navigationStack.Peek().GroupId, CourseId = _navigationStack.Peek().CourseId, SubjectId = subjectId, ParentId = null });
            flowLayoutPanel1.Controls.Clear();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);

                    HttpResponseMessage response = await client.GetAsync($"{ApiBaseUrl}/allAvailableCourses?level_id={_navigationStack.Peek().LevelId}&group_id={_navigationStack.Peek().GroupId}&course_id={_navigationStack.Peek().CourseId}&subject_id={subjectId}");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (result is Newtonsoft.Json.Linq.JArray)
                    {
                        DisplayContentHierarchy(JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse));
                    }
                    else if (result.ContainsKey("contents"))
                    {
                        DisplayContentHierarchy(JsonConvert.DeserializeObject<List<dynamic>>(result.contents.ToString()));
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
        private async void LoadContentChildren(int subjectId, int? parentId)
        {
            _navigationStack.Push(new NavigationState { LevelId = _navigationStack.Peek().LevelId, GroupId = _navigationStack.Peek().GroupId, CourseId = _navigationStack.Peek().CourseId, SubjectId = subjectId, ParentId = parentId });
            flowLayoutPanel1.Controls.Clear();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTtoken);

                    string url = $"{ApiBaseUrl}/allAvailableCourses?level_id={_navigationStack.Peek().LevelId}&group_id={_navigationStack.Peek().GroupId}&course_id={_navigationStack.Peek().CourseId}&subject_id={subjectId}";
                    if (parentId.HasValue)
                    {
                        url += $"&parent_id={parentId}";
                    }
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    if (result is Newtonsoft.Json.Linq.JArray)
                    {
                        DisplayContentHierarchy(JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse));
                    }
                    else if (result.ContainsKey("contents"))
                    {
                        DisplayContentHierarchy(JsonConvert.DeserializeObject<List<dynamic>>(result.contents.ToString()));
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
            Panel contentCard = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 60) / 3, 80),
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = content.content_id,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label contentLabel = new Label
            {
                Text = (string)content.content_name,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            contentCard.Controls.Add(contentLabel);
            flowLayoutPanel1.Controls.Add(contentCard);

            contentCard.Click += async (sender, e) =>
            {
                int? contentId = null;
                if (content.content_id != null)
                {
                    contentId = (int)content.content_id;
                }
                if (content.ContainsKey("containsChildren") && content.containsChildren == true)
                {
                    LoadContentChildren(_navigationStack.Peek().SubjectId.Value, contentId);
                }
                else
                {
                    if (contentId.HasValue)
                    {
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
        }
        private void AddCard(int id, string name, string svgData, int progressPercent, string type, string description = null)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 60) / 3, 80),
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = id,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblName = new Label
            {
                Text = $"{type.ToUpper()}: {name}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            card.Controls.Add(lblName);

            if (description != null)
            {
                Label lblDescription = new Label
                {
                    Text = $"Description: {description}",
                    Font = new Font("Arial", 9),
                    Location = new Point(10, 30),
                    AutoSize = true
                };
                card.Controls.Add(lblDescription);
            }
            card.Click += (sender, e) =>
            {
                switch (type)
                {
                    case "level":
                        LoadGroupsForLevel(id);
                        break;
                    case "group":
                        LoadCoursesForGroup(id);
                        break;
                    case "course":
                        ShowCourseDetails(id);
                        break;
                    case "subject":
                        ShowSubjectDetails(id);
                        break;
                }
            };
            flowLayoutPanel1.Controls.Add(card);
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop(); // Remove the current state
                var previousState = _navigationStack.Peek();
                if (previousState.SubjectId.HasValue)
                {
                    if (previousState.ParentId.HasValue)
                    {
                        LoadContentChildren(previousState.SubjectId.Value, previousState.ParentId);
                    }
                    else
                    {
                        ShowSubjectDetails(previousState.SubjectId.Value);
                    }
                }
                else if (previousState.CourseId.HasValue)
                {
                    ShowCourseDetails(previousState.CourseId.Value);
                }
                else if (previousState.GroupId.HasValue)
                {
                    LoadCoursesForGroup(previousState.GroupId.Value);
                }
                else if (previousState.LevelId.HasValue)
                {
                    LoadGroupsForLevel(previousState.LevelId.Value);
                }
                else
                {
                    LoadEducationalLevels();
                }
            }
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