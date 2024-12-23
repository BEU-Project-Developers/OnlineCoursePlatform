# Authentication and Course API

This Flask API provides user authentication (signup and login) and access to course information. It uses JWT for authentication and connects to two SQL Server databases (`pacademy` for users and `courses` for course data).

## Description

This API allows users to:

*   **Signup:** Create new user accounts with a username, email, and password.
*   **Login:** Authenticate existing users and receive a JWT token.
*   **Access Courses:** Retrieve hierarchical course information, including educational levels, groups, courses, subjects, and lectures.
*   **View Homepage Trailers:** Get a dynamic list of homepage trailers for display.

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
*   **Parameters:**
    *   `level_id` (int, optional): Filter courses by educational level.
    *   `group_id` (int, optional): Filter courses by group. Requires `level_id`.
    *   `course_id` (int, optional): Filter courses by course. Requires `level_id` and `group_id`.
    *  `subject_id` (int, optional): Filter courses by subject. Requires `level_id`, `group_id` and `course_id`.
    *  `parent_id` (int, optional): Filter content by parent id. Requires `level_id`, `group_id`, `course_id` and `subject_id`
*   **Response:**
    *   `200 OK`: Returns a list of educational levels, groups, courses, subjects or hierarchical content based on the filters.
    *   `400 Bad Request`: Invalid request parameters, returns message `{"error": "Invalid request parameters"}`.
    *   `401 Unauthorized`: If the token is missing or invalid. returns message `{"error": "Token is missing!"}` or `{"error": "Token is invalid!"}`.
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.
    
    **Example Response for levels:**
    ```json
        [
          {
            "type": "level",
            "id": 1,
            "name": "High School"
          },
          {
            "type": "level",
            "id": 2,
            "name": "Middle School"
          },
           {
            "type": "level",
            "id": 3,
            "name": "Primary School"
          }
        ]
    ```
   
    **Example Response for groups:**
    ```json
       [
          {
           "type": "group",
            "id": 1,
            "name": "Grade 9"
           },
           {
             "type": "group",
             "id": 2,
             "name": "Grade 10"
            }
          ]
    ```
     **Example Response for courses:**
    ```json
        [
          {
            "type": "course",
            "id": 1,
            "name": "Math"
           },
           {
            "type": "course",
             "id": 2,
             "name": "Science"
            }
         ]
    ```
    
     **Example Response for subjects:**
    ```json
       [
           {
             "type": "subject",
             "id": 1,
             "name": "Algebra",
              "description": "Basic Algebra concepts"
            },
            {
            "type": "subject",
            "id": 2,
            "name": "Geometry",
            "description": "Basic Geometry concepts"
          }
        ]
    ```
     **Example Response for hierarchical content:**
    ```json
       [
          {
            "content_id": 1,
            "content_name": "Introduction",
            "parent_id": null,
            "containsChildren": true
          },
          {
            "content_id": 2,
            "content_name": "Variables",
            "parent_id": 1,
            "containsChildren": true
          },
           {
            "content_id": 3,
            "content_name": "Equations",
            "parent_id": 1,
            "containsChildren": true
           }
        ]
    ```
**Get Lectures for Content (`/lectures/<int:content_id>`)**

*   **Method:** `GET`
*   **Headers:** Add an `Authorization` header with the value `Bearer YOUR_JWT_TOKEN`.
*  **Path Parameters:**
    *  `content_id` (int, required): The id of the content to get lectures for.
*   **Response:**
    *   `200 OK`: Returns a list of lectures for the given `content_id`.
    *    `401 Unauthorized`: If the token is missing or invalid. returns message `{"error": "Token is missing!"}` or `{"error": "Token is invalid!"}`.
    *   `500 Internal Server Error`: If an error occurred on the server `{"error": "An error occurred"}`.
    
    **Example Response:**
    ```json
        [
          {
            "lecture_id": 1,
            "lecture_no": 1,
            "lecture_link": "https://example.com/video1.mp4",
             "lecture_data": "{'lecture_type': 'video'}",
            "title": "Introduction to Algebra",
            "description": "What is algebra?"
           },
          {
           "lecture_id": 2,
            "lecture_no": 2,
            "lecture_link": "https://example.com/video2.mp4",
             "lecture_data": "{'lecture_type': 'video'}",
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
