from flask import Flask, request, jsonify
import pyodbc
from werkzeug.security import generate_password_hash, check_password_hash

app = Flask(__name__)

# Database connection string
DATABASE_CONFIG = {
    "DRIVER": "{ODBC Driver 17 for SQL Server}",
    "SERVER": "MY2NDGF",
    "DATABASE": "pacademy",
    "Trusted_Connection": "Yes",
}


def get_db_connection():
    try:
        conn = pyodbc.connect(
            f"DRIVER={DATABASE_CONFIG['DRIVER']};"
            f"SERVER={DATABASE_CONFIG['SERVER']};"
            f"DATABASE={DATABASE_CONFIG['DATABASE']};"
            f"Trusted_Connection={DATABASE_CONFIG['Trusted_Connection']};"
        )
        return conn
    except Exception as e:
        print(f"Database connection error: {e}")
        raise


# Login endpoint
@app.route("/login", methods=["POST"])
def login():
    data = request.json
    username_or_email = data["username_or_email"]
    password = data["password"]

    try:
        # Connect to the database
        conn = get_db_connection()
        cursor = conn.cursor()

        # Query to retrieve the stored password hash
        query = """
        SELECT PasswordHash FROM Users
        WHERE Username = ? OR Email = ?
        """
        cursor.execute(query, (username_or_email, username_or_email))
        result = cursor.fetchone()

        if result:
            stored_password_hash = result[0]
            # Compare the entered password with the stored hash
            if check_password_hash(stored_password_hash, password):
                return jsonify({"message": "Login successful"}), 200
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
        conn = get_db_connection()
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


if __name__ == "__main__":
    app.run(debug=True)
