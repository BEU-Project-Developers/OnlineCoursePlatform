# Prabesh Academy Desktop Application - Enhanced

This README provides a comprehensive overview of the Prabesh Academy desktop application, detailing its architecture, functionality, and how to set it up.

## A Quick Visual Intro

<div style="display: flex; flex-wrap: wrap; justify-content: center;">

 

  <div style="display: flex; width: 90%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/home.png" alt="Home Page" style="text-align: center; margin: auto; width: auto; height: auto; display: block;">
  </div>

<div style="display: flex; width: 90%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/playlist.png" alt="Playlist" style="width: auto; height: auto; display: block;">
  </div>

<div style="display: flex; width: 90%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/subjects.png" alt="Subjects" style="width: auto; height: auto; display: block;">
  </div>

<div style="display: flex; width: 50%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/player_full_screeen.png" alt="Player Full Screen" style="text-align: center; margin: auto; width: auto; height: auto; display: block;">
  </div>

   <div style="display: flex; width: 50%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/profile_viewer.png" alt="Profile Viewer" style="width: 300px; height: auto; display: block;">
  </div>

  <div style="display: flex; width: 30%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/login.png" alt="Login Form" style="width: 200px; height: auto; display: block;">
  </div>

  <div style="display: flex; width: 30%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/signup.png" alt="Signup Form" style="width: 200px; height: auto; display: block;">
  </div>

  <div style="display: flex; width: 30%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/subscription.png" alt="Subscription" style="width: 200px; height: auto; display: block;">
  </div>

  <div style="display: flex; width: 30%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/detailed_messages.png" alt="Detailed Messages" style="text-align: center; margin: auto; width: 500px; height: auto; display: block;">
  </div>

  <div style="display: flex; width: 30%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/error.png" alt="Error Screen" style="width: 300px; height: auto; display: block;">
  </div>

   <div style="display: flex; width: 30%; padding: 10px; box-sizing: border-box;">
    <img src=".github/images/success_message.png" alt="Success Message" style="width: 300px; height: auto; display: block;">
  </div>

</div>




## Description

The Prabesh Academy desktop application is a Windows Forms application developed using C# and the .NET Framework/ .NET. It provides a user-friendly interface for accessing educational content from Prabesh Academy, enabling users to browse courses, view lectures, and manage their profiles.

**Key Features:**

*   **User Authentication:** Secure login and signup functionality allowing users to create accounts and access personalized content. Utilizes JWT (JSON Web Token) for secure session management with the backend API.
*   **Course Catalog:** Dynamically displays a hierarchical course catalog fetched from a backend API. The catalog is structured into Educational Levels, Groups, Courses, Subjects, and finally, Content (Lectures).
*   **Hierarchical Navigation:**  Intuitive navigation through the course catalog with a stack-based history for easy backtracking and exploration.
*   **Dynamic UI Generation:**  The application dynamically generates user interface elements, such as course cards, based on data received from the backend API, providing a flexible and up-to-date course browsing experience.
*   **Lecture Viewing:** Integrated lecture viewer allowing users to access and watch educational content seamlessly within the application.  It uses a WebView2 control to display web-based lecture content, passing lecture data as parameters.
*   **User Profile Management:**  Initial profile setup and viewing capabilities. Users can initialize their profiles by selecting course level and group and view their profile information within the application.
*   **API Driven:**  Entirely driven by a backend API, ensuring data integrity and separation of concerns. This architecture allows for easy maintenance and scalability.
*   **Error Handling:** Robust error handling throughout the application, providing informative messages to the user in case of network issues, API errors, or other unexpected exceptions.
*   **Configuration Driven:**  Uses `App.config` for configuration, particularly for setting the `ApiBaseUrl`, making it easy to switch environments (development, testing, production).

> For detailed **API Documentation**, please refer to the [BACKEND Folder](./BACKEND/) within this repository. This folder contains comprehensive documentation on the backend API, including endpoints, request/response formats, and authentication methods.


## Architecture Overview

The application is built using a modular architecture, promoting code reusability, maintainability, and scalability. The core components are:

