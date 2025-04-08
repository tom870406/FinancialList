CREATE PROCEDURE DeleteProduct
    @ProductNo INT
AS
BEGIN
    DELETE FROM Product WHERE ProductNo = @ProductNo
END