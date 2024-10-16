CREATE PROCEDURE TokenDetails
	@UserName NVARCHAR(50)
AS
BEGIN
	SELECT 
		u.UserID, 
		u.Username, 
		CONCAT(u.FirstName, u.LastName) AS Name, 
		u.Email, 
		ru.RoleID, 
		r.RoleName,
		c.CartID
	FROM Users u 
	INNER JOIN UserRoles ru ON u.UserID = ru.UserID 
	INNER JOIN Roles r ON r.RoleID = ru.RoleID
	INNER JOIN Cart c ON c.UserID = u.UserID
END