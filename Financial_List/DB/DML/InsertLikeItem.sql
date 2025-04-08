CREATE PROCEDURE InsertLikeItem
    @UserID NVARCHAR(20),
    @ProductNo INT,
    @OrderAmount INT,
    @Account NVARCHAR(20),
    @TotalFee DECIMAL(18,2),
    @TotalAmount DECIMAL(18,2)
AS
BEGIN
    INSERT INTO LikeList (UserID, ProductNo, OrderAmount, Account, TotalFee, TotalAmount)
    VALUES (@UserID, @ProductNo, @OrderAmount, @Account, @TotalFee, @TotalAmount)
END
