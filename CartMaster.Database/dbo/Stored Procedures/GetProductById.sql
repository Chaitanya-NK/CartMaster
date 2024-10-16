CREATE PROCEDURE GetProductById
	@ProductID INT
AS
BEGIN
	SELECT *
	FROM Products
	WHERE ProductID = @ProductID
END