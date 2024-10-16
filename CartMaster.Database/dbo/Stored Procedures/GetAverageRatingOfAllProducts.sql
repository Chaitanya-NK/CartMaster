CREATE PROCEDURE GetAverageRatingOfAllProducts
AS
BEGIN
	SELECT
		p.ProductID,
		p.ProductName,
		(SELECT ISNULL(ROUND(AVG(CAST(r.Rating AS DECIMAL(10,2))),2), 0)
		 FROM ProductReviews r
		 WHERE r.ProductID = p.ProductID) AS AverageRating
	FROM Products p
END