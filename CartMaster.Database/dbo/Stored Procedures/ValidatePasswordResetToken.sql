CREATE PROCEDURE ValidatePasswordResetToken
	@Token NVARCHAR(100)
AS
BEGIN
	SELECT UserID, ExpiryDate
	FROM PasswordResetTokens
	WHERE Token = @Token
END