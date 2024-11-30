CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,  -- Automatically increments for each new user
    FirstName NVARCHAR(100) NOT NULL,       -- First name of the user
    LastName NVARCHAR(100) NOT NULL,        -- Last name of the user
    Email NVARCHAR(255) NOT NULL,           -- User's email address
    Username NVARCHAR(100) NOT NULL,        -- Unique username
    PasswordHash NVARCHAR(255) NOT NULL,    -- Hashed password
    ConfirmPasswordHash NVARCHAR(255),      -- Confirm password (can be optional if you decide not to store it in DB)
    CreatedAt DATETIME DEFAULT GETDATE(),   -- Date and time the user was created
    UpdatedAt DATETIME DEFAULT GETDATE()    -- Date and time the user details were last updated
);
