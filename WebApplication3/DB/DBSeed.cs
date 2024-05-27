using DB;

namespace WebApplication3.DB;

public static class DBSeedEx
{
    public static void SeedDb(this WebApplication webApplication)//method to seed Db when called
    {
        using (var scope = webApplication.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            SeedUser(dbContext);//add initial user data to the database.
            //SeedRequest(dbContext);
        }
    }
    //seeding db with initial data
    private static void SeedUser(AppDbContext dbContext)
    {
        if (!dbContext.AAA_USR_User.Any(x => x.AAA_USR_Name == "admin"))
        {
            var adminUser = new AAA_USR_User
            {
                AAA_USR_Name = "admin",
                AAA_USR_Email = "admin@gmail.com",
                AAA_USR_Password = "admin123",
            };
            dbContext.AAA_USR_User.Add(adminUser);
            dbContext.SaveChanges();
        }
    }

}