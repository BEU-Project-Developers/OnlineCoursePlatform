using System.Drawing.Drawing2D;
using Prabesh_Academy.Modules.Forms; // Add this line


namespace Prabesh_Academy.Modules.Views
{
    public partial class home_page : UserControl
    {
        private Panel headerPanel;
        private Panel mainPanel;
        private Color CommonBackColor = Color.White;
        private Color CardThemeColor = Color.WhiteSmoke;

        public home_page()
        {
            InitializeComponent();
            InitializeHeaderPanel();
            InitializeMainPanel();
            AddCoursesCardToMainPanel();

           
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            (this.ParentForm as Main).ShowLoginForm(); // Call the ShowLoginForm method in Main Form
        }

        private void SignupButton_Click(object sender, EventArgs e)
        {
            (this.ParentForm as Main)?.ShowSignupForm();
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
                BackColor = Color.SteelBlue,
                ForeColor = Color.AntiqueWhite,
                Size = new Size(80, 40)
            };

            Button signupButton = new Button
            {
                Text = "Signup",
                Font = new Font("Comic Sans MS", 12),
                BackColor = Color.SteelBlue,
                ForeColor = Color.AntiqueWhite,
                Size = new Size(100, 40)
            };

            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(loginButton);
            headerPanel.Controls.Add(signupButton);
            // Event handlers for login and signup buttons
            loginButton.Click += LoginButton_Click;
            signupButton.Click += SignupButton_Click;

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
            int courseCardHeight = 200;
            int margin = 20;
            mainPanel.Controls.Clear();

            var courses = new[]
            {
    new { Title = "Physics : Chapter 18 - Rotational Dynamics | Mechanics", Duration = "3 hours", Image = @"C:\Users\prabe\Documents\Class\3rd Year\Modern Programmin Language - 1\Project\Prabesh Academy\Assets\images\ai_generated\demo_thumb.png" },
    new { Title = "Chemistry : Chapter 4 - Ideal Gas Equation | Gas Laws 2", Duration = "5 hours", Image = @"C:\Users\prabe\Documents\Class\3rd Year\Modern Programmin Language - 1\Project\Prabesh Academy\Assets\images\ai_generated\demo_thumb.png" },
    new { Title = "Computer : Data Structure and Algorithms | Interview Preparation ", Duration = "7 hours", Image = @"C:\Users\prabe\Documents\Class\3rd Year\Modern Programmin Language - 1\Project\Prabesh Academy\Assets\images\ai_generated\maybe_logo.jpeg" },
    new { Title = "Demo Course 4", Duration = "45:45", Image = @"C:\Users\prabe\Documents\Class\3rd Year\Modern Programmin Language - 1\Project\Prabesh Academy\Assets\images\ai_generated\demo_thumb.png" },
    new { Title = "Demo Course 5", Duration = "05:00", Image = @"C:\Users\prabe\Documents\Class\3rd Year\Modern Programmin Language - 1\Project\Prabesh Academy\Assets\images\ai_generated\demo_thumb.png" },
};


            int coursesPerRow = Math.Max(1, (mainPanel.ClientSize.Width - margin) / (courseCardWidth + margin));
            int yPosition = margin;

            for (int i = 0; i < courses.Length; i++)
            {
                int row = i / coursesPerRow;
                int col = i % coursesPerRow;

                int xPosition = margin + col * (courseCardWidth + margin);
                int totalRowWidth = coursesPerRow * courseCardWidth + (coursesPerRow - 1) * margin;
                int xOffset = Math.Max(0, (mainPanel.ClientSize.Width - totalRowWidth) / 2);
                xPosition += xOffset;

                Panel courseCard = new Panel
                {
                    Size = new Size(courseCardWidth, courseCardHeight),
                    Location = new Point(xPosition, yPosition + row * (courseCardHeight + margin)),
                    BackColor = CommonBackColor,
                    BorderStyle = BorderStyle.None
                };

                AddSimpleCourseContent(courseCard, courses[i]);
                mainPanel.Controls.Add(courseCard);
            }
        }

