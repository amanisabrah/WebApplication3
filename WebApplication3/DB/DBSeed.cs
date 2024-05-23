using DB;

namespace WebApplication3.DB;

public static class DBSeedEx
{
    public static void SeedDb(this WebApplication webApplication)
    {
        using (var scope = webApplication.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            SeedUser(dbContext);
            //SeedRequest(dbContext);
        }
    }

    private static void SeedUser(AppDbContext dbContext)
    {
        if (!dbContext.USE_User.Any(x => x.USE_User_Name == "admin"))
        {
            var adminUser = new USE_User
            {
                USE_User_Name = "admin",
                USE_User_Email = "admin@gmail.com",
                USE_User_Password = "admin123",
            };
            dbContext.USE_User.Add(adminUser);
            dbContext.SaveChanges();
        }
    }
}