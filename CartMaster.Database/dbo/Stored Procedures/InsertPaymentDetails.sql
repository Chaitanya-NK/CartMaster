CREATE PROCEDURE InsertPaymentDetails
    @OrderID INT,
    @PaymentMethod NVARCHAR(50),
    @CardName NVARCHAR(100) = NULL,
    @CardNumber NVARCHAR(16) = NULL,
    @ExpiryDate NVARCHAR(7) = NULL,
    @CVV NVARCHAR(3) = NULL,
    @UpiId NVARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO PaymentDetails (OrderID, PaymentMethod, CardName, CardNumber, ExpiryDate, CVV, UpiId, PaymentStatus)
    VALUES (@OrderID, @PaymentMethod, @CardName, @CardNumber, @ExpiryDate, @CVV, @UpiId, 'Completed');
    
    SELECT SCOPE_IDENTITY() AS PaymentID;
END