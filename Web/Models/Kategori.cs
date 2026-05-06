using System.ComponentModel.DataAnnotations;

namespace TSD2491Gruppe25.Web.Models;

/// <summary>
/// Respresenterer en kategori i systemet.
/// En kategori kan inneholde flere bedrifter (1:n relasjon).
/// </summary>
public class Kategori
{
    /// <summary>
    /// Unik identifikator for kategorien.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Navnet på kategorien.
    /// Det må fylles ut et navn, og det kan maks være på 100 tegn.
    /// </summary>
    [Required(ErrorMessage = "Navn er påkrevd for kategorier.")]
    [StringLength(100, ErrorMessage = "Kategorinavnet kan maks være 100 tegn")]
    public string KategoriNavn { get; set; } = string.Empty;

    /// <summary>
    /// Liste over bedrifter som tilhører denne kategorien.
    /// </summary>
    public ICollection<Bedrift> Bedrifter { get; set; } = new List<Bedrift>();
}
