using AP.Data;
using System.Web.Mvc;


namespace AP.MVC.Controllers
{
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
