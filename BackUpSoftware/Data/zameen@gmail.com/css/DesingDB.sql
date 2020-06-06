CREATE TABLE Standard
(
   StandardId Int IDENTITY PRIMARY KEY,
   StandardName VARCHAR(50),
   Description VARCHAR(100)

)

CREATE TABLE Student

(
    StudentID INT IDENTITY PRIMARY KEY,
	FirstName varchar(50),
	LastName varchar(50),

	Fk_Standard_Id   INT FOREIGN KEY REFERENCES Standard(StandardId)

)

CREATE TABLE Teacher
(
  TeacherID INT IDENTITY PRIMARY KEY,
  TeacherName VARCHAR(50),

   Fk_Standard_Id   INT FOREIGN KEY REFERENCES Standard(StandardId)   


)

CREATE TABLE Course 
(
      CourseID INT IDENTITY PRIMARY KEY,
	  CourseName VARCHAR(50),
	  Location VARCHAR(50),
	  TeacherID INT,
	     FOREIGN KEY (TeacherID) REFERENCES Teacher(TeacherID)


)
CREATE TABLE StdCourse(
    StudentID INT,
    CourseID INT,
    FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Course(CourseID), 
    
	PRIMARY KEY (StudentID, CourseID)
);
CREATE TABLE StAddress
(
  
  Address1 VARCHAR(100),
  Address2 VARCHAR(100),
  City VARCHAR(50),
  State VARCHAR(50),

 StudentID INT UNIQUE FOREIGN KEY REFERENCES Student(StudentID),
 PRIMARY KEY(StudentID)
)