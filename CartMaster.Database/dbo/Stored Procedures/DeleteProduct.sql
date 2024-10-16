CREATE PROCEDURE DeleteProduct
	@ProductID INT
AS
BEGIN
	DELETE FROM Products
	WHERE ProductID = @ProductID
END