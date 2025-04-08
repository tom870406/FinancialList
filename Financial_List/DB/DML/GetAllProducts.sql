CREATE PROCEDURE GetAllProducts
AS
BEGIN
    SELECT ProductNo, ProductName, Price, FeeRate FROM Product
END