import pyodbc
import jwt
import datetime
import random

from flask import Flask, request, jsonify, url_for, send_file
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
            query = "SELECT level_id, level_name, graphic FROM EducationalLevels"
            cursor.execute(query)
            levels = cursor.fetchall()

            data = [
                {
                    "type": "level",
                    "id": level[0],
                    "name": level[1],
                    "svg": level[2],  # Extracting 'Graphic' column
                    "progress": random.randint(1, 100),  # Random progress value
                }
                for level in levels
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
            query = "SELECT group_id, map_description, graphic FROM EducationalLevel_CoursesMapping WHERE level_id = ?"
            cursor.execute(query, level_id)
            groups = cursor.fetchall()
            data = [
                {
                    "type": "group",
                    "id": group[0],
                    "name": group[1],
                    "svg": group[2],  # Extracting 'Graphic' column
                    "progress": random.randint(1, 100),  # Random progress value
                }
                for group in groups
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
                SELECT Courses.course_id, Courses.course_name, Courses.graphic 
                FROM Courses 
                INNER JOIN CourseMapping ON Courses.course_id = CourseMapping.course_id 
                WHERE CourseMapping.group_id = ?
            """
            cursor.execute(query, group_id)
            courses = cursor.fetchall()
            data = [
                {
                    "type": "course",
                    "id": course[0],
                    "name": course[1],
                    "svg": course[2],  # Extracting 'Graphic' column
                    "progress": random.randint(1, 100),  # Random progress value
                }
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
            query_check_subjects = (
                "SELECT COUNT(*) FROM SubjectCourseMappings WHERE course_id = ?"
            )
            cursor.execute(query_check_subjects, course_id)
            subject_count = cursor.fetchone()[0]
            query_subjects = """
                SELECT Subjects.subject_id, Subjects.subject_name, Subjects.description, Subjects.graphic
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
                    "svg": subject[3],  # Extracting 'Graphic' column
                    "progress": random.randint(1, 100),  # Random progress value
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
                SELECT content_id, content_name, parent_id, graphic
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
                                "type":"content",
                                "id": content[0],
                                "name": content[1],
                                "parent_id": content[2],
                                "svg": content[3],  # Extracting 'Graphic' column
                                "progress": random.randint(1, 100),  # Random progress value
                                "containsChildren": has_children,
                            }
                        )
                elif content[2] == parent_id:
                    content_list.append(
                        {
                            "type": "content",
                            "id": content[0],
                            "name": content[1],
                            "parent_id": content[2],
                            "svg": content[3],  # Extracting 'Graphic' column
                            "progress": random.randint(1, 100),  # Random progress value
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

def generate_streamable_url(file_path):
    """
    Generate a URL to stream the specified file.
    """
    filenames = [
        "a.webm",
        "b.mp4",
        "c.webm",
        "d.webm",
        "e.webm",
        "f.mkv",
        "g.mp4",
        "h.mkv",
        "hehe.webm",
    ]
    rand_name= random.choice(filenames)
    file_path = f"./assets/{rand_name}"

    return url_for("stream_media", file_path=file_path, _external=True)

@app.route("/stream", methods=["GET"])
def stream_media():
    """
    Serve a media file as a streamable response.
    """
    file_path = request.args.get("file_path")  # Get file path from query parameter
    try:
        return send_file(
            file_path,
            as_attachment=False,
            mimetype="video/mp4",
        )
    except Exception as e:
        return f"Error streaming file: {str(e)}", 500
# @token_required


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
                "lecture_link": generate_streamable_url(lecture[2]),
                "lecture_data": lecture[3],
                "title": lecture[4],
                "description": lecture[5],
            }
            for lecture in lectures
        ]
        print(jsonify(lecture_list))
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

