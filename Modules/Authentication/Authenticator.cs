using System;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Prabesh_Academy.Modules.Authentication
{
    internal class Authenticator
    {
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5000/") };

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
                // Create request data
                var requestData = new
                {
                    first_name = firstName,
                    last_name = lastName,
                    email = email,
                    username = username,
                    password = HashPassword(password)  // Send hashed password
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request to the signup endpoint
                HttpResponseMessage response = Task.Run(() => client.PostAsync("signup", content)).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sign up successful!");
                    return true;
                }
                else
                {
                    errorMessage = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"Error: {errorMessage}");
                    return false;
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
                // Create request data
                var requestData = new
                {
                    username_or_email = usernameOrEmail,
                    password = HashPassword(password)  // Send hashed password
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request to the login endpoint
                HttpResponseMessage response = Task.Run(() => client.PostAsync("login", content)).Result;

                if (response.IsSuccessStatusCode)
                {
                    // MessageBox.Show("Login successful!");
                    return true;
                }
                else
                {
                    errorMessage = response.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"Error: {errorMessage}");
                    return false;
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
