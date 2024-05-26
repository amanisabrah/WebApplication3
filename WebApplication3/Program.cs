using DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DB;

namespace WebApplication3;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();//Add Services to the Container
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();      
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer("Data Source=localhost;Initial Catalog=verb2;User ID=sa;Password=Qwertyuiop@123;Trust Server Certificate=True;");
        });//transitive service       
        var app = builder.Build();
        app.SeedDb();
        //Configure the HTTP Request Pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/", () => "Hello World!");
        app.MapGet("/Test", () => "Hello Test World!");
        app.Run();
    }
}
