
# Authentication and Course API

This Flask API provides user authentication (signup and login) and access to course information. It uses JWT for authentication and connects to two SQL Server databases (`pacademy` for users and `courses` for course data).

## Description

This API allows users to:

*   **Signup:** Create new user accounts with a username, email, and password.
*   **Login:** Authenticate existing users and receive a JWT token.
*   **Access Courses:** Retrieve hierarchical course information, including educational levels, groups, courses, subjects, and lectures.
*   **Manage Active User:** Get and update information about the currently logged-in user.
*   **Get Available Levels and Groups:** Retrieve lists of available educational levels and course groups.
*   **View Homepage Trailers:** Get a dynamic list of homepage trailers for display.
*   **Stream Media:** Access streamable media content.

## Getting Started

### Prerequisites

*   Python 3.6 or higher
*   pip (Python package installer)
*   ODBC Driver 17 for SQL Server (or another compatible driver). Make sure it's properly configured to connect to your SQL Server instances.
*   SQL Server instances for `pacademy` and `courses` databases.

### Installation

1.  Clone the repository:

    ```powershell
    git clone "https://github.com/BEU-Project-Developers/OnlineCoursePlatform.git"
    cd ./OnlineCoursePlatform/BACKEND
    ```

2.  Install the required packages:

    ```powershell
    pip install flask pyodbc PyJWT Werkzeug
    ```

    If you intend to deploy the application you should create a requirements.txt using the command:
    ```powershell
    pip freeze > requirements.txt
    ```

3. **Configure Database:** Ensure the `DATABASE_CONFIG` and `COURSES_DATABASE_CONFIG` dictionaries in the `main.py` are correctly configured to connect to your SQL Server instance.

    *   Replace `"MY2NDGF"` with your SQL Server instance name.
    *   Verify that the `DATABASE` parameters are correct.
    *  Make sure the `Trusted_Connection` parameter is correct based on your sql server configurations.

4.  **Set a Secret Key:** Modify the `app.config["SHHHH_CHUP"]` setting in your `main.py` to a strong, secret key. Do not use `"prabe.sh"` in production! Generate a unique one.

## Usage

### Running the Application

To start the Flask development server, run:
   ```bash
   python main.py
   ```
 The server will be available at http://127.0.0.1:5000 by default.

### Endpoints

**Signup (`/signup`)**

*   **Method:** `POST`
*   **Request Body:** JSON containing `first_name`, `last_name`, `email`, `username`, and `password`.
*   **Response:**
    *   `201 OK`: Signup successful with message `{"message": "Signup successful"}`
    *   `409 Conflict`: Username or email already exists with error message `{"error": "Username or email already exists"}`.
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.

**Login (`/login`)**

*   **Method:** `POST`
*   **Request Body:** JSON containing `username_or_email` and `password`.
*   **Response:**
    *   `200 OK`: Login successful, returning a JWT token `{"message": "Login successful", "token": "YOUR_JWT_TOKEN"}`.
    *   `401 Unauthorized`: Invalid username/email or password with error message `{"error": "Invalid username/email or password"}`.
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.

**Get All Available Courses (`/allAvailableCourses`)**

*   **Method:** `GET`
*   **Headers:** Add an `Authorization` header with the value `Bearer YOUR_JWT_TOKEN`.
*   **Query Parameters:**
    *   `level_id` (integer, optional):  Filters results to show groups within a specific educational level. If provided alone, it returns course groups for that level.
    *   `group_id` (integer, optional): Filters results to show courses within a specific group. Requires `level_id` to be specified. If provided with `level_id`, it returns courses within that group.
    *   `course_id` (integer, optional): Filters results to show subjects within a specific course. Requires both `level_id` and `group_id` to be specified. If provided with `level_id` and `group_id`, it returns subjects within that course.
    *   `subject_id` (integer, optional): Filters results to show hierarchical content within a specific subject. Requires `level_id`, `group_id` and `course_id` to be specified. If provided with `level_id`, `group_id` and `course_id`, it returns top-level content nodes for that subject.
    *   `parent_id` (integer, optional): Filters hierarchical content to only return items that are children of the specified `parent_id`. Requires `level_id`, `group_id`, `course_id`, and `subject_id` to be specified. Used to fetch child content nodes under a specific parent.

    * **If no parameters are provided:** Returns a list of all educational levels.

