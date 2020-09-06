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
    [DisplayName("Administración de zonas")]
    public class ZonasController : Controller
    {
        private readonly prjbaseContext _context;

        public ZonasController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Zonas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zona.ToListAsync());
        }
        [Authorize]
        // GET: Zonas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zona = await _context.Zona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zona == null)
            {
                return NotFound();
            }

            return View(zona);
        }
        [Authorize]
        // GET: Zonas/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        // POST: Zonas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Zona zona)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zona);
        }
        [Authorize]
        // GET: Zonas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zona = await _context.Zona.FindAsync(id);
            if (zona == null)
            {
                return NotFound();
            }
            return View(zona);
        }
        [Authorize]
        // POST: Zonas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Zona zona)
        {
            if (id != zona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZonaExists(zona.Id))
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
            return View(zona);
        }
        [Authorize]
        // GET: Zonas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zona = await _context.Zona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zona == null)
            {
                return NotFound();
            }

            return View(zona);
        }
        [Authorize]
        // POST: Zonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Zona.FindAsync(id);
            if (ZonaExists(id))
            {
                try
                {
                    _context.Zona.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Zona", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Zona", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool ZonaExists(int id)
        {
            return _context.Zona.Any(e => e.Id == id);
        }
    }
}
