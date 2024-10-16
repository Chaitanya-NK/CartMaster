CREATE PROCEDURE UpdateCategory
	@CategoryID INT,
	@CategoryName NVARCHAR(50),
	@ImageURL NVARCHAR(MAX)
AS
BEGIN
	UPDATE Categories
	SET
		CategoryName = @CategoryName,
		ImageURL = @ImageURL,
		ModifiedAt = GETDATE()
	WHERE CategoryID = @CategoryID
END