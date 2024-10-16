CREATE PROCEDURE GetAverageRatingByProductId
	@ProductID INT
AS
BEGIN
	SELECT ISNULL(ROUND(AVG(CAST(Rating AS DECIMAL(10,2))),2), 0) AS AverageRating
	FROM ProductReviews
	WHERE ProductID = @ProductID
END