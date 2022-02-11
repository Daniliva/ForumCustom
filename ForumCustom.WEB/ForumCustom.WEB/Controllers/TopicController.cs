using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace ForumCustom.WEB.Controllers
{
    [Authorize]
    public class TopicController : Controller
    {
        private readonly IUserManager _userManager;

        private readonly ITopicManager _topicManager;
        private readonly IMemberManager _memberManager;
        private readonly TopicTransform _topicTransform;

        public TopicController(ITopicManager topicManager, IUserManager userManager, IMemberManager memberManager)
        {
            this._topicManager = topicManager;
            this._userManager = userManager;
            this._memberManager = memberManager;
            _topicTransform = new TopicTransform();
        }

        // GET: TopicController

        public async Task<ActionResult> Index()
        {
            return View((await _topicManager.GetAll()));
        }

        [Authorize(Roles = "Author")]
        public async Task<ActionResult> YourTopic()
        {
            string? name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);
            var member = await _memberManager.GetMemberInfo(user);
            return View(await _topicManager.GetByNickName(member.NickName));
        }

        // GET: TopicController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var topic = await _topicManager.GetById(id);
            return View(_topicTransform.Transform(topic));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(int id, CommentInfo commentInfo)
        {
            string? name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);
            var member = await _memberManager.GetMemberInfo(user);
            commentInfo.Nickname = member.NickName;
            commentInfo.Id = 0;
            var topic = await _topicManager.GetById(id);
            await _topicManager.AddCommentToTopic(topic, commentInfo);
            topic = await _topicManager.GetById(id);
            return View(_topicTransform.Transform(topic));
        }

        // GET: TopicController/Create
        [Authorize(Roles = "Author")]
        public async Task<ActionResult> Create()
        {
            var name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);
            var member = await _memberManager.GetMemberInfo(user);
            if (member.NickName == null)
            {
                return RedirectToAction(nameof(Create), "Member");
            }
            return View();
        }

        // POST: TopicController/Create
        [HttpPost]
        [Authorize(Roles = "Author")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TopicInfo collection)
        {
            try
            {
                var name = HttpContext.User.Identity.Name;
                var user = await _userManager.GetUserByLogin(name);
                var result = await _topicManager.AddTopic(collection, user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TopicController/Edit/5
        [Authorize(Roles = "Author")]
        public async Task<ActionResult> Edit(int id)
        {
            var name = HttpContext.User.Identity.Name;
            var user = await _userManager.GetUserByLogin(name);
            var member = await _memberManager.GetMemberInfo(user);
            var d = _topicManager.GetStatusDictionary().Result.Select(x => x.Value);
            var topic = _topicManager.GetById(id).Result;
            ViewBag.Companies = new SelectList(d, topic.Status);
            return View(topic);
        }

        // POST: TopicController/Edit/5
        [HttpPost]
        [Authorize(Roles = "Author")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TopicInfo collection)
        {
            try
            {
                var name = HttpContext.User.Identity.Name;
                var user = await _userManager.GetUserByLogin(name);
                var member = await _memberManager.GetMemberInfo(user);
                var topic = await _topicManager.GetById(collection.Id);
                if (topic.Nickname == member.NickName)
                {
                    await _topicManager.ChangeTopic(collection);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TopicController/Delete/5
        [Authorize(Roles = "Author")]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _topicManager.GetById(id));
        }

        // POST: TopicController/Delete/5
        [HttpPost]
        [Authorize(Roles = "Author")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(TopicInfo topic)
        {
            try
            {
                await _topicManager.Delete(topic);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}