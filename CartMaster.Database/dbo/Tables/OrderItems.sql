CREATE TABLE OrderItems (
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
	CreatedAt DATETIME,
	ModifiedAt DATETIME,
    ReturnStatus NVARCHAR(20) DEFAULT 'Not Requested',
    ReturnRequested BIT DEFAULT 0,
    ImageURL NVARCHAR(MAX),
    ProductName NVARCHAR(100),
    ProductDescription NVARCHAR(MAX),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);