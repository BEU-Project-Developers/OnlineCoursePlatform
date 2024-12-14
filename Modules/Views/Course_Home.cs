using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Prabesh_Academy.Modules.Views
{
    public partial class Course_Home : UserControl
    {
        private Main mainFormInstance;
        public Course_Home(Main mainFormArg)
        {
            mainFormInstance = mainFormArg;
            InitializeComponent();

            this.Dock = DockStyle.Fill; // Ensure the UserControl fills the Main form

            if (mainFormInstance != null)
            {
                mainFormInstance.Controls.Clear(); // Clear previous controls
                mainFormInstance.Controls.Add(this); // Add this control to the Main form
            }

            LoadEducationalLevels();
        }



        private void LoadEducationalLevels()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyCourses"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT level_id, level_name FROM EducationalLevels";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int levelId = Convert.ToInt32(reader["level_id"]);
                        string levelName = reader["level_name"].ToString();
                        AddLevelCard(levelId, levelName);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void AddLevelCard(int levelId, string levelName)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 60)/3, 80), // Dynamically adjust width
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = levelId,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Ensure resizing
            };


            Label lblLevelName = new Label
            {
                Text = $"Level: {levelName}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            card.Controls.Add(lblLevelName);
            card.Click += (sender, e) => LoadGroupsForLevel(levelId);
            flowLayoutPanel1.Controls.Add(card);
        }

        private void LoadGroupsForLevel(int levelId)
        {
            flowLayoutPanel1.Controls.Clear(); // Clear previous content

            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyCourses"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT group_id, map_description FROM EducationalLevel_CoursesMapping WHERE level_id = @levelId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@levelId", levelId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int groupId = Convert.ToInt32(reader["group_id"]);
                        string mapDescription = reader["map_description"].ToString();
                        AddGroupCard(groupId, mapDescription);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void AddGroupCard(int groupId, string mapDescription)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 60)/3, 80), // Dynamically adjust width
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = groupId,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Ensure resizing
            };


            Label lblDescription = new Label
            {
                Text = $"Group: {mapDescription}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            card.Controls.Add(lblDescription);
            card.Click += (sender, e) => LoadCoursesForGroup(groupId);
            flowLayoutPanel1.Controls.Add(card);
        }

        private void LoadCoursesForGroup(int groupId)
        {
            flowLayoutPanel1.Controls.Clear(); // Clear previous content

            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyCourses"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Courses.course_id, Courses.course_name FROM Courses " +
                                   "INNER JOIN CourseMapping ON Courses.course_id = CourseMapping.course_id " +
                                   "WHERE CourseMapping.group_id = @groupId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@groupId", groupId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int courseId = Convert.ToInt32(reader["course_id"]);
                        string courseName = reader["course_name"].ToString();
                        AddCourseCard(courseId, courseName);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void AddCourseCard(int courseId, string courseName)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 60)/3, 80), // Dynamically adjust width
                BackColor = Color.LightGray,
                Margin = new Padding(10),
                Tag = courseId,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Ensure resizing
            };

            Label lblCourseName = new Label
            {
                Text = $"Course: {courseName}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 10),
                AutoSize = true
            };

            card.Controls.Add(lblCourseName);
            card.Click += (sender, e) => ShowCourseDetails(courseId); // Define this method to handle lectures or further details
            flowLayoutPanel1.Controls.Add(card);
        }

        private void ShowCourseDetails(int courseId)
        {
            // Check for multiple subjects in the course
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyCourses"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to check subject count for the course
                    string query = "SELECT COUNT(*) FROM SubjectCourseMappings WHERE course_id = @courseId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    int subjectCount = (int)command.ExecuteScalar();

                    if (subjectCount > 1)
                    {
                        // If multiple subjects, load subjects
                        LoadSubjectsForCourse(courseId);
                    }
                    else
                    {
                        // If single subject, directly fetch and return the subject_id
                        query = "SELECT subject_id FROM SubjectCourseMappings WHERE course_id = @courseId";
                        SqlCommand subjectCommand = new SqlCommand(query, connection);
                        subjectCommand.Parameters.AddWithValue("@courseId", courseId);
                        int subjectId = (int)subjectCommand.ExecuteScalar();

                        // Proceed to lecture or further action using subjectId
                        ShowSubjectDetails(subjectId); // or any other operation
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LoadSubjectsForCourse(int courseId)
        {
            flowLayoutPanel1.Controls.Clear(); // Clear previous content

            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyCourses"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to fetch subjects for the course
                    string query = "SELECT Subjects.subject_id, Subjects.subject_name, Subjects.description " +
                                   "FROM Subjects " +
                                   "INNER JOIN SubjectCourseMappings ON Subjects.subject_id = SubjectCourseMappings.subject_id " +
                                   "WHERE SubjectCourseMappings.course_id = @courseId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@courseId", courseId);
                    SqlDataReader reader = command.ExecuteReader();

                    // Add cards for each subject
                    while (reader.Read())
                    {
                        int subjectId = Convert.ToInt32(reader["subject_id"]);
                        string subjectName = reader["subject_name"].ToString();
                        string description = reader["description"].ToString();
                        AddSubjectCard(subjectId, subjectName, description);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void AddSubjectCard(int subjectId, string subjectName, string description)
        {
            Panel card = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 60)/3, 80), // Dynamically adjust width
                BackColor = Color.LightSalmon,
                Margin = new Padding(10),
                Tag = subjectId,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Ensure resizing
            };

            Label lblSubjectName = new Label
            {
                Text = $"Subject: {subjectName}",
                TextAlign= ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblDescription = new Label
            {
                Text = $"Description: {description}",
                Font = new Font("Arial", 9),
                Location = new Point(10, 30),
                AutoSize = true
            };

            card.Controls.Add(lblSubjectName);
            card.Controls.Add(lblDescription);
            card.Click += (sender, e) => ShowSubjectDetails(subjectId); // Define this for further navigation if required
            flowLayoutPanel1.Controls.Add(card);
        }
        private void ShowSubjectDetails(int subjectId)
        {
            ContentRetriever retriever = new ContentRetriever();
            List<HierarchicalContent> contentList = retriever.GetContentBySubject(subjectId);

            // Check if data was retrieved
            if (contentList.Count == 0)
            {
                MessageBox.Show("No content found for the selected subject.");
                return;
            }

            // Clear existing content
            flowLayoutPanel1.Controls.Clear(); // Clear previous content

            // Display the hierarchy
            DisplayContentHierarchy(contentList, null); // Pass 'null' as ParentId for top-level content
        }
        private void DisplayContentHierarchy(List<HierarchicalContent> contentList, int? parentId)
        {
            // Create a dictionary to hold the hierarchy (ContentId -> List of child content)
            var hierarchy = new Dictionary<int, List<HierarchicalContent>>();

            // Populate the hierarchy with parent-child relationships
            foreach (var content in contentList)
            {
                if (content.ParentId.HasValue)
                {
                    if (!hierarchy.ContainsKey(content.ParentId.Value))
                        hierarchy[content.ParentId.Value] = new List<HierarchicalContent>();
                    hierarchy[content.ParentId.Value].Add(content);
                }
            }

            // Get the top-level content (i.e., content with no parent)
            var topLevelContent = contentList.Where(c => c.ParentId == parentId).ToList();

            if (topLevelContent.Count == 0 && parentId == null)
            {
                MessageBox.Show("No top-level content found.");
                return;
            }

            // Display the content for each parent level
            foreach (var content in topLevelContent)
            {
                DisplayContentCard(content, hierarchy);
            }
        }

        private void DisplayContentCard(HierarchicalContent content, Dictionary<int, List<HierarchicalContent>> hierarchy)
        {
            Panel contentCard = new Panel
            {
                Size = new Size((flowLayoutPanel1.ClientSize.Width - 60)/3, 80), // Dynamically adjust width
                BackColor = Color.LightGoldenrodYellow, // Use desired background color
                Margin = new Padding(10),
                Tag = content.ContentId, // Store the ContentId for identification
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right // Ensure resizing
            };

            // Add content name to the card
            Label contentLabel = new Label
            {
                Text = content.ContentName,
                TextAlign = ContentAlignment.MiddleCenter,

                Font = new Font("Arial", 10, FontStyle.Bold), // Optional font styling
                Location = new Point(10, 10), // Position within the card
                AutoSize = true // Ensure proper size for text
            };

            contentCard.Controls.Add(contentLabel);
            flowLayoutPanel1.Controls.Add(contentCard); // Add the card to the container

            // Add a click event handler to expand/collapse children
            contentCard.Click += (sender, e) =>
            {
                // Handle click event to show child content
                Panel clickedCard = (Panel)sender;
                int contentId = (int)clickedCard.Tag; // Get the ContentId from the card's Tag

                if (hierarchy.ContainsKey(contentId))
                {
                    // Check if child content exists
                    var childContent = hierarchy[contentId];

                    if (childContent.Count > 0)
                    {
                        // Clear previous content and display children
                        flowLayoutPanel1.Controls.Clear();
                        foreach (var child in childContent)
                        {
                            DisplayContentCard(child, hierarchy);  // Recursively display child content
                        }
                    }
                    else
                    {
                        // End of the hierarchy reached, show a message for lectures
                        //MessageBox.Show("Lectures coming soon for: " + content.ContentName);
                        LectureView lectureView = new LectureView(mainFormInstance, contentId) { Dock = DockStyle.Fill };  // 'this' refers to the current form, typically 'Main'
                        mainFormInstance.Controls.Clear();
                        mainFormInstance.Controls.Add(lectureView); // Add to Main form dynamically

                    }
                }
                else
                {
                    // End of the hierarchy reached, show a message for lectures
                    LectureView lectureView = new LectureView(mainFormInstance, contentId)
                    {
                        Dock = DockStyle.Fill
                    };  // 'this' refers to the current form, typically 'Main'
                    mainFormInstance.Controls.Clear();
                    mainFormInstance.Controls.Add(lectureView); // Add to Main form dynamically
                }
            };
        }



    }
}
