using System.Text.Json.Serialization;
using JWTAPI.Context;
using JWTAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<MyDbContext>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthOption.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthOption.AUDIENCE,
        ValidateLifetime = true,
        IssuerSigningKey = AuthOption.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/api/getclothes", (MyDbContext context) =>
{
   
    return Results.Ok( context.Clothes.ToList());
    
    
});
app.MapGet("/api/getclothes/{name}", (MyDbContext context, string name) =>
{
   
    return Results.Ok( context.Clothes.Where(c => c.Name.ToLower().Contains(name.ToLower())).ToList());
    
    
});

app.MapGet("/api/getorderstatus/{statusId}", (MyDbContext context,int statusId) =>
{
    return Results.Ok(context.Orders.Where(o => o.Status.Equals(statusId)).ToList());
});

app.MapGet("/api/getusrorders/{userId}", (MyDbContext context, int userId) =>
{
    return Results.Ok(context.Orders.Where(o => o.Id.Equals(userId)).ToList());

});

app.MapPatch("/api/patchorderstus/{orderId}&{statusId}", async (MyDbContext context, int orderId, int statusId) =>
{
    var order = context.Orders.FirstOrDefault(o => o.Id.Equals(orderId));
    if (order == null)
    {
        return Results.NotFound();
    }
    
    order.Status = statusId;
    await context.SaveChangesAsync();
    return Results.Ok(order);
    
});

app.MapPatch("/api/patchsizeclothes/{clotheId}&{sizeId}&{quantity}", async (MyDbContext context, int clotheId, int sizeId, int quantity) =>
    {
        var clotheSize = context.ClothesSizes.FirstOrDefault(x => x.ClothesId == clotheId && x.SizeId == sizeId);

        if (clotheSize == null)
        {
            return Results.NotFound();
        }
        clotheSize.Size.Count =  quantity;
        
        await context.SaveChangesAsync();

        return Results.Ok(clotheSize);
    });
app.MapPatch("/api/patchclothesavalible/{clotheId}&{statusId}", async (MyDbContext context, int clotheId, int statusId) =>
{
    var clothe = context.Clothes.FirstOrDefault(o => o.Id.Equals(clotheId));
    if (clothe == null)
    {
        return Results.NotFound();
    }

    if (statusId == 0)
    {
        clothe.IsAvailable = false;
    }
    clothe.IsAvailable = true;
    await context.SaveChangesAsync();
    return Results.Ok(clothe);
    
});

app.Run();