using Domain.Entities;
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

            app.MapPost("/numbers{number:int}", async (int number, NumbersContext db) =>
            {
                var newNumber = new NumberEntity() { Number = number };

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
