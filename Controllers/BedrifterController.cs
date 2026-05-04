using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TSD2491Gruppe25.Web.Data;
using TSD2491Gruppe25.Web.Models;

namespace TSD2491Gruppe25.Web.Controllers
{
    public class BedrifterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BedrifterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bedrifter
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bedrifter.Include(b => b.Kategori);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bedrifter/Details/5
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

        // GET: Bedrifter/Create
        public IActionResult Create()
        {
            ViewData["KategoriId"] = new SelectList(_context.Kategorier, "Id", "KategoriNavn");
            return View();
        }

        // POST: Bedrifter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Organisasjonsnummer,Navn,Organisasjonsform,ErAktiv,Registreringsdato,Notat,KategoriId")] Bedrift bedrift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bedrift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorier, "Id", "KategoriNavn", bedrift.KategoriId);
            return View(bedrift);
        }

        // GET: Bedrifter/Edit/5
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

        // POST: Bedrifter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Organisasjonsnummer,Navn,Organisasjonsform,ErAktiv,Registreringsdato,Notat,KategoriId")] Bedrift bedrift)
        {
            if (id != bedrift.Id)
            {
                return NotFound();
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
            ViewData["KategoriId"] = new SelectList(_context.Kategorier, "Id", "KategoriNavn", bedrift.KategoriId);
            return View(bedrift);
        }

        // GET: Bedrifter/Delete/5
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

        // POST: Bedrifter/Delete/5
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
