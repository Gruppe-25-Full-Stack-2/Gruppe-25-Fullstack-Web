using System.ComponentModel.DataAnnotations;

namespace TSD2491Gruppe25.Web.Models;

/// <summary>
/// Representerer en bedrift. En bedrift må tilhøre en kategori.
/// Hver bedrift har et Organisasjonsnummer som er unikt for bedriften.
/// </summary>
public class Bedrift
{
    /// <summary>
    /// Primærnøkkelen i databasen
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Organisasjonsnummer til en norsk bedrift.
    /// Denne må være unik.
    /// </summary>
    [Required]
    [StringLength(9, MinimumLength = 9)]
    public string Organisasjonsnummer { get; set; } = string.Empty;

    /// <summary>
    /// Bedriftens navn
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Navn { get; set; } = string.Empty;

    /// <summary>
    /// En beskrivelse av organisasjonsform.
    /// </summary>
    [StringLength(100)]
    public string? Organisasjonsform { get; set; }

    /// <summary>
    /// Mulighet for å sette et aktiv/inaktiv flagg. Per nå får alle importerte bedrifter denne satt til true ved importering.
    /// </summary>
    [Display(Name = "Aktiv")]
    public bool ErAktiv { get; set; }

    /// <summary>
    /// Dato for når bedriften ble registrert.
    /// </summary>
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
    public DateTime? Registreringsdato { get; set; }

    /// <summary>
    /// Mulighet for egendefinert notat om bedriften.
    /// </summary>
    [StringLength(500)]
    public string? Notat { get; set; }

    /// <summary>
    /// Fremmednøkkel til Kategori.
    /// Alle bedrifter må tilhøre en kategori.
    /// </summary>
    [Required]
    public int KategoriId { get; set; }

    /// <summary>
    /// Tilhører navigasjon, denne tillater Include (b => b.Kategori) i controller
    /// </summary>
    public Kategori? Kategori { get; set; }
}
