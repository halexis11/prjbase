using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prjbase.Models;
using Rotativa.AspNetCore;

namespace prjbase.Controllers
{
    [Authorize]
    [System.ComponentModel.DisplayName("Administración de notas de remisión")]
    public class NotaRemisionsController : Controller
    {
        private readonly prjbaseContext _context;
        class detalleresult
        {
            public int codigo { get; set; }
            public string producto { get; set; }
            public decimal cantidad { get; set; }
        }
        public NotaRemisionsController(prjbaseContext context)
        {
            _context = context;
        }

        // GET: NotaRemisions1
        public async Task<IActionResult> Index(int IdProgramacion)
        {
            var prjbaseContext = _context.NotaRemision.Where(n=>n.IdProgramacion == IdProgramacion)
                .Include(n => n.IdCentroEducativoNavigation)
                .Include(n=>n.IdCentroEducativoNavigation.IdDepartamentoNavigation)
                .Include(n => n.IdCentroEducativoNavigation.IdBodegaNavigation)
                .Include(n => n.IdCentroEducativoNavigation.IdBodegaNavigation.IdSubOficinaNavigation)
                .Include(n => n.IdProductoNavigation)
                .Include(n=> n.IdProductoNavigation.Embalaje)
                .Include(n => n.IdProgramacionNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation.IdDepartamentoNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation.IdMunicipioNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation)
                .Include(n => n.IdProgramacionNavigation.IdRemesaNavigation)
                .Include(n => n.IdProgramacionNavigation.IdFocalizacionNavigation);
            ViewBag.idprogramacion = IdProgramacion;
            return View(await prjbaseContext.ToListAsync());
        }

        // GET: NotaRemisions1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaRemision = await _context.NotaRemision
                .Include(n => n.IdCentroEducativoNavigation)
                .Include(n => n.IdProductoNavigation)
                .Include(n => n.IdProgramacionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaRemision == null)
            {
                return NotFound();
            }

            return View(notaRemision);
        }

        // GET: NotaRemisions1/Create
        public IActionResult Create()
        {
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Nombre");
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre");
            ViewData["IdProgramacion"] = new SelectList(_context.Programacion, "Id", "Estado");
            return View();
        }

