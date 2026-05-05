using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Services;
using TSD2491Gruppe25.Web.ViewModels;

namespace TSD2491Gruppe25.Web.Controllers;

public class BrregImportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly BrregImportService _brregImportService;

    public BrregImportController(
        ApplicationDbContext context,
        BrregImportService brregImportService)
    {
        _context = context;
        _brregImportService = brregImportService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vm = new BrregImportViewModel
        {
            Kategorier = await HentKategorierAsync()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(BrregImportViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Kategorier = await HentKategorierAsync();
            return View(vm);
        }

        var kategoriFinnes = await _context.Kategorier.AnyAsync(k => k.Id == vm.KategoriId);
        if (!kategoriFinnes)
        {
            ModelState.AddModelError(nameof(vm.KategoriId), "Valgt kategori finnes ikke.");
            vm.Kategorier = await HentKategorierAsync();
            return View(vm);
        }

        try
        {
            var resultat = await _brregImportService.ImporterBedrifterAsync(vm.Soketekst!, vm.KategoriId, 20);

            if (resultat.AntallTreff == 0)
            {
                TempData["InfoMessage"] = $"Søket ga ingen treff i Brreg for «{vm.Soketekst}».";
            }
            else
            {
                TempData["SuccessMessage"] =
                    $"Brreg returnerte {resultat.AntallTreff} treff. " +
                    $"Nye lagt til: {resultat.AntallNye}. " +
                    $"Oppdatert: {resultat.AntallOppdatert}.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            TempData["ErrorMessage"] = "Klarte ikke å nå Brreg. Prøv igjen senere.";
            return RedirectToAction(nameof(Index));
        }
        catch (TaskCanceledException)
        {
            TempData["ErrorMessage"] = "Forespørselen mot Brreg tok for lang tid. Prøv igjen.";
            return RedirectToAction(nameof(Index));
        }
    }

    private async Task<List<SelectListItem>> HentKategorierAsync()
    {
        return await _context.Kategorier
            .OrderBy(k => k.KategoriNavn)
            .Select(k => new SelectListItem
            {
                Value = k.Id.ToString(),
                Text = k.KategoriNavn
            })
            .ToListAsync();
    }
}
