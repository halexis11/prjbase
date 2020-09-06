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
    public class ConciliacionWBsController : Controller
    {
        private readonly prjbaseContext _context;

        public ConciliacionWBsController(prjbaseContext context)
        {
            _context = context;
        }

        // GET: ConciliacionWBs
        public async Task<IActionResult> Index(int Id)
        {
            string query;
            query = @"select wprod.Id as IdProducto, wemb.Nombre Embalaje, wprod.Nombre Producto, sum(wdet.Cantidad) as CantidadWB
                    ,coalesce(nr.total, 0) as CantidadNR
                    from WayBillDetalle wdet
                    inner join Producto wprod on wprod.id = wdet.IdProducto
                    inner join Embalaje wemb on wemb.Id = wprod.EmbalajeId
                    inner join WayBill w on w.Id = wdet.IdWayBill
                    inner join Bodega b on b.Id = w.IdBodega
                    inner join Remesa r on r.Id = w.IdRemesa
                    inner join Programacion p on p.IdBodega = w.IdBodega and p.IdRemesa = w.IdRemesa
                    left outer join(select nr.IdProducto, nr.IdProgramacion, sum(Cantidad) total from NotaRemision nr group by nr.idprogramacion, nr.IdProducto) nr on nr.IdProgramacion = p.Id and nr.IdProducto = wdet.IdProducto
                    left outer join Producto nrprod on nrprod.Id = nr.IdProducto
                    left outer join Embalaje nremb on nremb.Id = nrprod.EmbalajeId
                    where w.id = {0}
                    group by w.id, wprod.Id, b.Nombre, r.Nombre, wemb.Nombre, wprod.Nombre, nr.total
                    order by wprod.Id";
            return View(await _context.ConciliacionWBs.FromSql(string.Format(query, Id)).ToListAsync());
        }

    }
}
