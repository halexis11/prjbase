using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjbase.Models;

namespace prjbase.Controllers
{
    public class MunicipiosController : Controller
    {
        private readonly prjbaseContext _context;

        public MunicipiosController(prjbaseContext context)
        {
            _context = context;
        }

        // GET: Municipios
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.Municipio.Include(m => m.Departamento);
            return View(await prjbaseContext.ToListAsync());
        }

        // GET: Municipios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipio
                .Include(m => m.Departamento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipio == null)
            {
                return NotFound();
            }

            return View(municipio);
        }

        // GET: Municipios/Create
        public IActionResult Create()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "Id", "Codigo");
            return View();
        }

        // POST: Municipios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nombre,FechaCreacion,DepartamentoId")] Municipio municipio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(municipio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "Id", "Codigo", municipio.DepartamentoId);
            return View(municipio);
        }

        // GET: Municipios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipio.FindAsync(id);
            if (municipio == null)
            {
                return NotFound();
            }
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "Id", "Codigo", municipio.DepartamentoId);
            return View(municipio);
        }

        // POST: Municipios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nombre,FechaCreacion,DepartamentoId")] Municipio municipio)
        {
            if (id != municipio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(municipio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MunicipioExists(municipio.Id))
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
            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "Id", "Codigo", municipio.DepartamentoId);
            return View(municipio);
        }

        // GET: Municipios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipio = await _context.Municipio
                .Include(m => m.Departamento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipio == null)
            {
                return NotFound();
            }

            return View(municipio);
        }

        // POST: Municipios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var municipio = await _context.Municipio.FindAsync(id);
            _context.Municipio.Remove(municipio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MunicipioExists(int id)
        {
            return _context.Municipio.Any(e => e.Id == id);
        }
        public JsonResult GetMunicipios(int id)
        {
            List<Municipio> list = new List<Municipio>();
            list = _context.Municipio.Where(a => a.DepartamentoId == id).ToList();
            return Json(new SelectList(list, "Id", "Nombre"));
        }
    }
}
