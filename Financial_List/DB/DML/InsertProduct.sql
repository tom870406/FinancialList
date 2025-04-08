CREATE PROCEDURE InsertProduct
    @ProductName NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @FeeRate DECIMAL(5, 2)
AS
BEGIN
    INSERT INTO Product (ProductName, Price, FeeRate)
    VALUES (@ProductName, @Price, @FeeRate)
END