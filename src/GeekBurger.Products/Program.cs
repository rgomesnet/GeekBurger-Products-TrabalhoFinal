using GeekBurger.Products.Application.AddProduct;
using GeekBurger.Products.Application.DeleteProduct;
using GeekBurger.Products.Application.GetProduct;
using GeekBurger.Products.Domain.Repositories;
using GeekBurger.Products.Domain.Services;
using GeekBurger.Products.Extesnsions;
using GeekBurger.Products.Infra.MessagesBus;
using GeekBurger.Products.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Products",
        Version = "v1"
    });
});

builder.Services.Configure<ServiceBusConfiguration>(
    builder.Configuration.GetSection("serviceBus"));

builder.Services.AddDbContext<ProductsDbContext>();

builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductChangedEventRepository, ProductChangedEventRepository>();

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IGetProductService, GetProductService>();
builder.Services.AddScoped<IAddProductService, AddProductService>();
builder.Services.AddScoped<IDeleteProductService, DeleteProductService>();
builder.Services.AddScoped<IProductChangedService, ProductChangedService>();


var app = builder.Build();

app.UseRouting();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Services.GetService<IServiceScopeFactory>()!
            .CreateScope()
            .ServiceProvider
            .GetService<ProductsDbContext>()!
            .Seed();

app.Run();