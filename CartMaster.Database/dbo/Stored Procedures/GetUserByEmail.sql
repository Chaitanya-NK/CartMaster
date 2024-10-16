﻿CREATE PROCEDURE GetUserByEmail
	@Email NVARCHAR(100)
AS
BEGIN
	SELECT UserId, Email
	FROM Users 
	WHERE Email = @Email
END