using Prabesh_Academy.Modules.Authentication;

namespace Prabesh_Academy.Modules.Forms
{
    public class LoginForm : Form
    {
        private Main mainForm;

        private TextBox passwordTextBox, usernameTextBox;

        public LoginForm(Main mainFormInstance)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill; // Ensure LoginForm fills its parent
            if (mainFormInstance != null)
                this.mainForm = mainFormInstance;
            CreateLoginPanel();
        }



        private Panel CreateLoginPanel()
        {
            // Parent Panel
            Panel loginPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(50)
            };

            // Central Table Layout
            TableLayoutPanel centralLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 1
            };

            centralLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            centralLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Inner Table Layout (content)
            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(10),
                Anchor = AnchorStyles.None // Center align within parent
            };

            // Title Label
            Label titleLabel = new Label
            {
                Text = "Login",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                AutoSize = true, // Automatically adjust size to fit content
                TextAlign = ContentAlignment.MiddleCenter,
            };

            contentLayout.Controls.Add(titleLabel, 0, 0);

            // Username Input
            usernameTextBox = new TextBox
            {
                PlaceholderText = "Username",
                Font = new Font("Arial", 14),
                BorderStyle = BorderStyle.FixedSingle,
                Width = 300,
                Margin = new Padding(0, 20, 0, 10)
            };
            contentLayout.Controls.Add(usernameTextBox, 0, 1);

            // Password Input
            passwordTextBox = new TextBox
            {
                PlaceholderText = "Password",
                Font = new Font("Arial", 14),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '*',
                Width = 300,
                Margin = new Padding(0, 10, 0, 10)
            };
            contentLayout.Controls.Add(passwordTextBox, 0, 2);

            // Login Button
            Button loginButton = new Button
            {
                Text = "Login",
                Font = new Font("Arial", 14),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Width = 300,
                AutoSize = true,
                //Margin = new Padding(0, 10, 0, 10),
                //Padding = new Padding(20, 10, 20, 10)
            };
            contentLayout.Controls.Add(loginButton, 0, 3);
            loginButton.Click += LoginButton_Click;



            // Back Button
            Button backButton = new Button
            {
                Text = "Back",
                Font = new Font("Arial", 12),
                BackColor = Color.LightGray,
                ForeColor = Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat,
                Width = 300,
                Margin = new Padding(0, 10, 0, 10),
                Padding = new Padding(20, 10, 20, 10),
                AutoSize = true,

            };
            backButton.Click += BackButton_Click;
            contentLayout.Controls.Add(backButton, 0, 4);

            // Add content layout to central layout
            centralLayout.Controls.Add(contentLayout, 0, 0);

            // Add central layout to the panel
            loginPanel.Controls.Add(centralLayout);

            // Add panel to the form
            this.Controls.Add(loginPanel);


            return loginPanel;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            string errorMessage;
            Authenticator authenticator = new Authenticator();

            // Authenticate login
            bool loginSuccess = authenticator.Login(username, password, out errorMessage);

            if (loginSuccess)
            {
                MessageBox.Show("Login successful! " +
                    "Courses are cooking... Wait a while 😉");
                mainForm.LoadHomePage();
                // Redirect to the home page or next screen here
            }
            else
            {
                //MessageBox.Show($"Login failed: {errorMessage}");
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            mainForm.LoadHomePage();
        }
    }
}