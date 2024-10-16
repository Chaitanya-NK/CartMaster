CREATE PROCEDURE GetMenuItemsByRoleId
	@RoleID INT
AS
BEGIN
	SELECT mi.MenuItemID, mi.Name, mi.Route
	FROM MenuItems mi
	INNER JOIN RoleMenuMapping rmm ON mi.MenuItemID = rmm.MenuItemID
	WHERE rmm.RoleID = @RoleID
END