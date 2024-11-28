using Microsoft.Data.SqlClient;
using Prabesh_Academy.Modules.Views;

namespace Prabesh_Academy
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            LoadHomePage();
        }

        public void LoadHomePage()
        {
            // Clear current controls if needed
            this.Controls.Clear();
            // Create home_page instance and add it to the Main form
            home_page homepage = new home_page(); // Create home page instance
            homepage.Dock = DockStyle.Fill; // Fill the entire form
            this.Controls.Add(homepage); // Add the home page control to the form
        }
    }
}
