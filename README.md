# Prabesh Academy Desktop Application - Enhanced

This README provides a comprehensive overview of the Prabesh Academy desktop application, detailing its architecture, functionality, and how to set it up.


## A Quick Visual Intro

<div style="display: flex; flex-wrap: wrap; justify-content: center; align-items: flex-start; gap: 10px;">

<div style="width: 100%; max-width: 700px; display: flex; justify-content: center; margin-bottom: 10px;">
    <img src=".github/images/home.png" alt="Home Page" style="width: 100%; max-width: 600px; height: auto; display: block;">
</div>

  <div style="display: flex; flex-wrap: wrap; width: 100%; justify-content:center; gap: 10px;">

<div style="display: flex; width: auto; max-width: 250px; padding: 10px; box-sizing: border-box;  align-items: center; flex-direction: column;">
        <img src=".github/images/login.png" alt="Login Form" style="width: 100%; max-width: 200px; height: auto; display: block; margin-bottom: 5px;">
        <span>Login Screen</span>
</div>


<div style="display: flex; width: auto; max-width: 350px; padding: 10px; box-sizing: border-box;  align-items: center; flex-direction: column;">
    <img src=".github/images/error.png" alt="Error Screen" style="width: 100%; max-width: 300px; height: auto; display: block; margin-bottom: 5px;">
    <span>Error Screen</span>
</div>

<div style="display: flex; width: auto; max-width: 250px; padding: 10px; box-sizing: border-box; align-items: center; flex-direction: column;">
    <img src=".github/images/signup.png" alt="Signup Form" style="width: 100%; max-width: 200px; height: auto; display: block; margin-bottom: 5px;">
    <span>Signup Screen</span>
</div>

   </div>

</div>

## Description

The Prabesh Academy desktop application is a Windows Forms application that provides a user interface to interact with course data and perform authentication. It allows users to:

*   **Login and Signup:** Securely create new accounts and login to existing ones using a remote API.
*   **Course Browsing:** Explore available courses through a hierarchical structure including educational levels, groups, courses, subjects and lectures.
*   **Dynamic Home Page:** Display a responsive home page with a dynamic list of courses fetched from a backend API.
*   **Hierarchical Navigation**: Provides easy navigation through the courses using a stack to maintain application state.
*  **Lecture Viewing:** Lets you view details about lectures that a content contains.

## Architecture Overview

The application follows a modular structure, separating concerns for authentication, UI, and data interaction. Key components include:

*   **`Main.cs`**: The main form, which acts as a container for other user controls. It manages navigation and switches between different screens.
*  **`LoginForm.cs`**: A form for user authentication which utilizes the `Authenticator` class to log users in, stores the token in a static class for future use and allows users to go back to the home screen.
*  **`SignupForm.cs`**: A form to allow users to create new accounts which also uses the `Authenticator` class and redirects to the login page once the user is successfully signed up.
*   **`Course_Home.cs`**: A user control that displays the course catalog, manages hierarchical navigation and implements the back button functionality. It fetches data from a backend API and dynamically creates UI components.
*   **`home_page.cs`**: A user control for the application's landing page which provides access to the login/signup buttons and also fetches courses dynamically from a backend API.
*   **`LectureView.cs`**: A user control to view lecture details.
*   **`Authenticator.cs`**:  A class responsible for handling user authentication and registration with the backend API.
*   **`TokenManager.cs`**:  A static class to store and manage the JWT token.

## Detailed Functionality

### 1. Main Form (`Main.cs`)
    *   Initializes the main window and manages the different UI screens.
    *   Manages the login and signup forms to ensure they fit inside the Main Form.
    *   Contains methods to switch between the home page, login form, signup form, and the course home screen.
    *   Sets the main window to fill the screen by default.

### 2. Authentication

   *   **`LoginForm.cs`**
        *   Displays the login UI with fields for username/email and password.
        *   Calls the `Authenticator.Login()` method to authenticate the user.
        *   Upon successful login, clears all controls from the Main Form and loads the `Course_Home` user control.
        *   Allows users to go back to the main home page when needed.
   *  **`SignupForm.cs`**
        * Displays the signup form with fields for name, email, username and password.
        *   Uses the `Authenticator.SignUp()` to register the users.
        *   On success redirects to the `LoginForm`.

   *  **`Authenticator.cs`**
        *   Sends login and signup requests to the designated API endpoints.
        *   Manages the JSON serialization and deserialization required.
        *   Parses the API response for JWT and updates the static `TokenManager.cs` class for global access.
   *  **`TokenManager.cs`**
        *  Provides a static property to store the JWT token after a user logs in.
        * Provides methods to update the token and verify that a token has been generated.

