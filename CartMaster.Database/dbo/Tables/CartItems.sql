﻿CREATE TABLE CartItems (
	CartItemId INT IDENTITY(1,1) PRIMARY KEY,
	CartID INT,
	ProductID INT,
	Quantity INT NOT NULL,
	ProductName VARCHAR(100),
	Price DECIMAL(10,2),
	ImageURL NVARCHAR(MAX),
	FOREIGN KEY (CartID) REFERENCES Cart(CartID),
	FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
	UNIQUE (CartID, ProductID)
);