using Microsoft.EntityFrameworkCore;
using SweetGlazeCRM.domain.entities;
using SweetGlazeCRM.infrastructure.data;
using SweetGlazeCRM.infrastructure.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MasterCRMDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MasterCRM")));

builder.Services.AddScoped<ITenantDatabaseResolver, TenantDatabaseResolver>();
builder.Services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();

builder.Services.AddDbContext<TenantCRMDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TenantCRM")));

// Add services to the container.
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// =====================================================
// CREATE COMPANY
// =====================================================

app.MapPost("/companies", async (
    Company company,
    MasterCRMDbContext db) =>
{
    db.Companies.Add(company);

    await db.SaveChangesAsync();

    return Results.Created(
        $"/companies/{company.CompanyId}",
        company);
});


// =====================================================
// CREATE DEVICE
// =====================================================

app.MapPost("/devices", async (
    Device device,
    MasterCRMDbContext db) =>
{
    db.Devices.Add(device);

     await db.SaveChangesAsync();

    return Results.Created(
        $"/devices/{device.DeviceId}",
        device);
});


// =====================================================
// CREATE COMPANY DATABASE
// =====================================================

app.MapPost("/company-databases", async (
    CompanyDatabase companyDatabase,
    MasterCRMDbContext db) =>
{
    db.CompanyDatabases.Add(companyDatabase);

    await db.SaveChangesAsync();

    return Results.Created(
        $"/company-databases/{companyDatabase.CompanyDatabaseId}",
        companyDatabase);
});


// DEBUG TENANT DATABASE INFO
app.MapGet("/debug-tenant/{companyId:int}", async (
    int companyId,
    ITenantDatabaseResolver resolver) =>
{
    var info = await resolver.GetDatabaseInfoAsync(companyId);

    return Results.Ok(new
    {
        companyId,
        serverName = info.ServerName,
        databaseName = info.DatabaseName,
        credentialKey = info.CredentialKey
    });
});
// =====================================================
// TEST TENANT DATABASE CONNECTION
// =====================================================

app.MapGet("/test-tenant/{companyId:int}", async (
    int companyId,
    ITenantDbContextFactory tenantFactory) =>
{
    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    var productCount =
        await tenantDb.Products.CountAsync();

    return Results.Ok(new
    {
        companyId,
        productCount
    });
});


// =====================================================
// CREATE TENANT PRODUCT
// =====================================================

app.MapPost("/tenant/{companyId:int}/products", async (
    int companyId,
    Product product,
    ITenantDbContextFactory tenantFactory) =>
{
    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    tenantDb.Products.Add(product);

    await tenantDb.SaveChangesAsync();

    return Results.Created(
        $"/tenant/{companyId}/products/{product.ProductId}",
        product);
});


// =====================================================
// GET TENANT PRODUCTS
// =====================================================

app.MapGet("/tenant/{companyId:int}/products", async (
    int companyId,
    ITenantDbContextFactory tenantFactory) =>
{
    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    var products = await tenantDb.Products
        .AsNoTracking()
        .OrderBy(x => x.ProductId)
        .ToListAsync();

    return Results.Ok(products);
});


app.Run();