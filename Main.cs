using Microsoft.Data.SqlClient;

namespace Prabesh_Academy
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyDBstring"].ConnectionString;
        }

        private void ConnectToDatabase()
        {
            try
            {
                string connectionString = GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Connected successfully!");

                    string query = "SELECT * FROM test"; // Replace "test" with your table name
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    string displayData = "Database Test Table Details:\n";
                    while (reader.Read())
                    {
                        displayData += $"ID: {reader["id"]}\n"; // Replace with your column names
                    }
                    reader.Close();

                    MessageBox.Show(displayData); // Display results in a MessageBox
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToDatabase();
        }

    }
}
