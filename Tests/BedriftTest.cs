using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Controllers;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace TSD2491Gruppe25.Web.Tests;

public class BedriftTest
{
    private static ApplicationDbContext GetDb()
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

    private static Bedrift SeedBedrift(
      ApplicationDbContext db,
      int kategoriId,
      string orgnr = "123456789",
      string navn = "TestBedrift")
    {
        var bedrift = new Bedrift
        {
            Organisasjonsnummer = orgnr,
            Navn = navn,
            KategoriId = kategoriId
        };
        db.Bedrifter.Add(bedrift);
        db.SaveChanges();
        return bedrift;
    }


    [Fact]
    public async Task CreateBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);
        var kategori = SeedKategori(db);

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

        var kategori = SeedKategori(db);
        var bedrift = SeedBedrift(db, kategori.Id);

        bedrift.Navn = "OppdatertBedrift";

        await controller.Edit(bedrift.Id, bedrift);

        Assert.Equal("OppdatertBedrift", db.Bedrifter.First().Navn);
    }

    [Fact]
    public async Task DetailsBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori = SeedKategori(db);
        var bedrift = SeedBedrift(db, kategori.Id);

        var result = await controller.Details(bedrift.Id) as ViewResult;

        Assert.NotNull(result);
        Assert.IsType<Bedrift>(result.Model);
    }

    [Fact]
    public async Task DeleteBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori = SeedKategori(db);
        var bedrift = SeedBedrift(db, kategori.Id);

        await controller.DeleteConfirmed(bedrift.Id);

        Assert.Equal(0, db.Bedrifter.Count());
    }

    [Fact]
    public async Task SearchBedriftTest()
    {
        var db = GetDb();
        var controller = new BedrifterController(db);

        var kategori1 = SeedKategori(db, "TestKategori1");
        var kategori2 = SeedKategori(db, "TestKategori2");

        SeedBedrift(db, kategori1.Id, "111111111", "TestBedrift1");
        SeedBedrift(db, kategori2.Id, "222222222", "TestBedrift2");


        var result = await controller.Index(kategori1.Id) as ViewResult;
        var model = result?.Model as IEnumerable<Bedrift>;

        Assert.NotNull(model);
        Assert.Single(model);
        Assert.Equal("TestBedrift1", model.First().Navn);
    }
}
