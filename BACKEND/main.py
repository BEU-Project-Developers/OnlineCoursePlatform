import pyodbc
import jwt
import datetime

from flask import Flask, request, jsonify
from werkzeug.security import generate_password_hash, check_password_hash
from functools import wraps


app = Flask(__name__)
app.config["SHHHH_CHUP"] = "prabe.sh"  # Replace with a strong secret key

# Database connection strings
DATABASE_CONFIG = {
    "DRIVER": "{ODBC Driver 17 for SQL Server}",
    "SERVER": "MY2NDGF",
    "DATABASE": "pacademy",
    "Trusted_Connection": "Yes",
}

COURSES_DATABASE_CONFIG = {
    "DRIVER": "{ODBC Driver 17 for SQL Server}",
    "SERVER": "MY2NDGF",
    "DATABASE": "courses",
    "Trusted_Connection": "Yes",
}


def get_db_connection(database_config):
    try:
        conn = pyodbc.connect(
            f"DRIVER={database_config['DRIVER']};"
            f"SERVER={database_config['SERVER']};"
            f"DATABASE={database_config['DATABASE']};"
            f"Trusted_Connection={database_config['Trusted_Connection']};"
        )
        return conn
    except Exception as e:
        print(f"Database connection error: {e}")
        raise

# Authentication decorator
def token_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        token = None

        if "Authorization" in request.headers:
            token = request.headers["Authorization"].split(" ")[1]

        if not token:
            return jsonify({"error": "Token is missing!"}), 401

        try:
            jwt.decode(token, app.config["SHHHH_CHUP"], algorithms=["HS256"])
        except:
            return jsonify({"error": "Token is invalid!"}), 401

        return f(*args, **kwargs)

    return decorated


# Login endpoint
@app.route("/login", methods=["POST"])
def login():
    data = request.json
    username_or_email = data["username_or_email"]
    password = data["password"]

    try:
        # Connect to the database
        conn = get_db_connection(DATABASE_CONFIG)
        cursor = conn.cursor()

        # Query to retrieve the stored password hash
        query = """
        SELECT UserId, PasswordHash FROM Users
        WHERE Username = ? OR Email = ?
        """
        cursor.execute(query, (username_or_email, username_or_email))
        result = cursor.fetchone()

        if result:
            user_id, stored_password_hash = result
            # Compare the entered password with the stored hash
            if check_password_hash(stored_password_hash, password):
                # Generate JWT
                token = jwt.encode(
                    {
                        "user_id": user_id,
                        "exp": datetime.datetime.utcnow()
                        + datetime.timedelta(minutes=30),
                    },
                    app.config["SHHHH_CHUP"],
                    algorithm="HS256",
                )
                return jsonify({"message": "Login successful", "token": token}), 200
            else:
                return jsonify({"error": "Invalid username/email or password"}), 401
        else:
            return jsonify({"error": "Invalid username/email or password"}), 401
    except Exception as e:
        return jsonify({"error": f"An error occurred: {str(e)}"}), 500
    finally:
        conn.close()

# Signup endpoint
@app.route("/signup", methods=["POST"])
def signup():
    data = request.json
    first_name = data["first_name"]
    last_name = data["last_name"]
    email = data["email"]
    username = data["username"]
    password = data["password"]

    try:
        # Connect to the database
        conn = get_db_connection(DATABASE_CONFIG)
        cursor = conn.cursor()

        # Check if the username or email already exists
        check_query = """
        SELECT COUNT(*) FROM Users
        WHERE Username = ? OR Email = ?
        """
        cursor.execute(check_query, (username, email))
        user_count = cursor.fetchone()[0]

        if user_count > 0:
            return jsonify({"error": "Username or email already exists"}), 409

        # Hash the password
        hashed_password = generate_password_hash(password)

        # Insert the new user
        insert_query = """
        INSERT INTO Users (FirstName, LastName, Email, Username, PasswordHash)
        VALUES (?, ?, ?, ?, ?)
        """
        cursor.execute(
            insert_query, (first_name, last_name, email, username, hashed_password)
        )
        conn.commit()

        return jsonify({"message": "Signup successful"}), 201
    except Exception as e:
        return jsonify({"error": f"An error occurred: {str(e)}"}), 500
    finally:
        conn.close()


