CREATE PROCEDURE UpdateCoupon
	@CouponID INT,
	@CouponName VARCHAR(25),
	@CouponDescription NVARCHAR(MAX),
	@ValidFrom DATETIME,
	@ValidTo DATETIME,
	@DiscountPercentage INT,
	@IsValid BIT
AS
BEGIN
	UPDATE Coupons
	SET
		CouponName = @CouponName,
		CouponDescription = @CouponDescription,
		ValidFrom = @ValidFrom,
		ValidTo = @ValidTo,
		IsValid = @IsValid,
		DiscountPercentage = @DiscountPercentage
	WHERE CouponID = @CouponID
END