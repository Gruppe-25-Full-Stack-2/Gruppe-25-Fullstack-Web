using System.ComponentModel.DataAnnotations;

namespace TSD2491Gruppe25.Web.Models;

public class Kategori
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Navn er påkrevd for kategorier.")]
    [StringLength(100, ErrorMessage = "Kategorinavnet kan maks være 100 tegn")]
    public string KategoriNavn { get; set; } = string.Empty;

    public ICollection<Bedrift> Bedrifter { get; set; } = new List<Bedrift>();
}
