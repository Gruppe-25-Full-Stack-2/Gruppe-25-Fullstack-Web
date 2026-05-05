using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TSD2491Gruppe25.Web.Data;

public static class BrukerInitialiserer
{
    public const string DefaultEmail = "admin@usn.no";
    public const string DefaultPassword = "Admin123!";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var context = sp.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();

        if (await userManager.FindByEmailAsync(DefaultEmail) is not null)
        {
            return;
        }

        var bruker = new IdentityUser
        {
            UserName = DefaultEmail,
            Email = DefaultEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(bruker, DefaultPassword);
    }
}
