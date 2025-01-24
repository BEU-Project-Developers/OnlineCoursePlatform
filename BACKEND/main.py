import pyodbc
import jwt
import datetime
import random

from flask import Flask, request, jsonify, url_for, send_file
from werkzeug.security import generate_password_hash, check_password_hash
from functools import wraps


app = Flask(__name__)
app.config["SHHHH_CHUP"] = "prabe.sh"

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
    conn = None
    try:
        conn = pyodbc.connect(
            f"DRIVER={database_config['DRIVER']};"
            f"SERVER={database_config['SERVER']};"
            f"DATABASE={database_config['DATABASE']};"
            f"Trusted_Connection={database_config['Trusted_Connection']};"
        )
        return conn
    except pyodbc.Error as ex:
        sqlstate = ex.args[0]
        if sqlstate == "28000":
            print("Database login failed")
            return None  # or raise exception, handle as needed
        else:
            print(f"Database connection error: {ex}")
            return None  # or raise exception, handle as needed
    except Exception as e:
        print(f"General database connection error: {e}")
        return None  # or raise exception, handle as needed


def token_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        token = None
        auth_header = request.headers.get("Authorization")

        if not auth_header:
            return jsonify({"error": "Token is missing!"}), 401

        try:
            token_parts = auth_header.split()
            if len(token_parts) != 2 or token_parts[0].lower() != "bearer":
                return jsonify({"error": "Invalid token format!"}), 401
            token = token_parts[1]
        except Exception:
            return jsonify({"error": "Invalid authorization header!"}), 401

        if not token:
            return jsonify({"error": "Token is missing!"}), 401

        try:
            jwt.decode(token, app.config["SHHHH_CHUP"], algorithms=["HS256"])
        except jwt.ExpiredSignatureError:
            return jsonify({"error": "Token has expired!"}), 401
        except jwt.InvalidTokenError:
            return jsonify({"error": "Token is invalid!"}), 401
        except Exception as e:
            return jsonify({"error": f"Token verification failed: {str(e)}"}), 401

        return f(*args, **kwargs)

    return decorated


@app.route("/login", methods=["POST"])
def login():
    data = request.json
    if not data or "username_or_email" not in data or "password" not in data:
        return jsonify({"error": "Missing username_or_email or password"}), 400

    username_or_email = data.get("username_or_email")
    password = data.get("password")

    conn = None
    try:
        conn = get_db_connection(DATABASE_CONFIG)
        if not conn:
            return jsonify({"error": "Database connection failed"}), 500
        cursor = conn.cursor()

        query = """
        SELECT UserId, PasswordHash FROM Users
        WHERE Username = ? OR Email = ?
        """
        cursor.execute(query, (username_or_email, username_or_email))
        result = cursor.fetchone()

        if result:
            user_id, stored_password_hash = result
            if check_password_hash(stored_password_hash, password):
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
        print(f"Login error: {e}")  # Log the error for debugging
        return jsonify({"error": "Login failed"}), 500  # Generic error to user
    finally:
        if conn:
            conn.close()


@app.route("/signup", methods=["POST"])
def signup():
    data = request.json
    required_fields = ["first_name", "last_name", "email", "username", "password"]
    if not data or not all(field in data for field in required_fields):
        return jsonify({"error": "Missing required fields"}), 400

    first_name = data.get("first_name")
    last_name = data.get("last_name")
    email = data.get("email")
    username = data.get("username")
    password = data.get("password")

    conn = None
    try:
        conn = get_db_connection(DATABASE_CONFIG)
        if conn is None:
            return jsonify({"error": "Database connection failed"}), 500

        cursor = conn.cursor()

        check_query = """
        SELECT COUNT(*) FROM Users
        WHERE Username = ? OR Email = ?
        """
        cursor.execute(check_query, (username, email))
        user_count = cursor.fetchone()[0]

        if user_count > 0:
            return jsonify({"error": "Username or email already exists"}), 409

        hashed_password = generate_password_hash(password)

        insert_query = """
        INSERT INTO Users (FirstName, LastName, Email, Username, PasswordHash)
        VALUES (?, ?, ?, ?, ?)
        """
        cursor.execute(
            insert_query, (first_name, last_name, email, username, hashed_password)
        )
        conn.commit()

        return jsonify({"message": "Signup successful"}), 201
    except pyodbc.IntegrityError as e:
        if "Violation of UNIQUE KEY constraint" in str(e):
            return jsonify({"error": "Username or email already exists"}), 409
        else:
            print(f"Signup database error: {e}")  # Log specific DB error
            return jsonify({"error": "Signup failed due to database error"}), 500
    except Exception as e:
        print(f"Signup error: {e}")  # Log general error
        return jsonify({"error": "Signup failed"}), 500
    finally:
        if conn:
            conn.close()


