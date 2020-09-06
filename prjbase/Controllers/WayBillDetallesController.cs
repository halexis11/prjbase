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
    [DisplayName("Administración de detalles de WayBill")]
    public class WayBillDetallesController : Controller
    {
        private readonly prjbaseContext _context;

        public WayBillDetallesController(prjbaseContext context)
        {
            _context = context;
        }

        // GET: WayBillDetalles
        public async Task<IActionResult> Index(int Id)
        {
            var prjbaseContext = _context.WayBillDetalle.Include(w => w.IdProductoNavigation).Include(w => w.IdWayBillNavigation).Where(x => x.IdWayBill==Id);
            return View(await prjbaseContext.ToListAsync());
        }

        // GET: WayBillDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wayBillDetalle = await _context.WayBillDetalle
                .Include(w => w.IdProductoNavigation)
                .Include(w => w.IdWayBillNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wayBillDetalle == null)
            {
                return NotFound();
            }

            return View(wayBillDetalle);
        }

        // GET: WayBillDetalles/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre");
            ViewData["IdWayBill"] = new SelectList(_context.WayBill, "Id", "Codigo");
            return View();
        }

        // POST: WayBillDetalles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdWayBill,IdProducto,Cantidad")] WayBillDetalle wayBillDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wayBillDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", wayBillDetalle.IdProducto);
            ViewData["IdWayBill"] = new SelectList(_context.WayBill, "Id", "Codigo", wayBillDetalle.IdWayBill);
            return View(wayBillDetalle);
        }

        // GET: WayBillDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wayBillDetalle = await _context.WayBillDetalle.FindAsync(id);
            if (wayBillDetalle == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", wayBillDetalle.IdProducto);
            ViewData["IdWayBill"] = new SelectList(_context.WayBill, "Id", "Codigo", wayBillDetalle.IdWayBill);
            return View(wayBillDetalle);
        }

        // POST: WayBillDetalles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdWayBill,IdProducto,Cantidad")] WayBillDetalle wayBillDetalle)
        {
            if (id != wayBillDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wayBillDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WayBillDetalleExists(wayBillDetalle.Id))
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
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", wayBillDetalle.IdProducto);
            ViewData["IdWayBill"] = new SelectList(_context.WayBill, "Id", "Codigo", wayBillDetalle.IdWayBill);
            return View(wayBillDetalle);
        }

        // GET: WayBillDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wayBillDetalle = await _context.WayBillDetalle
                .Include(w => w.IdProductoNavigation)
                .Include(w => w.IdWayBillNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wayBillDetalle == null)
            {
                return NotFound();
            }

            return View(wayBillDetalle);
        }

        // POST: WayBillDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wayBillDetalle = await _context.WayBillDetalle.FindAsync(id);
            _context.WayBillDetalle.Remove(wayBillDetalle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WayBillDetalleExists(int id)
        {
            return _context.WayBillDetalle.Any(e => e.Id == id);
        }
    }
}
