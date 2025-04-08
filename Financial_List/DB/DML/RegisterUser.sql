CREATE PROCEDURE RegisterUser
    @UserID NVARCHAR(10),
    @UserName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Account NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [User] WHERE UserID = @UserID)
    BEGIN
        RAISERROR('此帳號已存在', 16, 1)
        RETURN
    END

    INSERT INTO [User] (UserID, UserName, Email, Account, [Password])
    VALUES (@UserID, @UserName, @Email, @Account, @Password)
END
