CREATE TABLE Reports (
    ReportID INT IDENTITY(1,1) PRIMARY KEY,
    PetID INT NOT NULL,
    UserID nvarchar(450) NOT NULL,
    ReportType NVARCHAR(10) CHECK (ReportType IN ('Lost', 'Found')) NOT NULL,
    ReportDate DATETIME DEFAULT GETDATE(),
    Details NVARCHAR(MAX) NULL,
    FOREIGN KEY (PetID) REFERENCES Pets(PetID),
    FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id)
);