CREATE PROCEDURE Login
	@Username NVARCHAR(50),
	@Password NVARCHAR(255)
AS
BEGIN
	SELECT 
		u.UserID, 
		u.Username, 
		u.FirstName,
		u.LastName,
		u.Email, 
		ru.RoleID, 
		r.RoleName,
		c.CartID
	FROM Users u 
	INNER JOIN UserRoles ru ON u.UserID = ru.UserID 
	INNER JOIN Roles r ON r.RoleID = ru.RoleID
	INNER JOIN Cart c ON c.UserID = u.UserID
	WHERE u.Username = @Username AND u.Password = @Password
END