*   **Response:**
    *   `200 OK`: Returns a list of educational levels, groups, courses, subjects or hierarchical content based on the provided query parameters.
    *   `400 Bad Request`: Invalid request parameters, returns message `{"error": "Invalid request parameters"}`.
    *   `401 Unauthorized`: If the token is missing or invalid, returns message `{"error": "Token is missing!"}` or `{"error": "Token is invalid!"}`.
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.

    **Example Response for levels (no parameters):**
    ```json
        [
          {
            "type": "level",
            "id": 1,
            "name": "High School",
            "svg": "<svg>...</svg>",
            "progress": 67
          },
          {
            "type": "level",
            "id": 2,
            "name": "Middle School",
            "svg": "<svg>...</svg>",
            "progress": 23
          },
           {
            "type": "level",
            "id": 3,
            "name": "Primary School",
            "svg": "<svg>...</svg>",
            "progress": 95
          }
        ]
    ```

    **Example Response for groups (with `level_id=1`):**
    ```json
       [
          {
           "type": "group",
            "id": 1,
            "name": "Grade 9",
            "svg": "<svg>...</svg>",
            "progress": 55
           },
           {
             "type": "group",
             "id": 2,
             "name": "Grade 10",
             "svg": "<svg>...</svg>",
             "progress": 32
            }
          ]
    ```

     **Example Response for courses (with `level_id=1&group_id=1`):**
    ```json
        [
          {
            "type": "course",
            "id": 1,
            "name": "Math",
            "svg": "<svg>...</svg>",
            "progress": 78
           },
           {
            "type": "course",
             "id": 2,
             "name": "Science",
             "svg": "<svg>...</svg>",
             "progress": 49
            }
         ]
    ```

     **Example Response for subjects (with `level_id=1&group_id=1&course_id=1`):**
    ```json
       [
           {
             "type": "subject",
             "id": 1,
             "name": "Algebra",
              "description": "Basic Algebra concepts",
              "svg": "/static/subject_graphics/algebra.svg",
              "progress": 89
            },
            {
            "type": "subject",
            "id": 2,
            "name": "Geometry",
            "description": "Basic Geometry concepts",
            "svg": "/static/subject_graphics/geometry.svg",
            "progress": 61
          }
        ]
    ```

     **Example Response for hierarchical content (with `level_id=1&group_id=1&course_id=1&subject_id=1`):**
    ```json
       [
          {
            "type": "content",
            "id": 1,
            "name": "Introduction",
            "parent_id": null,
            "svg": "/static/content_graphics/introduction.svg",
            "progress": 92,
            "containsChildren": true
          },
          {
            "type": "content",
            "id": 2,
            "name": "Variables",
            "parent_id": 1,
            "svg": "/static/content_graphics/variables.svg",
            "progress": 75,
            "containsChildren": true
          },
           {
            "type": "content",
            "id": 3,
            "name": "Equations",
            "parent_id": 1,
            "svg": "/static/content_graphics/equations.svg",
            "progress": 80,
            "containsChildren": false
           }
        ]
    ```

**Get Lectures for Content (`/lectures/<int:content_id>`)**

*   **Method:** `GET`
*   **Headers:** Add an `Authorization` header with the value `Bearer YOUR_JWT_TOKEN`.
*  **Path Parameters:**
    *  `content_id` (integer, required): The ID of the content to retrieve lectures for.
*   **Response:**
    *   `200 OK`: Returns a list of lectures for the given `content_id`.
    *    `401 Unauthorized`: If the token is missing or invalid, returns message `{"error": "Token is missing!"}` or `{"error": "Token is invalid!"}`.
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.

    **Example Response:**
    ```json
        [
          {
            "lecture_id": 1,
            "lecture_no": 1,
            "lecture_link": "http://localhost:5000/stream?file_path=./assets/a.webm",
             "lecture_data": null,
            "title": "Introduction to Algebra",
            "description": "What is algebra?"
           },
          {
           "lecture_id": 2,
            "lecture_no": 2,
            "lecture_link": "http://localhost:5000/stream?file_path=./assets/b.mp4",
             "lecture_data": null,
            "title": "Basic Variables",
            "description": "Basic algebra variables"
            }
         ]
    ```

**Get Home Page Trailers (`/dynamicHome`)**

*   **Method:** `GET`
*   **Response:**
    *   `200 OK`: Returns a list of courses for the home page.

        **Example Response:**
        ```json
          [
            {
             "Id": 1,
             "Title": "Math for Beginners",
             "Duration": "12 hours",
             "Image": "/static/images/math.png"
            },
           {
             "Id": 2,
             "Title": "Intro to Science",
             "Duration": "10 hours",
             "Image": "/static/images/science.png"
            }
           ]
       ```

    *   `404 Not Found` : If no courses are found returns `{"error": "No courses found"}`.