        // POST: NotaRemisions1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdProgramacion,IdCentroEducativo,IdProducto,Cantidad")] NotaRemision notaRemision)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notaRemision);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Id", notaRemision.IdCentroEducativo);
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Nombre", notaRemision.IdProducto);
            ViewData["IdProgramacion"] = new SelectList(_context.Programacion, "Id", "Estado", notaRemision.IdProgramacion);
            return View(notaRemision);
        }

        // GET: NotaRemisions1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaRemision = await _context.NotaRemision.FindAsync(id);
            if (notaRemision == null)
            {
                return NotFound();
            }
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo.Where(x=>x.Id == notaRemision.IdCentroEducativo), "Id", "Nombre", notaRemision.IdCentroEducativo);
            ViewData["IdProducto"] = new SelectList(_context.Producto.Where(x=>x.Id == notaRemision.IdProducto), "Id", "Nombre", notaRemision.IdProducto);
            ViewData["IdProgramacions"] = new SelectList(_context.Programacion.Where(x=>x.Id == notaRemision.IdProgramacion), "Id", "Fecha", notaRemision.IdProgramacion);
            ViewBag.idprogramacion = notaRemision.IdProgramacion;
            return View(notaRemision);
        }

        // POST: NotaRemisions1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdProgramacion,IdCentroEducativo,IdProducto,Cantidad")] NotaRemision notaRemision)
        {
            if (id != notaRemision.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notaRemision);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaRemisionExists(notaRemision.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { IdProgramacion = notaRemision.IdProgramacion });
            }
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo.Where(x => x.Id == notaRemision.IdCentroEducativo), "Id", "Nombre", notaRemision.IdCentroEducativo);
            ViewData["IdProducto"] = new SelectList(_context.Producto.Where(x => x.Id == notaRemision.IdProducto), "Id", "Nombre", notaRemision.IdProducto);
            ViewData["IdProgramacions"] = new SelectList(_context.Programacion.Where(x => x.Id == notaRemision.IdProgramacion), "Id", "Fecha", notaRemision.IdProgramacion);
            ViewBag.idprogramacion = notaRemision.IdProgramacion;
            return View(notaRemision);
        }

        // GET: NotaRemisions1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notaRemision = await _context.NotaRemision
                .Include(n => n.IdCentroEducativoNavigation)
                .Include(n => n.IdProductoNavigation)
                .Include(n => n.IdProgramacionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notaRemision == null)
            {
                return NotFound();
            }

            return View(notaRemision);
        }

        // POST: NotaRemisions1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.NotaRemision.FindAsync(id);
            if (NotaRemisionExists(id))
            {
                try
                {
                    _context.NotaRemision.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Nota de remisión", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Nota de remisión", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool NotaRemisionExists(int id)
        {
            return _context.NotaRemision.Any(e => e.Id == id);
        }
        public object Generar(int IdProgramacion)
        {
            try
            {
                var IdPrograParam = new SqlParameter("@IdProgramacion", IdProgramacion);
                _context.Database.ExecuteSqlCommand("exec  GenerarNR @IdProgramacion", IdPrograParam);
            }
            catch (Exception e)
            {
                return new { data = false };
            }
            return new { data = true };
        }

        public JsonResult ActualizarDetalle(int IdProgramacion, string idcentro)
        {
            try
            {
                var detalle = _context.ProgramacionProductoDetalle.Include(x=>x.IdProductoNavigation).Where(x => x.IdCentroEducativo == idcentro && x.IdProgramacion == IdProgramacion);
                var result = new List<detalleresult>();
                foreach (var item in detalle)
                {
                    result.Add(new detalleresult { codigo= item.IdProducto
                        , producto = item.IdProductoNavigation.Nombre
                        , cantidad = item.Cantidad});
                }
                JsonResult res = Json(result);
                return res;
            }
            catch (Exception e)
            {
                return Json(new { data = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> NotaRemision(int IdProgramacion)
        {
            var reg = await _context.NotaRemision
                .Include(n => n.IdCentroEducativoNavigation)
                 .Include(n => n.IdCentroEducativoNavigation.IdDepartamentoNavigation)
                .Include(n => n.IdCentroEducativoNavigation.IdMunicipioNavigation)
                .Include(n => n.IdProductoNavigation)
                 .Include(n => n.IdProductoNavigation.Embalaje)
                .Include(n => n.IdProgramacionNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation.IdDepartamentoNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation.IdMunicipioNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation).Where(n => n.IdProgramacion == IdProgramacion)
                .OrderBy(x=>x.IdCentroEducativo)
                .ToListAsync();
            return View(reg);
        }
        public async Task<IActionResult> NotaRemisionReporte(int IdProgramacion)
        {
            var reg = await _context.NotaRemision
                .Include(n => n.IdCentroEducativoNavigation)
                .Include(n => n.IdCentroEducativoNavigation.IdDistritoNavigation)
                .Include(n => n.IdCentroEducativoNavigation.IdDepartamentoNavigation)
                .Include(n => n.IdCentroEducativoNavigation.IdMunicipioNavigation)
                .Include(n => n.IdProductoNavigation)
                .Include(n=> n.IdProductoNavigation.Embalaje)
                .Include(n => n.IdProgramacionNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation.IdDepartamentoNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation.IdMunicipioNavigation)
                .Include(n => n.IdProgramacionNavigation.IdBodegaNavigation).Where(n => n.IdProgramacion == IdProgramacion)
                .OrderBy(x => x.IdCentroEducativo).ThenBy(x=>x.IdProducto)
                .ToListAsync();
            var report = new ViewAsPdf("NotaRemision")
            {
                PageMargins = { Left = 15, Bottom = 5, Right = 10, Top = 5 },
                Model = reg
            };
            return report;            
        }
    }
}
