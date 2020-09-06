using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using prjbase.Controllers;
using prjbase.Models;

namespace prjbase.Controllers
{
    public class ImportarMuestraController : Controller
    {
        private readonly prjbaseContext _context;

        public ImportarMuestraController(prjbaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<JsonResult> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return Json(Response<List<ImportarMuestra>>.GetResult(-1, "formfile is empty"));
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Json(Response<List<ImportarMuestra>>.GetResult(-1, "Not Support file extension"));
            }
            List<ImportarMuestra> items = new List<ImportarMuestra>();
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
                        for (int row = 2; row <= rowCount; row++)
                        {
                            ImportarMuestra item = new ImportarMuestra();
                            item.departamento = (worksheet.Cells[row, 1].Text == "" ? null : worksheet.Cells[row, 1].Text);
                            item.municipio = (worksheet.Cells[row, 2].Text == "" ? null : worksheet.Cells[row, 2].Text);
                            item.bodega = (worksheet.Cells[row, 3].Text == "" ? null : worksheet.Cells[row, 3].Text);
                            item.focalizacion = (worksheet.Cells[row, 4].Text == "" ? null : worksheet.Cells[row, 4].Text);
                            item.remesa = (worksheet.Cells[row, 5].Text == "" ? null : worksheet.Cells[row, 5].Text);
                            item.fecha = DateTime.Parse(worksheet.Cells[row, 6].Text);
                            TryValidateModel(item);
                            var res = ModelState.IsValid;
                            if (res == false)
                            {
                                errors += 1;
                                message= string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                            }
                            else
                            {
                                message = "Ok";
                            }

                            item.resultado = message;
                            items.Add(item);
                            
                            ModelState.Clear();
                        }
                        if (errors==0)
                        { 
                            _context.AddRange(items);
                            _context.SaveChanges();
                            return Json(Response<List<ImportarMuestra>>.GetResult(0, "OK", items));
                        }
                    }
                    catch (Exception e)
                    {
                        return Json(Response<List<ImportarMuestra>>.GetResult(-2, e.Message, items));
                    }
                }
            }
            return Json(Response<List<ImportarMuestra>>.GetResult(0, "OK", items));
        }

    }
}