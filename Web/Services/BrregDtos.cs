using System.Text.Json.Serialization;

namespace TSD2491Gruppe25.Web.Services;

/// <summary>
/// Respresenterer hele responsen fra Brønnøysundregisterene sitt API.
/// Inneholder en liste over enhetene hentet fra API-et. 
/// </summary>
public class BrregSearchResponseDto
{
    /// <summary>
    /// Den innebygde delen av API-responsen som inneholder liste over enheter.
    /// </summary>
    [JsonPropertyName("_embedded")]
    public BrregEmbeddedDto? Embedded { get; set; }
}

/// <summary>
/// Representerer "embedded" delen til API-responsen.
/// Inneholder listen over enheter som er hentet fra Brreg.
/// </summary>
public class BrregEmbeddedDto
{
    /// <summary>
    /// Liste over enheter/virksomheter hentet fra Enhetsregisteret.
    /// </summary>
    [JsonPropertyName("enheter")]
    public List<BrregEnhetDto> Enheter { get; set; } = new();
}

/// <summary>
/// Representerer en enhet fra Brreg.
/// Inneholder grunnleggende informasjon om organisasjonen.
/// </summary>
public class BrregEnhetDto
{
    /// <summary>
    /// Organisasjonsnummeret til virksomheter (9 siffer).
    /// </summary>
    [JsonPropertyName("organisasjonsnummer")]
    public string? Organisasjonsnummer { get; set; }

    /// <summary>
    /// Navnet på virksomheten slik det er registrert i Enhetsregisteret.
    /// </summary>
    [JsonPropertyName("navn")]
    public string? Navn { get; set; }

    /// <summary>
    /// Informasjon om organisasjonsform, inkludert kode og beskrivelse.
    /// </summary>
    [JsonPropertyName("organisasjonsform")]
    public BrregKodeBeskrivelseDto? Organisasjonsform { get; set; }

    /// <summary>
    /// Dato virksomheten ble registrer i Enhetsregisteret.
    /// </summary>
    [JsonPropertyName("registreringsdatoEnhetsregisteret")]
    public DateTime? RegistreringsdatoEnhetsregisteret { get; set; }
}

/// <summary>
/// Respresenterer organiasjonsform.
/// Brukes for å beskrive type virksomhet.
/// </summary>
public class BrregKodeBeskrivelseDto
{
    /// <summary>
    /// Koden som identifiserer organiasjonsformen (f.eks. AS).
    /// </summary>
    [JsonPropertyName("kode")]
    public string? Kode { get; set; }

    /// <summary>
    /// Tekstlig beskrivelse av organisasjonsformen.
    /// </summary>
    [JsonPropertyName("beskrivelse")]
    public string? Beskrivelse { get; set; }
}
