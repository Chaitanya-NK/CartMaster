CREATE PROCEDURE ViewUserOrders
	@UserID INT
AS
BEGIN
	SELECT 
		o.OrderID, 
		o.OrderDate, 
		o.Status, 
		o.TotalAmount,
		oi.OrderItemID,
		oi.ProductID,
		oi.Quantity,
		oi.Price,
		oi.ProductName,
		oi.ProductDescription,
		oi.ImageURL,
		oi.ReturnStatus,
		oi.ReturnRequested,
		o.DiscountAmount,
		o.FinalAmount,
		o.CouponID
	FROM Orders o
	INNER JOIN OrderItems oi on o.OrderID = oi.OrderID
	WHERE o.UserID = @UserID
	ORDER BY OrderID DESC
END