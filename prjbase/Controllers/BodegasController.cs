using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjbase.Models;

namespace prjbase.Controllers
{
    [Authorize]
    [DisplayName("Administración de bodegas")]
    public class BodegasController : Controller
    {
        private readonly prjbaseContext _context;
        private UserManager<Users> _userManager;

        public BodegasController(prjbaseContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        [Authorize]
        // GET: Bodegas
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.Bodega.Include(b => b.IdDepartamentoNavigation).Include(b => b.IdDistritoNavigation).Include(b => b.IdMunicipioNavigation).Include(b => b.IdSubOficinaNavigation);
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: Bodegas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodega = await _context.Bodega
                .Include(b => b.IdDepartamentoNavigation)
                .Include(b => b.IdDistritoNavigation)
                .Include(b => b.IdMunicipioNavigation)
                .Include(b => b.IdSubOficinaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodega == null)
            {
                return NotFound();
            }

            return View(bodega);
        }
        [Authorize]
        // GET: Bodegas/Create
        public IActionResult Create(int id)
        {
            ViewData["IdSubOficina"] = new SelectList(_context.SubOficina, "Id", "Nombre");
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre");
            
            return View();
        }
        [Authorize]
        // POST: Bodegas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDepartamento,IdMunicipio,IdDistrito,IdSubOficina,Nombre,Direccion,Responsable,TelefonoResponsable,Responsable2,TelefonoResponsable2,Estado")] Bodega bodega)
        {
            bodega.Estado = true;
            if (ModelState.IsValid)
            {
                _context.Add(bodega);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", bodega.IdDepartamento);
            ViewData["IdDistrito"] = new SelectList(_context.Distrito.Where(x=>x.IdDepartamento == bodega.IdDepartamento), "Id", "Nombre", bodega.IdDistrito);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x=>x.DepartamentoId == bodega.IdDepartamento), "Id", "Nombre", bodega.IdMunicipio);
            ViewData["IdSubOficina"] = new SelectList(_context.SubOficina, "Id", "Nombre", bodega.IdSubOficina);
            return View(bodega);
        }
        [Authorize]
        // GET: Bodegas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodega = await _context.Bodega.FindAsync(id);
            if (bodega == null)
            {
                return NotFound();
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", bodega.IdDepartamento);
            ViewData["IdDistrito"] = new SelectList(_context.Distrito.Where(x=>x.IdDepartamento == bodega.IdDepartamento), "Id", "Nombre", bodega.IdDistrito);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x=>x.DepartamentoId == bodega.IdDepartamento), "Id", "Nombre", bodega.IdMunicipio);
            ViewData["IdSubOficina"] = new SelectList(_context.SubOficina, "Id", "Nombre", bodega.IdSubOficina);
            return View(bodega);
        }
        [Authorize]
        // POST: Bodegas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdDepartamento,IdMunicipio,IdDistrito,IdSubOficina,Nombre,Direccion,Responsable,TelefonoResponsable,Responsable2,TelefonoResponsable2,Estado")] Bodega bodega)
        {
            if (id != bodega.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bodega);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodegaExists(bodega.Id))
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
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", bodega.IdDepartamento);
            ViewData["IdDistrito"] = new SelectList(_context.Distrito.Where(x => x.IdDepartamento == bodega.IdDepartamento), "Id", "Nombre", bodega.IdDistrito);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == bodega.IdDepartamento), "Id", "Nombre", bodega.IdMunicipio);
            ViewData["IdSubOficina"] = new SelectList(_context.SubOficina, "Id", "Nombre", bodega.IdSubOficina);
            return View(bodega);
        }
        [Authorize]
        // GET: Bodegas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodega = await _context.Bodega
                .Include(b => b.IdDepartamentoNavigation)
                .Include(b => b.IdDistritoNavigation)
                .Include(b => b.IdMunicipioNavigation)
                .Include(b => b.IdSubOficinaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodega == null)
            {
                return NotFound();
            }

            return View(bodega);
        }
        [Authorize]
        // POST: Bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Bodega.FindAsync(id);
            if (BodegaExists(id))
            {
                try
                {
                    _context.Bodega.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Bodegas", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Bodega", "No se encontró el Centro Educativo.");
                return View(item);
            }

        }

        private bool BodegaExists(int id)
        {
            return _context.Bodega.Any(e => e.Id == id);
        }

        public JsonResult GetBodegas(int id)
        {
            List<Bodega> list = new List<Bodega>();
            list = _context.Bodega.Where(a => a.IdMunicipio == id).ToList();
            return Json(new SelectList(list, "Id", "Nombre"));
        }
        public JsonResult GetBodegaSubOficina(int id)
        {
            List<Bodega> list = new List<Bodega>();
            list = _context.Bodega.Where(a => a.IdSubOficina == id).ToList();
            return Json(new SelectList(list, "Id", "Nombre"));
        }
    }
}
