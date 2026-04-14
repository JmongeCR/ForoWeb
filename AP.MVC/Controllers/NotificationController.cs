using System.Web.Mvc;
using AP.Business;

namespace AP.MVC.Controllers
{
    // Muestra las notificaciones del usuario y las marca como leidas al entrar
    // SOLID: SRP - este controller solo maneja lo de notificaciones
    public class NotificationController : Controller
    {
        private readonly NotificationBusiness _biz = new NotificationBusiness();

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Access");

            int userId = (int)Session["UserId"];
            var list = _biz.GetByUser(userId);
            _biz.MarkAllRead(userId);

            return View(list);
        }

        // Se llama desde el layout para mostrar el contador en el icono de campana
        [ChildActionOnly]
        public ActionResult BellBadge()
        {
            if (Session["UserId"] == null)
                return Content("");

            int count = _biz.GetUnreadCount((int)Session["UserId"]);
            return PartialView("_BellBadge", count);
        }
    }
}
