CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT,
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
	CreatedAt DATETIME,
	ModifiedAt DATETIME,
    DiscountAmount DECIMAL(10,2) DEFAULT 0,
    FinalAmount DECIMAL(10,2),
    CouponID INT NULL
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (CouponID) REFERENCES Coupons(CouponID)
);