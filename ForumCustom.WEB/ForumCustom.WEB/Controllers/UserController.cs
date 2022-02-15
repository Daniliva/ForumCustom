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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        public UserController(IUserManager userManager)
        {
            this._userManager = userManager;
            _userRoleTransform = new UserRoleTransform();
            this._userTransform = new UserTransform();
        }

        /// <summary>
        /// Return list of users and available only for users who have admin role
        /// </summary>
        /// <returns>View with list of users</returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var list = await _userManager.GetAll();
            return View(list.Select(x => _userTransform.Transform(x)));
        }

        /// <summary>
        /// Return form for Registration user after get request
        /// </summary>
        /// <returns>View form for Registration</returns>
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>View user</returns>
        public async Task<ActionResult> Details()
        {
            var identity = HttpContext.User.Identity;

            var user = await _userManager.GetUserByLogin(identity.Name);
            return View(_userTransform.Transform(user));
        }

        /// <summary>
        /// Get information for registration and registration user in system with role "User"
        /// Redirect to login method, if registration was successful or return form if not
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(LoginInfo collection)
        {
            try
            {
                await _userManager.RegistrationUserInfo(collection.Login, collection.Password);
                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>Return form for login user</returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Enter user in system or return form
        /// </summary>
        /// <param name="collection">Entities consists of login and password</param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <returns>Return from for change user login or password</returns>
        public ActionResult Change()
        {
            return View();
        }

        /// <summary>
        /// Change user use entities LoginChangeModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Change(LoginChangeModel item)
        {
            try
            {
                var user = await _userManager.Login(item.Login, item.Password);
                await _userManager.ChangeUser(user, item.NewPassword, item.NewLogin);
                return RedirectToAction(nameof(Change));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Return info user with information
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return form user with information</returns>
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeRoles(int id)
        {
            var d = _userRoleTransform.Transform(await _userManager.GetId(id));
            var roles = await _userManager.GetAllRoles();

            d.SelectedSubjects.AddRange(roles.Select(x => x.Name));
            return View(d);
        }

        /// <summary>
        /// Change role for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Exit user and redirect to login form
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var g = HttpContext.User.Identity;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}