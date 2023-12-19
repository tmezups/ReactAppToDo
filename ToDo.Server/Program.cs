using Todo.Server.Configuration;
using Todo.Server.Repositories;
using Todo.Server.Services;
using Todo.Server.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperConnectionProvider>();
builder.Services.AddSingleton<ToDoRepository>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<AccountRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddAuthentication().AddCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; //this is just so that we dont need certs, we should never do this in a production site
    options.LoginPath = new PathString("/Login");
    options.LogoutPath = new PathString("/LogOut");
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("https://localhost:5173", 
                    "https://localhost:5000",
                    "http://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseCors();
app.UseStaticFiles();

// making swagger available in all envs, it requires authentication
app.UseMiddleware<SwaggerBasicAuthMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program { }