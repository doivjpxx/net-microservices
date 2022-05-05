using ShoppingCart.EventFeed;
using ShoppingCart.ShoppingCart;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IShoppingCartStore, ShoppingCartStore>();

// Add http client
builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

// Run app
app.Run();