using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amazon.S3;
using dreamify_recording_manager.Handlers;

using dreamify.Domain.Entities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Identity;
using drf.Application.Abstracts;
using drf.Application.Services;
using drf.Infrastructure;
using drf.Infrastructure.Options;
using drf.Infrastructure.Processors;
using drf.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

// builder.Services.Configure<ApiBehaviorOptions>(options =>
// {
//     options.InvalidModelStateResponseFactory = context =>
//     {
//         var response = new
//         {
//             error = "Invalid request format. Please check your input and try again."
//         };
//         return new BadRequestObjectResult(response);
//     };
// });
// builder.Services.Configure<ApiBehaviorOptions>(options =>
// {
//     options.InvalidModelStateResponseFactory = context =>
//     {
//         // Log the actual validation errors
//         foreach (var error in context.ModelState)
//         {
//             Console.WriteLine($"ModelState Error - Key: {error.Key}");
//             foreach (var err in error.Value.Errors)
//             {
//                 Console.WriteLine($"  Error: {err.ErrorMessage}");
//             }
//         }
//         
//         var response = new
//         {
//             error = "Invalid request format. Please check your input and try again.",
//             details = context.ModelState
//         };
//         return new BadRequestObjectResult(response);
//     };
// });





//builder.Services.AddOpenApi();

builder.Services.Configure<JwtOptions> (builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));
builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection(AwsOptions.AwsOptionsKey));
//builder.Services.Configure<OpenAiOptions> (builder.Configuration.GetSection(OpenAiOptions.OpenApiOptionsKey));
//builder.Services.Configure<MailOptions> (builder.Configuration.GetSection(MailOptions.MailOptionsKey));

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var awsOption = builder.Configuration.GetSection(AwsOptions.AwsOptionsKey)
        .Get<AwsOptions>() ?? throw new ArgumentException(nameof(AwsOptions));
    var config = new AmazonS3Config
    {
        ServiceURL = awsOption.ServiceUrl,
        ForcePathStyle = true 
    };
    
  
    return new AmazonS3Client(
        awsOption.AccessKey,
        awsOption.SecretKey,
        config
    );
});

builder.Services.AddIdentity<User, IdentityRole<Guid>>(opt =>
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequiredLength = 8;
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseMySQL(builder.Configuration.GetConnectionString("DbConnectionString"));
});

//builder.Services.AddScoped<IAuthTokenProcessor,AuthTokenProcessor>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDreamsRepository, DreamsRepository>();
builder.Services.AddScoped<IBucketService, BucketService>();
builder.Services.AddScoped<IS3Processor, S3Processor>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
// builder.Services.AddScoped<IAccountService,AccountService>();
// builder.Services.AddScoped<IOpenAiRequestProcessor, OpenAiRequestProcessor>();
// builder.Services.AddScoped<IOpenAiService, OpenAiService>();
// builder.Services.AddScoped<IGoogleTokenProcessor, GoogleTokenProcessor>();
// builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
// builder.Services.AddScoped<IEmailServiceProcessors, EmailServiceProcessors>();

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }
)
.AddJwtBearer(options =>
{
    var jwtOption = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey)
        .Get<JwtOptions>() ?? throw new ArgumentException(nameof(JwtOptions));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOption.Issuer,
            ValidAudience = jwtOption.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret))
        
    };
    //Console.WriteLine($"Validation - Secret first 10 chars: {jwtOption.Secret.Substring(0, 10)}");
    Console.WriteLine($"Validation - Issuer: {jwtOption.Issuer}");
    Console.WriteLine($"Validation - Audience: {jwtOption.Audience}");
    
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            Console.WriteLine($"Exception type: {context.Exception.GetType().Name}");
            if (context.Exception.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {context.Exception.InnerException.Message}");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully!");
            Console.WriteLine($"Principal identity name: {context.Principal.Identity.Name}");
            Console.WriteLine($"Principal identity authenticated: {context.Principal.Identity.IsAuthenticated}");
            Console.WriteLine($"Claims count: {context.Principal.Claims.Count()}");
            
            // Debug: Print all claims
            foreach (var claim in context.Principal.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }
            // Force set the HttpContext user
            context.HttpContext.User = context.Principal;
            
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            Console.WriteLine("=== JWT MESSAGE RECEIVED ===");
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                context.Token = authHeader.Substring("Bearer ".Length).Trim();
                Console.WriteLine("Token extracted successfully");
            }
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("dreamify API");
    });
}

app.UseCors("AllowOrigins");
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseRouting(); 
app.Use(async (context, next) =>
{
    Console.WriteLine($"=== MIDDLEWARE DEBUG: {context.Request.Method} {context.Request.Path} ===");
    Console.WriteLine($"Authorization header exists: {context.Request.Headers.ContainsKey("Authorization")}");
    
    await next.Invoke();
    Console.WriteLine($"=== MIDDLEWARE DEBUG END: Response Status {context.Response.StatusCode} ===");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
