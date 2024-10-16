CREATE PROCEDURE AddProductToCart
	@CartID INT,
	@ProductID INT,
	@Quantity INT,
	@ProductName VARCHAR(100),
	@Price DECIMAL(10,2),
	@ImageURL NVARCHAR(MAX)
AS
BEGIN
	IF EXISTS (SELECT 1 FROM CartItems WHERE CartID = @CartID AND ProductID = @ProductID)
		BEGIN
			UPDATE CartItems
			SET Quantity = Quantity + @Quantity
			WHERE CartID = @CartID AND ProductID = @ProductID
		END
	ELSE
		BEGIN
			INSERT INTO CartItems (CartID, ProductID, Quantity, ProductName, Price, ImageURL)
			VALUES (@CartID, @ProductID, @Quantity, @ProductName, @Price, @ImageURL)
		END
END