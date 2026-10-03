using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

using System.Security.Claims;
using JWTAPI;
using JWTAPI.Context;
using JWTAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddCors();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateLifetime =  true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});

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

app.MapGet("/api/getuserorders/{userId}", (MyDbContext context, int userId) =>
{
    return Results.Ok(context.Orders.Where(o => o.Id.Equals(userId)).ToList());

});

app.MapPatch("/api/patchorderstatus/{orderId}&{statusId}", async (MyDbContext context, int orderId, int statusId) =>
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
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

    if (statusId == 0)
    {
        clothe.IsAvailable = false;
    }
    clothe.IsAvailable = true;
    await context.SaveChangesAsync();
    return Results.Ok(clothe);
    
});
app.MapPost("/auth/register", async (User user, MyDbContext cnt) =>
{
    var exists = await cnt.Users
        .AnyAsync(x => x.Login == user.Login);

    if (exists)
        return Results.BadRequest("Пользователь с таким логином уже существует");

    user.Role = 1;

    cnt.Users.Add(user);
    await cnt.SaveChangesAsync();

    return Results.Ok(new
    {
        user.Id,
        user.Login,
        user.Role
    });
});

app.MapPost("/auth/login", async (AuthData data, MyDbContext cnt) =>
{
    var user = await cnt.Users
        .Where(u => u.Login == data.Login && u.Password == data.Password)
        .Select(u => new
        { u.Id, u.Login, u.Role, RoleName = u.RoleNavigation.Role1 })
        .FirstOrDefaultAsync();

    if (user == null)
        return Results.Unauthorized();

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Login),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.AddDays(365),
        signingCredentials: new SigningCredentials(
            AuthOptions.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256)
    );

    var token = new JwtSecurityTokenHandler().WriteToken(jwt);

    return Results.Ok(new
    {
        User = user,
        Token = token
    });
});

app.MapPost("/api/post/orders", async (Order order, ClaimsPrincipal user, MyDbContext cnt) =>
{
    var login = user.FindFirst(ClaimTypes.Name)?.Value;

    var currentUser = await cnt.Users
        .FirstOrDefaultAsync(x => x.Login == login);

    if (currentUser == null)
        return Results.Unauthorized();

    order.UserId = currentUser.Id;
    order.DateTime = DateOnly.FromDateTime(DateTime.Now);
    order.Status = 1;

    cnt.Orders.Add(order);

    await cnt.SaveChangesAsync();

    return Results.Ok(order);
}).RequireAuthorization();


    

app.Run();