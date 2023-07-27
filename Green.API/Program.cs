using Green.API.Middleware;
using Green.Application;
using Green.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Green.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplications();

            //it could be potentially gets from Azure Key Vault
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.SetRepositories(connectionString);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandling>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}