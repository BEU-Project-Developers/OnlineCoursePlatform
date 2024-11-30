using Microsoft.Data.SqlClient;
using Prabesh_Academy.Modules.Forms;
using Prabesh_Academy.Modules.Views;

namespace Prabesh_Academy
{
    public partial class Main : Form
    {
        private LoginForm loginForm;
        private home_page homepage; // Declare homepage here
        private SignupForm signupForm; // Add SignupForm field

        public Main()
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
        }

        public void LoadHomePage()
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