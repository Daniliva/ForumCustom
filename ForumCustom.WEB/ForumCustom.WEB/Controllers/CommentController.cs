using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForumCustom.WEB.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly ITopicManager _topicManager;
        private readonly IMemberManager _memberManager;
        private readonly ICommentManager _commentManager;
        private readonly CommentTopicModelTransform _commentTopicModelTransform;
        private readonly CommentTransform _commentTransform;

        public CommentController(ITopicManager topicManager, IMemberManager memberManager, IUserManager userManager, ICommentManager commentManager)
        {
            _topicManager = topicManager;
            _memberManager = memberManager;
            _userManager = userManager;
            _commentManager = commentManager;
            _commentTransform = new CommentTransform();
            _commentTopicModelTransform = new CommentTopicModelTransform();
        }

        // GET: CommentController
        public async Task<ActionResult> Index()
        {
            var name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);

            var member = await _memberManager.GetMemberInfo(user);
            if (member.NickName == null)
            {
                return RedirectToAction("Create", "Member");
            }
            return View(await _commentManager.GetByNickName(member.NickName));
        }

        // GET: CommentController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);
            var member = await _memberManager.GetMemberInfo(user);
            if (member.NickName == null)
            {
                return RedirectToAction("Create", "Member");
            }
            var comment = _commentTopicModelTransform.Transform(await _commentManager.GetTopicByIdComment(id));
            var commentFind = await _commentManager.GetByNickNameId(member.NickName, id);
            comment.NickName = commentFind.Nickname;
            comment.Comment = commentFind.Comment;
            comment.Id = commentFind.Id;
            return View(comment);
        }

        // GET: CommentController/Create

        // GET: CommentController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);

            var member = await _memberManager.GetMemberInfo(user);
            if (member.NickName == null)
            {
                return RedirectToAction("Create", "Member");
            }
            var comment = await _commentManager.GetByNickNameId(member.NickName, id);

            return View(_commentTransform.Transform(comment));
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CommentInfo collection)
        {
            try
            {
                var name = HttpContext.User.Identity.Name;
                var user = await _userManager.GetUserByLogin(name);

                var member = await _memberManager.GetMemberInfo(user);
                if (member.NickName == null)
                {
                    return RedirectToAction("Create", "Member");
                }
                collection.Nickname = member.NickName;
                await _commentManager.ChangeComment(collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var name = HttpContext.User.Identity.Name;
            var comment = _commentTopicModelTransform.Transform(await _commentManager.GetTopicByIdComment(id));
            var commentFind = await _commentManager.GetById(id);
            comment.NickName = commentFind.Nickname;
            comment.Comment = commentFind.Comment;
            comment.Id = commentFind.Id;
            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, CommentInfo collection)
        {
            try
            {
                var commentFind = await _commentManager.GetById(id);
                _ = _commentManager.Delete(commentFind, await _commentManager.GetTopicByIdComment(id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}