def get_courses_db_connection(database_config):
    conn = None
    try:
        conn = pyodbc.connect(
            f"DRIVER={database_config['DRIVER']};"
            f"SERVER={database_config['SERVER']};"
            f"DATABASE={database_config['DATABASE']};"
            f"Trusted_Connection={database_config['Trusted_Connection']};"
        )
        return conn
    except pyodbc.Error as ex:
        sqlstate = ex.args[0]
        if sqlstate == "28000":
            print("Courses DB login failed")
            return None
        else:
            print(f"Courses DB connection error: {ex}")
            return None
    except Exception as e:
        print(f"General Courses DB connection error: {e}")
        return None


def extract_ids_from_levels(
    token,
):
    course_level_string = None
    group_level_string = None
    course_level_id = None
    group_id = None
    user_id = None
    user_conn = None
    user_cursor = None
    courses_conn = None
    courses_cursor = None

    try:
        decoded_token = jwt.decode(
            token,
            app.config["SHHHH_CHUP"],
            algorithms=["HS256"],
        )
        user_id = decoded_token["user_id"]

        user_conn = get_db_connection(DATABASE_CONFIG)
        if not user_conn:
            print("Error connecting to user database in extract_ids")
            return None, None

        user_cursor = user_conn.cursor()

        user_query = """
            SELECT CourseLevel, GroupLevel
            FROM dbo.Users
            WHERE UserID = ?
        """
        user_cursor.execute(user_query, (user_id,))
        user_data = user_cursor.fetchone()

        if user_data:
            course_level_string = user_data[0]
            group_level_string = user_data[1]

        courses_conn = get_courses_db_connection(
            COURSES_DATABASE_CONFIG
        )  # Corrected function call
        if not courses_conn:
            print("Error connecting to courses database in extract_ids")
            return None, None
        courses_cursor = courses_conn.cursor()

        level_query = """
            SELECT level_id
            FROM dbo.EducationalLevels
            WHERE level_name = ?
        """
        courses_cursor.execute(level_query, (course_level_string,))
        level_data = courses_cursor.fetchone()
        if level_data:
            course_level_id = level_data[0]

        group_query = """
            SELECT group_id
            FROM dbo.CourseGroups
            WHERE group_name = ?
        """
        courses_cursor.execute(group_query, (group_level_string,))
        group_data = courses_cursor.fetchone()
        if group_data:
            group_id = group_data[0]

    except jwt.ExpiredSignatureError:
        print("Token has expired in extract_ids")
    except jwt.InvalidTokenError:
        print("Invalid token in extract_ids")
    except Exception as e:
        print(f"Error in extract_ids_from_levels: {e}")
    finally:
        if user_cursor:
            user_cursor.close()
        if user_conn:
            user_conn.close()
        if courses_cursor:
            courses_cursor.close()
        if courses_conn:
            courses_conn.close()

    return course_level_id, group_id


