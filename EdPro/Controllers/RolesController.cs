using EdPro.Models;
using EdPro.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibraryWebApplication.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //public IActionResult Index() => View(_roleManager.Roles.ToList());
        public async Task<IActionResult> Index(string? f, int? id)
        {
            ViewBag.F = null;
            if (f != null)
            {
                var rr = _userManager.GetUsersInRoleAsync(f);
                if (rr != null) ViewBag.F = "У цієї ролі є користувач";
                else RedirectToAction("Delete", "IdentityRoles", new { id = id });
            }
            return View(_roleManager.Roles.ToList());

        }
        public IActionResult UserList() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId, string? f)
        {
            // отримуємо користувача
            User user = await _userManager.FindByIdAsync(userId);
            ViewBag.F = f;
            if (user != null)
            {
                //список ролей користувача
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // отримуємо користувача
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // список ролей користувача
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // список ролей, які було додано
                var addedRoles = roles.Except(userRoles);
                // список ролей, які було видалено
                var removedRoles = userRoles.Except(roles);

                if (userRoles.Count() == 0)
                {
                    await _userManager.RemoveFromRolesAsync(user, removedRoles);
                    await _userManager.AddToRolesAsync(user, addedRoles);
                    return RedirectToAction("UserList");
                }
                if (roles.Count() > 1)
                { return RedirectToAction("Edit", "Roles", new { userId = userId, f = "Можна мати лише одну роль" }); }
                if (roles.Count() == 0)
                { return RedirectToAction("Edit", "Roles", new { userId = userId, f = "Повинна бути хоча б одна роль" }); }

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

    }
}
