using JwtTokenAuthentication;
using JwtTokenAuthentication.Services;
using JwtTokenAuthentication.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
// Add services to the container.

builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IProductService, ProductService>();


//here jwtsettings variable is used to get the value from appsetting.json file



var jwtSettings = builder.Configuration.GetSection("Jwt");



// it will asp.net authentication scheme

//Authorization : Bearer <asfjsdlkfslfkskldfkldskl>


builder.Services.AddAuthentication(
    options =>
    {
        // Authenticate="who is the user?
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        // 401 authorizated erorr
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // here we can check who create the token(
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    }

);


builder.Services.AddAuthorization(options =>
{

    // create a polic name"Adminonly"

    // this policy can access where role have admin and  employee

    options.AddPolicy("AdminOnly", policy =>
    policy.RequireRole("Admin", "Manager"));

});



//builder.Services.AddSingleton<UserData>();

//ILogger<

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token Example : Bearer asdfasdfadfsdadsfsds",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });


    //we will add bearer scheme definition for all the above the endpoints 
    // so swagger automatically sends the token with every request after you can click on authorize
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
        {
            Reference= new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }

        },
         new string[]{}
    }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//
app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();
