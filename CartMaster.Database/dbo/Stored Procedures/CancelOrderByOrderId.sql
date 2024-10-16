CREATE PROCEDURE CancelOrderByOrderId
	@OrderID INT
AS
BEGIN
	UPDATE Orders
	SET
		Status = 'Cancelled'
	WHERE OrderID = @OrderID

	UPDATE Products
	SET StockQuantity = StockQuantity + oi.Quantity
	FROM Products p
	INNER JOIN OrderItems oi ON p.ProductID = oi.ProductID
	WHERE oi.OrderID = @OrderID
END