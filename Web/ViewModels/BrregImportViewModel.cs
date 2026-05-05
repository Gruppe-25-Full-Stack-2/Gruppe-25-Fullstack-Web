using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TSD2491Gruppe25.Web.ViewModels;

public class BrregImportViewModel
{
    [Required(ErrorMessage = "Søketekst påkrevd.")]
    [Display(Name = "Søk etter bedrift")]
    public string? Soketekst { get; set; }

    [Required(ErrorMessage = "Kategori påkrevd.")]
    [Display(Name = "Kategori")]
    public int KategoriId { get; set; }

    public List<SelectListItem> Kategorier { get; set; } = new();
}
