using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TSD2491Gruppe25.Web.ViewModels;
/// <summary>
/// View-model for søk og import fra Brreg.
/// </summary>
public class BrregImportViewModel
{
    /// <summary>
    /// Bedriftsnavn som skal søkes på i Brreg.
    /// </summary>
    [Required(ErrorMessage = "Søketekst påkrevd.")]
    [Display(Name = "Søk etter bedrift")]
    public string? Soketekst { get; set; }

    /// <summary>
    /// KategoriId bedrifter skal knyttes mot i søket.
    /// </summary>
    [Required(ErrorMessage = "Kategori påkrevd.")]
    [Display(Name = "Kategori")]
    public int KategoriId { get; set; }

    /// <summary>
    /// Tillater oss å vise tilgjengelige Kategorier. Denne fylles inn av controlleren, slik at nedtrekksmeny kun viser tilgjenglige kategorier
    /// </summary>
    public List<SelectListItem> Kategorier { get; set; } = new();
}
