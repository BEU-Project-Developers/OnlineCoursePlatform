namespace Prabesh_Academy.Modules.Views
{
    public partial class home_page : UserControl
    {
        private Panel headerPanel;
        private Panel mainPanel;

        public home_page()
        {
            InitializeComponent();
            InitializeHeaderPanel();
            InitializeMainPanel();
            AddCoursesCardToMainPanel();
        }
        // Initialize the main panel where course cards will be displayed
        private void InitializeMainPanel()
        {
            Panel mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill; // Fill the remaining space
            this.Controls.Add(mainContainer); // Add the container panel to the form

            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.AutoScroll = true;
            mainPanel.Resize += (s, e) => AddCoursesCardToMainPanel();
            mainContainer.Controls.Add(mainPanel); // Add mainPanel to the container

            // Set the padding on the container panel to create the desired spacing.
            mainContainer.Padding = new Padding(0, headerPanel.Height + 10, 0, 0);
        }
        private void InitializeHeaderPanel()
        {
            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 150;
            this.Controls.Add(headerPanel);

            Label headerLabel = new Label
            {
                Text = "Welcome to Prabesh Academy 🤗",
                Font = new Font("Comic Sans MS", 20, FontStyle.Bold),
                ForeColor = Color.MediumPurple,
                AutoSize = true,  // Set AutoSize to true if you want it to adjust size dynamically
                //Dock = DockStyle.Fill,  // Allow it to expand in the panel
                //TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent  // Ensure there's no background that might cause clipping
            };


            Button loginButton = new Button
            {
                Text = "Login",
                Font = new Font("Comic Sans MS", 12),
                BackColor = Color.LightPink,
                ForeColor = Color.DarkSlateBlue,
                Size = new Size(80, 40)
            };

            Button signupButton = new Button
            {
                Text = "Signup",
                Font = new Font("Comic Sans MS", 12),
                BackColor = Color.LightCoral,
                ForeColor = Color.DarkSlateBlue,
                Size = new Size(80, 40)
            };

            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(loginButton);
            headerPanel.Controls.Add(signupButton);

            headerPanel.Resize += (s, e) =>
            {
                headerLabel.Location = new Point((headerPanel.Width - headerLabel.Width) / 2, 10);
                loginButton.Location = new Point((headerPanel.Width - (loginButton.Width + signupButton.Width + 20)) / 2, 70);
                signupButton.Location = new Point(loginButton.Right + 20, loginButton.Top);
            };
        }

        private void AddCoursesCardToMainPanel()
        {
            int courseCardWidth = 250;
            int courseCardHeight = 350;
            int margin = 20;
            mainPanel.Controls.Clear();

            var courses = new[]
            {
        new { Title = "Course 1", Description = "Introduction to C#", Duration = "3 hours", Rating = 4.5, Progress = 50 },
        new { Title = "Course 2", Description = "Mastering Python", Duration = "5 hours", Rating = 4.8, Progress = 80 },
        new { Title = "Course 3", Description = "Advanced Java", Duration = "7 hours", Rating = 4.2, Progress = 20 },
        new { Title = "Course 4", Description = "Data Structures and Algorithms", Duration = "10 hours", Rating = 4.9, Progress = 90 },
        new { Title = "Course 5", Description = "Web Development Fundamentals", Duration = "6 hours", Rating = 4.6, Progress = 75 },
        new { Title = "Course 6", Description = "Machine Learning Basics", Duration = "8 hours", Rating = 4.7, Progress = 60 },
        new { Title = "Course 7", Description = "Introduction to AI", Duration = "4 hours", Rating = 4.3, Progress = 30 }
    };


            int coursesPerRow = Math.Max(1, (mainPanel.ClientSize.Width - margin) / (courseCardWidth + margin));
            int yPosition = margin;
            for (int i = 0; i < courses.Length; i++)
            {
                int row = i / coursesPerRow; //Determine row number
                int col = i % coursesPerRow; //Determine column number

                int xPosition = margin + col * (courseCardWidth + margin); // Calculate x position of card


                // Calculate x offset for centering the row, accounting for all cards
                int totalRowWidth = coursesPerRow * courseCardWidth + (coursesPerRow - 1) * margin;
                int xOffset = Math.Max(0, (mainPanel.ClientSize.Width - totalRowWidth) / 2); //Center the row if space available

                xPosition += xOffset; // Apply the offset to center the row


                Panel courseCard = new Panel
                {
                    Size = new Size(courseCardWidth, courseCardHeight),
                    Location = new Point(xPosition, yPosition + row * (courseCardHeight + margin)), //Move to the next row
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(10)
                };

                AddCourseContent(courseCard, courses[i]);
                mainPanel.Controls.Add(courseCard);
            }
        }



        private void AddCourseContent(Panel courseCard, dynamic course)
        {
            // Use TableLayoutPanel for better layout control
            TableLayoutPanel tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.ColumnCount = 1;
            tableLayout.RowCount = 5;
            tableLayout.AutoSize = true;

            // Improved Styling for the TableLayoutPanel
            tableLayout.BackColor = Color.WhiteSmoke; // Softer background color
            tableLayout.Padding = new Padding(15);   // Added padding for better spacing
            tableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayout.BorderStyle = BorderStyle.None; //Remove the default border

            // 1. Thumbnail (with image handling)

            PictureBox thumbnail = new PictureBox();
            thumbnail.Dock = DockStyle.Fill;
            thumbnail.SizeMode = PictureBoxSizeMode.Zoom; // Adjust image to fit
            //thumbnail.Image = GetCourseImage(course.Title); // Function to get image (see below) 
            thumbnail.BackColor = Color.LightGray; //Fallback if image load fails

            tableLayout.Controls.Add(thumbnail, 0, 0);
            //tableLayout.RowStyles[0].Height = 120; // Set row height for the thumbnail


            // 2. Title (improved styling)
            Label titleLabel = new Label();
            titleLabel.Text = course.Title;
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold); // Larger font size
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.AutoSize = false;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Padding = new Padding(0, 5, 0, 5);
            tableLayout.Controls.Add(titleLabel, 0, 1);


            // 3. Description (improved styling)
            Label descriptionLabel = new Label();
            descriptionLabel.Text = course.Description;
            descriptionLabel.Font = new Font("Arial", 11);
            descriptionLabel.Dock = DockStyle.Fill;
            descriptionLabel.AutoSize = false;
            descriptionLabel.TextAlign = ContentAlignment.TopLeft;
            descriptionLabel.MaximumSize = new Size(0, 60); // Increased height for description
            descriptionLabel.Padding = new Padding(0, 5, 0, 5);
            tableLayout.Controls.Add(descriptionLabel, 0, 2);


            // 4. Details (improved styling)
            Panel detailsPanel = new Panel();
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.AutoSize = true;

            // Use FlowLayoutPanel for details, but style it better.
            FlowLayoutPanel flowLayout = new FlowLayoutPanel();
            flowLayout.Dock = DockStyle.Fill;
            flowLayout.FlowDirection = FlowDirection.LeftToRight;
            flowLayout.AutoSize = true;
            flowLayout.WrapContents = false; // Don't wrap to next line if space is available
            flowLayout.Padding = new Padding(0, 5, 0, 5);

            AddDetailsLabel(flowLayout, "Duration:", course.Duration, Color.DarkBlue);
            AddDetailsLabel(flowLayout, "Rating:", course.Rating.ToString(), Color.DarkGreen);
            AddDetailsLabel(flowLayout, "Progress:", course.Progress + "%", Color.DarkOrange);


            detailsPanel.Controls.Add(flowLayout);
            tableLayout.Controls.Add(detailsPanel, 0, 3);


            // 5. Enroll Button (improved styling)
            Button enrollButton = new Button();
            enrollButton.Text = "Enroll Now";
            enrollButton.Dock = DockStyle.Fill;
            enrollButton.BackColor = Color.SteelBlue;
            enrollButton.ForeColor = Color.White;
            enrollButton.FlatStyle = FlatStyle.Flat; // Removes default 3D look
            enrollButton.FlatAppearance.BorderSize = 0; // Removes border
            tableLayout.Controls.Add(enrollButton, 0, 4);
            //tableLayout.RowStyles[4].Height = 40;


            courseCard.Controls.Add(tableLayout);
        }

        // Helper function to get course image (replace with your image loading logic)
        //private Image GetCourseImage(string courseTitle)
        //{
        //    //  Replace this with your actual image loading logic. 
        //    //  This is a placeholder.  You might load from resources, a database, or a file system.

        //    string imageName = courseTitle.Replace(" ", "_").ToLower() + ".png"; //Example naming convention
        //    //return Properties.Resources.ResourceManager.GetObject(imageName) as Image;

        //}

        // Modified AddDetailsLabel to accept color
        private void AddDetailsLabel(FlowLayoutPanel panel, string labelText, string value, Color color)
        {
            Label label = new Label();
            label.Text = $"{labelText} {value}";
            label.Font = new Font("Arial", 10);
            label.AutoSize = true;
            label.ForeColor = color;  // Set label color
            label.Margin = new Padding(5, 0, 5, 0);
            panel.Controls.Add(label);
        }

    }
}
