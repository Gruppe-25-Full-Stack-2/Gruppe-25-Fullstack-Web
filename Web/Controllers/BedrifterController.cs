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
    /// Håndterer CRUD for Bedrift og tillater filtrering på Kategori
    /// </summary>
    public class BedrifterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BedrifterController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Viser liste over alle berifter.
        /// Tillater også å sortere på Kategori
        /// </summary>
        /// <param name="kategoriId">
        /// Hvis denne er satt vises kun bedrifter med denne kategoriId
        /// </param>
        /// <returns>Returnerer en liste med bedrifter</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? kategoriId)
        {
            var query = _context.Bedrifter.Include(b => b.Kategori).AsQueryable();

            if (kategoriId.HasValue)
            {
                query = query.Where(b => b.KategoriId == kategoriId.Value);
            }

            ViewBag.KategoriFilter = new SelectList(
                await _context.Kategorier.ToListAsync(), "Id", "KategoriNavn", kategoriId);

            return View(await query.ToListAsync());
        }


        /// <summary>
        /// Viser detaljer for en valgt bedrift.
        /// </summary>
        /// <param name="id">Bedriftens Id</param>
        /// <returns>Viser et View av bedrift, eller NotFound ved manglende Id</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bedrift = await _context.Bedrifter
                .Include(b => b.Kategori)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bedrift == null)
            {
                return NotFound();
            }

            return View(bedrift);
        }

        /// <summary>
        ///  Viser skjema som tillater å opprette en bedrift
        /// </summary>
        /// <returns>Returnerer et View</returns>
        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewData["KategoriId"] = new SelectList(_context.Kategorier, "Id", "KategoriNavn");
            return View();
        }

        /// <summary>
        /// Oppretter en bedrift i databasen.
        /// Feiler hvis organisasjonsnummeret finnes i databasen allerede
        /// </summary>
        /// <param name="bedrift">Bedriften som skal opprettes</param>
        /// <returns>Vellyket kjøring returneres det til index. Ved feil returneres det tilbake i et view</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Organisasjonsnummer,Navn,Organisasjonsform,ErAktiv,Registreringsdato,Notat,KategoriId")] Bedrift bedrift)
        {
            if (await _context.Bedrifter.AnyAsync(b => b.Organisasjonsnummer == bedrift.Organisasjonsnummer))
            {
                ModelState.AddModelError(nameof(bedrift.Organisasjonsnummer),
                    "Organisasjonsnummeret er allerede registrert.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(bedrift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorier, "Id", "KategoriNavn", bedrift.KategoriId);
            return View(bedrift);
        }


        /// <summary>
        /// Viser skjema for redigering av bedrift
        /// </summary>
        /// <param name="id">Bedriftens Id</param>
        /// <returns>Returnerer et view, eller feil hvis l</returns>
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bedrift = await _context.Bedrifter.FindAsync(id);
            if (bedrift == null)
            {
                return NotFound();
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorier, "Id", "KategoriNavn", bedrift.KategoriId);
            return View(bedrift);
        }

        /// <summary>
        /// Lagrer endringer for en gitt bedrift.
        /// </summary>
        /// <param name="id"> Bedriften som skal endres. Må være lik bedrift</param>
        /// <param name="bedrift">Oppdaterer den valgte bedriften</param>
        /// <returns>Returneres til Index ved vellyket endring, ved feil returneres det i et view</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
         [Bind("Id,Organisasjonsnummer,Navn,Organisasjonsform,ErAktiv,Registreringsdato,Notat,KategoriId")]
  Bedrift bedrift)
        {
            if (id != bedrift.Id)
            {
                return NotFound();
            }

            if (await _context.Bedrifter.AnyAsync(b => b.Organisasjonsnummer == bedrift.Organisasjonsnummer
        && b.Id != bedrift.Id))
            {
                ModelState.AddModelError(nameof(bedrift.Organisasjonsnummer),
                    "Organisasjonsnummeret er allerede registrert på en annen bedrift.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bedrift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedriftExists(bedrift.Id))
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
            ViewData["KategoriId"] = new SelectList(_context.Kategorier, "Id", "KategoriNavn",
        bedrift.KategoriId);
            return View(bedrift);
        }

        /// <summary>
        /// Viser skjema for bekreftelse om bedriften skal slettes
        /// </summary>
        /// <param name="id">Id tilhørende bedriften som skal slettes</param>
        /// <returns>View med bedriften</returns>
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bedrift = await _context.Bedrifter
                .Include(b => b.Kategori)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bedrift == null)
            {
                return NotFound();
            }

            return View(bedrift);
        }

        /// <summary>
        /// Sletter bedriften fra databasen.
        /// </summary>
        /// <param name="id">Id til bedriften som slettes</param>
        /// <returns>returneres til Index</returns>
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bedrift = await _context.Bedrifter.FindAsync(id);
            if (bedrift != null)
            {
                _context.Bedrifter.Remove(bedrift);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedriftExists(int id)
        {
            return _context.Bedrifter.Any(e => e.Id == id);
        }
    }
}
