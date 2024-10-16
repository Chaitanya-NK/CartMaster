CREATE PROCEDURE RemoveProductFromCart
	@CartID INT,
	@ProductID INT
AS
BEGIN
	DELETE FROM CartItems
	WHERE CartID = @CartID AND ProductID = @ProductID
END