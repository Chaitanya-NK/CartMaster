CREATE PROCEDURE AddCategory
	@CategoryName NVARCHAR(50),
	@ImageURL NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO Categories (CategoryName, ImageURL, CreatedAt)
	VALUES (@CategoryName, @ImageURL, GETDATE())
END