CREATE PROCEDURE LoginUser
    @UserID NVARCHAR(10),
    @Password NVARCHAR(50)
AS
BEGIN
    SELECT * FROM [User]
    WHERE UserID = @UserID AND [Password] = @Password
END
