using API.Data;  // Pour AppDbContext
using API.Services;  // Pour IUserService, UserService, AgenceService, FournitureService, AuthService
using API.Middleware; 
using Microsoft.EntityFrameworkCore;
 // Pour JsonRequestMiddleware

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new API.Helpers.IntJsonConverter());
    });

// Configurer la connexion à la base de données Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajouter les services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAgenceService, AgenceService>();
builder.Services.AddScoped<IFournitureService, FournitureService>();
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<AuthService>();

// Configurer CORS - Configuration simplifiée pour permettre toutes les origines
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// IMPORTANT: Placez UseCors avant les autres middlewares
app.UseCors("AllowAll");

// Middleware pour traiter les requêtes JSON
app.UseMiddleware<JsonRequestMiddleware>();

// Configurer le pipeline de requêtes HTTP
if (app.Environment.IsDevelopment())
{
    // Supprimer Swagger ici
    // app.UseSwagger();
    // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Fournitures v1"));
}

// Commentez cette ligne si vous rencontrez des problèmes avec HTTPS
// app.UseHttpsRedirection();

app.UseRouting();    // Placez ceci après UseCors()
app.UseAuthorization();
app.MapControllers();

// Définir le port d'écoute sur 5000
app.Urls.Add("http://localhost:5000");

app.Run();