*   **`Main.cs`**: The primary application form, acting as the main window and container for other user controls. It is responsible for:
    *   Initializing the application UI.
    *   Managing navigation between different user controls (Home Page, Login, Signup, Course Home, Lecture View, Profile Viewer).
    *   Handling the overall application lifecycle.
*   **`LoginForm.cs`**: Handles user login functionality:
    *   Provides a user interface for entering login credentials (username/email and password).
    *   Utilizes the `Authenticator` class to send login requests to the backend API.
    *   Upon successful authentication, retrieves and stores the JWT token using `TokenManager`.
    *   Navigates the user to the `Course_Home` screen after successful login.
    *   Includes a button to return to the main `home_page`.
*   **`SignupForm.cs`**: Handles user account creation:
    *   Provides a user interface for new user registration, collecting details like name, email, username, and password.
    *   Uses the `Authenticator` class to send signup requests to the backend API.
    *   On successful signup, redirects the user to the `LoginForm` to log in.
*   **`Course_Home.cs`**:  The main user control for displaying and navigating the course catalog:
    *   Fetches and displays courses dynamically from the backend API, presented as interactive cards.
    *   Implements hierarchical navigation, allowing users to drill down from Educational Levels to specific Lectures.
    *   Manages navigation history using a stack (`_navigationHistory`), enabling "Back" functionality.
    *   Dynamically creates UI cards to represent course items, adapting to the content structure from the API.
    *   Handles user interactions with course cards to navigate deeper into the catalog.
    *   Integrates with `LectureView.cs` to display lecture content.
    *   Includes navigation buttons for Home (reset navigation) and Refresh (reload content).
*   **`home_page.cs`**:  The initial landing page of the application:
    *   Displays a welcome message and a list of featured courses (fetched dynamically from the backend).
    *   Provides buttons to navigate to the `LoginForm` and `SignupForm`.
    *   Utilizes `TableLayoutPanel` for responsive layout of course cards.
*   **`LectureView.cs`**:  Displays lecture content:
    *   Loads lecture details for a specific content ID from the backend API.
    *   Utilizes a `WebView2` control to render web-based lecture content.
    *   Passes lecture data to the web player within the `WebView2` as URL parameters.
*   **`ProfileViewer.cs`**: Displays user profile information.
    *   Fetches active user profile data from the backend API.
    *   Displays user details including profile picture, username, full name, bio, and other relevant information such as course level, group, and subscription status.
    *   Presents user data in a structured and readable format within the application.
*   **`initMyProfile.cs`**:  User control for initial profile setup upon first login or when the profile is not fully initialized.
    *   Presents a form for users to select their Course Level and Education Group.
    *   Fetches available Course Levels and Education Groups from the backend API to populate dropdown lists.
    *   Updates user profile information on the backend upon submission, marking the profile as initialized.
    *   Handles coupon code input (though not fully implemented in the provided code).
    *   Transitions the user to the main `Course_Home` screen after successful profile initialization.
*   **`Authenticator.cs`**:  Handles authentication and registration logic:
    *   Encapsulates methods for user login (`Login`) and signup (`SignUp`).
    *   Communicates with the backend API endpoints for authentication and registration.
    *   Handles JSON serialization of request data and deserialization of API responses.
    *   Manages JWT token retrieval and storage via `TokenManager`.
*   **`TokenManager.cs`**:  Manages JWT token storage and access:
    *   Provides a static property `JWTToken` to store the current user's JWT.
    *   Provides a static method `UpdateToken` to set or clear the JWT token.
    *   Ensures the JWT token is accessible globally throughout the application for secure API requests.
*   **`NavigationState.cs`**: A class to manage and clone the current navigation state, for navigation history implementation in `Course_Home.cs`.

## Detailed Functionality

### 1. Main Application Flow (`Main.cs`, `home_page.cs`, `LoginForm.cs`, `SignupForm.cs`)
    *   Upon application launch, `Main.cs` loads the `home_page.cs` user control, presenting the initial landing screen.
    *   The `home_page.cs` fetches and displays a list of available courses and provides "Login" and "Signup" buttons.
    *   Clicking "Login" or "Signup" navigates to the `LoginForm.cs` or `SignupForm.cs` user controls, respectively, within the `Main` form.
    *   `LoginForm.cs` and `SignupForm.cs` handle user input, call `Authenticator.cs` to interact with the backend API, and manage authentication state.
    *   Successful login redirects to `Course_Home.cs`, clearing previous controls and loading the course navigation UI.

