CREATE PROCEDURE UpdateProduct
    @ProductNo INT,
    @ProductName NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @FeeRate DECIMAL(5, 2)
AS
BEGIN
    UPDATE Product
    SET ProductName = @ProductName, Price = @Price, FeeRate = @FeeRate
    WHERE ProductNo = @ProductNo
END