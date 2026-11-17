
using Flotta_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Flotta_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<FlottaContext>(option => {
                string conn = builder.Configuration.GetConnectionString("FlottaConnection")
                    ?? throw new InvalidOperationException("Hiányzik a FlottaConnection connection string.");
                // A mellékelt DbContext MariaDB 10.4.32 alapján készült.
                // Nem próbál csatlakozni az adatbázishoz már az indítás pillanatában.
                option.UseMySql(conn, new MariaDbServerVersion(new Version(10, 4, 32)));
                    });
            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
