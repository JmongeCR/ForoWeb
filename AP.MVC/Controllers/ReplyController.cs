using System.Web.Mvc;
using AP.Business;
using AP.Models;
using AP.MVC.Filters;

namespace AP.MVC.Controllers
{
    [SessionAuthorize]
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
                UserId = (int)Session["UserId"],
                Message = message.Trim()
            });

            return RedirectToAction("Details", "Thread", new { id = threadId });
        }
    }
}
