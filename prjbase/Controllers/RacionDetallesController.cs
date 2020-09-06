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
    [DisplayName("Administración de detalles de ración")]
    public class RacionDetallesController : Controller
    {
        private readonly prjbaseContext _context;

        public RacionDetallesController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: RacionDetalles
        public async Task<IActionResult> Index(int id)
        {
            var prjbaseContext = _context.RacionDetalle.Where(x=>x.IdRacion == id).Include(r => r.IdProductoNavigation).Include(r => r.IdRacionNavigation);
            ViewBag.idracion = id;
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: RacionDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racionDetalle = await _context.RacionDetalle
                .Include(r => r.IdProductoNavigation)
                .Include(r => r.IdRacionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racionDetalle == null)
            {
                return NotFound();
            }

            return View(racionDetalle);
        }
        [Authorize]
        // GET: RacionDetalles/Create
        public IActionResult Create(int idRacion)
        {
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre");
            ViewData["IdRacions"] = new SelectList(_context.Racion.Where(x=>x.Id == idRacion), "Id", "Nivel");
            ViewBag.idracion = idRacion;
            return View();
        }
        [Authorize]
        // POST: RacionDetalles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRacion,IdProducto,CantidadGramos")] RacionDetalle racionDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racionDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { idRacion = racionDetalle.IdRacion });
            }
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", racionDetalle.IdProducto);
            ViewData["IdRacions"] = new SelectList(_context.Racion.Where(x=>x.Id == racionDetalle.IdRacion), "Id", "Nivel", racionDetalle.IdRacion);
            ViewBag.idracion = racionDetalle.IdRacion;
            return View(racionDetalle);
        }
        [Authorize]
        // GET: RacionDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racionDetalle = await _context.RacionDetalle.FindAsync(id);
            if (racionDetalle == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Producto.Where(x => x.Id == racionDetalle.IdProducto), "Id", "Nombre", racionDetalle.IdProducto);
            ViewData["IdRacions"] = new SelectList(_context.Racion.Where(x => x.Id == racionDetalle.IdRacion), "Id", "Nivel", racionDetalle.IdRacion);
            ViewBag.idracion = racionDetalle.IdRacion;
            return View(racionDetalle);
        }
        [Authorize]
        // POST: RacionDetalles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRacion,IdProducto,CantidadGramos")] RacionDetalle racionDetalle)
        {
            if (id != racionDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racionDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacionDetalleExists(racionDetalle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { idRacion = racionDetalle.IdRacion });
            }
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", racionDetalle.IdProducto);
            ViewData["IdRacions"] = new SelectList(_context.Racion.Where(x => x.Id == racionDetalle.IdRacion), "Id", "Nivel", racionDetalle.IdRacion);
            ViewBag.idracion = racionDetalle.IdRacion;
            return View(racionDetalle);
            
        }
        [Authorize]
        // GET: RacionDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var racionDetalle = await _context.RacionDetalle
                .Include(r => r.IdProductoNavigation)
                .Include(r => r.IdRacionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racionDetalle == null)
            {
                return NotFound();
            }

            return View(racionDetalle);
        }
        [Authorize]
        // POST: RacionDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.RacionDetalle.FindAsync(id);
            if (RacionDetalleExists(id))
            {
                try
                {
                    _context.RacionDetalle.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Detalle de ración", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Detalle de ración", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool RacionDetalleExists(int id)
        {
            return _context.RacionDetalle.Any(e => e.Id == id);
        }
    }
}
