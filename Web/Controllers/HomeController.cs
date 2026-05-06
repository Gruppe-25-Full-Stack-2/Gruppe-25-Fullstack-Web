using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TSD2491Gruppe25.Web.Models;

namespace TSD2491Gruppe25.Web.Controllers;

/// <summary>
/// Hovedcontroller for applikasjonen. 
/// Inndeholder standardsider som Index, Privacy og Error.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Viser startsiden.
    /// </summary>
    /// <returns>Index-view</returns>
    public IActionResult Index()
    {
        return View();
    }


    /// <summary>
    /// Viser feilsiden ved unntak eller feil i applikasjonen.
    /// Inneholder informasjon om request-ID for feilsøking.
    /// </summary>
    /// <returns>Error-view med ErrorViewModel</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
