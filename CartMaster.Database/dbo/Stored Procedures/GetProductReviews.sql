CREATE PROCEDURE GetProductReviews
	@ProductID INT
AS
BEGIN
	SELECT *
	FROM ProductReviews
	WHERE ProductID = @ProductID
END