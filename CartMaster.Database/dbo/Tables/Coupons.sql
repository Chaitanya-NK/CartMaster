CREATE TABLE Coupons (
	CouponID INT IDENTITY(1,1) PRIMARY KEY,
	CouponName VARCHAR(25) NOT NULL,
	CouponDescription NVARCHAR(MAX) NOT NULL,
	ValidFrom DATETIME,
	ValidTo DATETIME,
	IsValid BIT,
	DiscountPercentage INT
)