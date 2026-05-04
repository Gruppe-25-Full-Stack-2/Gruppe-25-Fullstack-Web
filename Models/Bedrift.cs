using System.ComponentModel.DataAnnotations;

namespace TSD2491Gruppe25.Web.Models;

public class Bedrift
{
    public int Id { get; set; }

    [Required]
    [StringLength(9, MinimumLength = 9)]
    public string Organisasjonsnummer { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Navn { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Organisasjonsform { get; set; }

    public bool ErAktiv { get; set; }

    public DateTime? Registreringsdato { get; set; }

    [StringLength(500)]
    public string? Notat { get; set; }

    [Required]
    public int KategoriId { get; set; }

    public Kategori? Kategori { get; set; }
}
