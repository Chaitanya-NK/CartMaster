CREATE PROCEDURE CreateUserSession
	@UserID INT,
	@SessionID UNIQUEIDENTIFIER,
	@LoginTime DATETIME
AS
BEGIN
	INSERT INTO UserSessions(SessionID, UserID, LoginTime, IsActive)
	VALUES (@SessionID, @UserID, @LoginTime, 1)
END