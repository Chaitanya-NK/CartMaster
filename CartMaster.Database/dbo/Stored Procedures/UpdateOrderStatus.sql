CREATE PROCEDURE UpdateOrderStatus
	@OrderID INT,
	@Status NVARCHAR(20)
AS
BEGIN
	UPDATE Orders
	SET
		Status = @Status
	WHERE OrderID = @OrderID
END