**Get Active User (`/activeUser`)**

*   **Method:** `GET`
*   **Headers:** Add an `Authorization` header with the value `Bearer YOUR_JWT_TOKEN`.
*   **Response:**
    *   `200 OK`: Returns the data of the currently logged-in user.
    *   `401 Unauthorized`: If the token is missing or invalid, returns message `{"error": "Token is missing!"}` or `{"error": "Token is invalid!"}`.
    *   `404 Not Found`: If the user is not found (should not happen if token is valid). Returns `{"error": "User not found"}`
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.

    **Example Response:**
    ```json
    {
        "UserID": 1,
        "FirstName": "John",
        "LastName": "Doe",
        "Email": "john.doe@example.com",
        "Username": "johndoe",
        "PasswordHash": "...",
        "CreatedAt": "2024-07-28T10:00:00",
        "UpdatedAt": "2024-07-28T10:00:00",
        "Initialized": true,
        "SubscriptionStatus": "active",
        "CourseLevel": 1,
        "GroupLevel": 1,
        "ProfilePic": "/static/profiles/default.png",
        "Bio": "Learning enthusiast"
    }
    ```

**Update Active User (`/activeUser`)**

*   **Method:** `PUT`
*   **Headers:** Add an `Authorization` header with the value `Bearer YOUR_JWT_TOKEN`.
*   **Request Body:** JSON containing the fields to be updated for the user. All fields are optional; only provided fields will be updated.
*   **Response:**
    *   `200 OK`: User updated successfully. Returns `{"message": "User updated successfully"}`.
    *   `401 Unauthorized`: If the token is missing or invalid, returns message `{"error": "Token is missing!"}` or `{"error": "Token is invalid!"}`.
    *   `404 Not Found`: If the user is not found (should not happen if token is valid). Returns `{"error": "User not found"}`
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.

    **Example Request Body:**
    ```json
    {
        "FirstName": "Johnny",
        "Bio": "Experienced learner"
    }
    ```

**Get Available Levels and Groups (`/availableLevelsAndGroups`)**

*   **Method:** `GET`
*   **Headers:** Add an `Authorization` header with the value `Bearer YOUR_JWT_TOKEN`.
*   **Response:**
    *   `200 OK`: Returns a JSON object containing lists of `educational_levels` and `course_groups`.
    *   `401 Unauthorized`: If the token is missing or invalid, returns message `{"error": "Token is missing!"}` or `{"error": "Token is invalid!"}`.
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.

    **Example Response:**
    ```json
    {
        "educational_levels": [
            {
                "level_id": 1,
                "level_name": "High School",
                "Graphic": "/static/level_graphics/high_school.svg"
            },
            {
                "level_id": 2,
                "level_name": "Middle School",
                "Graphic": "/static/level_graphics/middle_school.svg"
            }
        ],
        "course_groups": [
            {
                "group_id": 1,
                "group_name": "Grade 9",
                "Graphic": "/static/group_graphics/grade_9.svg"
            },
            {
                "group_id": 2,
                "group_name": "Grade 10",
                "Graphic": "/static/group_graphics/grade_10.svg"
            }
        ]
    }
    ```

**Stream Media (`/stream`)**

*   **Method:** `GET`
*   **Query Parameters:**
    *   `file_path` (string, required): The path to the media file to stream.  *Currently, this path is for internal server-side files and is for development/demonstration purposes.*  **In a production environment, media streaming would typically be handled via more secure and robust methods.**
*   **Response:**
    *   `200 OK`: Streams the requested media file.
    *   `500 Internal Server Error`: If an error occurred while streaming the file.

    **Example Usage:** `http://localhost:5000/stream?file_path=./assets/a.webm` (This is a simplified example; in practice, `file_path` handling might be more complex and secured).

## Contributing

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Make your changes and commit them with descriptive messages.
4.  Create a pull request to the main branch.

## License

MIT

## Acknowledgements

*   This project uses the following libraries:
    *   Flask: Web framework
    *   pyodbc: Database interaction
    *   PyJWT: JWT encoding/decoding
    *   Werkzeug: Security functions (password hashing)

## Author

*   Author: Prabesh Aryal
*   Email: hello@prabe.sh
*   Github: https://github.com/prabeshAryal