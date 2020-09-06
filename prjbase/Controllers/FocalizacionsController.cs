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
    [DisplayName("Administración de Focalización")]
    public class FocalizacionsController : Controller
    {
        private readonly prjbaseContext _context;

        public FocalizacionsController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Focalizacions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Focalizacion.ToListAsync());
        }
        [Authorize]
        // GET: Focalizacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var focalizacion = await _context.Focalizacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (focalizacion == null)
            {
                return NotFound();
            }

            return View(focalizacion);
        }
        [Authorize]
        // GET: Focalizacions/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        // POST: Focalizacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Estado")] Focalizacion focalizacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(focalizacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(focalizacion);
        }
        [Authorize]
        // GET: Focalizacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var focalizacion = await _context.Focalizacion.FindAsync(id);
            if (focalizacion == null)
            {
                return NotFound();
            }
            return View(focalizacion);
        }
        [Authorize]
        // POST: Focalizacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Estado")] Focalizacion focalizacion)
        {
            if (id != focalizacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(focalizacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FocalizacionExists(focalizacion.Id))
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
            return View(focalizacion);
        }
        [Authorize]
        // GET: Focalizacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var focalizacion = await _context.Focalizacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (focalizacion == null)
            {
                return NotFound();
            }

            return View(focalizacion);
        }
        [Authorize]
        // POST: Focalizacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Focalizacion.FindAsync(id);
            if (FocalizacionExists(id))
            {
                try
                {
                    _context.Focalizacion.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Focalización", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Focalización", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool FocalizacionExists(int id)
        {
            return _context.Focalizacion.Any(e => e.Id == id);
        }
    }
}
