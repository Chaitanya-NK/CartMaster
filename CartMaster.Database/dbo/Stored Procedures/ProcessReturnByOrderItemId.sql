CREATE PROCEDURE ProcessReturnByOrderItemId
	@OrderItemID INT,
	@ReturnStatus NVARCHAR(20)
AS
BEGIN
	UPDATE OrderItems
	SET
		ReturnStatus = @ReturnStatus
	WHERE OrderItemID = @OrderItemID

	UPDATE Products
	SET
		StockQuantity = StockQuantity + (SELECT Quantity FROM OrderItems WHERE OrderItemID = @OrderItemID)
		WHERE ProductID = (SELECT ProductID FROM OrderItems WHERE OrderItemID = @OrderItemID)
END