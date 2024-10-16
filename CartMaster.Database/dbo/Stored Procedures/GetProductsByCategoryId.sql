CREATE PROCEDURE GetProductsByCategoryId
	@CategoryID INT
AS
BEGIN
	SELECT *
	FROM Products
	WHERE CategoryID = @CategoryID
END