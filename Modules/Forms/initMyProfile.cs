using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prabesh_Academy.Modules.Views;
using System.IO;
using Svg;
using System.Configuration;
using Prabesh_Academy.Modules.Authentication;
using System.Net.Http.Headers;
using Newtonsoft.Json; // Required for file operations

namespace Prabesh_Academy.Modules.Forms
{
    public partial class initMyProfile : UserControl
    {
        private Main _mainForm; // Store MainForm instance
        private const string CouponPlaceholderText = "Enter Coupon";
        private readonly string _apiBaseUrl;
        private readonly string _jwtToken;

        public initMyProfile(Main mainFormInstance, UserProfile user_info)
        {

            _apiBaseUrl = ConfigurationManager.ConnectionStrings["ApiBaseUrl"]?.ConnectionString;
            _jwtToken = TokenManager.JWTToken;

            InitializeComponent();
            _mainForm = mainFormInstance; // Store the MainForm instance
            this.Dock = DockStyle.Fill; // Make it fill the parent container (MainForm)

            LoadUserProfile(user_info); // Load user data into the form
            PopulateComboBoxesAsync();
        }
        private async Task PopulateComboBoxesAsync()
        {
            try
            {
                var levelsAndGroups = await GetAvailableLevelsAndGroupsAsync();
                if (levelsAndGroups != null)
                {
                    // Populate Course Levels ComboBox
                    if (levelsAndGroups.ContainsKey("Levels") && levelsAndGroups["Levels"] != null)
                    {
                        courseLevel.Items.Clear();
                        foreach (var levelPair in levelsAndGroups["Levels"])
                        {
                            courseLevel.Items.Add(levelPair.Value); // Add Level names to combobox
                        }
                        if (courseLevel.Items.Count > 0)
                        {
                            courseLevel.SelectedIndex = 0; // Optionally select the first item
                        }
                    }

                    // Populate Education Groups ComboBox
                    if (levelsAndGroups.ContainsKey("Groups") && levelsAndGroups["Groups"] != null)
                    {
                        educationGroup.Items.Clear();
                        foreach (var groupPair in levelsAndGroups["Groups"])
                        {
                            educationGroup.Items.Add(groupPair.Value); // Add Group names to combobox
                        }
                        if (educationGroup.Items.Count > 0)
                        {
                            educationGroup.SelectedIndex = 0; // Optionally select the first item
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching levels and groups from API: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Log the exception details, especially ex.StatusCode if available for network errors.
                Console.WriteLine($"HTTP Request Error: {ex.Message}, Status Code: {ex.StatusCode}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while populating dropdowns: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Unexpected Error populating dropdowns: {ex}");
            }
        }


        private void LoadUserProfile(UserProfile userInfo)
        {
            if (userInfo != null)
            {
                fName.Text = userInfo.FirstName ?? "First Name"; // Use null-coalescing operator for default value
                lName.Text = userInfo.LastName ?? "Last Name";   // Use null-coalescing operator for default value
                username_label.Text = string.IsNullOrEmpty(userInfo.Username) ? "@username" : "@" + userInfo.Username;

                string svgText = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 32 32\" style=\"enable-background:new 0 0 32 32\" xml:space=\"preserve\"><path d=\"M16 14c-3.86 0-7-3.14-7-7s3.14-7 7-7 7 3.14 7 7-3.14 7-7 7zm0-12c-2.757 0-5 2.243-5 5s2.243 5 5 5 5-2.243 5-5-2.243-5-5-5zM27 32a1 1 0 0 1-1-1v-6.115a6.95 6.95 0 0 0-6.942-6.943h-6.116A6.95 6.95 0 0 0 6 24.885V31a1 1 0 1 1-2 0v-6.115c0-4.93 4.012-8.943 8.942-8.943h6.116c4.93 0 8.942 4.012 8.942 8.943V31a1 1 0 0 1-1 1z\"/></svg>";
                // Load Profile Picture - Handle null or empty path and potential errors
                if (!string.IsNullOrEmpty(userInfo.ProfilePic))
                {
                    try
                    {
                        // Assuming ProfilePic is a file path. Adjust if it's a URL or byte array.
                        if (File.Exists(userInfo.ProfilePic))
                        {
                            ProfilePIC.Image = Image.FromFile(userInfo.ProfilePic);
                        }
                        else
                        {
                            // Handle case where file doesn't exist (e.g., set a default image)
                            ProfilePIC.Image = SvgDocument.Open<SvgDocument>(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(svgText))).Draw();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle image loading errors (e.g., log the error, set a default image)
                        Console.WriteLine($"Error loading profile picture: {ex.Message}");
                        ProfilePIC.Image = SvgDocument.Open<SvgDocument>(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(svgText))).Draw();
                    }
                }
                else
                {
                    // If ProfilePic path is null or empty, set a default image
                    ProfilePIC.Image = SvgDocument.Open<SvgDocument>(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(svgText))).Draw();
                }

                // Select Course Level in ComboBox - Handle cases where the value might not be in the list
                if (!string.IsNullOrEmpty(userInfo.CourseLevel) && courseLevel.Items.Contains(userInfo.CourseLevel))
                {
                    courseLevel.SelectedItem = userInfo.CourseLevel;
                }
                else if (courseLevel.Items.Count > 0)
                {
                    courseLevel.SelectedIndex = 0; // Select the first item as default if no match or empty data
                }

                // Select Education Group in ComboBox - Handle cases where the value might not be in the list
                if (!string.IsNullOrEmpty(userInfo.GroupLevel) && educationGroup.Items.Contains(userInfo.GroupLevel))
                {
                    educationGroup.SelectedItem = userInfo.GroupLevel;
                }
                else if (educationGroup.Items.Count > 0)
                {
                    educationGroup.SelectedIndex = 0; // Select the first item as default if no match or empty data
                }
            }
            else
            {
                // Handle case where userInfo is null (e.g., log error, display default form)
                Console.WriteLine("UserProfile object is null.");
                // Optionally set default values for labels and combo boxes or handle as needed.
            }
        }


        private void CouponCode_Enter(object sender, EventArgs e)
        {
            if (CouponCode.Text == CouponPlaceholderText)
            {
                CouponCode.Text = "";
                CouponCode.ForeColor = Color.Black; // Change color when typing
            }
        }

        private void CouponCode_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CouponCode.Text))
            {
                CouponCode.Text = CouponPlaceholderText;
                CouponCode.ForeColor = Color.Gray; // Reset to placeholder color
            }
        }

        public async Task<Dictionary<string, Dictionary<int, string>>> GetAvailableLevelsAndGroupsAsync()
        {
            using (var client = new HttpClient())
            {
                // Set authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

                // Call the /availableLevelsAndGroups endpoint
                var response = await client.GetAsync($"{_apiBaseUrl}/availableLevelsAndGroups");
                response.EnsureSuccessStatusCode();

                // Deserialize JSON response
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AvailableLevelsAndGroupsResponse>(jsonString);

                // Map group IDs and names
                var groupMappings = new Dictionary<int, string>();
                foreach (var group in result.CourseGroups)
                {
                    groupMappings[group.GroupId] = group.GroupName;
                }

                // Map level IDs and names
                var levelMappings = new Dictionary<int, string>();
                foreach (var level in result.EducationalLevels)
                {
                    levelMappings[level.LevelId] = level.LevelName;
                }

                // Return both mappings in a dictionary
                return new Dictionary<string, Dictionary<int, string>>
            {
                { "Groups", groupMappings },
                { "Levels", levelMappings }
            };
            }
        }

        private async void paymentButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

                    var selectedCourseLevel = courseLevel.SelectedItem?.ToString();
                    var selectedEducationGroup = educationGroup.SelectedItem?.ToString();

                    if (string.IsNullOrEmpty(selectedCourseLevel) || string.IsNullOrEmpty(selectedEducationGroup))
                    {
                        MessageBox.Show("Please select both Course Level and Education Group.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var updateData = new Dictionary<string, string>
                    {
                        { "Initialized", "1" },
                        { "SubscriptionStatus", "subscribed" },
                        { "CourseLevel", selectedCourseLevel },
                        { "GroupLevel", selectedEducationGroup }
                    };

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync("/activeUser", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Subscribed to plan! \n Redirecting to your course", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _mainForm.Controls.Clear();
                        _mainForm.courseHome = null;
                        _mainForm.LoadCourseHome();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed to update profile. Status Code: {response.StatusCode}\nError: {errorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Optionally log the error message for debugging
                        Console.WriteLine($"API Error: Status Code: {response.StatusCode}, Error: {errorMessage}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network error occurred: {ex.Message}", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"HTTP Request Exception: {ex}");
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"JSON serialization error: {ex.Message}", "Serialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"JSON Exception: {ex}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Unexpected Exception: {ex}");
            }
        }
    }




    // Define models for the JSON response
    public class AvailableLevelsAndGroupsResponse
{
    [JsonProperty("course_groups")]
    public List<CourseGroup> CourseGroups { get; set; }

    [JsonProperty("educational_levels")]
    public List<EducationalLevel> EducationalLevels { get; set; }
}

public class CourseGroup
{
    [JsonProperty("group_id")]
    public int GroupId { get; set; }

    [JsonProperty("group_name")]
    public string GroupName { get; set; }
}

public class EducationalLevel
{
    [JsonProperty("level_id")]
    public int LevelId { get; set; }

    [JsonProperty("level_name")]
    public string LevelName { get; set; }
}

}