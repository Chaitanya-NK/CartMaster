CREATE PROCEDURE GetCategoryById
	@CategoryID INT
AS
BEGIN
	SELECT *
	FROM Categories
	WHERE CategoryID = @CategoryID
END