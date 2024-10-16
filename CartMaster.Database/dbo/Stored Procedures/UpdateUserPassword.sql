CREATE PROCEDURE UpdateUserPassword
	@UserID INT,
	@NewPassword NVARCHAR(255)
AS
BEGIN
	UPDATE Users
	SET Password = @NewPassword
	WHERE UserID = @UserID
END