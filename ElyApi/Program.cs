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
                app.UseSwaggerUI();
            }

            app.MapGet("/numbers", async(NumbersContext db) => 
                await db.Numbers.ToListAsync()      
            ).WithName("GetNumbersHistory");

            app.MapPost("/", () => "Hello World!").WithName(""); 

            app.Run();
        }
    }
}
