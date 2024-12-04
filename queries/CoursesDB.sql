CREATE TABLE EducationalLevels (
    level_id INT IDENTITY(1,1) PRIMARY KEY,
    level_name VARCHAR(255) UNIQUE
);
GO

CREATE TABLE Courses (
    course_id INT IDENTITY(1,1) PRIMARY KEY,
    level_id INT REFERENCES EducationalLevels(level_id),
    course_name VARCHAR(255),
    description TEXT
);
GO

CREATE TABLE Subjects (
    subject_id INT IDENTITY(1,1) PRIMARY KEY,
    course_id INT REFERENCES Courses(course_id),
    subject_name VARCHAR(255)
);
GO

CREATE TABLE HierarchicalContent (
    content_id INT IDENTITY(1,1) PRIMARY KEY,
    parent_id INT REFERENCES HierarchicalContent(content_id),
    subject_id INT REFERENCES Subjects(subject_id),
    content_type VARCHAR(20) CHECK (content_type IN ('division', 'topic', 'subtopic')),
    content_name VARCHAR(255),
    content_details TEXT
);
GO

CREATE TABLE Lectures (
    lecture_id INT IDENTITY(1,1) PRIMARY KEY,
    content_id INT REFERENCES HierarchicalContent(content_id),
    lecture_no INT,
    lecture_link VARCHAR(255),
    lecture_thumbnail VARCHAR(255),
    lecture_materials TEXT,
    duration TIME,
    views INT DEFAULT 0,
    quality VARCHAR(255),
    title VARCHAR(255),
    description TEXT,
    added_date DATETIME2 DEFAULT GETDATE(),
    updated_date DATETIME2 DEFAULT GETDATE()
);
GO

-- Trigger creation in its own batch
CREATE TRIGGER TR_Lectures_UpdatedDate
ON Lectures
AFTER UPDATE
AS
BEGIN
    UPDATE Lectures
    SET updated_date = GETDATE()
    WHERE lecture_id IN (SELECT lecture_id FROM inserted);
END;
GO