CREATE PROCEDURE CheckUserPurchase
	@UserID INT,
	@ProductID INT
AS
BEGIN
	SELECT COUNT(1)
	FROM Orders o
	INNER JOIN OrderItems oi ON o.OrderID = oi.OrderID
	WHERE o.UserID = @UserID
	AND oi.ProductID = @ProductID
	AND o.Status = 'Delivered'
END