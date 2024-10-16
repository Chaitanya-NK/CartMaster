CREATE PROCEDURE UpdateProduct
	@ProductID INT,
	@ProductName NVARCHAR(100),
	@ProductDescription NVARCHAR(MAX),
	@Price DECIMAL(10, 2),
	@StockQuantity INT,
	@CategoryID INT,
	@ImageURL NVARCHAR(MAX)
AS
BEGIN
	UPDATE Products
	SET
		ProductName = @ProductName,
		ProductDescription = @ProductDescription,
		Price = @Price,
		StockQuantity = @StockQuantity,
		CategoryID = @CategoryID,
		ImageURL = @ImageURL,
		ModifiedAt = GETDATE()
	WHERE ProductID = @ProductID
END