using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace TSD2491Gruppe25.Web.Controllers
{
    /// <summary>
    /// Controller som håndterer CRUD-operasjoner for Kategori.
    /// Inkluderer opprettelse, redigering, detaljer og sletting av kategorier.
    /// </summary>
    public class KategorierController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initialiserer KategoriController med databasekontekst.
        /// </summary>
        /// <param name="context">ApplicationDbContext brukes for databaseoperasjoner.</param>
        public KategorierController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Henter alle kategorier fra databasen, inkludert relaterte bedrifter.
        /// [AllowAnonymous] gjør at denne handlingen kan brukes uten at brukeren er innlogget.
        /// </summary>
        /// <returns>Liste over kategorier</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var kategorier = await _context.Kategorier.Include(k => k.Bedrifter).ToListAsync();
            return View(kategorier);
        }

        /// <summary>
        /// Henter detaljer for en kategori basert på ID.
        /// Inkluderer relaterte bedrifter.
        /// </summary>
        /// <param name="id">ID til kategorien</param>
        /// <returns>Returnerer View med kategori dersom det finnes, ellers returneres NotFound.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategorier
                .Include(k => k.Bedrifter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        /// <summary>
        /// Viser skjema for opprettelse av ny kategori.
        /// </summary>
        /// <returns>Creste-view med tomt skjema.</returns>
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Oppretter ny kategori i databasen.
        /// </summary>
        /// <param name="kategori">Kategoriobjekt som inneholder data fra skjemaet (Id, Kategorinavn).</param>
        /// <returns>Redirecter til Index ved vellykket lagring, ellers returnes til Create-viewet.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KategoriNavn")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategori);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        /// <summary>
        /// Viser redigeringsskjemaet for en kategori. 
        /// Brukeren må være innlogget for å kunne gjennomføre operasjonen.
        /// </summary>
        /// <param name="id">ID til kategorien som skal redigeres.</param>
        /// <returns>Returnerer Edit-view med kategori-data dersom den finnes, ellers returneres NotFound.</returns>
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategorier.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }
            return View(kategori);
        }

        /// <summary>
        /// Oppdaterer eksisterende kategori i databasen basert på innsendte verdier. 
        ///  Brukeren må være innlogget for å kunne gjennomføre operasjonen.
        /// </summary>
        /// <param name="id">ID til kategorien som skal oppdateres.</param>
        /// <param name="kategori">Kategori-objektet med oppdaterte verdier.</param>
        /// <returns>
        /// Redirecter til Index ved vellykket oppdatering.
        /// Returnerer til view dersom vaildering feiler.
        /// Returnerer NotFound dersom ID-en ikke stemmer.
        /// </returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KategoriNavn")] Kategori kategori)
        {
            if (id != kategori.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriExists(kategori.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        /// <summary>
        /// Viser en beskreftelsesside før sletting av en kategori.
        /// Brukeren må være innlogget for å kunne gjennomføre operasjonen.
        /// </summary>
        /// <param name="id">ID til kategorien som skal slettes.</param>
        /// <returns>Returnerer Delete-View med kategori-data dersom den finnes, eller returneres NotFound.</returns>
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategorier
                .Include(k => k.Bedrifter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        /// <summary>
        /// Sletter kategorien fra databasen basert på ID. 
        /// Brukeren må være innlogget for å kunne gjennomføre operasjonen.
        /// </summary>
        /// <param name="id">ID til kategorien som skal slettes.</param>
        /// <returns>Redirecter til Index etter vellykket sletting.</returns>
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategori = await _context.Kategorier.FindAsync(id);
            if (kategori != null)
            {
                _context.Kategorier.Remove(kategori);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Sjekker om en kategori finnes i databsen basert på ID.
        /// </summary>
        /// <param name="id">ID til kategorien som sakl sjekkes.</param>
        /// <returns>True dersom kategorien finnes i databasen, ellers false.</returns>
        private bool KategoriExists(int id)
        {
            return _context.Kategorier.Any(e => e.Id == id);
        }
    }
}
