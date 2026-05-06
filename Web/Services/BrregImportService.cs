using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Models;

namespace TSD2491Gruppe25.Web.Services;
/// <summary>
/// Henter bedrifter fra Brønnøysundregisterets åpne API og lagrer resultatet i databasen, se <see href="https://data.brreg.no/enhetsregisteret/api/docs/index.html"/>.
/// For å forhindre at bedrifter blir lagt til flere ganger, sjekkes det om bedriften finnes og så oppdateres heller bedriften hvis organisasjonsnummerer allerede eksisterer.
/// Hvis det skulle komme resultat uten organisasjonsnummer eller navn, blir disse ignorert.
/// API-URL settes i Program.cs.
/// </summary>
public class BrregImportService
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _context;

    public BrregImportService(HttpClient httpClient, ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    /// <summary>
    /// Søker mot Brreg på bedriftnavn og lagrer nye treff i databasen, mens eksisterende bedrifter oppdateres.
    /// </summary>
    /// <param name="soketekst">Bedriftsnavnet det søkes på</param>
    /// <param name="kategoriId">Kategorien bedriften skal legges i</param>
    /// <param name="size">Maks antall treff. Satt til 20.</param>
    /// <returns>Returnerer antall bedrifter som ble lagt til og oppdatert.</returns>
    public async Task<BrregImportResultat> ImporterBedrifterAsync(string soketekst, int kategoriId, int size = 20)
    {
        var url = $"api/enheter?navn={Uri.EscapeDataString(soketekst)}&size={size}";

        using var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        await using var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync<BrregSearchResponseDto>(stream, options);
        var enheter = dto?.Embedded?.Enheter ?? new List<BrregEnhetDto>();

        var antallNye = 0;
        var antallOppdatert = 0;

        foreach (var enhet in enheter)
        {
            if (string.IsNullOrWhiteSpace(enhet.Organisasjonsnummer) || string.IsNullOrWhiteSpace(enhet.Navn))
            {
                continue;
            }

            var eksisterende = await _context.Bedrifter.FirstOrDefaultAsync(b => b.Organisasjonsnummer == enhet.Organisasjonsnummer);

            if (eksisterende is null)
            {
                var nyBedrift = new Bedrift
                {
                    Organisasjonsnummer = enhet.Organisasjonsnummer,
                    Navn = enhet.Navn,
                    Organisasjonsform = enhet.Organisasjonsform?.Beskrivelse,
                    ErAktiv = true,
                    Registreringsdato = enhet.RegistreringsdatoEnhetsregisteret,
                    Notat = $"Importert fra Brreg {DateTime.Now:dd.MM.yyyy}",

                    KategoriId = kategoriId
                };

                _context.Bedrifter.Add(nyBedrift);
                antallNye++;
            }
            else
            {
                eksisterende.Navn = enhet.Navn;
                eksisterende.Organisasjonsform = enhet.Organisasjonsform?.Beskrivelse;
                eksisterende.Registreringsdato = enhet.RegistreringsdatoEnhetsregisteret;
                eksisterende.KategoriId = kategoriId;
                eksisterende.Notat = $"Sist oppdatert fra Brreg {DateTime.Now:dd.MM.yyyy}";
                antallOppdatert++;
            }
        }

        await _context.SaveChangesAsync();
        return new BrregImportResultat(enheter.Count, antallNye, antallOppdatert);
    }
}

/// <summary>
/// Resultatet fra importeringen av bedrifter fra Brreg.
/// </summary>
/// <param name="AntallTreff">Totalt antall bedrifter fra Brreg</param>
/// <param name="AntallNye">Antall nye bedrifter lagt til i databasen</param>
/// <param name="AntallOppdatert">Antall oppdaterte bedrifter i databasen</param>
public record BrregImportResultat(int AntallTreff, int AntallNye, int AntallOppdatert);
