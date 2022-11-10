using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using store_management_api.Data.implementations;
using store_management_api.Data.Repository.implementations;
using store_management_api.Data.Repository.Interfaces;
using store_management_api.DBcontext;
using store_management_api.Helpers;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddSwaggerGen(setupAction =>
{
        setupAction.AddSecurityDefinition("StoreManagementApiBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            Description = "Ac� pegar el token generado al loguearse."
        });

        setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "StoreManagementApiBearerAuth"
                    } //Tiene que coincidir con el id seteado arriba en la definici�n
                }, new List<string>()
            }
        });
});

builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticaci�n que tenemos que elegir despu�s en PostMan para pasarle el token
    .AddJwtBearer(options => //Ac� definimos la configuraci�n de la autenticaci�n. le decimos qu� cosas queremos comprobar. La fecha de expiraci�n se valida por defecto.
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    }
);

builder.Services.AddDbContext<UsuarioContext>(dbContextOptions => dbContextOptions.UseSqlite(
    builder.Configuration["ConnectionStrings:StoreManagementAPIDBConnectionString"]));

#region Injection
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuariosRepository>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();