CREATE PROCEDURE UpdateCartItemQuantity
	@CartID INT,
	@ProductID INT,
	@Quantity INT
AS
BEGIN
	UPDATE CartItems
	SET Quantity = @Quantity
	WHERE CartID = @CartID AND ProductID = @ProductID
END