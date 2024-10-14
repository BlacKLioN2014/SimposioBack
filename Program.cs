using SimposioBack.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.OpenApi.Models;
using SimposioBack.Repository.IRepository;
using SimposioBack.Repository;


var builder = WebApplication.CreateBuilder(args);


//CORS
builder.Services.AddCors(p => p.AddPolicy("politicaCors", build =>
{
    build.WithOrigins("http://localhost:5046", "https://localhost:7259", "http://172.16.101.20:81", "http://localhost:81")
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials(); // No uses AllowAnyOrigin()
}));


//Cache
builder.Services.AddResponseCaching();


// Servicio SQL
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
                  opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));


//Respetar mayusculas al inicio de variables en respuesta.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Para respetar mayúsculas
    });


// Servicio Mapper
builder.Services.AddAutoMapper(typeof(Mapper));


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{

    c.EnableAnnotations(); // Asegúrate de habilitar anotaciones

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Api Simposio",
        Description = "Primera version",
        //TermsOfService = new Uri("https://google.com.mx"),
        Contact = new OpenApiContact
        {
            Name = "Abraham Jimenez",
            Email = "abraham.jimenez@mepiel.com.mx",

        },
        License = new OpenApiLicense
        {
            Name = "Licencia Personal",
            //Url = new Uri("https://google.com.mx")
        }
    });
});


builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();

}


app.UseSwagger();


app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api V1");
});


app.UseCors("politicaCors");


app.UseHttpsRedirection();


app.UseAuthorization();


app.MapControllers();


app.Run();
