using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForumCustom.WEB.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        // GET: MemberController
        private readonly IUserManager _userManager;

        private readonly IMemberManager _memberManager;

        public MemberController(IUserManager userManager, IMemberManager memberManager)
        {
            this._userManager = userManager;
            this._memberManager = memberManager;
        }

        // GET: MemberController/Details/5
        [Authorize]
        public async Task<ActionResult> Details()
        {
            var name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);

            var member = await _memberManager.GetMemberInfo(user);
            return View(member);
        }

        // GET: MemberController/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);

            var member = await _memberManager.GetMemberInfo(user);
            return View(member);
        }

        // POST: MemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MemberInfo collection)
        {
            try
            {
                var name = HttpContext.User.Identity.Name;
                var user = await _userManager.GetUserByLogin(name);
                var member = await _memberManager.GetMemberInfo(user);
                if (member.MemberId <= 0 && member.Email == null)
                {
                    collection.Email = user.Login;
                    await _memberManager.AddMemberForUser(collection, user);
                }
                else
                {
                    member.FirstName = collection.FirstName;
                    member.LastName = collection.LastName;
                    member.NickName = collection.NickName;
                    member.DateOfBirth = collection.DateOfBirth;
                    member.Email = user.Login;
                    await _memberManager.ChangeInfo(member, user);
                }
                return RedirectToAction(nameof(Details));
            }
            catch
            {
                return View();
            }
        }
    }
}