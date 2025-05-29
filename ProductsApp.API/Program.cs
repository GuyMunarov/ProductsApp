using ProductsApp.API.Products.Managers;
using ProductsApp.API.Swagger;
using ProductsApp.API.Users.Managers;
using ProductsApp.Infrastructure;
using ProductsApp.Persistance;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services
    .AddOpenApi("v1", options => { options.AddDocumentTransformer<OpenApiBearerTransformer>(); })
    .InstallPersistance()
    .InstallInfrastructure(builder.Configuration)
    .AddScoped<IProductsManager, ProductsManager>()
    .AddScoped<IUserManager, UserManager>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "ProductsApp.API"));
}

SeedManager.SeedRecords(app.Services);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => new { Status = "Ok" })
    .WithName("Healthcheck");

app.Run();