### 3. Course Navigation and Display

   *   **`Course_Home.cs`**
        *   Displays the main course catalog page after a user has logged in.
        *   Fetches the list of educational levels on load and displays them as cards.
        *   Fetches all the data through the back end API.
        *   Uses a stack to maintain the navigation history, allowing users to go back to previous screens.
        *   Dynamically creates UI cards with the course information using a loop.
        *   Implements the back button functionality to provide easy navigation.
        *   Handles navigating to the `LectureView` when selecting a content with no children.
        *   Provides methods to load different levels of content i.e:
             * Educational Levels.
             * Groups for a selected Level.
             * Courses for a selected Group.
             * Subject details for selected Course, and
             * Hierarchical content for the subjects.

   *  **`LectureView.cs`**
        *   Loads the lecture details for a content id from the backend API.
        *   Displays all lecture information that a content id contains.
       
### 4. Home Page

   *  **`home_page.cs`**
      *   Displays a welcoming home page, including a header and a list of courses.
      *   Fetches a list of courses dynamically from the backend.
      *  Utilizes a `TableLayoutPanel` for layout and creates all of the cards dynamically.
      *  Provides login and signup buttons to navigate to the respective forms.
      * Displays a responsive list of course cards fetched from the backend API.
      * Handles errors and displays appropriate messages.

## Getting Started

### Prerequisites

*   Windows Operating System
*   .NET Framework 4.7.2 or later (.NET 6 or later recommended for modern projects)
*   Microsoft SQL Server (Required for the backend API)
*   ODBC Driver 17 for SQL Server (or another compatible driver)
*   Microsoft Visual Studio or .NET SDK
*   Backend API Server running and accessible at `ApiBaseUrl` specified in configuration

### Installation

1.  **Clone the repository:**

    ```powershell
    git clone https://github.com/BEU-Project-Developers/OnlineCoursePlatform.git
    cd OnlineCoursePlatform
    ```

2.  **Open the Solution:** Open the project's `.sln` file (`Prabesh_Academy.sln`) in Visual Studio.
    Alternatively use .Net CLI with `dotnet build` to build the project.

3. **Configuration:**
   *  **API Base URL:** Update the `ApiBaseUrl` connection string in `App.config` (or `appsettings.json` if using .NET Core/6+) for both your `Course_Home.cs` and `home_page.cs` classes. Make sure the configuration is in the following format
       ```xml
          <connectionStrings>
              <add name="ApiBaseUrl" connectionString="YOUR_API_BASE_URL"/>
          </connectionStrings>
       ```
       *  Replace `YOUR_API_BASE_URL` with your base API Url e.g `http://localhost:5000/`
4.  **Build and Run:**
    *   In Visual Studio: Build the project (Ctrl+Shift+B) and run the application (F5).
    * In terminal use the command `dotnet run` or browse to the `/bin/Debug/net7.0-windows` folder and execute the `.exe` file.

## Usage

1.  **Launch the Application:** Run the `Prabesh_Academy.exe` from the `/bin/Debug/net7.0-windows` folder.
2.  **Initial Home Page:** The initial home page is loaded by default where you have two options: `Login` or `Signup`.
3.  **Login/Signup:** Click the appropriate buttons on the `home_page` to navigate to either of the authentication screens.
4.  **Course Browsing:** After successful login, you are redirected to the `Course_Home` view, where you can navigate through the course levels.
5.  **Navigation:**  Click on a card to navigate deeper into the hierarchy. Use the back button to move back to the previous levels of content.
6.  **Lecture Viewing:** Click on the final content to view the lecture details in `LectureView`.

## Contributing

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Make your changes and commit them with descriptive messages.
4.  Create a pull request to the main branch.

## License

- MIT

## Acknowledgements

*   This project uses the following libraries:
    *   Microsoft.Data.SqlClient: For database interaction.
    *   .NET Framework / .NET 6 or Later: Windows Forms, Http Clients, other libraries
    *   Newtonsoft.Json: For JSON serialization/deserialization.

## Author

*   Author: Prabesh Aryal
*   Email: hello@prabe.sh
*   Github: https://github.com/prabeshAryal