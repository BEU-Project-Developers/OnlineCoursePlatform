using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Prabesh_Academy.Modules.Authentication
{
    internal class Authenticator
    {
        // Method to hash password using SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute hash of the password
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Method to handle signup
        public bool SignUp(string firstName, string lastName, string email, string username, string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Construct connection string (should be from your config file)
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyDBstring"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@Username", username);

                    int userCount = (int)checkCmd.ExecuteScalar();
                    if (userCount > 0)
                    {
                        errorMessage = "Username already exists.";
                        return false;
                    }

                    // Hash the password before saving
                    string hashedPassword = HashPassword(password);

                    // Insert new user with hashed password
                    string insertQuery = "INSERT INTO Users (FirstName, LastName, Email, Username, PasswordHash) VALUES (@FirstName, @LastName, @Email, @Username, @Password)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@FirstName", firstName);
                    insertCmd.Parameters.AddWithValue("@LastName", lastName);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Password", hashedPassword);  // Store hashed password

                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Sign up successful!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
                MessageBox.Show(errorMessage);  // Display error in MessageBox
                return false;
            }
        }

        // Method to handle login
        public bool Login(string usernameOrEmail, string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Construct connection string (should be from your config file)
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["pAcademyDBstring"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Hash the entered password to compare with the stored hash
                    string hashedPassword = HashPassword(password);

                    // Query to check login credentials based on either username or email
                    string query = "SELECT COUNT(*) FROM Users WHERE (Username = @UsernameOrEmail OR Email = @UsernameOrEmail) AND PasswordHash = @Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UsernameOrEmail", usernameOrEmail);  // Check for both username or email
                    command.Parameters.AddWithValue("@Password", hashedPassword);  // Compare with hashed password

                    int result = (int)command.ExecuteScalar();
                    if (result > 0)
                    {
                        //MessageBox.Show("Login successful!");
                        return true;
                    }
                    else
                    {
                        errorMessage = "Invalid username/email or password.";
                        MessageBox.Show(errorMessage);  // Show error in MessageBox
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
                MessageBox.Show(errorMessage);  // Show error in MessageBox
                return false;
            }
        }

    }
}
