-- 建立 User
CREATE TABLE [User] (
    UserID NVARCHAR(10) PRIMARY KEY,
	Password NVARCHAR(100) NOT NULL,
    UserName NVARCHAR(50),
    Email NVARCHAR(100),
    Account NVARCHAR(50)
);

-- 建立 Product
CREATE TABLE Product (
    ProductNo INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    FeeRate DECIMAL(5,3) NOT NULL
);

-- 建立 LikeList
CREATE TABLE LikeList (
    SN INT PRIMARY KEY IDENTITY(1,1),
    UserID NVARCHAR(10) NOT NULL,
    ProductNo INT NOT NULL,
    OrderAmount INT NOT NULL,
    Account NVARCHAR(20) NOT NULL,
    TotalFee DECIMAL(18,2) NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_LikeList_User FOREIGN KEY (UserID) REFERENCES [User](UserID),
    CONSTRAINT FK_LikeList_Product FOREIGN KEY (ProductNo) REFERENCES Product(ProductNo)
);
