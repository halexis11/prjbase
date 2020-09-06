using System;
using System.Collections.Generic;
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
    public class EmbalajesController : Controller
    {
        private readonly prjbaseContext _context;

        public EmbalajesController(prjbaseContext context)
        {
            _context = context;
        }

        // GET: Embalajes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Embalaje.ToListAsync());
        }

        // GET: Embalajes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var embalaje = await _context.Embalaje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (embalaje == null)
            {
                return NotFound();
            }

            return View(embalaje);
        }

        // GET: Embalajes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Embalajes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Embalaje embalaje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(embalaje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(embalaje);
        }

        // GET: Embalajes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var embalaje = await _context.Embalaje.FindAsync(id);
            if (embalaje == null)
            {
                return NotFound();
            }
            return View(embalaje);
        }

        // POST: Embalajes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Embalaje embalaje)
        {
            if (id != embalaje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(embalaje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmbalajeExists(embalaje.Id))
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
            return View(embalaje);
        }

        // GET: Embalajes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var embalaje = await _context.Embalaje
                .FirstOrDefaultAsync(m => m.Id == id);
            if (embalaje == null)
            {
                return NotFound();
            }

            return View(embalaje);
        }

        // POST: Embalajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Embalaje.FindAsync(id);
            if (EmbalajeExists(id))
            {
                try
                {
                    _context.Embalaje.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Embalaje", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Embalaje", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool EmbalajeExists(int id)
        {
            return _context.Embalaje.Any(e => e.Id == id);
        }
    }
}
