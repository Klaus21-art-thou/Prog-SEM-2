CREATE DATABASE RaceDay;
GO
USE RaceDay;
GO
-- Code was created through the guidance and help and whilist not originally made throuh the use of AI

IF OBJECT_ID('dbo.Results', 'U') IS NOT NULL DROP TABLE dbo.Results;
IF OBJECT_ID('dbo.Participants', 'U') IS NOT NULL DROP TABLE dbo.Participants;
IF OBJECT_ID('dbo.Fees', 'U') IS NOT NULL DROP TABLE dbo.Fees;
IF OBJECT_ID('dbo.Tracks', 'U') IS NOT NULL DROP TABLE dbo.Tracks;
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL DROP TABLE dbo.Events;
IF OBJECT_ID('dbo.Teams', 'U') IS NOT NULL DROP TABLE dbo.Teams;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) CONSTRAINT PK_Users PRIMARY KEY,
    Username VARCHAR(100) NOT NULL CONSTRAINT UQ_Users_Username UNIQUE,
    UserEmail VARCHAR(100) NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
    UserRole VARCHAR(20) NOT NULL CONSTRAINT DF_Users_Role DEFAULT 'Participant',
    UserBio VARCHAR(MAX) NULL
);

CREATE TABLE Events (
    EventID INT IDENTITY(1,1) CONSTRAINT PK_Events PRIMARY KEY,
    OrganiserID INT NOT NULL,
    EventName VARCHAR(150) NOT NULL,
    EventDescription VARCHAR(MAX) NULL,
    EventDate DATETIME NOT NULL,
    EventLocation VARCHAR(150) NOT NULL,
    EventType VARCHAR(20) NOT NULL CONSTRAINT DF_Events_Type DEFAULT 'run',
    CONSTRAINT FK_Events_Organiser FOREIGN KEY (OrganiserID) REFERENCES Users(UserID)
);

CREATE TABLE Tracks (
    TrackID INT IDENTITY(1,1) CONSTRAINT PK_Tracks PRIMARY KEY,
    EventID INT NOT NULL,
    TrackName VARCHAR(100) NOT NULL,
    DistanceKM DECIMAL(5,2) NOT NULL,
    CONSTRAINT FK_Tracks_Event FOREIGN KEY (EventID) REFERENCES Events(EventID) ON DELETE CASCADE
);

CREATE TABLE Fees (
    FeeID INT IDENTITY(1,1) CONSTRAINT PK_Fees PRIMARY KEY,
    TrackID INT NOT NULL,
    FeeName VARCHAR(100) NOT NULL,
    FeeAmount DECIMAL(10,2) NOT NULL CONSTRAINT DF_Fees_Amount DEFAULT 0.00,
    FeeCurrency VARCHAR(10) NOT NULL CONSTRAINT DF_Fees_Currency DEFAULT 'ZAR',
    CONSTRAINT FK_Fees_Track FOREIGN KEY (TrackID) REFERENCES Tracks(TrackID) ON DELETE CASCADE
);

CREATE TABLE Teams (
    TeamID INT IDENTITY(1,1) CONSTRAINT PK_Teams PRIMARY KEY,
    TeamName VARCHAR(100) NOT NULL CONSTRAINT UQ_Teams_Name UNIQUE,
    TeamDescription VARCHAR(MAX) NULL
);

CREATE TABLE Participants (
    ParticipantID INT IDENTITY(1,1) CONSTRAINT PK_Participants PRIMARY KEY,
    UserID INT NOT NULL,
    EventID INT NOT NULL,
    TrackID INT NOT NULL,
    FeeID INT NOT NULL,
    TeamID INT NULL,
    RegistrationDate DATETIME NOT NULL CONSTRAINT DF_Participants_RegDate DEFAULT GETDATE(),
    CONSTRAINT FK_Participants_User FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Participants_Event FOREIGN KEY (EventID) REFERENCES Events(EventID),
    CONSTRAINT FK_Participants_Track FOREIGN KEY (TrackID) REFERENCES Tracks(TrackID),
    CONSTRAINT FK_Participants_Fee FOREIGN KEY (FeeID) REFERENCES Fees(FeeID),
    CONSTRAINT FK_Participants_Team FOREIGN KEY (TeamID) REFERENCES Teams(TeamID),
    CONSTRAINT UQ_Participant_Enrollment UNIQUE (UserID, EventID)
);

