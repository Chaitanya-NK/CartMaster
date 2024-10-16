﻿CREATE TABLE Wishlist (
	WishlistID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL,
	ProductID INT NOT NULL,
	ProductName NVARCHAR(100),
	ProductDescription NVARCHAR(MAX),
	Price DECIMAL(10,2),
	ImageURL NVARCHAR(MAX)
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
	FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
)