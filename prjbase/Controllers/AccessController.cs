using prjbase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace prjbase.Controllers
{
    [Authorize]   
    [DisplayName("Access Management")]
    public class AccessController : Controller
    {
        private readonly prjbaseContext _dbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<Users> _userManager;

        public AccessController(
            prjbaseContext dbContext,
            RoleManager<ApplicationRole> roleManager,
            UserManager<Users> userManager
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // GET: Access
        [DisplayName("User List")]
        public async Task<ActionResult> Index()
        {
            var query = await (
                    from user in _dbContext.Users
                    join ur in _dbContext.UserRoles on user.Id equals ur.UserId into UserRoles                    
                    from userRole in UserRoles.DefaultIfEmpty()
                    join rle in _dbContext.Roles on userRole.RoleId equals rle.Id into Roles
                    from role in Roles.DefaultIfEmpty()
                    join sub in _dbContext.SubOficina on user.IdSubOficina  equals sub.Id into subof
                    from IdSubOficina in subof.DefaultIfEmpty()
                    join bodega in _dbContext.Bodega on user.IdBodega equals bodega.Id into bod
                    from IdBodega in bod.DefaultIfEmpty()
                    select new { user, userRole, role, IdSubOficina, IdBodega }
                ).ToListAsync();

            var userList = new List<UserRoleViewModel_Input>();
            foreach (var grp in query.GroupBy(q => q.user.Id))
            {
                var first = grp.First();
                userList.Add(new UserRoleViewModel_Input
                {
                    UserId = first.user.Id,
                    UserName = first.user.UserName,
                    Roles = first.role != null ? grp.Select(g => g.role).Select(r => r.Name) : new List<string>(),
                    IdBodegaNavigation = first.user.IdBodegaNavigation,
                    IdSubOficinaNavigation = first.user.IdSubOficinaNavigation,
                    IdSubOficina = first.user.IdSubOficina,
                    IdBodega = first.user.IdBodega
                });
            }

            return View(userList);
        }

        // GET: Access/Edit
        [DisplayName("Edit User Access")]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserRoleViewModel_Input
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = userRoles,
                IdBodega = user.IdBodega,
                IdSubOficina = user.IdSubOficina,
                IdBodegaNavigation = user.IdBodegaNavigation,
                IdSubOficinaNavigation = user.IdSubOficinaNavigation
            };

            //Entidad entidad = _dbContext.Entidad.Include(x => x.TipoEntidad).FirstOrDefault(x=>x.Id == user.UserEntidad);

            //var rolesp = _dbContext.TipoEntidadRol.Where(x => x.TipoEntidadId == entidad.TipoEntidadId);

            ViewData["Roles"] = await _roleManager.Roles.ToListAsync();

            ViewData["SubOficina"] = new SelectList(_dbContext.SubOficina, "Id", "Nombre", user.IdSubOficina);
            if (user.IdSubOficina ==null)
            {
                ViewData["Bodega"] = new SelectList(_dbContext.Bodega, "Id", "Nombre");
            }
            else
            {
                ViewData["Bodega"] = new SelectList(_dbContext.Bodega.Where(x=>x.IdSubOficina == user.IdSubOficina), "Id", "Nombre", user.IdBodega);
            }
            
            return View(userViewModel);
        }

        // POST: Access/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserRoleViewModel_Input viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View(viewModel);
            }

            var Users = _dbContext.Users.Find(viewModel.UserId);
            if (Users == null)
            {
                ModelState.AddModelError("", "User not found");
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View();
            }
            Users.IdSubOficina = viewModel.IdSubOficina;
            Users.IdBodega = viewModel.IdBodega;

            var userRoles = await _userManager.GetRolesAsync(Users);
            await _userManager.UpdateAsync(Users);
            await _userManager.RemoveFromRolesAsync(Users, userRoles);
            await _userManager.AddToRolesAsync(Users, viewModel.Roles);

            return RedirectToAction("Index");
        }
    }
}