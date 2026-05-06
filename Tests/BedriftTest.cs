using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Controllers;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace TSD2491Gruppe25.Web.Tests;

public class BedriftTest
{
    private ApplicationDbContext GetDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CreateBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori = new Kategori 
        { 
            KategoriNavn = "TestKategori" 
        };
        db.Kategorier.Add(kategori);
        db.SaveChanges();

        var bedrift = new Bedrift
        {
            Organisasjonsnummer = "123456789",
            Navn = "TestBedrift",
            Organisasjonsform = "AS",
            ErAktiv = true,
            Registreringsdato = DateTime.Now,
            Notat = "Testnotat",
            KategoriId = kategori.Id
        };

        await controller.Create(bedrift);

        Assert.Equal(1, db.Bedrifter.Count());
    }

    [Fact]
    public async Task EditBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori = new Kategori 
        { 
            KategoriNavn = "TestKategori" 
        };
        db.Kategorier.Add(kategori);
        db.SaveChanges();

        var bedrift = new Bedrift
        {
            Organisasjonsnummer = "123456789",
            Navn = "TestBedrift",
            KategoriId = kategori.Id
        };

        db.Bedrifter.Add(bedrift);
        db.SaveChanges();

        bedrift.Navn = "OppdatertBedrift";

        await controller.Edit(bedrift.Id, bedrift);

        Assert.Equal("OppdatertBedrift", db.Bedrifter.First().Navn);
    }

    [Fact]
    public async Task DetailsBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori = new Kategori 
        { 
            KategoriNavn = "TestKategori" 
        };
        db.Kategorier.Add(kategori);
        db.SaveChanges();

        var bedrift = new Bedrift
        {
            Organisasjonsnummer = "123456789",
            Navn = "TestBedrift",
            KategoriId = kategori.Id
        };

        db.Bedrifter.Add(bedrift);
        db.SaveChanges();

        var result = await controller.Details(bedrift.Id) as ViewResult;

        Assert.NotNull(result);
        Assert.IsType<Bedrift>(result.Model);
    }

    [Fact]
    public async Task DeleteBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori = new Kategori 
        { 
            KategoriNavn = "TestKategori" 
        };
        db.Kategorier.Add(kategori);
        db.SaveChanges();

        var bedrift = new Bedrift
        {
            Organisasjonsnummer = "123456789",
            Navn = "TestBedrift",
            KategoriId = kategori.Id
        };

        db.Bedrifter.Add(bedrift);
        db.SaveChanges();

        await controller.DeleteConfirmed(bedrift.Id);

        Assert.Equal(0, db.Bedrifter.Count());
    }

    [Fact]
    public async Task SearchBedrift()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori1 = new Kategori
        {
            KategoriNavn = "TestKategori1"
        };
        var kategori2 = new Kategori
        {
            KategoriNavn = "TestKategori2"
        };

        db.Kategorier.AddRange(kategori1, kategori2);
        db.SaveChanges();

        db.Bedrifter.AddRange(
            new Bedrift
            {
                Organisasjonsnummer = "111111111", 
                Navn = "TestBedrift1", 
                KategoriId = kategori1.Id
            },
            new Bedrift
            {
                Organisasjonsnummer = "222222222", 
                Navn = "TestBedrift2", 
                KategoriId = kategori2.Id
            }
        );
        db.SaveChanges();

        var result = await controller.Index(kategori1.Id) as ViewResult;
        var model = result?.Model as IEnumerable<Bedrift>;

        Assert.Single(model);
        Assert.Equal("TestBedrift1", model.First().Navn);
    }

    
}