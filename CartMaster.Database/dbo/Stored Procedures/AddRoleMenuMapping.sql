CREATE PROCEDURE AddRoleMenuMapping
	@RoleID INT,
	@MenuItemID INT
AS
BEGIN
	INSERT INTO RoleMenuMapping (RoleID, MenuItemID, CreatedAt)
	VALUES (@RoleID, @MenuItemID, GETDATE())
END