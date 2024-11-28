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

        // Initialize the header section with buttons
        private void InitializeHeaderPanel()
        {
            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;  // Dock it to the top of the UserControl
            headerPanel.Height = 150;          // Set an appropriate height for the header
            this.Controls.Add(headerPanel);    // Add headerPanel to the UserControl
            
            Label headerLabel = new Label();
            headerLabel.Text = "Welcome to Prabesh Academy 🤗";
            headerLabel.Font = new Font("Comic Sans MS", 20, FontStyle.Bold);
            headerLabel.ForeColor = Color.MediumPurple;
            headerLabel.AutoSize = true;
            headerLabel.Location = new Point(220, 20);
            // Add the buttons to the header panel

            Button loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Font = new Font("Comic Sans MS", 12);
            loginButton.BackColor = Color.LightPink;
            loginButton.ForeColor = Color.DarkSlateBlue;
            loginButton.Location = new Point(300,70);
            loginButton.Size = new Size(80, 40);

            Button signupButton = new Button();
            signupButton.Text = "Signup";
            signupButton.Font = new Font("Comic Sans MS", 12);
            signupButton.BackColor = Color.LightCoral;
            signupButton.ForeColor = Color.DarkSlateBlue;
            signupButton.Location = new Point(400, 70);
            signupButton.Size = new Size(80, 40);

            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(loginButton);
            headerPanel.Controls.Add(signupButton);

        }

        // Initialize the main panel where course cards will be displayed
        private void InitializeMainPanel()
        {
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;  // Dock it to fill the remaining space
            mainPanel.Location = new Point(0, headerPanel.Bottom);  // Position below headerPanel
            mainPanel.AutoScroll = true;  // Enable scrolling for mainPanel
            this.Controls.Add(mainPanel);  // Add mainPanel to the UserControl
        }

        private void AddCoursesCardToMainPanel()
        {
            int courseCardWidth = 250;
            int courseCardHeight = 350;
            int margin = 20;
            int coursesPerRow = 3;
            int xPosition = margin;
            int yPosition = 20;  // Start below the header section

            // Dummy data for courses
            var courses = new[]
            {
                new { Title = "Course 1", Description = "Introduction to C#", Duration = "3 hours", Rating = 4.5, Progress = 50 },
                new { Title = "Course 2", Description = "Mastering Python", Duration = "5 hours", Rating = 4.8, Progress = 80 },
                new { Title = "Course 3", Description = "Web Development", Duration = "4 hours", Rating = 4.2, Progress = 30 },
                new { Title = "Course 4", Description = "Data Structures & Algorithms", Duration = "6 hours", Rating = 4.7, Progress = 60 },
                new { Title = "Course 5", Description = "Machine Learning", Duration = "7 hours", Rating = 4.9, Progress = 90 },
                new { Title = "Course 6", Description = "Cloud Computing", Duration = "8 hours", Rating = 4.6, Progress = 70 },
            };

            // Loop through the courses and create course cards
            foreach (var course in courses)
            {
                // Create a panel for each course card
                Panel courseCard = new Panel();
                courseCard.Size = new Size(courseCardWidth, courseCardHeight);
                courseCard.Location = new Point(xPosition, yPosition);
                courseCard.BackColor = Color.White;
                courseCard.BorderStyle = BorderStyle.FixedSingle;
                courseCard.Padding = new Padding(10);

                // Create a thumbnail (rectangle filled with green color)
                Panel thumbnail = new Panel();
                thumbnail.Size = new Size(courseCardWidth - 20, 120); // Slightly smaller to add padding
                thumbnail.BackColor = Color.Green;
                courseCard.Controls.Add(thumbnail);

                // Title Label
                Label titleLabel = new Label();
                titleLabel.Text = course.Title;
                titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
                titleLabel.Location = new Point(10, thumbnail.Bottom + 10);
                titleLabel.AutoSize = true;
                courseCard.Controls.Add(titleLabel);

                // Description Label
                Label descriptionLabel = new Label();
                descriptionLabel.Text = course.Description;
                descriptionLabel.Font = new Font("Arial", 10);
                descriptionLabel.Location = new Point(10, titleLabel.Bottom + 5);
                descriptionLabel.Size = new Size(courseCardWidth - 20, 40); // Fixed size for description
                descriptionLabel.ForeColor = Color.Gray;
                courseCard.Controls.Add(descriptionLabel);

                // Duration Label
                Label durationLabel = new Label();
                durationLabel.Text = "Duration: " + course.Duration;
                durationLabel.Font = new Font("Arial", 10);
                durationLabel.Location = new Point(10, descriptionLabel.Bottom + 5);
                durationLabel.AutoSize = true;
                courseCard.Controls.Add(durationLabel);

                // Rating Label
                Label ratingLabel = new Label();
                ratingLabel.Text = "Rating: " + course.Rating;
                ratingLabel.Font = new Font("Arial", 10);
                ratingLabel.Location = new Point(10, durationLabel.Bottom + 5);
                ratingLabel.AutoSize = true;
                courseCard.Controls.Add(ratingLabel);

                // Progress Label
                Label progressLabel = new Label();
                progressLabel.Text = "Progress: " + course.Progress + "%";
                progressLabel.Font = new Font("Arial", 10);
                progressLabel.Location = new Point(10, ratingLabel.Bottom + 5);
                progressLabel.AutoSize = true;
                courseCard.Controls.Add(progressLabel);

                // Add the course card to mainPanel
                mainPanel.Controls.Add(courseCard);

                // Update positions for the next card
                xPosition += courseCardWidth + margin;

                // Check if the row is full (3 cards per row)
                if (xPosition + courseCardWidth > mainPanel.Width - margin)
                {
                    xPosition = margin;  // Reset x to the left side
                    yPosition += courseCardHeight + margin;  // Move down for the next row
                }
            }
        }
    }
}
