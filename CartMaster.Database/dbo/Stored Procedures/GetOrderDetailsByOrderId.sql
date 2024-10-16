CREATE PROCEDURE GetOrderDetailsByOrderId
	@OrderID INT
AS
BEGIN
	SELECT o.OrderID, o.OrderDate, o.Status, o.TotalAmount, o.DiscountAmount, o.FinalAmount, o.CouponID
	FROM Orders o
	WHERE o.OrderID = @OrderID

	SELECT oi.ProductID, oi.Price, oi.Quantity, oi.ProductName, oi.ProductDescription, oi.ImageURL, oi.ReturnStatus, oi.ReturnRequested
	FROM OrderItems oi
	WHERE oi.OrderID = @OrderID
END