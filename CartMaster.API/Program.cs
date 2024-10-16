using CartMaster.API;
using CartMaster.Business.IServices;
using CartMaster.Business.Services;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Data.Repositories;
using CartMaster.TokenGeneration.TokenImplementation;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddTransient<IProductReviewService, ProductReviewService>();
builder.Services.AddTransient<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddTransient<IMenuItemService, MenuItemService>();
builder.Services.AddTransient<IWishlistRepository, WishlistRepository>();
builder.Services.AddTransient<IWishlistService, WishlistService>();
builder.Services.AddTransient<IOTPRepository, OTPRepository>();
builder.Services.AddTransient<IOTPService, OTPService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IDashboardCountsRepository, DashboardCountsRepository>();
builder.Services.AddTransient<IDashboardCountsService, DashboardCountsService>();
builder.Services.AddTransient<ICouponRepository, CouponRepository>();
builder.Services.AddTransient<ICouponService, CouponService>();
builder.Services.AddTransient<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddTransient<IUserSessionService, UserSessionService>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IToken, Token>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "CartMaster",
            Version = "v1",
            Description = @"This is the API documentation for the Cart Master E-Commerce Website.

    Cart Master provides two types of users: Customers and Admins. 

Customer Features:
- Registration & Login: Users can register a new account, log in using their credentials, and securely manage their session.
- Product Browsing & Search: Customers can view a catalog of products, with options to search for products by name or description.
- Product Filters: Users can apply filters by category, price range, or sorting (A-Z, Z-A, low-high price, high-low price).
- Cart Management: Customers can add products to their cart, view items in their cart, update quantities, or remove items.
- Checkout & Payment: Users can proceed to checkout by providing their delivery address and selecting a payment method (Card, UPI, or Cash on Delivery).
- Order Management: Customers can view a list of their past and ongoing orders, track the status of specific orders, and download invoices.
- Order Cancellation & Returns: Users can cancel an order if it has not yet shipped or request a return for delivered products.
- Profile Management: Customers can view and update their profile information, including shipping addresses and contact details.
- Wishlist: Customers can add items to their wishlist for later reference or remove them as needed.
- Logout: Securely log out from their session.

Admin Features:
- Product Management: Admins can add new products, update existing product details (name, description, price, stock), and remove products from the inventory.
- Category Management: Admins can manage product categories by adding, editing, viewing, or deleting categories.
- User Management: Admins can view a list of registered users and their order history, including user-specific order details.
- Order Management: Admins can update the status of user orders (e.g., pending, shipped, delivered) and handle return requests submitted by customers.
- Dashboard & Analytics: Admins have access to a comprehensive dashboard that displays key insights such as:
    - Product & Category Metrics: View total product count, categories, low-stock products, out-of-stock items.
    - User Metrics: Monitor total registered users, monthly user growth, and a list of inactive users.
    - Order & Sales Insights: Access sales data by month, track total revenue, and view canceled or returned orders.
    - Review & Wishlist Insights: View top-reviewed products and wishlist trends to gain insights into customer preferences.
    - Repeat Customers & Revenue: Track repeat customers and view revenue generated from them.
- Data Export: Admins can export various data sets (e.g., products, orders, users) to Excel for reporting and analysis purposes.

        This API is designed to support all functionalities of Cart Master, enabling seamless management of the e-commerce platform for both customers and administrators."
        });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.EnableAnnotations();
    options.OperationFilter<SwaggerFileOperationFilter>();
});


var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables()
                        .Build();


string? connectionString = configuration.GetConnectionString("ConnectionString");

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
