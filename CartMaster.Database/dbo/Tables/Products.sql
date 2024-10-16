CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    ProductDescription NVARCHAR(MAX),
    Price DECIMAL(10, 2) NOT NULL,
    StockQuantity INT NOT NULL,
    CategoryID INT,
    ImageURL NVARCHAR(MAX),
    CreatedAt DATETIME,
	ModifiedAt DATETIME,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);