CREATE PROCEDURE RequestReturnByOrderItemId
	@OrderItemID INT
AS
BEGIN
	UPDATE OrderItems
	SET
		ReturnStatus = 'Return Requested',
		ReturnRequested = 1
	WHERE OrderItemID = @OrderItemID
END