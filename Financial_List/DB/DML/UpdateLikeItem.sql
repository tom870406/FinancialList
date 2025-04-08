CREATE PROCEDURE UpdateLikeItem
    @SN INT,
    @OrderAmount INT,
    @Account NVARCHAR(20),
    @TotalFee DECIMAL(18,2),
    @TotalAmount DECIMAL(18,2)
AS
BEGIN
    UPDATE LikeList
    SET OrderAmount = @OrderAmount,
        Account = @Account,
        TotalFee = @TotalFee,
        TotalAmount = @TotalAmount
    WHERE SN = @SN
END