CREATE PROCEDURE InsertResetToken
	@UserID INT,
	@Token NVARCHAR(100),
	@ExpiryDate DATETIME
AS
BEGIN
	INSERT INTO PasswordResetTokens (UserID, Token, ExpiryDate) 
	VALUES (@UserID, @Token, @ExpiryDate)
END