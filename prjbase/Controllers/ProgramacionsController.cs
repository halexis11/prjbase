using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjbase.Models;

namespace prjbase.Controllers
{
    [Authorize]
    [DisplayName("Administración de Programación")]
    public class ProgramacionsController : Controller
    {
        private readonly prjbaseContext _context;
        private UserManager<Users> _userManager;
        public ProgramacionsController(prjbaseContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // GET: Programacions
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.Programacion.Include(p => p.IdBodegaNavigation)
                .Include(p=>p.ProgramacionCentroEducativo)
                .Include(p => p.IdFocalizacionNavigation)
                .Include(p => p.IdRemesaNavigation)
                .Include(x => x.IdBodegaNavigation.IdDepartamentoNavigation)
                .Include(x => x.IdBodegaNavigation.IdMunicipioNavigation).AsQueryable();
            
            var user = _userManager.GetUserAsync(User);
            if (user.Result.IdSubOficina != null)
            {
                prjbaseContext = prjbaseContext.Where(x => x.IdBodegaNavigation.IdSubOficina == user.Result.IdSubOficina);
                if (user.Result.IdBodega!=null)
                {
                    prjbaseContext = prjbaseContext.Where(x => x.IdBodegaNavigation.Id == user.Result.IdBodega);
                }
                return View(await prjbaseContext.ToListAsync());
                 
            }
            else
            {
                return View(await prjbaseContext.ToListAsync());
            }
            
            
        }
        [Authorize]
        // GET: Programacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacion = await _context.Programacion
                .Include(p => p.IdBodegaNavigation)
                .Include(p => p.IdFocalizacionNavigation)
                .Include(p => p.IdRemesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programacion == null)
            {
                return NotFound();
            }

