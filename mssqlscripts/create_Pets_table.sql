CREATE TABLE Pets (
    PetID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Species NVARCHAR(20) CHECK (Species IN ('Dog', 'Cat', 'Other')) NOT NULL,
    Breed NVARCHAR(50) NULL,
    Color NVARCHAR(30) NULL,
    Age INT NULL,
    Weight FLOAT NULL,
    MicrochipID NVARCHAR(50) UNIQUE NULL,
    PhotoURL NVARCHAR(255) NULL,
    Status NVARCHAR(10) CHECK (Status IN ('Lost', 'Found')) NOT NULL,
    LastSeenLocation NVARCHAR(255) NULL,
    Description NVARCHAR(MAX) NULL,
    UserID nvarchar(450) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id)
);