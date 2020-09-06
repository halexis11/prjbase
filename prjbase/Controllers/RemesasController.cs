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
    [DisplayName("Administración de remesas")]
    public class RemesasController : Controller
    {
        private readonly prjbaseContext _context;

        public RemesasController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Remesas
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.Remesa.Include(r => r.IdZonaNavigation);
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: Remesas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remesa = await _context.Remesa
                .Include(r => r.IdZonaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (remesa == null)
            {
                return NotFound();
            }

            return View(remesa);
        }
        [Authorize]
        // GET: Remesas/Create
        public IActionResult Create()
        {
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre");
            return View();
        }
        [Authorize]
        // POST: Remesas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Ano,Dias,IdZona,Estado")] Remesa remesa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(remesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", remesa.IdZona);
            return View(remesa);
        }
        [Authorize]
        // GET: Remesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remesa = await _context.Remesa.FindAsync(id);
            if (remesa == null)
            {
                return NotFound();
            }
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", remesa.IdZona);
            return View(remesa);
        }
        [Authorize]
        // POST: Remesas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Ano,Dias,IdZona,Estado")] Remesa remesa)
        {
            if (id != remesa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(remesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RemesaExists(remesa.Id))
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
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", remesa.IdZona);
            return View(remesa);
        }
        [Authorize]
        // GET: Remesas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remesa = await _context.Remesa
                .Include(r => r.IdZonaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (remesa == null)
            {
                return NotFound();
            }

            return View(remesa);
        }
        [Authorize]
        // POST: Remesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Remesa.FindAsync(id);
            if (RemesaExists(id))
            {
                try
                {
                    _context.Remesa.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Remesa", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Remesa", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool RemesaExists(int id)
        {
            return _context.Remesa.Any(e => e.Id == id);
        }
    }
}
