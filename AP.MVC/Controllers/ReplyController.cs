using System.Web.Mvc;
using AP.Business;
using AP.Models;

namespace APMVC.Controllers
{
    public class ReplyController : Controller
    {
        private readonly ReplyBusiness _biz = new ReplyBusiness();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int threadId, string message)
        {
            if (threadId <= 0 || string.IsNullOrWhiteSpace(message))
                return RedirectToAction("Details", "Thread", new { id = threadId });

            _biz.CreateReply(new Reply
            {
                ThreadId = threadId,
                UserId = 1,
                Message = message
            });

            return RedirectToAction("Details", "Thread", new { id = threadId });
        }
    }
}