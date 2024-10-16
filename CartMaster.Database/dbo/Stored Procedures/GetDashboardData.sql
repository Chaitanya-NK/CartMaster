CREATE PROCEDURE GetDashboardData
AS
BEGIN
	-- Dashboard Counts
	SELECT
		(SELECT COUNT(*) FROM Products) AS TotalProducts,
		(SELECT COUNT(*) FROM Users) AS TotalUsers,
		(SELECT COUNT(*) FROM Orders) AS TotalOrders,
		(SELECT SUM(TotalAmount) FROM Orders) AS Revenue,
		(SELECT COUNT(*) FROM ProductReviews) AS TotalReviews,
		(SELECT COUNT(*) FROM OrderItems WHERE ReturnRequested = 1) AS PendingReturns,
		(SELECT COUNT(*) FROM Products WHERE StockQuantity = 0) AS OutOfStockProducts,
		(SELECT COUNT(*) AS RepeatCustomersCount
		FROM (
			SELECT UserID
			FROM Orders
			GROUP BY UserID
			HAVING COUNT(OrderID) > 1
		) AS RepeatCustomers) As RepeatCustomersCount,
		(SELECT COUNT(*) FROM Orders WHERE Status = 'Cancelled') AS CancelledOrders,
		(SELECT COUNT(*) FROM Coupons WHERE IsValid = 1) As Coupons,
		(SELECT COUNT(*) FROM UserSessions WHERE IsActive = 1) AS CurrentLogins,
		(SELECT COUNT(*) FROM Categories) AS TotalCategories

	-- Wishlist Insights
	SELECT ProductID, ProductName, COUNT(*) AS WishlistCount 
	FROM Wishlist
	GROUP BY ProductID, ProductName
	ORDER BY WishlistCount DESC;

	-- Sales Data Per Month
	SELECT YEAR(OrderDate) AS Year, MONTH(OrderDate) AS Month, SUM(TotalAmount) AS Sales 
    FROM Orders
    GROUP BY YEAR(OrderDate), MONTH(OrderDate)
    ORDER BY Year, Month;

	-- User Growth
	SELECT YEAR(CreatedAt) AS Year, MONTH(CreatedAt) AS Month, COUNT(*) AS UserRegistrations
    FROM Users
    GROUP BY YEAR(CreatedAt), MONTH(CreatedAt)
    ORDER BY Year, Month;

	-- Top Selling Products
	SELECT TOP 5 ProductID, ProductName, SUM(Quantity) AS TotalSold
    FROM OrderItems
    GROUP BY ProductID, ProductName
    ORDER BY TotalSold DESC;

	-- Top Reviewed Products
	SELECT TOP 5 
        p.ProductID,
        p.ProductName,
        COUNT(r.ReviewID) AS ReviewCount,
		ISNULL(ROUND(AVG(CAST(r.Rating AS DECIMAL(10,2))),2), 0) AS AverageRating
    FROM ProductReviews r
    INNER JOIN Products p ON r.ProductID = p.ProductID
    GROUP BY p.ProductID, p.ProductName
    ORDER BY AverageRating DESC;

	-- Low Stock Products
	SELECT 
        p.ProductID,
        p.ProductName,
        p.StockQuantity
    FROM Products p
    WHERE p.StockQuantity <= 10 -- Consider 10 as low stock threshold
    ORDER BY p.StockQuantity ASC;

	-- Inactive Users
	SELECT u.Username, CONCAT(u.FirstName, ' ', u.LastName) AS Name, u.Email
    FROM Users u
    LEFT JOIN Orders o ON u.UserID = o.UserID
    WHERE o.OrderID IS NULL OR DATEDIFF(MONTH, o.OrderDate, GETDATE()) > 6
    ORDER BY u.UserID;

	-- Best Categories (Sales)
	SELECT TOP 5 c.CategoryName, SUM(oi.Quantity) AS TotalSold
    FROM OrderItems oi
    INNER JOIN Products p ON oi.ProductID = p.ProductID
    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
    GROUP BY c.CategoryName
    ORDER BY TotalSold DESC;
END