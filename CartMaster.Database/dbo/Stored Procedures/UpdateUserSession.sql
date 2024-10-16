CREATE PROCEDURE UpdateUserSession
	@SessionID UNIQUEIDENTIFIER,
	@LogoutTime DATETIME
AS
BEGIN
	UPDATE UserSessions
	SET LogoutTime = @LogoutTime, IsActive = 0
	WHERE SessionID = @SessionID
END