# Route for viewing or updating the active user
@app.route("/activeUser", methods=["GET", "PUT"])
@token_required
def active_user():
    try:
        # Decode user ID from token
        token = request.headers.get("Authorization").split(" ")[1]
        decoded_token = jwt.decode(
            token, app.config["SHHHH_CHUP"], algorithms=["HS256"]
        )
        user_id = decoded_token["user_id"]

        conn = get_db_connection(DATABASE_CONFIG)
        cursor = conn.cursor()

        if request.method == "GET":
            # Get the active user data
            query = """
                SELECT UserID, FirstName, LastName, Email, Username, PasswordHash, CreatedAt, UpdatedAt, Initialized, 
                       SubscriptionStatus, CourseLevel, GroupLevel, ProfilePic, Bio
                FROM dbo.Users
                WHERE UserID = ?
            """
            cursor.execute(query, user_id)
            user = cursor.fetchone()

            if not user:
                return jsonify({"error": "User not found"}), 404

            user_data = {
                "UserID": user.UserID,
                "FirstName": user.FirstName,
                "LastName": user.LastName,
                "Email": user.Email,
                "Username": user.Username,
                "PasswordHash": user.PasswordHash,
                "CreatedAt": user.CreatedAt,
                "UpdatedAt": user.UpdatedAt,
                "Initialized": user.Initialized,
                "SubscriptionStatus": user.SubscriptionStatus,
                "CourseLevel": user.CourseLevel,
                "GroupLevel": user.GroupLevel,
                "ProfilePic": user.ProfilePic,
                "Bio": user.Bio,
            }

            return jsonify(user_data)

        elif request.method == "PUT":
            # Get the current user data
            query = """
                SELECT FirstName, LastName, Email, Username, PasswordHash, Initialized, SubscriptionStatus, 
                       CourseLevel, GroupLevel, ProfilePic, Bio
                FROM dbo.Users
                WHERE UserID = ?
            """
            cursor.execute(query, user_id)
            current_user = cursor.fetchone()

            if not current_user:
                return jsonify({"error": "User not found"}), 404

            # Get updated data from the request
            data = request.get_json()
            updated_data = {
                "FirstName": data.get("FirstName", current_user.FirstName),
                "LastName": data.get("LastName", current_user.LastName),
                "Email": data.get("Email", current_user.Email),
                "Username": data.get("Username", current_user.Username),
                "PasswordHash": data.get("PasswordHash", current_user.PasswordHash),
                "Initialized": data.get("Initialized", current_user.Initialized),
                "SubscriptionStatus": data.get(
                    "SubscriptionStatus", current_user.SubscriptionStatus
                ),
                "CourseLevel": data.get("CourseLevel", current_user.CourseLevel),
                "GroupLevel": data.get("GroupLevel", current_user.GroupLevel),
                "ProfilePic": data.get("ProfilePic", current_user.ProfilePic),
                "Bio": data.get("Bio", current_user.Bio),
            }

            # Update query
            update_query = """
                UPDATE dbo.Users
                SET FirstName = ?, LastName = ?, Email = ?, Username = ?, PasswordHash = ?, Initialized = ?,
                    SubscriptionStatus = ?, CourseLevel = ?, GroupLevel = ?, ProfilePic = ?, Bio = ?, UpdatedAt = GETDATE()
                WHERE UserID = ?
            """

            cursor.execute(
                update_query,
                updated_data["FirstName"],
                updated_data["LastName"],
                updated_data["Email"],
                updated_data["Username"],
                updated_data["PasswordHash"],
                updated_data["Initialized"],
                updated_data["SubscriptionStatus"],
                updated_data["CourseLevel"],
                updated_data["GroupLevel"],
                updated_data["ProfilePic"],
                updated_data["Bio"],
                user_id,
            )

            conn.commit()

            return jsonify({"message": "User updated successfully"})

    except Exception as e:
        return jsonify({"error": str(e)}), 500

    finally:
        conn.close()

# Route for getting available levels and course groups
@app.route("/availableLevelsAndGroups", methods=["GET"])
@token_required
def get_available_levels_and_groups():
    try:
        conn = get_db_connection(COURSES_DATABASE_CONFIG)
        cursor = conn.cursor()

        # Query for educational levels
        levels_query = """
            SELECT level_id, level_name, Graphic
            FROM dbo.EducationalLevels
        """
        cursor.execute(levels_query)
        levels = cursor.fetchall()

        # Query for course groups
        groups_query = """
            SELECT group_id, group_name, Graphic
            FROM dbo.CourseGroups
        """
        cursor.execute(groups_query)
        groups = cursor.fetchall()

        levels_data = [
            {
                "level_id": level.level_id,
                "level_name": level.level_name,
                "Graphic": level.Graphic,
            }
            for level in levels
        ]

        groups_data = [
            {
                "group_id": group.group_id,
                "group_name": group.group_name,
                "Graphic": group.Graphic,
            }
            for group in groups
        ]

        return jsonify(
            {"educational_levels": levels_data, "course_groups": groups_data}
        )

    except Exception as e:
        return jsonify({"error": str(e)}), 500

    finally:
        conn.close()


if __name__ == "__main__":
    app.run(debug=True)
