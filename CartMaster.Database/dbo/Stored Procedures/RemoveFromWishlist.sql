CREATE PROCEDURE RemoveFromWishlist
    @UserID INT,
    @ProductID INT
AS
BEGIN
    DELETE FROM Wishlist 
    WHERE UserID = @UserID AND ProductID = @ProductID
END