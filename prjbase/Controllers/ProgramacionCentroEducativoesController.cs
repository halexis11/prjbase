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
    [DisplayName("Administración de programación/Centro Educativo")]
    public class ProgramacionCentroEducativoesController : Controller
    {
        private readonly prjbaseContext _context;

        public ProgramacionCentroEducativoesController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: ProgramacionCentroEducativoes
        public async Task<IActionResult> Index(int id)
        {
            var prjbaseContext = _context.ProgramacionCentroEducativo.Include(p => p.IdCentroEducativoNavigation).Include(p=>p.IdCentroEducativoNavigation.IdDepartamentoNavigation).Include(p=>p.IdProgramacionNavigation.IdBodegaNavigation).Include(p => p.IdProgramacionNavigation).Where(x=>x.IdProgramacion == id);
            ViewBag.idprogramacion = id;
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: ProgramacionCentroEducativoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacionCentroEducativo = await _context.ProgramacionCentroEducativo
                .Include(p => p.IdCentroEducativoNavigation)
                .Include(p => p.IdProgramacionNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programacionCentroEducativo == null)
            {
                return NotFound();
            }

            return View(programacionCentroEducativo);
        }
        [Authorize]
        // GET: ProgramacionCentroEducativoes/Create
        public IActionResult Create(int idProgramacion)
        {
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre");
            //ViewData["IdCentroEducativo"] = new SelectList(List<>, "Id", "Nombre");
            ViewData["IdProgramacions"] = new SelectList(_context.Programacion.Where(a => a.Id == idProgramacion).Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.IdBodegaNavigation.Nombre + " - " + s.IdRemesaNavigation.Nombre + " - " + s.Fecha.ToShortDateString()
            }), "Value", "Text");
            ViewBag.idprogramacion = idProgramacion;
            return View();
        }
        [Authorize]
        // POST: ProgramacionCentroEducativoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdProgramacion,IdCentroEducativo,Ninos,Ninas,Total")] ProgramacionCentroEducativo programacionCentroEducativo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programacionCentroEducativo);
                await _context.SaveChangesAsync();
                ViewBag.idprogramacion = programacionCentroEducativo.IdProgramacion;
                return RedirectToAction(nameof(Index), new { id = programacionCentroEducativo.IdProgramacion });
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", programacionCentroEducativo.IdCentroEducativoNavigation.IdDepartamento);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x=>x.DepartamentoId == programacionCentroEducativo.IdCentroEducativoNavigation.IdDepartamento), "Id", "Nombre", programacionCentroEducativo.IdCentroEducativoNavigation.IdMunicipio);
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Nombre", programacionCentroEducativo.IdCentroEducativo);
            ViewData["IdProgramacions"] = new SelectList(_context.Programacion.Where(x => x.Id == programacionCentroEducativo.IdProgramacion), "Id", "Fecha", programacionCentroEducativo.IdProgramacion);
            ViewBag.idprogramacion = programacionCentroEducativo.IdProgramacion;
            return View(programacionCentroEducativo);
        }
        [Authorize]
        // GET: ProgramacionCentroEducativoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacionCentroEducativo = await _context.ProgramacionCentroEducativo.FindAsync(id);
            if (programacionCentroEducativo == null)
            {
                return NotFound();
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", programacionCentroEducativo.IdCentroEducativoNavigation.IdDepartamento);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == programacionCentroEducativo.IdCentroEducativoNavigation.IdDepartamento), "Id", "Nombre", programacionCentroEducativo.IdCentroEducativoNavigation.IdMunicipio);
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Id", programacionCentroEducativo.IdCentroEducativo);
            ViewData["IdProgramacions"] = new SelectList(_context.Programacion, "Id", "Estado", programacionCentroEducativo.IdProgramacion);
            ViewBag.idprogramacion = programacionCentroEducativo.IdProgramacion;
            return View(programacionCentroEducativo);
        }
        [Authorize]
        // POST: ProgramacionCentroEducativoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdProgramacion,IdCentroEducativo,Ninos,Ninas,Total")] ProgramacionCentroEducativo programacionCentroEducativo)
        {
            if (id != programacionCentroEducativo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programacionCentroEducativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramacionCentroEducativoExists(programacionCentroEducativo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.idprogramacion = programacionCentroEducativo.IdProgramacion;
                return RedirectToAction(nameof(Index), new { id = programacionCentroEducativo.IdProgramacion });
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", programacionCentroEducativo.IdCentroEducativoNavigation.IdDepartamento);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == programacionCentroEducativo.IdCentroEducativoNavigation.IdDepartamento), "Id", "Nombre", programacionCentroEducativo.IdCentroEducativoNavigation.IdMunicipio);
            ViewData["IdCentroEducativo"] = new SelectList(_context.CentroEducativo, "Id", "Id", programacionCentroEducativo.IdCentroEducativo);
            ViewData["IdProgramacions"] = new SelectList(_context.Programacion, "Id", "Estado", programacionCentroEducativo.IdProgramacion);
            ViewBag.idprogramacion = programacionCentroEducativo.IdProgramacion;
            return View(programacionCentroEducativo);
        }
        [Authorize]
        // GET: ProgramacionCentroEducativoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacionCentroEducativo = await _context.ProgramacionCentroEducativo
                .Include(p => p.IdCentroEducativoNavigation)
                .Include(p => p.IdProgramacionNavigation)
                .Include(p => p.IdProgramacionNavigation.IdBodegaNavigation)
                .Include(p => p.IdCentroEducativoNavigation.IdDepartamentoNavigation)
                .Include(p => p.IdCentroEducativoNavigation.IdMunicipioNavigation)
                .Include(p => p.IdProgramacionNavigation.IdFocalizacionNavigation)
                .Include(p => p.IdProgramacionNavigation.IdRemesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programacionCentroEducativo == null)
            {
                return NotFound();
            }
            ViewBag.idprogramacion = programacionCentroEducativo.IdProgramacion;
            return View(programacionCentroEducativo);
        }
        [Authorize]
        // POST: ProgramacionCentroEducativoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programacionCentroEducativo = await _context.ProgramacionCentroEducativo.FindAsync(id);
            _context.ProgramacionCentroEducativo.Remove(programacionCentroEducativo);
            await _context.SaveChangesAsync();
            ViewBag.idprogramacion = programacionCentroEducativo.IdProgramacion;
            return RedirectToAction(nameof(Index), new { id = programacionCentroEducativo.IdProgramacion });
        }

        private bool ProgramacionCentroEducativoExists(int id)
        {
            return _context.ProgramacionCentroEducativo.Any(e => e.Id == id);
        }

        public object Agregar(int idProgramacion, string idCentroEducativo)
        {
            ProgramacionCentroEducativo programacionce = new ProgramacionCentroEducativo();
            var ce = _context.CentroEducativo.Find(idCentroEducativo);
            programacionce.IdProgramacion = idProgramacion;
            programacionce.IdCentroEducativo = ce.Id;
            programacionce.Ninos = ce.Ninos;
            programacionce.Ninas = ce.Ninas;
            programacionce.Total = ce.Total;
            try
            {
                _context.Add(programacionce);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new { data = false };
            }
            return new { data = true};
        }
    }
}
