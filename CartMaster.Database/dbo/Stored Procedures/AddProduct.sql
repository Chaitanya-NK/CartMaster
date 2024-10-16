CREATE PROCEDURE AddProduct
	@ProductName NVARCHAR(100),
	@ProductDescription NVARCHAR(MAX),
	@Price DECIMAL(10, 2),
	@StockQuantity INT,
	@CategoryID INT,
	@ImageURL NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Products (ProductName, ProductDescription, Price, StockQuantity, CategoryID, ImageURL, CreatedAt)
	VALUES (@ProductName, @ProductDescription, @Price, @StockQuantity, @CategoryID, @ImageURL, GETDATE())
END