            return View(programacion);
        }
        [Authorize]
        // GET: Programacions/Create
        public IActionResult Create()
        {
            
            var user = _userManager.GetUserAsync(User);
            
            if (user.Result.IdSubOficina!=null)
            {
                var bodegas = _context.Bodega.AsQueryable();
                
                
                if (user.Result.IdBodega!=null)
                {
                    ViewData["IdBodega"] = new SelectList(bodegas.Where(x=>x.Id == user.Result.IdBodega), "Id", "Nombre", bodegas.FirstOrDefault().Id);
                }
                else
                {
                    ViewData["IdBodega"] = new SelectList(bodegas.Where(x=>x.IdSubOficina == user.Result.IdSubOficina), "Id", "Nombre", bodegas.FirstOrDefault().Id);
                }
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento.Where(x=>bodegas.Select(c=>c.IdDepartamento).Contains(x.Id)), "Id", "Nombre", bodegas.FirstOrDefault().IdDepartamento);
                ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => bodegas.Select(c => c.IdDepartamento).Contains(x.DepartamentoId)), "Id", "Nombre", bodegas.FirstOrDefault().IdMunicipio);
            
            }
            else
            {
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre");
            }


            
            ViewData["IdFocalizacion"] = new SelectList(_context.Focalizacion, "Id", "Nombre");
            ViewData["IdRemesa"] = new SelectList(_context.Remesa.Where(x=>x.Estado == true), "Id", "Nombre");
            return View();
        }
        [Authorize]
        // POST: Programacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRemesa,IdDepartamento,IdMunicipio,IdBodega,IdFocalizacion,Fecha,Estado,FechaCreacion,UsuarioCreacion,FechaModificacion,UsuarioModificacion")] Programacion programacion)
        {
            var user2 = _userManager.GetUserAsync(User);
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (programacion.IdBodega==null)
                {
                    if (user2.Result.IdSubOficina == null)
                    {
                        ModelState.AddModelError("IdBodega", "No es posible generar la programación para todas las Sub-Oficinas");
                    }
                    else
                    {
                        var bodegasUser = _context.Bodega.AsQueryable();
                        if (user2.Result.IdBodega!=null)
                        {
                            bodegasUser = bodegasUser.Where(x => x.Id == user2.Result.IdBodega);
                        }
                        else
                        {
                            bodegasUser = bodegasUser.Where(x => x.IdSubOficina == user2.Result.IdSubOficina);
                        }
                        int idprogramacion;
                        
                        foreach(Bodega item in bodegasUser)
                        {
                            //crear la programacion
                            Programacion programacionbodega = new Programacion { IdRemesa = programacion.IdRemesa
                            , IdDepartamento = programacion.IdDepartamento,IdMunicipio = programacion.IdMunicipio, IdBodega = item.Id
                            , IdFocalizacion = programacion.IdFocalizacion, Fecha = programacion.Fecha, Estado = "1"
                            , FechaCreacion = DateTime.Now, UsuarioCreacion = user.Id, FechaModificacion = DateTime.Now, UsuarioModificacion = user.Id};
                            
                            _context.Add(programacionbodega);
                            await _context.SaveChangesAsync();
                            idprogramacion = programacionbodega.Id;

                            //actualizar la lista de escuelas de la programación
                            var IdPrograParam = new SqlParameter("@IdProgramacion", idprogramacion);
                            var IdBodega = new SqlParameter("@IdBodega", item.Id);
                            _context.Database.ExecuteSqlCommand("exec  ProgramacionCentroEducativo_insert @IdBodega, @IdProgramacion", IdBodega, IdPrograParam);
                            //actualizar el detalle de productos de la programación
                            _context.Database.ExecuteSqlCommand("exec  ActualizarProgramacion @IdProgramacion", IdPrograParam);
                            return RedirectToAction(nameof(Index));
                        }
                        
                    }
                }
                else
                {
                    
                    programacion.FechaCreacion = DateTime.Now;
                    programacion.FechaModificacion = DateTime.Now;
                    programacion.UsuarioCreacion = user.Id;
                    programacion.UsuarioModificacion = user.Id;
                    _context.Add(programacion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                

            }

            

            if (user2.Result.IdSubOficina != null)
            {
                var bodegas = _context.Bodega.Where(x => x.IdSubOficinaNavigation.Id == user2.Result.IdSubOficina);
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento.Where(x => bodegas.Select(c => c.IdDepartamento).Contains(x.Id)), "Id", "Nombre", programacion.IdDepartamento);
                ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => bodegas.Select(c => c.IdDepartamento).Contains(x.DepartamentoId)), "Id", "Nombre", programacion.IdMunicipio);
                ViewData["IdBodega"] = new SelectList(bodegas, "Id", "Nombre", programacion.IdBodega);
            }
            else
            {
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", programacion.IdDepartamento);
                ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == programacion.IdDepartamento), "Id", "Nombre", programacion.IdMunicipio);
                ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x => x.IdMunicipio == programacion.IdMunicipio), "Id", "Nombre", programacion.IdBodega);
            }

            
            
            ViewData["IdFocalizacion"] = new SelectList(_context.Focalizacion, "Id", "Nombre", programacion.IdFocalizacion);
            ViewData["IdRemesa"] = new SelectList(_context.Remesa.Where(x => x.Estado == true), "Id", "Nombre", programacion.IdRemesa);
            return View(programacion);
        }
        [Authorize]
        // GET: Programacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programacion = await _context.Programacion.FindAsync(id);
            if (programacion == null)
            {
                return NotFound();
            }
            var user = _userManager.GetUserAsync(User);

            if (user.Result.IdSubOficina != null)
            {
                var bodegas = _context.Bodega.Where(x => x.Id == user.Result.IdSubOficina);
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento.Where(x => bodegas.Select(c => c.IdDepartamento).Contains(x.Id)), "Id", "Nombre", programacion.IdDepartamento);
                ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => bodegas.Select(c => c.IdDepartamento).Contains(x.DepartamentoId)), "Id", "Nombre", programacion.IdMunicipio);
                ViewData["IdBodega"] = new SelectList(bodegas, "Id", "Nombre", programacion.IdBodega);
            }
            else
            {
                ViewData["IdBodega"] = new SelectList(_context.Bodega, "Id", "Nombre");
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", programacion.IdDepartamento);
                ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == programacion.IdDepartamento), "Id", "Nombre", programacion.IdMunicipio);
                
            }
            ViewData["IdFocalizacion"] = new SelectList(_context.Focalizacion, "Id", "Nombre", programacion.IdFocalizacion);
            ViewData["IdRemesa"] = new SelectList(_context.Remesa, "Id", "Nombre", programacion.IdRemesa);
            return View(programacion);
        }
        [Authorize]
        // POST: Programacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRemesa, IdDepartamento, IdMunicipio,IdBodega,IdFocalizacion,Fecha,Estado,FechaCreacion,UsuarioCreacion,FechaModificacion,UsuarioModificacion")] Programacion programacion)
        {
            if (id != programacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                  
                    programacion.FechaModificacion = DateTime.Now;
                    programacion.UsuarioModificacion = user.Id;
                    _context.Update(programacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramacionExists(programacion.Id))
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
            var user2 = _userManager.GetUserAsync(User);

            if (user2.Result.IdSubOficina != null)
            {
                var bodegas = _context.Bodega.Where(x => x.IdSubOficina == user2.Result.IdSubOficina);
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento.Where(x => bodegas.Select(c => c.IdDepartamento).Contains(x.Id)), "Id", "Nombre", bodegas.FirstOrDefault().IdDepartamento);
                ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => bodegas.Select(c => c.IdDepartamento).Contains(x.DepartamentoId)), "Id", "Nombre", bodegas.FirstOrDefault().IdMunicipio);
                ViewData["IdBodega"] = new SelectList(bodegas, "Id", "Nombre", programacion.IdBodega);
            }
            else
            {
                ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", programacion.IdBodegaNavigation.IdDepartamento);
                ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == programacion.IdBodegaNavigation.IdDepartamento), "Id", "Nombre", programacion.IdBodegaNavigation.IdMunicipio);
            }
            ViewData["IdFocalizacion"] = new SelectList(_context.Focalizacion, "Id", "Nombre", programacion.IdFocalizacion);
            ViewData["IdRemesa"] = new SelectList(_context.Remesa, "Id", "Nombre", programacion.IdRemesa);
            return View(programacion);
        }
        [Authorize]
        // GET: Programacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var programacion = await _context.Programacion
                .Include(p => p.IdBodegaNavigation)
                .Include(p => p.IdFocalizacionNavigation)
                .Include(p => p.IdRemesaNavigation)
                .Include(p => p.ProgramacionCentroEducativo)
                .Include(p => p.ProgramacionProductoDetalle)
                .Include(p=> p.NotaRemision)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programacion == null)
            {
                return NotFound();
            }

            return View(programacion);
        }
        [Authorize]
        // POST: Programacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programacion = await _context.Programacion.Include(x=>x.ProgramacionCentroEducativo)
                                     .Include(x=> x.ProgramacionProductoDetalle)
                                     .Include(x=>x.NotaRemision)
                                     .FirstOrDefaultAsync(x=>x.Id==id);
            if (programacion == null || programacion.NotaRemision.Count() > 0
               || programacion.ProgramacionProductoDetalle.Count() > 0
               || programacion.ProgramacionCentroEducativo.Count() > 0)
            {
                ModelState.AddModelError("Programación", "No se puede eliminar el registro");
                return View(programacion);
            }
            else
            {
                _context.Programacion.Remove(programacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
        }

        [Authorize]
        public async Task<IActionResult> Aprobar(int id)
        {
            var programacion = await _context.Programacion.FindAsync(id);
            programacion.Estado = "2";
            var user = _userManager.GetUserAsync(User);
            programacion.UsuarioModificacion = user.Result.Id;
            programacion.FechaModificacion = DateTime.Now;
            _context.Programacion.Update(programacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Inicio(int id)
        {
            
            var programacion = await _context.Programacion.FindAsync(id);
            
            programacion.Estado = "1";
            var user = _userManager.GetUserAsync(User);
            programacion.UsuarioModificacion = user.Result.Id;
            programacion.FechaModificacion = DateTime.Now;
            _context.Programacion.Update(programacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Paso2(int id)
        {
            var programacion = await _context.Programacion.FindAsync(id);
            programacion.Estado = "3";
            var user = _userManager.GetUserAsync(User);
            programacion.UsuarioModificacion = user.Result.Id;
            programacion.FechaModificacion = DateTime.Now;
            _context.Programacion.Update(programacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> VolverPaso2(int id)
        {
            var programacion = await _context.Programacion.FindAsync(id);
            programacion.Estado = "2";
            var user = _userManager.GetUserAsync(User);
            programacion.UsuarioModificacion = user.Result.Id;
            programacion.FechaModificacion = DateTime.Now;
            _context.Programacion.Update(programacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramacionExists(int id)
        {
            return _context.Programacion.Any(e => e.Id == id);
        }
    }
}