@app.route("/allAvailableCourses", methods=["GET"])
@token_required
def get_all_available_courses():
    token = None
    try:
        auth_header = request.headers.get("Authorization")
        if not auth_header:
            return jsonify({"error": "Authorization header missing"}), 401
        token_parts = auth_header.split()
        if len(token_parts) != 2 or token_parts[0].lower() != "bearer":
            return jsonify({"error": "Invalid token format"}), 401
        token = token_parts[1]
    except Exception as e:
        return jsonify({"error": f"Error extracting token: {str(e)}"}), 400

    level_id, group_id = extract_ids_from_levels(token)
    if (
        level_id is None
        and group_id is None
        and request.args.get("level_id") is None
        and request.args.get("group_id") is None
        and request.args.get("course_id") is None
        and request.args.get("subject_id") is None
        and request.args.get("parent_id") is None
    ):
        pass  # allow to fetch top level if no params are given either from token or URL params
    else:
        level_id_param = request.args.get("level_id", type=int)
        group_id_param = request.args.get("group_id", type=int)
        course_id = request.args.get("course_id", type=int)
        subject_id = request.args.get("subject_id", type=int)
        parent_id = request.args.get("parent_id", type=int)

        level_id = level_id_param if level_id_param is not None else level_id
        group_id = group_id_param if group_id_param is not None else group_id

    conn = None
    try:
        conn = get_courses_db_connection(COURSES_DATABASE_CONFIG)
        if not conn:
            return jsonify({"error": "Failed to connect to courses database"}), 500
        cursor = conn.cursor()

        if (
            level_id is None
            and group_id is None
            and course_id is None
            and subject_id is None
            and parent_id is None
        ):
            query = "SELECT level_id, level_name, graphic FROM EducationalLevels"
            cursor.execute(query)
            levels = cursor.fetchall()

            data = [
                {
                    "type": "level",
                    "id": level[0],
                    "name": level[1],
                    "svg": level[2],
                    "progress": random.randint(1, 100),
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
            query = "SELECT group_id, map_description, graphic FROM EducationalLevel_CoursesMapping WHERE level_id = ?"
            cursor.execute(query, level_id)
            groups = cursor.fetchall()
            data = [
                {
                    "type": "group",
                    "id": group[0],
                    "name": group[1],
                    "svg": group[2],
                    "progress": random.randint(1, 100),
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
                    "svg": course[2],
                    "progress": random.randint(1, 100),
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
                    "svg": subject[3],
                    "progress": random.randint(1, 100),
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
                                "type": "content",
                                "id": content[0],
                                "name": content[1],
                                "parent_id": content[2],
                                "svg": content[3],
                                "progress": random.randint(1, 100),
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
                            "svg": content[3],
                            "progress": random.randint(1, 100),
                            "containsChildren": has_children,
                        }
                    )
            return jsonify(content_list), 200

        else:
            return jsonify({"error": "Invalid request parameters"}), 400

    except Exception as e:
        print(f"Error in get_all_available_courses: {e}")  # Log the error
        return jsonify({"error": "Failed to fetch courses"}), 500
    finally:
        if conn:
            conn.close()


def generate_streamable_url(file_path):
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
    rand_name = random.choice(filenames)
    file_path = f"./assets/{rand_name}"  # Corrected file path to use random filename, and ignore input file_path.

    return url_for("stream_media", file_path=file_path, _external=True)


@app.route("/stream", methods=["GET"])
def stream_media():
    file_path = request.args.get("file_path")
    if not file_path:
        return jsonify({"error": "File path missing"}), 400

    try:
        return send_file(
            file_path,
            as_attachment=False,
            mimetype="video/mp4",
        )
    except FileNotFoundError:
        return jsonify({"error": "File not found"}), 404
    except Exception as e:
        print(f"Streaming error: {e}")  # Log the error
        return jsonify({"error": "Error streaming file"}), 500


@app.route("/lectures/<int:content_id>", methods=["GET"])
@token_required
def get_lectures_for_content(content_id):
    conn = None
    try:
        conn = get_courses_db_connection(COURSES_DATABASE_CONFIG)
        if not conn:
            return jsonify({"error": "Failed to connect to courses database"}), 500
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
        return jsonify(lecture_list), 200

    except Exception as e:
        print(f"Lecture retrieval error: {e}")  # Log error
        return jsonify({"error": "Failed to fetch lectures"}), 500
    finally:
        if conn:
            conn.close()


@app.route("/dynamicHome", methods=["GET"])
def dynamic_home():
    conn = None
    try:
        conn = get_db_connection(COURSES_DATABASE_CONFIG)
        if not conn:
            return jsonify({"error": "Database connection failed"}), 500

        cursor = conn.cursor()
        cursor.execute(
            "SELECT TOP 8 * FROM HomepageTrailers ORDER BY NEWID()"
        )  # Execute directly with cursor
        course = cursor.fetchall()

        if not course:
            return jsonify({"error": "No courses found"}), 404

        courses_data = [
            {
                "Id": row.Id,  # Access by column name
                "Title": row.Title,
                "Duration": row.Duration,
                "Image": row.Image,
            }
            for row in course
        ]

        return jsonify(courses_data)

    except Exception as e:
        print(f"Dynamic home error: {e}")  # Log error
        return jsonify({"error": "Failed to load homepage data"}), 500
    finally:
        if conn:
            conn.close()


@app.route("/activeUser", methods=["GET", "PUT"])
@token_required
def active_user():
    conn = None
    try:
        token_str = request.headers.get("Authorization")
        if not token_str:
            return jsonify({"error": "Authorization header missing"}), 401
        token_parts = token_str.split()
        if len(token_parts) != 2 or token_parts[0].lower() != "bearer":
            return jsonify({"error": "Invalid token format"}), 401
        token = token_parts[1]

        decoded_token = jwt.decode(
            token, app.config["SHHHH_CHUP"], algorithms=["HS256"]
        )
        user_id = decoded_token["user_id"]

        conn = get_db_connection(DATABASE_CONFIG)
        if not conn:
            return jsonify({"error": "Database connection failed"}), 500
        cursor = conn.cursor()

        if request.method == "GET":
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

            data = request.get_json()
            if not data:
                return jsonify({"error": "No data provided for update"}), 400

            updated_data = {
                "FirstName": data.get("FirstName", current_user.FirstName),
                "LastName": data.get("LastName", current_user.LastName),
                "Email": data.get("Email", current_user.Email),
                "Username": data.get("Username", current_user.Username),
                "PasswordHash": data.get(
                    "PasswordHash", current_user.PasswordHash
                ),  # Note: Directly updating PasswordHash is generally insecure in real applications, consider password reset flows.
                "Initialized": data.get("Initialized", current_user.Initialized),
                "SubscriptionStatus": data.get(
                    "SubscriptionStatus", current_user.SubscriptionStatus
                ),
                "CourseLevel": data.get("CourseLevel", current_user.CourseLevel),
                "GroupLevel": data.get("GroupLevel", current_user.GroupLevel),
                "ProfilePic": data.get("ProfilePic", current_user.ProfilePic),
                "Bio": data.get("Bio", current_user.Bio),
            }

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

    except jwt.ExpiredSignatureError:
        return jsonify({"error": "Token has expired!"}), 401
    except jwt.InvalidTokenError:
        return jsonify({"error": "Token is invalid!"}), 401
    except Exception as e:
        print(f"Active user error: {e}")  # Log error
        return jsonify({"error": "Failed to process active user request"}), 500

    finally:
        if conn:
            conn.close()


@app.route("/availableLevelsAndGroups", methods=["GET"])
@token_required
def get_available_levels_and_groups():
    conn = None
    try:
        conn = get_courses_db_connection(COURSES_DATABASE_CONFIG)
        if not conn:
            return jsonify({"error": "Database connection failed"}), 500
        cursor = conn.cursor()

        levels_query = """
            SELECT level_id, level_name, Graphic
            FROM dbo.EducationalLevels
        """
        cursor.execute(levels_query)
        levels = cursor.fetchall()

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
        print(f"Available levels/groups error: {e}")  # Log error
        return jsonify({"error": "Failed to fetch levels and groups"}), 500

    finally:
        if conn:
            conn.close()


if __name__ == "__main__":
    app.run(debug=True)
