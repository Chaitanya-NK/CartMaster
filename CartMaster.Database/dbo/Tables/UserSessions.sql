﻿CREATE TABLE UserSessions (
    LoginSessionID INT IDENTITY(1,1) PRIMARY KEY,
    SessionID UNIQUEIDENTIFIER,
	UserID INT,
    LoginTime DATETIME NOT NULL,
    LogoutTime DATETIME NULL,
	IsActive BIT NOT NULL DEFAULT 1,
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
);