using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
    [DisplayName("Administración de Programación/Productos")]
    public class ProgramacionProductoDetallesController : Controller
    {
        private readonly prjbaseContext _context;

        public ProgramacionProductoDetallesController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: ProgramacionProductoDetalles
        public async Task<IActionResult> Index(int IdProgramacion)
        {
            var prjbaseContext = _context.ProgramacionProductoDetalle
                .Include(p => p.IdCentroEducativoNavigation)
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdProgramacionNavigation).Where(p=>p.IdProgramacion == IdProgramacion);
            ViewBag.idprogramacion = IdProgramacion;
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: ProgramacionProductoDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacionProductoDetalle = await _context.ProgramacionProductoDetalle
                .Include(p => p.IdCentroEducativoNavigation)
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdProgramacionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programacionProductoDetalle == null)
            {
                return NotFound();
            }

            return View(programacionProductoDetalle);
        }
        [Authorize]
        // GET: ProgramacionProductoDetalles/Create
        public IActionResult Create()
        {
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Id");
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre");
            ViewData["IdProgramacion"] = new SelectList(_context.Programacion, "Id", "Estado");
            return View();
        }
        [Authorize]
        // POST: ProgramacionProductoDetalles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdProgramacion,IdCentroEducativo,IdProducto,Cantidad")] ProgramacionProductoDetalle programacionProductoDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programacionProductoDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Id", programacionProductoDetalle.IdCentroEducativo);
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", programacionProductoDetalle.IdProducto);
            ViewData["IdProgramacion"] = new SelectList(_context.Programacion, "Id", "Estado", programacionProductoDetalle.IdProgramacion);
            return View(programacionProductoDetalle);
        }
        [Authorize]
        // GET: ProgramacionProductoDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacionProductoDetalle = await _context.ProgramacionProductoDetalle.FindAsync(id);
            if (programacionProductoDetalle == null)
            {
                return NotFound();
            }
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Id", programacionProductoDetalle.IdCentroEducativo);
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", programacionProductoDetalle.IdProducto);
            ViewData["IdProgramacion"] = new SelectList(_context.Programacion, "Id", "Estado", programacionProductoDetalle.IdProgramacion);
            return View(programacionProductoDetalle);
        }
        [Authorize]
        // POST: ProgramacionProductoDetalles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdProgramacion,IdCentroEducativo,IdProducto,Cantidad")] ProgramacionProductoDetalle programacionProductoDetalle)
        {
            if (id != programacionProductoDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programacionProductoDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramacionProductoDetalleExists(programacionProductoDetalle.Id))
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
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Id", programacionProductoDetalle.IdCentroEducativo);
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", programacionProductoDetalle.IdProducto);
            ViewData["IdProgramacion"] = new SelectList(_context.Programacion, "Id", "Estado", programacionProductoDetalle.IdProgramacion);
            return View(programacionProductoDetalle);
        }
        [Authorize]
        // GET: ProgramacionProductoDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacionProductoDetalle = await _context.ProgramacionProductoDetalle
                .Include(p => p.IdCentroEducativoNavigation)
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdProgramacionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programacionProductoDetalle == null)
            {
                return NotFound();
            }

            return View(programacionProductoDetalle);
        }
        [Authorize]
        // POST: ProgramacionProductoDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programacionProductoDetalle = await _context.ProgramacionProductoDetalle.FindAsync(id);
            _context.ProgramacionProductoDetalle.Remove(programacionProductoDetalle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramacionProductoDetalleExists(int id)
        {
            return _context.ProgramacionProductoDetalle.Any(e => e.Id == id);
        }

        public object Actualizar(int IdProgramacion)
        {
            try
            {
                var IdPrograParam = new SqlParameter("@IdProgramacion", IdProgramacion);
                _context.Database.ExecuteSqlCommand("exec  ActualizarProgramacion @IdProgramacion", IdPrograParam);
            }
            catch (Exception e)
            {
                return new { data = false };
            }
            return new { data = true };
        }
        }
}
