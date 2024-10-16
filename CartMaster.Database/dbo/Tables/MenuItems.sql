﻿CREATE TABLE MenuItems (
    MenuItemID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Route NVARCHAR(100) NOT NULL,
	CreatedAt DATETIME,
	ModifiedAt DATETIME
);