namespace CartMaster.Static
{
    public class StaticStrings
    {
        public const string DBString = "ConnectionString";
    }

    public class StaticToken
    {
        public const string UserID = "UserID";
        public const string Username = "Username";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string Email = "Email";
        public const string RoleID = "RoleID";
        public const string RoleName = "RoleName";
        public const string CartID = "CartID";
        public const string JwtKey = "sdfgbhnjmsdfgbhnjmfdsxdcfvgbhnmgvfcdsxdcfvgbnmmjhgvfcxcvbnmbvcxcvbnm";
        public const string JwtIssuer = "https://www.google.com";
        public const string JwtAudience = "https://www.google.com";
        public const int JwtTokenExpiryMinutes = 1000;
    }

    public class StaticUser
    {
        public const string UserRegisterSuccess = "User Registered Successfully";
        public const string UserUpdateSuccess = "User Updated Successfully";
        public const string SaveUserAddressSuccess = "User Address Saved Successfully";
    }

    public class StaticLogin
    {
        public const string InvalidUser = "Wrong Credentials";
    }

    public class StaticCategory
    {
        public const string AddCategorySuccess = "Category Added Successfully";
        public const string DeleteCategorySuccess = "Category Deleted Successfully";
        public const string UpdateCategorySuccess = "Category Updated Successfully";
        public const string OperationSuccess = "Action Successful";
    }

    public class StaticProduct
    {
        public const string AddProductSuccess = "Product Added Successfully";
        public const string DeleteProductSuccess = "Product Deleted Successfully";
        public const string UpdateProductSuccess = "Product Updated Successfully";
        public const string OperationSuccess = "Action Successful";
    }

    public class StaticCart
    {
        public const string CreateCartSuccess = "Cart Created Successfully";
        public const string AddProductToCartSuccess = "Product Added to Cart Successfully";
        public const string RemoveProductFromCartSuccess = "Product Removed from the Cart Successfully";
        public const string UpdateCartItemQuantitySuccess = "Cart Item Quantity Updated Successfully";
    }

    public class StaticOrder
    {
        public const string CreateOrderSuccess = "Order Created Successfully";
        public const string UpdateOrderStatusSuccess = "Order Status Updated Successfully";
        public const string CancelOrderSuccess = "Order Cancelled Successfully.";
        public const string RequestReturnSuccess = "Return Requested Successfully";
        public const string ProcessReturnSuccess = "Return Processed Successfully";
    }

    public class StaticProductReview
    {
        public const string AddProductReviewSuccess = "Product Review Added Successfully";
        public const string DeleteProductReviewSuccess = "Product Review Deleted Successfully";
        public const string UpdateProductReviewSuccess = "Product Review Updated Successfully";
        public const string ProductReviewsSuccess = "Product Review Retrieved Successfully";
    }

    public class StaticWishlist
    {
        public const string AddWishlsitSuccess = "Product Added to Wishlist Successfully";
        public const string RemoveWishlsitSuccess = "Product Removed From Wishlist Successfully";
        public const string OperationSuccess = "Action Successful";
    }
}