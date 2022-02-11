using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Models;
using ForumCustom.WEB.Domain.Transform;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForumCustom.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly UserTransform _userTransform;
        private readonly UserRoleTransform _userRoleTransform;

        public UserController(IUserManager userManager)
        {
            this._userManager = userManager;
            _userRoleTransform = new UserRoleTransform();
            this._userTransform = new UserTransform();
        }

        // GET: UserController
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var list = await _userManager.GetAll();
            return View(list.Select(x => _userTransform.Transform(x)));
        }

        public ActionResult Registration()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(LoginInfo collection)
        {
            try
            {
                await _userManager.RegistrationUserInfo(collection.Login, collection.Password);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginInfo collection)
        {
            try
            {
                UserInfo user = await _userManager.Login(collection.Login, collection.Password);
                if (ModelState.IsValid)
                {
                    if (user != null)
                    {
                        await Authenticate(user); // аутентификация

                        return RedirectToAction("Index", "User");
                    }
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                return View(collection);
            }
            catch
            {
                return View(collection);
            }
        }

        private async Task Authenticate(UserInfo user)
        {
            List<ClaimsIdentity> listClaimsIdentities = new List<ClaimsIdentity>();
            foreach (var role in user.Role)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                listClaimsIdentities.Add(id);
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(listClaimsIdentities));
        }

        public ActionResult Change()
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Change(LoginChangeModel item)
        {
            try
            {
                var user = await _userManager.Login(item.Login, item.Password);
                await _userManager.ChangeUser(user, item.NewPassword, item.NewLogin);
                return RedirectToAction(nameof(Logout));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeRoles(int id)
        {
            var d = _userRoleTransform.Transform(await _userManager.GetId(id));
            var roles = await _userManager.GetAllRoles();

            d.SelectedSubjects.AddRange(roles.Select(x => x.Name));
            return View(d);
        }

        // POST: UserController/Edit/5await _userManager.GetId(id)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeRoles(UserRoleModel user)
        {
            try
            {
                var g = HttpContext.User.Identity;
                var userFind = await _userManager.GetUserByLogin(user.Login);
                var admin = await _userManager.GetUserByLogin(g.Name);
                await _userManager.ChangeRole(userFind, user.SelectedSubjects, admin);
                return RedirectToAction(nameof(ChangeRoles), "User", new { id = user.UserId });
                //   return View();
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var g = HttpContext.User.Identity;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}