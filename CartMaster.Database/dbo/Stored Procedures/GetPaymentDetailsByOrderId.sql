CREATE PROCEDURE GetPaymentDetailsByOrderID
    @OrderID INT
AS
BEGIN
    SELECT * FROM PaymentDetails WHERE OrderID = @OrderID;
END