﻿CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Address NVARCHAR(MAX),
    PhoneNumber NVARCHAR(20),
    CreatedAt DATETIME,
	ModifiedAt DATETIME
);