@app.route("/allAvailableCourses", methods=["GET"])
@token_required
def get_all_available_courses():
    level_id = request.args.get("level_id", type=int)
    group_id = request.args.get("group_id", type=int)
    course_id = request.args.get("course_id", type=int)
    subject_id = request.args.get("subject_id", type=int)
    parent_id = request.args.get("parent_id", type=int)

    try:
        conn = get_db_connection(COURSES_DATABASE_CONFIG)
        cursor = conn.cursor()

        if (
            level_id is None
            and group_id is None
            and course_id is None
            and subject_id is None
            and parent_id is None
        ):
            # Fetch Educational Levels
            query = "SELECT level_id, level_name FROM EducationalLevels"
            cursor.execute(query)
            levels = cursor.fetchall()

            data = [
                {"type": "level", "id": level[0], "name": level[1]} for level in levels
            ]
            return jsonify(data), 200

        elif (
            level_id is not None
            and group_id is None
            and course_id is None
            and subject_id is None
            and parent_id is None
        ):
            # Fetch Groups for a level
            query = "SELECT group_id, map_description FROM EducationalLevel_CoursesMapping WHERE level_id = ?"
            cursor.execute(query, level_id)
            groups = cursor.fetchall()
            data = [
                {"type": "group", "id": group[0], "name": group[1], "graphic":""} for group in groups
            ]
            return jsonify(data), 200

        elif (
            level_id is not None
            and group_id is not None
            and course_id is None
            and subject_id is None
            and parent_id is None
        ):
            # Fetch Courses for a group
            query = """
                SELECT Courses.course_id, Courses.course_name 
                FROM Courses 
                INNER JOIN CourseMapping ON Courses.course_id = CourseMapping.course_id 
                WHERE CourseMapping.group_id = ?
            """
            cursor.execute(query, group_id)
            courses = cursor.fetchall()
            data = [
                {"type": "course", "id": course[0], "name": course[1]}
                for course in courses
            ]
            return jsonify(data), 200

        elif (
            level_id is not None
            and group_id is not None
            and course_id is not None
            and subject_id is None
            and parent_id is None
        ):
            # Fetch subjects or single subject id
            # Check subject count
            query_check_subjects = (
                "SELECT COUNT(*) FROM SubjectCourseMappings WHERE course_id = ?"
            )
            cursor.execute(query_check_subjects, course_id)
            subject_count = cursor.fetchone()[0]
            query_subjects = """
                    SELECT Subjects.subject_id, Subjects.subject_name, Subjects.description
                    FROM Subjects
                    INNER JOIN SubjectCourseMappings ON Subjects.subject_id = SubjectCourseMappings.subject_id
                    WHERE SubjectCourseMappings.course_id = ?
                """
            cursor.execute(query_subjects, course_id)
            subjects = cursor.fetchall()
            data = [
                {
                    "type": "subject",
                    "id": subject[0],
                    "name": subject[1],
                    "description": subject[2],
                }
                for subject in subjects
            ]

            return jsonify(data), 200
        elif (
            level_id is not None
            and group_id is not None
            and course_id is not None
            and subject_id is not None
        ):
            # Fetch Hierarchical Content for a subject
            query = """
                    SELECT content_id, content_name, parent_id
                    FROM HierarchicalContent
                    WHERE subject_id = ?
                """
            cursor.execute(query, subject_id)
            contents = cursor.fetchall()

            content_list = []

            for content in contents:
                has_children = False
                for check_content in contents:
                    if check_content[2] == content[0]:
                        has_children = True
                        break

                if parent_id is None:
                    if content[2] is None:
                        content_list.append(
                            {
                                "content_id": content[0],
                                "content_name": content[1],
                                "parent_id": content[2],
                                "containsChildren": has_children,
                            }
                        )
                elif content[2] == parent_id:
                    content_list.append(
                        {
                            "content_id": content[0],
                            "content_name": content[1],
                            "parent_id": content[2],
                            "containsChildren": has_children,
                        }
                    )
            return jsonify(content_list), 200

        else:
            return jsonify({"error": "Invalid request parameters"}), 400

    except Exception as e:
        return jsonify({"error": f"An error occurred: {str(e)}"}), 500
    finally:
        conn.close()


@app.route("/lectures/<int:content_id>", methods=["GET"])
@token_required
def get_lectures_for_content(content_id):
    try:
        conn = get_db_connection(COURSES_DATABASE_CONFIG)
        cursor = conn.cursor()

        query = "SELECT lecture_id, lecture_no, lecture_link, lecture_data, title, description FROM Lectures WHERE content_id = ?"
        cursor.execute(query, content_id)
        lectures = cursor.fetchall()

        lecture_list = [
            {
                "lecture_id": lecture[0],
                "lecture_no": lecture[1],
                "lecture_link": lecture[2],
                "lecture_data": lecture[3],
                "title": lecture[4],
                "description": lecture[5],
            }
            for lecture in lectures
        ]
        return jsonify(lecture_list), 200

    except Exception as e:
        return jsonify({"error": f"An error occurred: {str(e)}"}), 500
    finally:
        if "conn" in locals() and conn:
            conn.close()

# Dynamic route for fetching all courses
@app.route("/dynamicHome", methods=["GET"])
def dynamic_home():
    # Establish database connection
    conn = get_db_connection(COURSES_DATABASE_CONFIG)

    # Query the database for all the courses
    course = conn.execute(
        "SELECT TOP 8 * FROM HomepageTrailers ORDER BY NEWID()"
    ).fetchall()

    # print(course)
    # Close the connection
    conn.close()

    # If no courses found
    if not course:
        return jsonify({"error": "No courses found"}), 404

    # Prepare data for the response
    courses_data = [
        {
            "Id": course[0],
            "Title": course[1],
            "Duration": course[2],
            "Image": course[3],
        }
        for course in course
    ]

    # Return the courses as a JSON response
    return jsonify(courses_data)


if __name__ == "__main__":
    app.run(debug=True)
