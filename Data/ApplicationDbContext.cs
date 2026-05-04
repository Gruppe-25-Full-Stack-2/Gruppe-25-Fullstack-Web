using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Models;

namespace TSD2491Gruppe25.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Bedrift> Bedrifter => Set<Bedrift>();
    public DbSet<Kategori> Kategorier => Set<Kategori>();
}
