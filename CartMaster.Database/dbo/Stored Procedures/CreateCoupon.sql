CREATE PROCEDURE CreateCoupon
	@CouponName VARCHAR(25),
	@CouponDescription NVARCHAR(MAX),
	@ValidFrom DATETIME,
	@ValidTo DATETIME,
	@DiscountPercentage INT,
	@IsValid BIT
AS
BEGIN
	INSERT INTO Coupons (CouponName, CouponDescription, ValidFrom, ValidTo, IsValid, DiscountPercentage)
	VALUES (@CouponName, @CouponDescription, @ValidFrom, @ValidTo, @IsValid, @DiscountPercentage)
END