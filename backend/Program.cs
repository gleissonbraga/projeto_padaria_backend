using backend.Config.db;
using backend.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


#region Conection Banco
Env.Load("../render.env");
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
            policy.WithOrigins("http://localhost:3000", "https://projeto-padaria-frontend.vercel.app") // URL do seu front-end
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
#endregion

#region JWT
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
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
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<PagamentoService>();
#endregion

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000"; // fallback para teste local

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // 50 MB, por exemplo
});

var app = builder.Build();

app.Urls.Add($"http://0.0.0.0:{port}");



#region creating tables in render
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<Conexao>();
//    db.Database.Migrate();
//}
#endregion

// Habilitar página de erro detalhada (apenas para teste)
app.UseDeveloperExceptionPage();


#region SWAGGER

app.UseSwagger();
app.UseSwaggerUI();

#endregion

app.UseCors("AllowFrontend");

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();
