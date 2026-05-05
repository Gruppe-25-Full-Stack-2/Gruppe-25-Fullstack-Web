using System.Text.Json.Serialization;

namespace TSD2491Gruppe25.Web.Services;

public class BrregSearchResponseDto
{
    [JsonPropertyName("_embedded")]
    public BrregEmbeddedDto? Embedded { get; set; }
}

public class BrregEmbeddedDto
{
    [JsonPropertyName("enheter")]
    public List<BrregEnhetDto> Enheter { get; set; } = new();
}

public class BrregEnhetDto
{
    [JsonPropertyName("organisasjonsnummer")]
    public string? Organisasjonsnummer { get; set; }

    [JsonPropertyName("navn")]
    public string? Navn { get; set; }

    [JsonPropertyName("organisasjonsform")]
    public BrregKodeBeskrivelseDto? Organisasjonsform { get; set; }

    [JsonPropertyName("registreringsdatoEnhetsregisteret")]
    public DateTime? RegistreringsdatoEnhetsregisteret { get; set; }
}

public class BrregKodeBeskrivelseDto
{
    [JsonPropertyName("kode")]
    public string? Kode { get; set; }

    [JsonPropertyName("beskrivelse")]
    public string? Beskrivelse { get; set; }
}
