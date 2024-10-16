CREATE PROCEDURE CreateOrder
    @UserID INT,
    @TotalAmount DECIMAL(10,2),
    @CouponID INT = NULL -- Optional, can be null if no coupon is applied
AS
BEGIN
    -- Declare variables for discount and final amount calculation
	DECLARE @DiscountPercentage INT = 0;
    DECLARE @DiscountAmount DECIMAL(10, 2) = 0;
    DECLARE @FinalAmount DECIMAL(10, 2);

    -- Check if a valid coupon is provided
    IF @CouponID IS NOT NULL
    BEGIN
        -- Retrieve the discount amount associated with the coupon
        SELECT @DiscountPercentage = DiscountPercentage 
        FROM Coupons 
        WHERE CouponID = @CouponID;

		-- Calculate the Dicount Amount based on the total amount
		SET @DiscountAmount = (@TotalAmount * @DiscountPercentage) / 100
	
		-- Calculate the Final Amount by subtracting the discount
		SET @FinalAmount = @TotalAmount - @DiscountAmount;
	END
    ELSE
    BEGIN
        -- If no coupon is applied, the Final Amount is equal to the Total Amount
        SET @FinalAmount = @TotalAmount;
    END

    -- Insert into Orders table
    INSERT INTO Orders (UserID, Status, TotalAmount, DiscountAmount, FinalAmount, CouponID, CreatedAt, ModifiedAt)
    VALUES (@UserID, 'Pending', @TotalAmount, @DiscountAmount, @FinalAmount, @CouponID, GETDATE(), GETDATE());

    -- Get the newly inserted OrderID
    DECLARE @OrderID INT = SCOPE_IDENTITY();

    -- Insert items from CartItems into OrderItems
    INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price, ProductName, ProductDescription, ImageURL)
    SELECT @OrderID, P.ProductID, CI.Quantity, P.Price, P.ProductName, P.ProductDescription, P.ImageURL
    FROM CartItems CI
    JOIN Products P ON CI.ProductID = P.ProductID
    WHERE CI.CartID = (SELECT CartID FROM Cart WHERE UserID = @UserID);

    -- Update the stock quantity of the products
    UPDATE Products
    SET StockQuantity = StockQuantity - CI.Quantity
    FROM Products P
    JOIN CartItems CI ON P.ProductID = CI.ProductID
    WHERE CI.CartID = (SELECT CartID FROM Cart WHERE UserID = @UserID);

    -- Clear the cart after the order is created
    DELETE FROM CartItems WHERE CartID = (SELECT CartID FROM Cart WHERE UserID = @UserID);
END;