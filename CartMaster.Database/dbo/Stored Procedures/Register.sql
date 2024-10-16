CREATE PROCEDURE Register
	@Username NVARCHAR(50),
	@Password NVARCHAR(255),
	@Email NVARCHAR(100),
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@PhoneNumber NVARCHAR(50),
	@UserID INT OUTPUT,
	@OutputEmail NVARCHAR(100) OUTPUT
AS
BEGIN
	INSERT INTO Users (Username, Password, Email, FirstName, LastName, PhoneNumber, CreatedAt)
	VALUES (@Username, @Password, @Email, @FirstName, @LastName, @PhoneNumber, GETDATE())

	SET @UserID = SCOPE_IDENTITY();
	SET @OutputEmail = @Email

	INSERT INTO UserRoles (UserID, RoleID)
	SELECT @UserID, RoleID FROM Roles WHERE RoleName = 'USER'

	INSERT INTO Cart (UserID) VALUES (@UserID)
END