CREATE PROCEDURE GetUserLikeListBySN
    @UserID NVARCHAR(20),
	@SN INT
AS
BEGIN
    SELECT 
		l.SN,
		l.ProductNo,
		l.UserID,
        p.ProductName,
        p.Price,
        p.FeeRate,
        l.OrderAmount,
        l.TotalFee,
        l.TotalAmount,
        u.Account,
        u.Email
    FROM LikeList l
    JOIN Product p ON l.ProductNo = p.ProductNo
    JOIN [User] u ON l.UserID = u.UserID
    WHERE l.UserID = @UserID
	AND l.SN = @SN
END
