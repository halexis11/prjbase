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
    [DisplayName("Administración de Distritos")]
    public class DistritoesController : Controller
    {
        private readonly prjbaseContext _context;

        public DistritoesController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Distritoes
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.Distrito.Include(d => d.IdDepartamentoNavigation);
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: Distritoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distrito
                .Include(d => d.IdDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }
        [Authorize]
        // GET: Distritoes/Create
        public IActionResult Create()
        {
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre");
            return View();
        }
        [Authorize]
        // POST: Distritoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDepartamento,Nombre,Descripcion,Estado")] Distrito distrito)
        {
            if (ModelState.IsValid)
            {
                distrito.Estado = true;
                _context.Add(distrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", distrito.IdDepartamento);
            return View(distrito);
        }
        [Authorize]
        // GET: Distritoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distrito.FindAsync(id);
            if (distrito == null)
            {
                return NotFound();
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", distrito.IdDepartamento);
            return View(distrito);
        }
        [Authorize]
        // POST: Distritoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdDepartamento,Nombre,Descripcion,Estado")] Distrito distrito)
        {
            if (id != distrito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    distrito.Estado = true;
                    _context.Update(distrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistritoExists(distrito.Id))
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
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", distrito.IdDepartamento);
            return View(distrito);
        }
        [Authorize]
        // GET: Distritoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distrito
                .Include(d => d.IdDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }
        [Authorize]
        // POST: Distritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Distrito.FindAsync(id);
            if (DistritoExists(id))
            {
                try
                {
                    _context.Distrito.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Distrito", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Distrito", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool DistritoExists(int id)
        {
            return _context.Distrito.Any(e => e.Id == id);
        }
        public JsonResult GetDistritos(int id)
        {
            List<Distrito> list = new List<Distrito>();
            list = _context.Distrito.Where(a => a.IdDepartamento == id).ToList();
            return Json(new SelectList(list, "Id", "Nombre"));
        }
    }
}
