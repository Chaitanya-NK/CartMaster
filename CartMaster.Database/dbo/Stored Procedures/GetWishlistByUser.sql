CREATE PROCEDURE GetWishlistByUser
    @UserID INT
AS
BEGIN
    SELECT *
    FROM Wishlist
    WHERE UserID = @UserID
END