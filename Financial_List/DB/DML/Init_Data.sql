-- User
INSERT INTO [User] (UserID, Password, UserName, Email, Account) VALUES
('A123456789','A123456789', N'王大明', 'daming@example.com', '123456789012'),
('B987654321','A123456789', N'陳小美', 'xiaomei@example.com', '987654321098'),
('C246810121','A123456789', N'林志遠', 'chihyuan@example.com', '246810121314');

-- Product
INSERT INTO Product (ProductName, Price, FeeRate) VALUES
(N'高收益債券基金', 1000.00, 0.015),
(N'美國S&P500 ETF', 1500.00, 0.010),
(N'台灣加權指數ETF', 900.00, 0.008),
(N'全球科技基金', 2000.00, 0.012),
(N'黃金現貨ETF', 1800.00, 0.010),
(N'新興市場債券', 1300.00, 0.018),
(N'美元定存商品', 10000.00, 0.005),
(N'綠能產業基金', 1700.00, 0.013),
(N'AI人工智慧基金', 2200.00, 0.014),
(N'不動產投資信託(REITs)', 1600.00, 0.011);

-- LikeList
INSERT INTO LikeList (UserID, ProductNo, OrderAmount, Account, TotalFee, TotalAmount) VALUES
-- 王大明
('A123456789', 1, 2, '123456789012', 30.00, 2030.00),
('A123456789', 4, 1, '123456789012', 24.00, 2024.00),

-- 陳小美
('B987654321', 2, 3, '987654321098', 45.00, 4545.00),
('B987654321', 5, 1, '987654321098', 18.00, 1818.00),

-- 林志遠
('C246810121', 6, 2, '246810121314', 46.80, 2646.00),
('C246810121', 9, 1, '246810121314', 30.80, 2230.00);
