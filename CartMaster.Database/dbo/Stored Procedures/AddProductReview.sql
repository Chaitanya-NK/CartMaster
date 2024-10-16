CREATE PROCEDURE AddProductReview
	@ProductID INT,
	@UserID INT,
	@Username VARCHAR(50),
	@Rating INT,
	@Comment NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO ProductReviews (ProductID, UserID, Rating, Comment, CreatedAt, Username)
	VALUES (@ProductID, @UserID, @Rating, @Comment, GETDATE(), @Username)
END