CREATE TABLE Results (
    ResultID INT IDENTITY(1,1) CONSTRAINT PK_Results PRIMARY KEY,
    ParticipantID INT NOT NULL CONSTRAINT UQ_Results_Participant UNIQUE,
    FinishTimeSeconds INT NOT NULL,
    FinishingPosition INT NOT NULL,
    CONSTRAINT FK_Results_Participant FOREIGN KEY (ParticipantID) REFERENCES Participants(ParticipantID) ON DELETE CASCADE
);
GO

INSERT INTO Users (Username, UserEmail, UserRole, UserBio) VALUES
('Thabo Mbeki', 'thabo.organiser@raceday.co.za', 'Organiser', 'Lead organizer for Gauteng road races.'),
('Anika van der Merwe', 'anika.org@capetowntrail.co.za', 'Organiser', 'Western Cape athletics event director.'),
('Sipho Ndlovu', 'sipho.runner@gmail.com', 'Participant', 'Marathon runner aiming for Comrades qualifier.'),
('Lize Botha', 'lize.cyclist@yahoo.com', 'Participant', 'Avid road cyclist and ultra-marathoner.'),
('David Kholofelo', 'david.k@outlook.com', 'Participant', 'Trail enthusiast and social walker.');

INSERT INTO Teams (TeamName, TeamDescription) VALUES
('Highveld Striders', 'Gauteng-based long distance running club.'),
('Cape Velo Club', 'Cape Town road and mountain cycling team.');

INSERT INTO Events (OrganiserID, EventName, EventDescription, EventDate, EventLocation, EventType) VALUES
(1, 'Soweto Marathon 2026', 'The iconic Peoples Race through historic Soweto.', '2026-11-01 06:00:00', 'Soweto, Johannesburg', 'run'),
(2, 'Cape Town Cycle Tour 2026', 'World famous scenic cycle route around Cape Peninsula.', '2026-03-08 06:30:00', 'Cape Town, Western Cape', 'cycle'),
(1, 'Pretoria Spring Walk', 'Community health and fun walk through Jacaranda city.', '2026-10-10 08:00:00', 'Pretoria, Gauteng', 'walk');

INSERT INTO Tracks (EventID, TrackName, DistanceKM) VALUES
(1, 'Full Marathon', 42.20),
(1, 'Half Marathon', 21.10),
(1, '10km Dash', 10.00),
(2, 'Main Cycle Route', 109.00),
(3, '5km Family Walk', 5.00);

INSERT INTO Fees (TrackID, FeeName, FeeAmount, FeeCurrency) VALUES
(1, 'Early Bird Entry', 350.00, 'ZAR'),
(1, 'Standard Entry', 420.00, 'ZAR'),
(2, 'Standard Entry', 280.00, 'ZAR'),
(3, 'Standard Entry', 180.00, 'ZAR'),
(4, 'General Cyclist', 650.00, 'ZAR'),
(5, 'Walker Entry', 80.00, 'ZAR');

INSERT INTO Participants (UserID, EventID, TrackID, FeeID, TeamID, RegistrationDate) VALUES
(3, 1, 1, 2, 1, '2026-08-10 14:22:00'),
(4, 1, 2, 3, NULL, '2026-08-12 09:15:00'),
(4, 2, 4, 5, 2, '2026-08-15 11:30:00'),
(5, 3, 5, 6, NULL, '2026-08-20 16:45:00');

INSERT INTO Results (ParticipantID, FinishTimeSeconds, FinishingPosition) VALUES
(1, 10440, 15),
(2, 5310, 42);

SELECT * FROM Users;
SELECT * FROM Events;
SELECT * FROM Tracks;
SELECT * FROM Fees;
SELECT * FROM Teams;
SELECT * FROM Participants;
SELECT * FROM Results;