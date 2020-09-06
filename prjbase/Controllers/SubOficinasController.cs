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
    [DisplayName("Administración de Sub Oficinas")]
    public class SubOficinasController : Controller
    {
        private readonly prjbaseContext _context;

        public SubOficinasController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: SubOficinas
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.SubOficina;
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: SubOficinas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subOficina = await _context.SubOficina
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subOficina == null)
            {
                return NotFound();
            }

            return View(subOficina);
        }
        [Authorize]
        // GET: SubOficinas/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        // POST: SubOficinas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Encargado,TelefonoEncargado,Estado")] SubOficina subOficina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subOficina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subOficina);
        }
        [Authorize]
        // GET: SubOficinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subOficina = await _context.SubOficina.FindAsync(id);
            if (subOficina == null)
            {
                return NotFound();
            }
           
            return View(subOficina);
        }
        [Authorize]
        // POST: SubOficinas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Encargado,TelefonoEncargado,Estado")] SubOficina subOficina)
        {
            if (id != subOficina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subOficina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubOficinaExists(subOficina.Id))
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
            return View(subOficina);
        }
        [Authorize]
        // GET: SubOficinas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subOficina = await _context.SubOficina
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subOficina == null)
            {
                return NotFound();
            }

            return View(subOficina);
        }
        [Authorize]
        // POST: SubOficinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.SubOficina.FindAsync(id);
            if (SubOficinaExists(id))
            {
                try
                {
                    _context.SubOficina.Remove(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Sub oficina", "Ha ocurrido un error al eliminar el registro.");
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("Sub Oficina", "No se encontró el registro.");
                return View(item);
            }
        }

        private bool SubOficinaExists(int id)
        {
            return _context.SubOficina.Any(e => e.Id == id);
        }
        public JsonResult GetSubOficina(int id)
        {
            List<SubOficina> list = new List<SubOficina>();
            list = _context.SubOficina.Where(a => a.Id == id).ToList();
            return Json(new SelectList(list, "Id", "Nombre"));
        }
    }
}
