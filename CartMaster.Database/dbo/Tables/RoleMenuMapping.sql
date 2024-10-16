CREATE TABLE RoleMenuMapping (
    RoleMenuMappingID INT IDENTITY(1,1) PRIMARY KEY,
    RoleID INT,
    MenuItemID INT,
	CreatedAt DATETIME,
	ModifiedAt DATETIME,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    FOREIGN KEY (MenuItemID) REFERENCES MenuItems(MenuItemID)
);