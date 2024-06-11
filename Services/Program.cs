using Data.Contexts;
using Data.Models.ProductDb;
using Data.Models.TransactionDb;
using Data.Models.UserDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services;
using Services.Repositories;
using Services.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddDbContext<ProductDbContext>();
builder.Services.AddDbContext<TransactionDbContext>();

builder.Services.AddScoped<IRepository<User>, GenericRepository<User, UserDbContext>>();
builder.Services.AddScoped<IRepository<Product>, GenericRepository<Product, ProductDbContext>>();
builder.Services.AddScoped<IRepository<Transaction>, GenericRepository<Transaction, TransactionDbContext>>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TransactionService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add a CORS policy for the client
// Add .AllowCredentials() for apps that use an Identity Provider for authn/z
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ThreeTierApp API", Version = "v1" });
});

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userContext = services.GetRequiredService<UserDbContext>();
        var productContext = services.GetRequiredService<ProductDbContext>();
        var transactionContext = services.GetRequiredService<TransactionDbContext>();

        userContext.Database.Migrate();
        productContext.Database.Migrate();
        transactionContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ThreeTierApp API v1"));
}

// Activate the CORS policy
app.UseCors("wasm");

app.UseHttpsRedirection();

// Define minimal API endpoints
app.MapGet("/", () => "Welcome to Three Tier App REST API! Add /swagger at the end of this address to access Swagger UI.");

app.MapGet("/users", async (UserService userService) => await userService.GetAllUsersAsync())
    .WithName("GetAllUsers")
    .WithOpenApi();

app.MapGet("/users/{id}", async (int id, UserService userService) => await userService.GetUserByIdAsync(id))
    .WithName("GetUserById")
    .WithOpenApi();

app.MapPost("/users", async (User user, UserService userService) => { await userService.AddUserAsync(user); return Results.Ok(); })
    .WithName("AddUser")
    .WithOpenApi();

app.MapPut("/users/{id}", async (int id, User user, UserService userService) => { await userService.UpdateUserAsync(user); return Results.Ok(); })
    .WithName("UpdateUser")
    .WithOpenApi();

app.MapDelete("/users/{id}", async (int id, UserService userService) => { await userService.DeleteUserAsync(id); return Results.Ok(); })
    .WithName("DeleteUser")
    .WithOpenApi();

app.MapGet("/products", async (ProductService productService) => await productService.GetAllProductsAsync())
    .WithName("GetAllProducts")
    .WithOpenApi();

app.MapGet("/products/{id}", async (int id, ProductService productService) => await productService.GetProductByIdAsync(id))
    .WithName("GetProductById")
    .WithOpenApi();

app.MapPost("/products", async (Product product, ProductService productService) => { await productService.AddProductAsync(product); return Results.Ok(); })
    .WithName("AddProduct")
    .WithOpenApi();

app.MapPut("/products/{id}", async (int id, Product product, ProductService productService) => { await productService.UpdateProductAsync(product); return Results.Ok(); })
    .WithName("UpdateProduct")
    .WithOpenApi();

app.MapDelete("/products/{id}", async (int id, ProductService productService) => { await productService.DeleteProductAsync(id); return Results.Ok(); })
    .WithName("DeleteProduct")
    .WithOpenApi();

app.MapGet("/transactions", async (TransactionService transactionService) => await transactionService.GetAllTransactionsAsync())
    .WithName("GetAllTransactions")
    .WithOpenApi();

app.MapGet("/transactions/{id}", async (int id, TransactionService transactionService) => await transactionService.GetTransactionByIdAsync(id))
    .WithName("GetTransactionById")
    .WithOpenApi();

app.MapPost("/transactions", async (Transaction transaction, TransactionService transactionService) => { await transactionService.AddTransactionAsync(transaction); return Results.Ok(); })
    .WithName("AddTransaction")
    .WithOpenApi();

app.MapPut("/transactions/{id}", async (int id, Transaction transaction, TransactionService transactionService) => { await transactionService.UpdateTransactionAsync(transaction); return Results.Ok(); })
    .WithName("UpdateTransaction")
    .WithOpenApi();

app.MapDelete("/transactions/{id}", async (int id, TransactionService transactionService) => { await transactionService.DeleteTransactionAsync(id); return Results.Ok(); })
    .WithName("DeleteTransaction")
    .WithOpenApi();

app.Run();
