using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using prjbase.Models;

namespace prjbase.Controllers
{
    [Authorize]
    [DisplayName("Administración de WayBill")]
    public class WayBillsController : Controller
    {
        private readonly prjbaseContext _context;
        private UserManager<Users> _userManager;

        public WayBillsController(prjbaseContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: WayBills
        public async Task<IActionResult> Index()
        {
            var prjbaseContext = _context.WayBill.Include(w => w.IdBodegaNavigation)
                                .Include(w=>w.WayBillDetalle)
                                .Include(w => w.IdRemesaNavigation);
            return View(await prjbaseContext.ToListAsync());
        }

        // GET: WayBills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wayBill = await _context.WayBill
                .Include(w => w.IdBodegaNavigation)
                .Include(w => w.IdRemesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wayBill == null)
            {
                return NotFound();
            }

            return View(wayBill);
        }

        // GET: WayBills/Create
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(User);

            if (user.Result.IdSubOficina != null)
            {
                if (user.Result.IdBodega != null)
                {
                    ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x=>x.Id == user.Result.IdBodega), "Id", "Nombre");
                }   
                else
                {
                    var bodegas = _context.Bodega.Where(x => x.Id == user.Result.IdSubOficina);
                    ViewData["IdBodega"] = new SelectList(bodegas, "Id", "Nombre");
                }
                
            }
            else
            {
                ViewData["IdBodega"] = new SelectList(_context.Bodega, "Id", "Nombre");
            }


            ViewData["IdRemesa"] = new SelectList(_context.Remesa, "Id", "Nombre");
            return View();
        }

        // POST: WayBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,IdRemesa,FechaDespacho,IdBodega,Estado,Year,FechaLlegada,IT,Nivel")] WayBill wayBill)
        {
            var user = _userManager.GetUserAsync(User);
            wayBill.UsuarioCreacion = user.Result.Id;
            wayBill.FechaCreacion = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                _context.Add(wayBill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { wayBill.Id });
            }
            
            if (user.Result.IdSubOficina != null)
            {
                if (user.Result.IdBodega != null)
                {
                    ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x => x.Id == user.Result.IdBodega), "Id", "Nombre", wayBill.IdBodega);
                }
                else
                {
                    var bodegas = _context.Bodega.Where(x => x.Id == user.Result.IdSubOficina);
                    ViewData["IdBodega"] = new SelectList(bodegas, "Id", "Nombre", wayBill.IdBodega);
                }
                
            }
            else
            {
                ViewData["IdBodega"] = new SelectList(_context.Bodega, "Id", "Nombre", wayBill.IdBodega);
            }

            ViewData["IdRemesa"] = new SelectList(_context.Remesa, "Id", "Nombre", wayBill.IdRemesa);
            return View(wayBill);
        }

        // GET: WayBills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wayBill = await _context.WayBill.FindAsync(id);
            if (wayBill == null)
            {
                return NotFound();
            }

            var user = _userManager.GetUserAsync(User);
            if (user.Result.IdSubOficina != null)
            {
                if (user.Result.IdBodega != null)
                {
                    ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x => x.Id == user.Result.IdBodega), "Id", "Nombre", wayBill.IdBodega);
                }
                else
                {
                    var bodegas = _context.Bodega.Where(x => x.Id == user.Result.IdSubOficina);
                    ViewData["IdBodega"] = new SelectList(bodegas, "Id", "Nombre", wayBill.IdBodega);
                }
            }
            else
            {
                ViewData["IdBodega"] = new SelectList(_context.Bodega, "Id", "Nombre", wayBill.IdBodega);
            }

            ViewData["IdRemesa"] = new SelectList(_context.Remesa, "Id", "Nombre", wayBill.IdRemesa);
            return View(wayBill);
        }

        // POST: WayBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,IdRemesa,FechaDespacho,IdBodega,Estado,Year,FechaLlegada,IT,Nivel,FechaCreacion,UsuarioCreacion")] WayBill wayBill)
        {
            if (id != wayBill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wayBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WayBillExists(wayBill.Id))
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

            var user = _userManager.GetUserAsync(User);
            if (user.Result.IdSubOficina != null)
            {
                if (user.Result.IdBodega != null)
                {
                    ViewData["IdBodega"] = new SelectList(_context.Bodega.Where(x => x.Id == user.Result.IdBodega), "Id", "Nombre", wayBill.IdBodega);
                }
                else
                {
                    var bodegas = _context.Bodega.Where(x => x.Id == user.Result.IdSubOficina);
                    ViewData["IdBodega"] = new SelectList(bodegas, "Id", "Nombre", wayBill.IdBodega);
                }
            }
            else
            {
                ViewData["IdBodega"] = new SelectList(_context.Bodega, "Id", "Nombre", wayBill.IdBodega);
            }

            ViewData["IdRemesa"] = new SelectList(_context.Remesa, "Id", "Nombre", wayBill.IdRemesa);
            return View(wayBill);
        }

        // GET: WayBills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wayBill = await _context.WayBill
                .Include(w => w.IdBodegaNavigation)
                .Include(w => w.IdRemesaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wayBill == null)
            {
                return NotFound();
            }

            return View(wayBill);
        }

        // POST: WayBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wayBill = await _context.WayBill.Include(x=>x.WayBillDetalle).FirstOrDefaultAsync(x=>x.Id==id);
            if (wayBill == null || wayBill.WayBillDetalle.Count() > 0)
            {
                ModelState.AddModelError("WayBill", "No se puede eliminar el registro");
                return View(wayBill);
            }
            else
            {
                _context.WayBill.Remove(wayBill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool WayBillExists(int id)
        {
            return _context.WayBill.Any(e => e.Id == id);
        }

        public async Task<JsonResult> Import([Bind("Id")] WayBill wayBill,IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return Json(Response<List<WayBillDetalle>>.GetResult(-1, "formfile is empty"));
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Json(Response<List<WayBillDetalle>>.GetResult(-1, "Not Support file extension"));
            }
            List<WayBillDetalle> items = new List<WayBillDetalle>();
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
                        int idproducto;
                        decimal cantidad;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            ModelState.Clear();
                            int.TryParse((worksheet.Cells[row, 1].Text == "" ? null : worksheet.Cells[row, 1].Text), out idproducto);
                            decimal.TryParse((worksheet.Cells[row, 2].Text == "" ? null : worksheet.Cells[row, 2].Text), out cantidad);

                            WayBillDetalle item = new WayBillDetalle();
                            item.IdWayBill = wayBill.Id;
                            item.IdProducto = idproducto;
                            item.Cantidad = cantidad;
                            item.Fecha = DateTime.Now;
                            TryValidateModel(item);
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

                            //ModelState.Clear();
                        }
                        if (errors == 0)
                        {

                            _context.AddRange(items);
                            _context.SaveChanges();
                            return Json(Response<List<WayBillDetalle>>.GetResult(0, "OK",items));
                        }
                    }
                    catch (Exception e)
                    {
                        return Json(Response<List<WayBillDetalle>>.GetResult(-2, e.Message, items));
                    }
                }
            }
            return Json(Response<List<WayBillDetalle>>.GetResult(0, "OK", items));
        }
    }
}
