CREATE PROCEDURE DeleteCategory
	@CategoryID INT
AS
BEGIN
	DELETE FROM Products
	WHERE CategoryID = @CategoryID

	DELETE FROM Categories
	WHERE CategoryID = @CategoryID
END