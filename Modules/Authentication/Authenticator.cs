using System;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace Prabesh_Academy.Modules.Authentication
{
    public static class TokenManager
    {
        // Static property to hold the JWT token
        private static string _jwtToken;

        // Property to get and set the JWT token
        public static string JWTToken
        {
            get { return _jwtToken; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _jwtToken = value;
                }
            }
        }

        // Method to update the JWT token
        public static void UpdateToken(string newToken)
        {
            if (!string.IsNullOrEmpty(newToken))
            {
                _jwtToken = newToken;
            }
        }


        // Method to check if the token is available
        public static bool IsTokenAvailable()
        {
            return !string.IsNullOrEmpty(_jwtToken);
        }
    }

    internal class Authenticator
    {
        private static readonly string ApiBaseUrl = ConfigurationManager.ConnectionStrings["ApiBaseUrl"].ConnectionString;
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };

        // Method to hash password using SHA256
      

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
                    password = password // Send hashed password
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
                    password = password
                };

                // Serialize request data to JSON
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request to the login endpoint
                HttpResponseMessage response = Task.Run(() => client.PostAsync("login", content)).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Parse response content to extract the token
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    dynamic responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Assign the token to JWTtoken
                    var token = responseData.token.ToString(); // Ensure it's a string
                    TokenManager.UpdateToken(token);


                    // Indicate success
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
