using backend.Config.db;
using backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;


#region Conection Banco
Env.Load();
var builder = WebApplication.CreateBuilder(args);

var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASS")};";
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // URL do seu front-end
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
#endregion

#region JWT
//var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
//var key = Encoding.UTF8.GetBytes(secretKey);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key)
//    };
//});
#endregion

#region config service
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Conexao>(options =>
            options.UseNpgsql(connectionString));

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<LoginService>();
#endregion

var app = builder.Build();
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000"; // padrão 5000
app.Urls.Add($"http://*:{port}");


#region SWAGGER

app.UseSwagger();
app.UseSwaggerUI();

#endregion

app.UseCors("AllowFrontend");

//app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();

app.Run();
