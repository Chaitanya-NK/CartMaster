CREATE PROCEDURE UpdateUser
	@UserID INT,
	@Username NVARCHAR(50),
	@Password NVARCHAR(255),
	@Email NVARCHAR(100),
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@PhoneNumber NVARCHAR(50),
	@Address NVARCHAR(MAX)
AS
BEGIN
	UPDATE Users
	SET
		UserName = @Username,
		Password = @Password,
		Email = @Email,
		FirstName = @FirstName,
		LastName = @LastName,
		PhoneNumber = @PhoneNumber,
		Address = @Address,
		ModifiedAt = GETDATE()
	WHERE UserID = @UserID
END