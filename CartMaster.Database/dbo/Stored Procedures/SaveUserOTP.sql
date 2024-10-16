CREATE PROCEDURE SaveUserOTP
	@UserID INT,
	@OTPCode VARCHAR(6),
	@ExpirationTime DATETIME
AS
BEGIN
	DELETE FROM UserOTP 
	WHERE UserID = @UserID

	INSERT INTO UserOTP (UserID, OTPCode, ExpirationTime)
	VALUES (@UserID, @OTPCode, @ExpirationTime)
END