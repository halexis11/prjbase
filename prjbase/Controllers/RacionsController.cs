using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjbase.Models;

namespace prjbase.Controllers
{
    [Authorize]
    [DisplayName("Administración de raciones")]
    public class RacionsController : Controller
    {
        private readonly prjbaseContext _context;

        public RacionsController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Racions
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.Racion.Include(r => r.IdZonaNavigation);
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: Racions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racion = await _context.Racion
                .Include(r => r.IdZonaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racion == null)
            {
                return NotFound();
            }

            return View(racion);
        }
        [Authorize]
        // GET: Racions/Create
        public IActionResult Create()
        {
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre");
            return View();
        }
        [Authorize]
        // POST: Racions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,IdZona,Nivel,Estado")] Racion racion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", racion.IdZona);
            return View(racion);
        }
        [Authorize]
        // GET: Racions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racion = await _context.Racion.FindAsync(id);
            if (racion == null)
            {
                return NotFound();
            }
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", racion.IdZona);
            return View(racion);
        }
        [Authorize]
        // POST: Racions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,IdZona,Nivel,Estado")] Racion racion)
        {
            if (id != racion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacionExists(racion.Id))
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
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", racion.IdZona);
            return View(racion);
        }
        [Authorize]
        // GET: Racions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racion = await _context.Racion
                .Include(r => r.IdZonaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racion == null)
            {
                return NotFound();
            }

            return View(racion);
        }
        [Authorize]
        // POST: Racions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Racion.FindAsync(id);
            if (RacionExists(id))
            {
                try
                {
                    _context.Racion.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Ración", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Ración", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool RacionExists(int id)
        {
            return _context.Racion.Any(e => e.Id == id);
        }
    }
}
