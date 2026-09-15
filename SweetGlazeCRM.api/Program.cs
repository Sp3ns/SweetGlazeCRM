using Microsoft.EntityFrameworkCore;
using SweetGlazeCRM.domain.Entities;
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


// =====================================================
// DEBUG TENANT DATABASE INFO
// =====================================================

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


// =====================================================
// CUSTOMER CRUD
// =====================================================


// =====================================================
// CREATE TENANT CUSTOMER
// =====================================================

app.MapPost("/tenant/{companyId:int}/customers", async (
    int companyId,
    Customer customer,
    ITenantDbContextFactory tenantFactory) =>
{
    customer.Id = 0;
    customer.CreatedAt = DateTime.UtcNow;
    customer.UpdatedAt = null;
    customer.IsActive = true;

    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    tenantDb.Customers.Add(customer);

    await tenantDb.SaveChangesAsync();

    return Results.Created(
        $"/tenant/{companyId}/customers/{customer.Id}",
        customer);
});


// =====================================================
// GET ALL TENANT CUSTOMERS
// =====================================================

app.MapGet("/tenant/{companyId:int}/customers", async (
    int companyId,
    ITenantDbContextFactory tenantFactory) =>
{
    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    var customers = await tenantDb.Customers
        .AsNoTracking()
        .OrderBy(x => x.Id)
        .ToListAsync();

    return Results.Ok(customers);
});


// =====================================================
// GET TENANT CUSTOMER BY ID
// =====================================================

app.MapGet("/tenant/{companyId:int}/customers/{id:int}", async (
    int companyId,
    int id,
    ITenantDbContextFactory tenantFactory) =>
{
    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    var customer = await tenantDb.Customers
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);

    if (customer == null)
    {
        return Results.NotFound("Customer not found.");
    }

    return Results.Ok(customer);
});


// =====================================================
// UPDATE TENANT CUSTOMER
// =====================================================

app.MapPut("/tenant/{companyId:int}/customers/{id:int}", async (
    int companyId,
    int id,
    Customer updatedCustomer,
    ITenantDbContextFactory tenantFactory) =>
{
    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    var customer = await tenantDb.Customers
        .FirstOrDefaultAsync(x => x.Id == id);

    if (customer == null)
    {
        return Results.NotFound("Customer not found.");
    }

    customer.FirstName = updatedCustomer.FirstName;
    customer.LastName = updatedCustomer.LastName;
    customer.Email = updatedCustomer.Email;
    customer.PhoneNumber = updatedCustomer.PhoneNumber;
    customer.CompanyName = updatedCustomer.CompanyName;
    customer.Address = updatedCustomer.Address;
    customer.Notes = updatedCustomer.Notes;
    customer.IsActive = updatedCustomer.IsActive;
    customer.UpdatedAt = DateTime.UtcNow;

    await tenantDb.SaveChangesAsync();

    return Results.Ok(customer);
});


// =====================================================
// DELETE TENANT CUSTOMER
// =====================================================

app.MapDelete("/tenant/{companyId:int}/customers/{id:int}", async (
    int companyId,
    int id,
    ITenantDbContextFactory tenantFactory) =>
{
    await using var tenantDb =
        await tenantFactory.CreateAsync(companyId);

    var customer = await tenantDb.Customers
        .FirstOrDefaultAsync(x => x.Id == id);

    if (customer == null)
    {
        return Results.NotFound("Customer not found.");
    }

    tenantDb.Customers.Remove(customer);

    await tenantDb.SaveChangesAsync();

    return Results.Ok("Customer deleted successfully.");
});


app.Run();