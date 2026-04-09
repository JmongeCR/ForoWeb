using System.Web.Mvc;
using AP.Data;
using AP.MVC.Filters;

namespace AP.MVC.Controllers
{
    [SessionAuthorize]
    public class CategoryController : Controller
    {
        public ActionResult Index()
        {
            var repo = new CategoryRepository();
            var data = repo.GetAll();
            return View(data);
        }
    }
}
