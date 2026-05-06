using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Models;

namespace TSD2491Gruppe25.Web.Data;

/// <summary>
/// Databasekontekst for applikasjonen.
/// Håndterer kommunikasjon mellom applikasjon og databasen.
/// </summary>
/// <param name="options">Innstillinger som brukes for å konfigurere databasekontesten og tilkobling til databasen.</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    /// <summary>
    /// Representerer tabellen for Bedrifter i databasen.
    /// </summary>
    public DbSet<Bedrift> Bedrifter => Set<Bedrift>();

    /// <summary>
    /// Representerer tabellen for Kategorier i databasen.
    /// </summary>
    public DbSet<Kategori> Kategorier => Set<Kategori>();

    /// <summary>
    /// Konfigurerer database-modellen.
    /// Det settes unike verdier for organisasjonsnummer for å unngå duplikater.
    /// </summary>
    /// <param name="builder">Modelbuilder som brukes til å konfigurere EF-modellen.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Bedrift>()
            .HasIndex(b => b.Organisasjonsnummer)
            .IsUnique();
    }
}
