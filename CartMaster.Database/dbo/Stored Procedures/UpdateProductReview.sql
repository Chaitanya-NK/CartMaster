CREATE PROCEDURE UpdateProductReview
	@ReviewID INT,
	@Rating INT,
	@Comment NVARCHAR(MAX)
AS
BEGIN
	UPDATE ProductReviews
	SET
		@Rating = Rating,
		@Comment = Comment,
		ModifiedAt = GETDATE()
	WHERE ReviewID = @ReviewID
END