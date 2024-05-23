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

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();
        
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer("Data Source=localhost;Initial Catalog=verb2;User ID=sa;Password=Qwertyuiop@123;Trust Server Certificate=True;");
        });//transitive service 


        var app = builder.Build();

        app.SeedDb();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        
        app.MapGet("/", () => "Hello World!");
        app.MapGet("/Test", () => "Hello Test World!");

        app.Run();
    }
}


//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);


//        builder.Services.AddDbContext<AppDbContext>(options =>
//        {
//            options.UseSqlServer("Data Source=localhost;Initial Catalog=verb2;User ID=sa;Password=Qwertyuiop@123;Trust Server Certificate=True;");
//        });

//        var app = builder.Build();

//        using (var scope = app.Services.CreateScope())
//        {
//            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//            DBSeed.SeedDb(dbContext);
//        }

//        app.MapGet("/", () => "Hello World!");

//        app.Run();
//    }
//}
