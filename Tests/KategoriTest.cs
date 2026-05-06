using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Controllers;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace TSD2491Gruppe25.Web.Tests;

public class KategoriTest
{
    private ApplicationDbContext GetDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new ApplicationDbContext(options);
    }

    private static Kategori SeedKategori(ApplicationDbContext db, string navn = "TestKategori")
    {
        var kategori = new Kategori { KategoriNavn = navn };
        db.Kategorier.Add(kategori);
        db.SaveChanges();
        return kategori;
    }

    [Fact]
    public async Task CreateKategoriTest()
    {
        var db = GetDb();
        var controller = new KategorierController(db);

        var kategori = new Kategori{ KategoriNavn = "TestKategori" };

        var resultat = await controller.Create(kategori);

        Assert.Equal(1, db.Kategorier.Count());

        var redirect = Assert.IsType<RedirectToActionResult>(resultat);
        Assert.Equal("Index", redirect.ActionName);
        
    }

    [Fact]
    public async Task EditKategoriTest()
    {
        var db = GetDb();
        var controller = new KategorierController(db);

        var kategori = SeedKategori(db);

        kategori.KategoriNavn = "OppdatertKategori";

        var result = await controller.Edit(kategori.Id, kategori);

        Assert.Equal("OppdatertKategori", db.Kategorier.First().KategoriNavn);
        
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }

    [Fact]
    public async Task DetailsKategoriTest()
    {
        var db = GetDb();
        var controller = new KategorierController(db);

        var kategori = SeedKategori(db);

        var result = await controller.Details(kategori.Id) as ViewResult;
    
        Assert.NotNull(result);
        Assert.IsType<Kategori>(result.Model);

        var model = Assert.IsType<Kategori>(result.Model);
        Assert.Equal(kategori.Id, model.Id);
    }

    [Fact]
    public async Task DeleteKategoriTest()
    {
        var db = GetDb();
        var controller = new KategorierController(db);

        var kategori = SeedKategori(db);

        var result = await controller.DeleteConfirmed(kategori.Id);

        Assert.Equal(0, db.Kategorier.Count());

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }
    
}