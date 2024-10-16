CREATE PROCEDURE DeleteProductReview
	@ReviewID INT
AS
BEGIN
	DELETE FROM ProductReviews
	WHERE ReviewID = @ReviewID
END