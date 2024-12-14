using System;
using System.Drawing;
using System.Windows.Forms;
using Prabesh_Academy.Modules.Authentication;

namespace Prabesh_Academy.Modules.Forms
{
    public class SignupForm : Form
    {
        private Main mainForm;
        private TextBox firstNameTextBox, lastNameTextBox, emailTextBox, usernameTextBox, passwordTextBox, confirmPasswordTextBox;
        private ProgressBar progressBar1;

        public SignupForm(Main mainFormInstance)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill; // Ensure SignupForm fills its parent
            if (mainFormInstance != null)
                this.mainForm = mainFormInstance;
            CreateSignupPanel();
        }

        private void CreateSignupPanel()
        {
            // Parent Panel
            Panel signupPanel = new Panel
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
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(10),
                Anchor = AnchorStyles.None, // Center align within parent
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };

            // Set column styles
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // Label column
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); // Input column

            // Title Label
            Label titleLabel = new Label
            {
                Text = "Sign Up",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                //Dock = DockStyle.Top
            };
            progressBar1 = new ProgressBar
            {
                BackColor = Color.White,
                ForeColor = Color.Orange,
                //progressBar1.Location = new Point(327, 176);
                Name = "progressBar1",
                Size = new Size(100, 10),
                TabIndex = 4,
                Visible = false
            };




            // Add title label spanning all columns
            contentLayout.Controls.Add(titleLabel, 0, 0);
            contentLayout.SetColumnSpan(titleLabel, 2);

            firstNameTextBox = AddLabelTextboxPair(contentLayout, "First Name:", false, 1);
            lastNameTextBox = AddLabelTextboxPair(contentLayout, "Last Name:", false, 2);
            emailTextBox = AddLabelTextboxPair(contentLayout, "Email:", false, 3);
            usernameTextBox = AddLabelTextboxPair(contentLayout, "Username:", false, 4);
            passwordTextBox = AddLabelTextboxPair(contentLayout, "Password:", false, 5, isPassword: true);
            contentLayout.Controls.Add(progressBar1);

            confirmPasswordTextBox = AddLabelTextboxPair(contentLayout, "Confirm Password:", false, 6, isPassword: true);

            passwordTextBox.TextChanged += passwordTextBox_TextChanged;


            // Signup Button
            Button signupButton = new Button
            {
                Text = "Sign Up",
                Font = new Font("Arial", 14),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 10)
            };
            signupButton.Click += SignupButton_Click;
            contentLayout.Controls.Add(signupButton, 0, 7);
            contentLayout.SetColumnSpan(signupButton, 2);

            // Back Button
            Button backButton = new Button
            {
                Text = "Back",
                Font = new Font("Arial", 12),
                BackColor = Color.LightGray,
                ForeColor = Color.DarkSlateBlue,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 10)
            };
            backButton.Click += BackButton_Click;
            contentLayout.Controls.Add(backButton, 0, 8);
            contentLayout.SetColumnSpan(backButton, 2);

            // Add content layout to central layout
            centralLayout.Controls.Add(contentLayout, 0, 0);

            // Add central layout to the parent panel
            signupPanel.Controls.Add(centralLayout);

            // Add parent panel to the form
            this.Controls.Add(signupPanel);
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            // Show and update progress bar based on password strength
            
            progressBar1.Visible = true;
            progressBar1.Value = passwordTextBox.Text.Length < 8 ? (int)(passwordTextBox.Text.Length * 12.5) : 100;
        }

        // Example: Inside the Signup button click event
        private void SignupButton_Click(object sender, EventArgs e)
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string email = emailTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string confirmPassword = confirmPasswordTextBox.Text;

            string errorMessage;
            Authenticator authenticator = new Authenticator();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                errorMessage = "All fields are required.";
                MessageBox.Show(errorMessage);  // Show error in MessageBox
                return;  // Don't proceed if any field is empty
            }

            if (passwordTextBox.Text.Length < 8)
            {
                errorMessage = "Put abit Stronger Password. Minimum Characters 8.";
                MessageBox.Show(errorMessage);  // Show error in MessageBox
                return;  // Don't proceed if any field is empty
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            bool signUpSuccess = authenticator.SignUp(firstName, lastName, email, username, password, out errorMessage);

            if (!signUpSuccess)
            {
                MessageBox.Show(errorMessage); // Show the error if signup fails
            }
            else
            {
                MessageBox.Show("Successfully created account! Please proceed to login.");
                mainForm.ShowLoginForm(); // Navigate to login page after successful signup
            }
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            mainForm.LoadHomePage();
        }

        // Helper function to add label-textbox pairs and return the textbox reference
        private TextBox AddLabelTextboxPair(TableLayoutPanel table, string labelText, bool optional, int row, bool isPassword = false)
        {
            Label label = new Label
            {
                Text = optional ? $"{labelText} (Optional)" : labelText,
                Font = new Font("Arial", 12),
                AutoSize = true,
                Margin = new Padding(3)
            };
            table.Controls.Add(label, 0, row);

            TextBox textBox = new TextBox
            {
                Font = new Font("Arial", 12),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(3)
            };

            if (isPassword)
                textBox.PasswordChar = '*'; // Mask input for password fields

            table.Controls.Add(textBox, 1, row);

            return textBox; // Return the TextBox reference for later use
        }

    }
}