### 2. User Authentication and Authorization (`Authenticator.cs`, `TokenManager.cs`, `LoginForm.cs`, `SignupForm.cs`)
    *   `Authenticator.cs` handles the core logic for authentication and registration, communicating with the backend API's `/register` and `/login` endpoints.
    *   Login and signup credentials entered in `LoginForm.cs` or `SignupForm.cs` are sent to the backend API using methods in `Authenticator.cs`.
    *   Upon successful login, the backend API returns a JWT, which is extracted by `Authenticator.cs` and stored securely in `TokenManager.cs`.
    *   `TokenManager.cs` acts as a static container for the JWT, making it accessible throughout the application for subsequent API requests, ensuring secure authorization.
    *   All authorized API requests (e.g., fetching course data, lecture content) include the JWT from `TokenManager.cs` in the `Authorization` header.

### 3. Course Catalog Navigation (`Course_Home.cs`)
    *   `Course_Home.cs` is the central component for course browsing, fetching data from the `/allAvailableCourses` API endpoint.
    *   It dynamically constructs UI "cards" for each course item (Educational Levels, Groups, Courses, Subjects, Content), based on the API response.
    *   Navigation is hierarchical: clicking on an Educational Level card loads Groups within that level, clicking on a Group loads Courses, and so on.
    *   A stack-based navigation history (`_navigationHistory`) is implemented to enable the "Back" button, allowing users to retrace their steps through the course hierarchy.
    *   The `LoadAvailableCourses` method handles fetching data from the API, parsing the JSON response, and updating the UI dynamically based on the current navigation level and user selections.
    *   Special handling for single item responses optimize navigation by automatically drilling down when only one item is returned by the API for a given level.

### 4. Lecture Viewing (`LectureView.cs`)
    *   When a user navigates to a "Content" item that is a lecture (determined by `containsChildren: false` and type `content` in `Course_Home.cs`), `LectureView.cs` is loaded.
    *   `LectureView.cs` fetches lecture details from the `/lectures/{contentId}` API endpoint.
    *   It embeds a `WebView2` control to display the lecture content. `WebView2` allows rendering web-based content (like HTML5 videos, interactive lectures) within the desktop application.
    *   Lecture data, retrieved from the API, is passed to an HTML player file (`player.html` assumed to be part of the project in `WebPlayer` folder - not directly shown in snippets but referenced), likely via URL parameters, allowing the web player to dynamically load and present the lecture.


### 5. User Profile Management (`initMyProfile.cs`, `ProfileViewer.cs`)
    *   **`initMyProfile.cs`**:
        *   Displayed when a new user logs in for the first time or if their profile is not yet initialized (`Initialized: false` from API).
        *   Fetches lists of available "Levels" and "Groups" using the `/availableLevelsAndGroups` API endpoint to populate dropdowns.
        *   Allows users to select their Course Level and Education Group.
        *   On "Payment Button" click (could be better named "Initialize Profile" or similar), updates the user profile via a `PUT` request to `/activeUser`, setting `Initialized: true`, `SubscriptionStatus: subscribed` (based on code).
        *   After successful profile initialization, navigates the user to `Course_Home.cs`.
    *   **`ProfileViewer.cs`**:
        *   A dedicated form (window) to display user profile information.
        *   Fetches user profile data from the `/activeUser` API endpoint using the JWT for authorization.
        *   Displays user details such as profile picture (loads from URL or uses default), username, full name, bio, email, course level, group, subscription status, user ID, creation and update timestamps, and initialization status.
        *   Presents the profile information in a clear and organized manner using labels and a `FlowLayoutPanel`.


