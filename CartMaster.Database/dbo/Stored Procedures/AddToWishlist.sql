CREATE PROCEDURE AddToWishlist
    @UserID INT,
    @ProductID INT,
    @ProductName NVARCHAR(100),
    @ProductDescription NVARCHAR(MAX),
    @Price DECIMAL(10, 2),
    @ImageURL NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Wishlist (UserID, ProductID, ProductName, ProductDescription, Price, ImageURL)
    VALUES (@UserID, @ProductID, @ProductName, @ProductDescription, @Price, @ImageURL)
END