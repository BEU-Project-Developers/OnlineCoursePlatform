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
using Prabesh_Academy.Modules.Forms;

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
            this.Controls.Clear();
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            _apiBaseUrl = ConfigurationManager.ConnectionStrings["ApiBaseUrl"]?.ConnectionString;
            _jwtToken = TokenManager.JWTToken;

            LoadBackButtonImage();
            //LoadAvailableCourses();
            CheckUserInitialization(); // Call the new method to check user initialization

        }


        private async void CheckUserInitialization()
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

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            try
            {
                var response = await client.GetAsync($"{_apiBaseUrl}/activeUser"); // Call /activeUser endpoint
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonString); // Deserialize to UserProfile class

                if (userProfile != null && !userProfile.Initialized)
                {
                    // User is NOT initialized, show initMyProfile form
                    this.Controls.Clear();
                    ShowInitMyProfileForm(userProfile);
                }
                else
                {
                    // User IS initialized, load courses as usual
                    LoadAvailableCourses();
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error communicating with the API (activeUser): {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing API response (activeUser): {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred (activeUser): {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowInitMyProfileForm(UserProfile userProfile)
        {
            // **Important:** Make sure you have a form named `initMyProfile` in your `Prabesh_Academy.Modules.Views` namespace.
            //                If it's named differently or in a different namespace, adjust accordingly.

            initMyProfile initProfileForm = new initMyProfile(mainFormInstance, userProfile); // Assuming your initMyProfile constructor takes 'Main' as argument
            mainFormInstance.Controls.Clear(); // Clear existing controls in MainForm
            mainFormInstance.Controls.Add(initProfileForm); // Add the initMyProfile form
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

            string userimgSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" xml:space=""preserve"" width=""2048"" height=""2048"" style=""shape-rendering:geometricPrecision;text-rendering:geometricPrecision;image-rendering:optimizeQuality;fill-rule:evenodd;clip-rule:evenodd""><defs><style>.fil0{fill:none}.fil1{fill:#471e00}.fil2{fill:#fad3a8}</style></defs><g id=""Layer_x0020_1""><path class=""fil0"" d=""M256 255.999h1536v1536H256z""/><path class=""fil0"" d=""M0 0h2048v2048H0z""/><path class=""fil1"" d=""M1011.14 256.001c230.326 0 417.024 196.533 417.024 438.966 0 242.45-186.698 438.984-417.024 438.984-230.309 0-417.017-196.534-417.017-438.984 0-242.433 186.708-438.966 417.017-438.966z""/><path class=""fil2"" d=""M1025.12 439.576c183.96 0 328.465 110.068 305.416 373.866-6.559 75.094 59.63 35.43 59.63 62.563 0 224.02-169.684 405.622-379.024 405.622-209.312 0-379.015-181.602-379.015-405.622 44.678 71.718 51.515-19.252 49.504-61.202-16.52-343.422 150.643-375.227 343.49-375.227z""/><path d=""m1083.14 935.237-19.691-22.037.526-.544.545-.51.563-.473.578-.438.593-.403.607-.366.619-.332.63-.297.64-.26.647-.225.655-.189.662-.155.666-.118.67-.084.671-.047.672-.013.674.024.67.058.67.094.664.129.66.164.651.2.646.236.636.27.626.306.614.34.602.377.587.412.573.447.556.483.539.517.52.553.493.581.462.601.429.621.397.638.365.655.331.67.302.682.268.694.236.706.203.715.173.723.139.73.107.735.076.738.044.742.012.741-.022.743-.053.74-.085.738-.117.733-.149.728-.18.72-.214.711-.245.702-.277.69-.31.679-.341.664-.373.648-.405.632-.437.614-.47.594-.5.573zm-143.986-.001 19.691-22.035 2.752 2.844 2.847 2.657 2.937 2.472 3.024 2.287 3.103 2.105 3.178 1.923 3.247 1.74 3.307 1.56 3.364 1.375 3.412 1.193 3.456 1.01 3.492.829 3.52.645 3.541.459 3.559.276 3.563.092 3.562-.092 3.559-.276 3.54-.46 3.522-.644 3.491-.828 3.456-1.011 3.415-1.193 3.362-1.375 3.307-1.56 3.247-1.74 3.178-1.923 3.103-2.105 3.024-2.287 2.937-2.472 2.847-2.657 2.752-2.845 19.691 22.037-3.789 3.918-3.933 3.67-4.062 3.416-4.183 3.167-4.294 2.912-4.392 2.658-4.482 2.405-4.564 2.15-4.636 1.896-4.695 1.642-4.748 1.389-4.79 1.135-4.828.883-4.852.63-4.867.378-4.878.127-4.88-.127-4.866-.378-4.852-.63-4.826-.883-4.792-1.135-4.748-1.39-4.695-1.64-4.635-1.898-4.563-2.15-4.483-2.404-4.392-2.658-4.294-2.912-4.183-3.167-4.062-3.417-3.933-3.67-3.789-3.918zm19.691-22.035-19.691 22.035-.5-.573-.47-.594-.437-.614-.405-.632-.373-.649-.341-.663-.31-.678-.277-.691-.245-.702-.214-.712-.18-.72-.15-.727-.116-.733-.085-.737-.053-.74-.022-.744.012-.742.044-.741.076-.739.107-.734.14-.73.172-.723.203-.714.236-.707.268-.694.301-.683.332-.668.365-.655.397-.639.429-.62.462-.601.493-.581.52-.553.539-.517.556-.482.573-.448.587-.41.602-.378.614-.34.626-.307.636-.27.646-.235.652-.2.659-.164.665-.13.668-.093.671-.058.673-.024.673.013.672.047.67.084.665.118.662.155.655.19.648.225.64.26.63.296.618.332.607.366.593.403.578.438.563.474.545.509.526.544z"" style=""fill:#c99154;fill-rule:nonzero""/><path class=""fil1"" d=""M1138.77 379.082c146.689 64.911 265.61 160.867 265.61 214.328 0 53.458-118.921 44.176-265.61-20.734-146.687-64.911-265.601-160.867-265.601-214.328 0-53.458 118.913-44.182 265.601 20.734z""/><path class=""fil2"" d=""M582.756 737.966c20.947-9.912 54.085 29.236 73.994 87.433 19.92 58.205 19.078 113.42-1.884 123.324-20.954 9.912-54.078-29.235-73.987-87.43-19.933-58.215-19.078-113.422 1.877-123.327zM1434.49 737.966c-27.191-9.912-63.367 29.236-80.787 87.433-17.42 58.205-9.474 113.42 17.733 123.324 27.2 9.912 63.367-29.235 80.781-87.43 17.418-58.215 9.471-113.422-17.727-123.327z""/><path class=""fil1"" d=""M750.107 414.677c-81.626 56.994-85.737 155.105-85.737 183.445 0 28.35 66.18 5.135 147.806-51.861C893.81 489.264 959.99 420.084 959.99 391.734c0-28.35-128.25-34.054-209.883 22.943z""/><path style=""fill:#e6b683"" d=""M838.231 1545.47h345.829v-541.55H838.231z""/><path d=""M1011.14 256.001c230.326 0 417.024 196.533 417.024 438.966 0 242.45-186.698 438.984-417.024 438.984-230.309 0-417.017-196.534-417.017-438.984 0-242.433 186.708-438.966 417.017-438.966z"" style=""fill:#333""/><path class=""fil2"" d=""M1330.54 813.442c-6.559 75.094 59.63 35.43 59.63 62.571 0 224.012-169.698 405.613-379.024 405.613s-379.015-181.602-379.015-405.613c44.678 71.703 51.515-19.26 49.504-61.21-9.618-199.957 110.055-328.458 113.83-375.694-10.22 41.346 197.112 163.12 405.148 164.959 41.285.366 53.432-15.232 76.046 15.574 27.677 37.697 55.121 148.086 53.881 193.8z""/><path d=""M1702.28 1633.99c-79.92-63.38-439.947-255.111-518.217-283.774-14.495 53.379-97.843 95.338-174.634 95.338-76.783 0-156.709-33.803-171.196-87.181-78.28 28.663-412.601 212.238-492.512 275.618 151.862 210.54 1171.86 210.826 1356.56 0z"" style=""fill:#0288d1""/><path class=""fil2"" d=""M1430.43 737.966c-27.207-9.912-63.367 29.236-80.787 87.433-17.412 58.205-9.481 113.42 17.717 123.324 27.207 9.912 63.383-29.235 80.788-87.43 17.42-58.215 9.489-113.422-17.718-123.327zM591.872 737.966c27.206-9.912 63.367 29.236 80.787 87.433 17.42 58.205 9.486 113.42-17.72 123.331-27.198 9.905-63.374-29.242-80.78-87.438-17.419-58.207-9.485-113.42 17.713-123.326z""/></g></svg>";
            user_button.BackgroundImage = SvgToBitmap(userimgSvg);
            user_button.BackgroundImageLayout = ImageLayout.Stretch;

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

        private async void user_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your profile is on way...");
            ProfileViewer hehe = new ProfileViewer();
            hehe.Show();
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

    public class UserProfile
    {
        public string Bio { get; set; }
        public string CourseLevel { get; set; }
        public DateTime? CreatedAt { get; set; } // Use DateTime? for nullable DateTime
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string GroupLevel { get; set; }
        public bool Initialized { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePic { get; set; }
        public string SubscriptionStatus { get; set; }
        public DateTime? UpdatedAt { get; set; } // Use DateTime? for nullable DateTime
        public int UserID { get; set; }
        public string Username { get; set; }
    }

}