### 6. Error Handling and UI Enhancements
    *   The application implements try-catch blocks and `response.EnsureSuccessStatusCode()` to handle network errors, API failures, JSON parsing errors, and unexpected exceptions.
    *   Error messages are displayed to the user via `MessageBox.Show()`, providing feedback on issues like API communication failures or incorrect input.
    *   UI elements, like the coupon code text box in `initMyProfile.cs`, include placeholder text and color changes for improved user experience.
    *   SVG icons are used for UI buttons (Back, Home, Refresh, Logout, User Profile), enhancing visual appeal and scalability across different resolutions.  SVG content is embedded directly in code and converted to Bitmaps at runtime.
    *   Asynchronous operations (using `async` and `await`) are used throughout the application for API calls and other potentially long-running tasks, preventing the UI from freezing and ensuring a responsive user experience.

## Getting Started

### Prerequisites

*   **Windows Operating System:** Designed and tested for Windows.
*   **.NET Framework 4.7.2 or later (.NET 7 or later recommended):** Ensure you have the .NET Framework Developer Pack or .NET SDK installed. (.NET 7 or later is recommended for better performance and latest features). You can download it from [Microsoft's .NET Download Page](https://dotnet.microsoft.com/download).
*   **Microsoft Visual Studio or .NET SDK:** To build and run the application, you will need Visual Studio (Community, Professional, or Enterprise) or the .NET SDK. Visual Studio Community is a free IDE suitable for development. Download Visual Studio from [Visual Studio Downloads](https://visualstudio.microsoft.com/downloads/).
*   **Backend API Server:** The Prabesh Academy desktop application relies on a running backend API server. Ensure you have the backend API (likely Flask-based, as mentioned in the description and `BACKEND` folder) set up and running. The API needs to be accessible from your desktop application, either locally or via a network. Refer to the `BACKEND` folder for setup instructions.
*   **WebView2 Runtime:** The `LectureView.cs` component uses Microsoft Edge WebView2 to display web-based content. Ensure the WebView2 Runtime is installed on your system. If it's not, the application might prompt you to install it, or you can download it from [Microsoft Edge WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/).

### Installation

1.  **Clone the repository:**
    If you haven't already, clone the project repository to your local machine using Git:

    ```powershell
    git clone https://github.com/BEU-Project-Developers/OnlineCoursePlatform.git
    cd OnlineCoursePlatform
    ```

2.  **Open the Solution in Visual Studio (Recommended):**
    *   Launch Visual Studio.
    *   Go to `File` > `Open` > `Project/Solution...`.
    *   Navigate to the cloned repository directory and select the `Prabesh_Academy.sln` solution file.
    *   Visual Studio will load the project, and you can explore the solution structure in the Solution Explorer.

    **OR Open with .NET CLI (Alternative):**
    *   Open a terminal or command prompt.
    *   Navigate to the repository directory where the `Prabesh_Academy.csproj` file is located.

3. **Configuration - API Base URL:**

   *  Locate the `App.config` file in the Solution Explorer (if using .NET Framework) or `appsettings.json` (if using .NET Core/.NET 7+) within the `Prabesh_Academy.Modules.Forms` and `Prabesh_Academy.Modules.Views` folders (check both if unsure which is used, generally `App.config` for Framework projects).
   *  Open `App.config` (or `appsettings.json`) and find the `<connectionStrings>` section.
   *  Update the `connectionString` attribute of the `ApiBaseUrl` entry with the base URL of your backend API server.

       **For `App.config` (XML Format):**
       ```xml
       <?xml version="1.0" encoding="utf-8" ?>
       <configuration>
           <connectionStrings>
               <add name="ApiBaseUrl" connectionString="YOUR_API_BASE_URL"/>
           </connectionStrings>
           </startup>
       </configuration>
       ```
       **For `appsettings.json` (JSON Format - less likely in provided code, but if using .NET Core/.NET 7):**
       ```json
       {
         "ConnectionStrings": {
           "ApiBaseUrl": "YOUR_API_BASE_URL"
         }
       }
       ```
       **Replace `YOUR_API_BASE_URL` with the actual base URL of your API server.** For example, if your API is running locally on port 5000, it might be `http://localhost:5000/`. Ensure there is no trailing slash in the URL.

4.  **Build and Run the Application:**

    **Using Visual Studio (Recommended):**
    *   In Visual Studio, go to `Build` > `Build Solution` (or press `Ctrl+Shift+B`) to compile the project.
    *   Once the build is successful, go to `Debug` > `Start Debugging` (or press `F5`) to run the application.

    **Using .NET CLI (Alternative):**
    *   In your terminal, navigate to the project directory (where `Prabesh_Academy.csproj` is).
    *   Run the command `dotnet build` to build the project.
    *   After successful build, navigate to the output directory (typically `bin\Debug\net7.0-windows\` or similar, depending on your target framework).
    *   Run the executable file, e.g., `Prabesh_Academy.exe`.


## Usage

1.  **Launch the Application:** After building, run the `Prabesh_Academy.exe` executable from the output directory (`bin\Debug\net[framework-version]-windows`).
2.  **Home Page:** The application will start with the `home_page`, displaying a welcome screen and available courses.
3.  **Login or Signup:**
    *   Click the "Login" button to navigate to the `LoginForm`. Enter your registered username/email and password, and click "Login."
    *   If you don't have an account, click the "Signup" button to go to the `SignupForm`. Fill in the registration details and click "Signup." After successful signup, you'll be redirected to the login page.
4.  **Course Browsing (After Login):**
    *   After successful login, you'll be taken to the `Course_Home` screen.
    *   You'll see cards representing Educational Levels. Click on a Level card to view Groups within that level. Continue clicking to navigate through Courses, Subjects, and Content.
    *   Use the "Back" button (orange arrow icon) in the top left to go back to the previous level in the course hierarchy.
    *   The "Home" button (house icon) resets the navigation back to the top level (Educational Levels).
    *   The "Refresh" button (circular arrow icon) reloads the content for the current level you are viewing.
5.  **Lecture Viewing:**
    *   When you navigate to a "Content" card that represents a lecture (and has no further children), clicking it will open the `LectureView`.
    *   The `LectureView` will load and display the lecture content within the WebView2 control.
6.  **Profile Viewing:**
    *   Click the "User" button (user icon, likely in the top right - based on code description).
    *   This will open the `ProfileViewer` form, displaying your profile information retrieved from the API.
7.  **Logout:**
    *   Click the "Logout" button (door icon, likely top right).
    *   You will be asked for confirmation. Clicking "Yes" will log you out, clear the JWT token, close the application, and relaunch it, taking you back to the initial `home_page`.

## Contributing

We welcome contributions to the Prabesh Academy desktop application! If you'd like to contribute, please follow these steps:

1.  **Fork the Repository:** Fork the main repository to your GitHub account.
2.  **Create a Branch:** Create a new branch for your feature or bug fix. Choose a descriptive branch name:
    ```bash
    git checkout -b feature/your-new-feature
    ```
    or
    ```bash
    git checkout -b bugfix/fix-login-error
    ```
3.  **Make Your Changes:** Implement your changes, ensuring they align with the project's coding standards and architecture. Commit your changes frequently with clear and concise commit messages.
4.  **Test Your Changes:** Thoroughly test your changes to ensure they work as expected and don't introduce regressions.
5.  **Create a Pull Request:** Once you are satisfied with your changes, push your branch to your forked repository on GitHub and create a pull request (PR) to the `main` branch of the original repository.
6.  **Code Review:** Your pull request will be reviewed by project maintainers. Be prepared to address any feedback or requested changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. (Note: Please add a LICENSE file if you are using the MIT License and clarify this point based on the actual license file if present in the repository).

## Acknowledgements

This project utilizes the following excellent libraries and technologies:

*   **.NET Framework / .NET 7 or Later:**  For building the Windows Forms application and core functionalities.
*   **Microsoft.Data.SqlClient:** For efficient and secure communication with Microsoft SQL Server databases (used in the backend API).
*   **Newtonsoft.Json (Json.NET):** For handling JSON serialization and deserialization, essential for communicating with the RESTful backend API.
*   **Microsoft Edge WebView2:** For embedding a modern web browser engine within the application, enabling the display of web-based lecture content smoothly within `LectureView.cs`.
*   **Svg.NET:** For rendering and displaying SVG (Scalable Vector Graphics) icons, providing resolution-independent and visually appealing UI elements.


## Author

*   **Author:** Prabesh Aryal
*   **Email:** hello@prabe.sh
*   **GitHub:** [https://github.com/prabeshAryal](https://github.com/prabeshAryal)