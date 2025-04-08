CREATE PROCEDURE GetProductById
    @ProductNo INT
AS
BEGIN
    SELECT ProductNo, ProductName, Price, FeeRate FROM Product WHERE ProductNo = @ProductNo
END