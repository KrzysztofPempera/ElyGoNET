using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Configuration;

namespace ElyApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("ElyDb");
            builder.Services.AddDbContext<NumbersContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped(hc => new HttpClient { BaseAddress = new Uri("http://localhost:8080") });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElyApi");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.MapPost("/numbers", async (HttpClient http, NumbersContext db) =>
            {
                var number = await http.GetFromJsonAsync<NumberModel>("/random");
                var newNumber = new NumberEntity() { Number = number.number };

                db.Numbers.Add(newNumber);
                await db.SaveChangesAsync();

                string? latestRecordsStr = Environment.GetEnvironmentVariable("LATEST_RECORDS");
                var latestRecords = int.TryParse(latestRecordsStr, out var value) ? value : 1;

                return await db.Numbers
                    .OrderByDescending(x => x.Id)
                    .Take(latestRecords)
                    .ToListAsync();
            }); 

            app.Run();
        }
    }
}
