using AP.Data;
using System.Linq;
using System.Web.Mvc;

namespace AP.MVC.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            var DataProvider = new AP.Data.DataProvider();
            var products = DataProvider.GetProducts();

            /*var products1 = products.ToList();
            products1.
            var products2 = products.ToArray();
            var products3 = products.ToDictionary(p => p.Id);*/

            return View(products);
        }
        public ActionResult Product()
        {
            var DataProvider = new AP.Data.DataProvider();
            var products = DataProvider.GetProducts();
            return View(products);
        }
    }
}