CREATE PROCEDURE SaveUserAddress
	@UserID INT,
	@Address NVARCHAR(MAX)
AS
BEGIN
	UPDATE Users
	SET
		Address = @Address,
		ModifiedAt = GETDATE()
	WHERE UserID = @UserID
END