        private void AddSimpleCourseContent(Panel courseCard, dynamic course)
        {
            // Main layout for the course card
            TableLayoutPanel video_card_layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(5),
                BackColor = CardThemeColor,
                ForeColor = CardThemeColor,
                BorderStyle = BorderStyle.None, // Removes the border

            };
            video_card_layout.RowStyles.Add(new RowStyle(SizeType.Percent, 80F)); // 80% for thumbnail
            video_card_layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F)); // 20% for title label
            video_card_layout.BackColor = CommonBackColor;

            // Add rounded corners by handling the Paint event
            // Add rounded corners by handling the Paint event
            video_card_layout.Paint += (sender, e) =>
            {
                int cornerRadius = 15; // Adjust the radius as needed
                Graphics g = e.Graphics;
                Rectangle rect = video_card_layout.ClientRectangle;

                // Define a rounded rectangle
                using (GraphicsPath path = GetRoundedRectanglePath(rect, cornerRadius))
                {
                    // Set the clip to the rounded rectangle
                    g.SetClip(path);

                    // Fill the inside of the rounded rectangle with the desired background color
                    using (Brush fillBrush = new SolidBrush(CardThemeColor)) // Use the desired color here
                    {
                        g.FillPath(fillBrush, path);
                    }

                    // Do not draw the border if you don't want it. Comment or remove this section if not needed.
                    // If you want a border, adjust the pen size and color
                    using (Pen borderPen = new Pen(Color.Black, 5)) // You can make this 0 to hide the border completely
                    {
                        g.DrawPath(borderPen, path); // Optional: remove or comment this line to hide the border
                    }

                    // Reset the clip region after drawing
                    g.ResetClip();
                }
            };

            // 1. Thumbnail Panel (Image with overlay)
            Panel thumbnailPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardThemeColor,
                BackgroundImage = course.Image != null ? Image.FromFile(course.Image) : null,
                BackgroundImageLayout = ImageLayout.Zoom // Maintains aspect ratio
            };

            // Duration label to overlay on the thumbnail
            Label durationLabel = new Label
            {
                Text = course.Duration,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = CommonBackColor,
                BackColor = Color.Black,
                //Padding = new Padding(0, 0, 5, 5),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };



            // Add the duration label dynamically at the bottom-right
            thumbnailPanel.Controls.Add(durationLabel);
            thumbnailPanel.Resize += (sender, e) =>
            {
                durationLabel.Location = new Point(
                    thumbnailPanel.Width - durationLabel.Width - 5, // Adjusted offset from the right
                    thumbnailPanel.Height - durationLabel.Height - 5 // Adjusted offset from the bottom
                );
            };

            // Set rounded corners by creating a region
            int cornerRadius = 2; // Adjust the radius as needed
            using (GraphicsPath path = new GraphicsPath())
            {
                // Define the rounded rectangle path
                path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90); // Top-left corner
                path.AddArc(durationLabel.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90); // Top-right corner
                path.AddArc(durationLabel.Width - cornerRadius * 2, durationLabel.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90); // Bottom-right corner
                path.AddArc(0, durationLabel.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90); // Bottom-left corner
                path.CloseFigure();

                // Set the label's region to the rounded rectangle
                durationLabel.Region = new Region(path);
            }

            video_card_layout.Controls.Add(thumbnailPanel, 0, 0);

            // 2. Title Label
            Label titleLabel = new Label
            {
                Text = course.Title,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Dock = DockStyle.Fill,
                ForeColor = Color.DarkBlue,
                BackColor= CardThemeColor,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.TopLeft, // Align text to the left
                Padding = new Padding(2, 3, 0, 3),

                AutoSize = false, // Ensures fixed size within allocated space
                MaximumSize = new Size(video_card_layout.Width, 0), // Max width for wrapping
            };

            // Limit title text to two lines
            titleLabel.Text = TruncateTextToTwoLines(titleLabel.Text, titleLabel.Font, video_card_layout.Width);

            video_card_layout.Controls.Add(titleLabel, 0, 1);

            // Add the layout to the course card
            courseCard.Controls.Add(video_card_layout);
        }

        // Helper function to create a rounded rectangle path
        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.StartFigure();

            // Top-left arc
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // Top-right arc
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // Bottom-right arc
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // Bottom-left arc
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        // Helper function to truncate text to two lines
        private string TruncateTextToTwoLines(string text, Font font, int maxWidth)
        {
            using (Graphics g = Graphics.FromImage(new Bitmap(1, 1)))
            {
                string[] words = text.Split(' ');
                string result = "";
                int lineCount = 0;
                string line = "";

                foreach (var word in words)
                {
                    string testLine = string.IsNullOrEmpty(line) ? word : line + " " + word;
                    SizeF textSize = g.MeasureString(testLine, font);

                    if (textSize.Width > maxWidth)
                    {
                        lineCount++;
                        if (lineCount >= 2) break; // Limit to two lines
                        result += line + "\n";
                        line = word;
                    }
                    else
                    {
                        line = testLine;
                    }
                }
                result += line;
                return result.TrimEnd('\n');
            }
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
