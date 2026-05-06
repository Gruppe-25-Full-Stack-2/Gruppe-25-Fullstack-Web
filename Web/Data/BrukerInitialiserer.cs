using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TSD2491Gruppe25.Web.Data;

/// <summary>
/// Legger til en bruker i databasen, slik at en bruker er tilgjengelig fra første start. Innloggingsinformasjon dokumenteres i README.md
/// </summary>
public static class BrukerInitialiserer
{
    /// <summary>
    /// Standard innloggingsnavn
    /// </summary>
    public const string DefaultEmail = "admin@usn.no";
    /// <summary>
    /// Passord for standardbruker
    /// </summary>
    public const string DefaultPassword = "Admin123!";

    /// <summary>
    /// Kjører og oppretter standardbruker hvis den ikke finnes fra før.
    /// Metoden kan kjøres ved hver oppstart.
    /// </summary>
    /// <param name="services">Henter brukere fra database</param>
    /// <returns>Fullfører når migrasjon er ferdig og eventuelt standardbruker er lagt til.</returns>
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
