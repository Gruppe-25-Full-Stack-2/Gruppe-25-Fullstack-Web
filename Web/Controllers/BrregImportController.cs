using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Services;
using TSD2491Gruppe25.Web.ViewModels;

namespace TSD2491Gruppe25.Web.Controllers;
/// <summary>
/// Tillater søk opp mot Brønnøysundregisteret ved bruk av API og legger bedriftene som blir funnet til i databasen.
/// Denne Controlleren håndterer input-validering og gir tilbakemelding, også ved feil i søket.
/// </summary>
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

    /// <summary>
    /// Viser et søkeskjema. Det søkes i bedriftnavn og det tas ikke hensyn til forskjell i store/små bokstaver
    /// </summary>
    /// <returns>Returnerer et view</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vm = new BrregImportViewModel
        {
            Kategorier = await HentKategorierAsync()
        };

        return View(vm);
    }
    /// <summary>
    /// Utfører API-søk mot Brreg. Det sjekkes for at kategori finnes og det gis tilbakemelding på om det er ingen treff.
    /// Det er satt en begrensning til maks 20 treff.
    /// </summary>
    /// <param name="vm">vm er kategorien det skal knyttes opp mot og søkeordet det skal søkes etter i Brreg</param>
    /// <returns>Ved vellyket kjøring returneres antall bedrifter som ble lagt til. Det returneres feil i view hvis kategori ikke finnes, eller brreg ikke er tilgjengelig</returns>
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
