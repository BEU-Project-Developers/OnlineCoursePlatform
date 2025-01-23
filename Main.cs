using Microsoft.Data.SqlClient;
using Prabesh_Academy.Modules.Authentication;
using Prabesh_Academy.Modules.Forms;
using Prabesh_Academy.Modules.Views;

namespace Prabesh_Academy
{
    public partial class Main : Form
    {
        private LoginForm loginForm;
        private home_page homepage; // Declare homepage here
        private SignupForm signupForm; // Add SignupForm field
        public Course_Home courseHome; // Replace homepage with Course_Home

        public Main()
        {
            try
            {
                InitializeComponent();
                this.WindowState = FormWindowState.Maximized;
                loginForm = new LoginForm(this);
                loginForm.TopLevel = false;
                loginForm.Visible = false;
                this.Controls.Add(loginForm);

                signupForm = new SignupForm(this); // Create SignupForm instance
                signupForm.TopLevel = false;
                signupForm.Visible = false;
                this.Controls.Add(signupForm);


                LoadHomePage();
                //ShowLoginForm();
                //ShowSignupForm();
                //LoadCourseHome();}
            }
            catch (Exception ex)
            {
                // Log the exception details
                //File.WriteAllText("startup_error.txt", ex.ToString()); // Example of logging
                MessageBox.Show($"Error during startup: {ex.Message}", "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionally, you might want to gracefully exit the application here:
                // Application.Exit();
            }
        }

        public void LoadHomePage()
        {
            try
            {
                loginForm.Visible = false;
                signupForm.Visible = false;

                if (homepage == null) // Create homepage if it doesn't exist
                {
                    homepage = new home_page { Dock = DockStyle.Fill };
                    this.Controls.Add(homepage);
                }

                homepage.Visible = true;
            }
            catch (Exception ex)
            {
                // Log the exception details
                MessageBox.Show($"Error in LoadHomePage: {ex.Message}", "LoadHomePage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadCourseHome() //COurse card after logged in 
        {
            loginForm.Visible = false;
            signupForm.Visible = false;
            if (homepage != null)
                homepage.Visible = false;
            if (courseHome == null) // Create Course_Home instance if it doesn't exist
            {
                courseHome = new Course_Home(this) { Dock = DockStyle.Fill };
                this.Controls.Clear();
                this.Controls.Add(courseHome);
            }

            courseHome.Visible = true;
            courseHome.BringToFront(); // Ensure it appears on top
        }

        public void ShowLoginForm()
        {
            if (!this.Controls.Contains(loginForm))
            {
                loginForm.Dock = DockStyle.Fill; // Ensure it fills the Main form
                this.Controls.Add(loginForm);
            }
            loginForm.Visible = true;
            loginForm.BringToFront();
        }

        public void ShowSignupForm() // Add ShowSignupForm method
        {
            if (!this.Controls.Contains(signupForm))
            {
                signupForm.Dock = DockStyle.Fill; // Ensure it fills the Main form
                this.Controls.Add(signupForm);
            }
            signupForm.Visible = true;
            signupForm.BringToFront();
        }

    }
}