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
        private readonly ThreadBusiness _threadBiz = new ThreadBusiness();
        private readonly NotificationBusiness _notifBiz = new NotificationBusiness();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int threadId, string message)
        {
            if (threadId <= 0 || string.IsNullOrWhiteSpace(message))
                return RedirectToAction("Details", "Thread", new { id = threadId });

            int currentUserId = (int)Session["UserId"];

            _biz.CreateReply(new Reply
            {
                ThreadId = threadId,
                UserId   = currentUserId,
                Message  = message.Trim()
            });

            // DP: Observer - el autor del hilo es el observador, cuando alguien responde lo notificamos
            // solo si quien responde es distinto al autor para no notificarse a si mismo
            var thread = _threadBiz.GetThread(threadId);
            if (thread != null && thread.UserId != currentUserId)
            {
                string autor = Session["UserName"]?.ToString() ?? "Alguien";
                _notifBiz.Notify(
                    thread.UserId,
                    $"{autor} respondio tu hilo: {thread.Title}",
                    $"/Thread/Details/{threadId}"
                );
            }

            return RedirectToAction("Details", "Thread", new { id = threadId });
        }
    }
}
