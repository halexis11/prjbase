using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;
using prjbase.Models;

namespace prjbase.Controllers
{
   // [Authorize]
    [DisplayName("Administración de Centros Educativos")]
    public class CentroEducativoesController : Controller
    {
        private readonly prjbaseContext _context;

        public CentroEducativoesController(prjbaseContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: CentroEducativoes
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.CentroEducativo.Include(c => c.IdBodegaNavigation).Include(c => c.IdDepartamentoNavigation).Include(c => c.IdDistritoNavigation).Include(c => c.IdMunicipioNavigation).Include(c => c.IdZonaNavigation);
            return View(await prjbaseContext.ToListAsync());
        }
        [Authorize]
        // GET: CentroEducativoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroEducativo = await _context.CentroEducativo
                .Include(c => c.IdBodegaNavigation)
                .Include(c => c.IdDepartamentoNavigation)
                .Include(c => c.IdDistritoNavigation)
                .Include(c => c.IdMunicipioNavigation)
                .Include(c => c.IdZonaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centroEducativo == null)
            {
                return NotFound();
            }

            return View(centroEducativo);
        }
        [Authorize]
        // GET: CentroEducativoes/Create
        public IActionResult Create()
        {
            
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre");
            //ViewData["IdBodega"] = new SelectList(_context.Bodega, "Id", "Nombre");
            //ViewData["IdDistrito"] = new SelectList(_context.Distrito, "Id", "Nombre");
            //ViewData["IdMunicipio"] = new SelectList(_context.Municipio, "Id", "Codigo");
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre");
            return View();
        }
        [Authorize]
        // POST: CentroEducativoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDepartamento,IdMunicipio,IdDistrito,IdBodega,IdZona,Nivel,Nombre,Direccion,Telefono,Estado,NombreContacto,CorreoContacto,TelefonoContacto,Ninos,Ninas,Total,FechaCreacion,FechaModificacion")] CentroEducativo centroEducativo)
        {
            if (!(CentroEducativoExists(centroEducativo.Id)))
            {
                if (ModelState.IsValid)
                {
                    centroEducativo.FechaCreacion = DateTime.Now;
                    centroEducativo.FechaModificacion = DateTime.Now;
                    _context.Add(centroEducativo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("Centro duplicado", "Ya existe un Centro Educativo con el Código ingresado.");
            }
                        
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", centroEducativo.IdDepartamento);
            ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x=>x.IdDepartamento == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdBodega);
            ViewData["IdDistrito"] = new SelectList(_context.Distrito.Where(x=>x.IdDepartamento == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdDistrito);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x=>x.DepartamentoId == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdMunicipio);
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", centroEducativo.IdZona);
            return View(centroEducativo);
        }
        [Authorize]
        // GET: CentroEducativoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroEducativo = await _context.CentroEducativo.FindAsync(id);
            if (centroEducativo == null)
            {
                return NotFound();
            }            
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", centroEducativo.IdDepartamento);
            ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x => x.IdDepartamento == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdBodega);
            ViewData["IdDistrito"] = new SelectList(_context.Distrito.Where(x => x.IdDepartamento == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdDistrito);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdMunicipio);
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", centroEducativo.IdZona);
            return View(centroEducativo);
        }
        [Authorize]
        // POST: CentroEducativoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,IdDepartamento,IdMunicipio,IdDistrito,IdBodega,IdZona,Nivel,Nombre,Direccion,Telefono,Estado,NombreContacto,CorreoContacto,TelefonoContacto,Ninos,Ninas,Total,FechaCreacion,FechaModificacion")] CentroEducativo centroEducativo)
        {
            if (id != centroEducativo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    centroEducativo.FechaModificacion = DateTime.Now;
                    _context.Update(centroEducativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroEducativoExists(centroEducativo.Id))
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
            
            ViewData["IdDepartamento"] = new SelectList(_context.Departamento, "Id", "Nombre", centroEducativo.IdDepartamento);
            ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x => x.IdDepartamento == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdBodega);
            ViewData["IdDistrito"] = new SelectList(_context.Distrito.Where(x => x.IdDepartamento == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdDistrito);
            ViewData["IdMunicipio"] = new SelectList(_context.Municipio.Where(x => x.DepartamentoId == centroEducativo.IdDepartamento), "Id", "Nombre", centroEducativo.IdMunicipio);
            ViewData["IdZona"] = new SelectList(_context.Zona, "Id", "Nombre", centroEducativo.IdZona);
            return View(centroEducativo);
        }
        [Authorize]
        // GET: CentroEducativoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroEducativo = await _context.CentroEducativo
                .Include(c => c.IdBodegaNavigation)
                .Include(c => c.IdDepartamentoNavigation)
                .Include(c => c.IdDistritoNavigation)
                .Include(c => c.IdMunicipioNavigation)
                .Include(c => c.IdZonaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centroEducativo == null)
            {
                return NotFound();
            }

            return View(centroEducativo);
        }
        [Authorize]
        // POST: CentroEducativoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var centroEducativo = await _context.CentroEducativo.FindAsync(id);
            if (CentroEducativoExists(id))
            {
                try
                {

                    _context.CentroEducativo.Remove(centroEducativo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Centro Educativo", "Ha ocurrido un error al eliminar el Centro Educativo.");
                    return View(centroEducativo);
                }
            }
            else
            {
                ModelState.AddModelError("Centro Educativo", "No se encontró el Centro Educativo.");
                return View(centroEducativo);
            }
            
        }

        private bool CentroEducativoExists(string id)
        {
            return _context.CentroEducativo.Any(e => e.Id == id);
        }
        public JsonResult GetCE(int id)
        {
            var list = _context.CentroEducativo.Where(a => a.IdMunicipio == id).Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Id + "-" + s.Nombre.ToString()
            }); ;
            return Json(new SelectList(list, "Value", "Text"));
        }

        public JsonResult GetCENoenProgram(int id, int idProgramacion)
        {
            var prog = _context.Programacion.Include(x=>x.IdRemesaNavigation).Where(x=>x.Id==idProgramacion).FirstOrDefault();
            var progce = _context.ProgramacionCentroEducativo.Where(x=>x.IdProgramacion == prog.Id).Select(x=>x.IdCentroEducativo).ToArray();
            
            var list = _context.CentroEducativo.Where(a => a.IdMunicipio == id && !progce.Contains(a.Id) && a.IdZona == prog.IdRemesaNavigation.IdZona).Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Id + "-" + s.Nombre.ToString()
            });
            return Json(new SelectList(list, "Value", "Text"));
        }

        public IActionResult Importar()
        {
            return View();
        }

        public async Task<JsonResult> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return Json(Response<List<ImportarCE>>.GetResult(-1, "El archivo se encuentra vacío"));
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Json(Response<List<ImportarMuestra>>.GetResult(-2, "Tipo de archivo no soportado"));
            }
            List<ImportarCE> items = new List<ImportarCE>();
            List<CentroEducativo> centroEducativosNew = new List<CentroEducativo>();
            List<CentroEducativo> centroEducativosUpd = new List<CentroEducativo>();
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    try
                    {
                        int errors = 0;
                        string message;
                        int? departamento, municipio, distrito, zona;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            ImportarCE item = new ImportarCE();
                            CentroEducativo centroEducativo = new CentroEducativo();
                            string codnivel = worksheet.Cells[row, 12].Text;
                            item.Codigo = (worksheet.Cells[row, 4].Text == "" ? null : worksheet.Cells[row, 4].Text) + codnivel;
                            centroEducativo = _context.CentroEducativo.Where(x => x.Id == item.Codigo).FirstOrDefault();
                            //cargar lista que se mostrará en el mantenimiento
                            item.Departamento = (worksheet.Cells[row, 1].Text == "" ? null : worksheet.Cells[row, 1].Text); ;
                            item.Municipio = (worksheet.Cells[row, 2].Text == "" ? null : worksheet.Cells[row, 2].Text);
                            item.Distrito = (worksheet.Cells[row, 3].Text == "" ? null : worksheet.Cells[row, 3].Text);
                            item.Zona = (worksheet.Cells[row, 8].Text == "" ? null : worksheet.Cells[row, 8].Text);
                            item.Nivel = (worksheet.Cells[row, 11].Text == "" ? null : worksheet.Cells[row, 11].Text);
                            item.Nombre = (worksheet.Cells[row, 6].Text == "" ? null : worksheet.Cells[row, 6].Text);
                            item.Direccion = (worksheet.Cells[row, 7].Text == "" ? null : worksheet.Cells[row, 7].Text);
                            item.Telefono = (worksheet.Cells[row, 15].Text == "" ? null : worksheet.Cells[row, 15].Text);
                            item.NombreContacto = (worksheet.Cells[row, 12].Text == "" ? null : worksheet.Cells[row, 12].Text);
                            item.TelefonoContacto = (worksheet.Cells[row, 16].Text == "" ? null : worksheet.Cells[row, 16].Text);
                            item.CorreoContacto = (worksheet.Cells[row, 17].Text == "" ? null : worksheet.Cells[row, 17].Text);
                            item.Ninos = int.Parse((worksheet.Cells[row, 19].Text == "" ? null : worksheet.Cells[row, 18].Text));
                            item.Ninas = int.Parse((worksheet.Cells[row, 18].Text == "" ? null : worksheet.Cells[row, 19].Text));
                            item.Total = int.Parse((worksheet.Cells[row, 20].Text == "" ? null : worksheet.Cells[row, 20].Text));
                            if (centroEducativo != null)
                            {
                                centroEducativo.Direccion = item.Direccion;
                                centroEducativo.NombreContacto = item.NombreContacto;
                                centroEducativo.TelefonoContacto = item.TelefonoContacto;
                                centroEducativo.Telefono = item.Telefono;
                                centroEducativo.Ninas = item.Ninas;
                                centroEducativo.Ninos = item.Ninos;
                                centroEducativo.Total = item.Ninos;
                                centroEducativo.CorreoContacto = item.CorreoContacto;
                                centroEducativosUpd.Add(centroEducativo);
                                centroEducativo.FechaModificacion = DateTime.Now;
                            }
                            else
                            {
                                centroEducativo = new CentroEducativo();
                                centroEducativo.Id = item.Codigo;
                                departamento = _context.Departamento.Where(x => x.Nombre.TrimEnd().TrimStart() == item.Departamento).FirstOrDefault()?.Id;
                                municipio = _context.Municipio.Where(x => x.Nombre.TrimEnd().TrimStart() == item.Municipio).FirstOrDefault()?.Id;
                                zona = _context.Zona.Where(x => x.Nombre.TrimEnd().TrimStart() == item.Zona).FirstOrDefault()?.Id;
                                distrito = _context.Zona.Where(x => x.Id == int.Parse(item.Distrito)).FirstOrDefault()?.Id;
                                if (departamento != null)
                                    centroEducativo.IdDepartamento = departamento.Value;
                                if (municipio != null)
                                    centroEducativo.IdMunicipio = municipio.Value;
                                if (zona != null)
                                    centroEducativo.IdZona = zona.Value;
                                if (distrito!=null)
                                    centroEducativo.IdDistrito = distrito.Value;
                                centroEducativo.Nombre = item.Nombre;
                                centroEducativo.Direccion = item.Nombre;
                                centroEducativo.Telefono = item.Telefono;
                                centroEducativo.NombreContacto = item.NombreContacto;
                                centroEducativo.TelefonoContacto = item.TelefonoContacto;
                                centroEducativo.CorreoContacto = item.CorreoContacto;
                                centroEducativo.Ninos = item.Ninos;
                                centroEducativo.Ninas = item.Ninas;
                                centroEducativo.Total = item.Ninas;
                                centroEducativo.Nivel = item.Nivel;
                                centroEducativosNew.Add(centroEducativo);
                                centroEducativo.Estado = true;
                                centroEducativo.FechaCreacion = DateTime.Now;
                                centroEducativo.FechaModificacion = DateTime.Now;
                            }
                            
                            TryValidateModel(centroEducativo);
                            var res = ModelState.IsValid;
                            if (res == false)
                            {
                                errors += 1;
                                message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                            }
                            else
                            {

                                message = "Ok";
                            }

                            item.resultado = message;
                            items.Add(item);

                            ModelState.Clear();
                        }
                        if (errors == 0)
                        {
                            if (centroEducativosNew.Count>0)
                            {
                                _context.AddRange(centroEducativosNew);
                                _context.SaveChanges();
                            }
                            if (centroEducativosNew.Count > 0)
                            {
                                _context.UpdateRange(centroEducativosUpd);
                                _context.SaveChanges();
                            }                            
                            return Json(Response<List<ImportarCE>>.GetResult(0, "OK", items));
                        }
                    }
                    catch (Exception e)
                    {
                        return Json(Response<List<ImportarCE>>.GetResult(-2, e.Message, items));
                    }
                }
            }
            return Json(Response<List<ImportarCE>>.GetResult(0, "OK", items));
        }
    }
}
