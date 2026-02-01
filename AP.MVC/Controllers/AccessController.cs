using AP.Data;
using System.Linq;
using System.Web.Mvc;

namespace AP.MVC.Controllers
{
    public class AccessController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [Route("productos/{id]")]

        public ActionResult Access(int id)
        {
            return RedirectPermanent ("");
        }

    }
}