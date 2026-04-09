using System.Net;
using System.Web.Mvc;
using AP.Business;
using AP.Models;
using AP.MVC.Models;

namespace AP.MVC.Controllers
{
    // SOLID: Single Responsibility Principle (SRP) - maneja unicamente las acciones HTTP
    //        del foro: listar hilos por categoria, crear hilo y ver detalle con respuestas.
    // SOLID: Dependency Inversion Principle (DIP) - depende de ThreadBusiness (abstraccion),
    //        no de repositorios concretos.
    public class ThreadController : Controller
    {
        private readonly ThreadBusiness _biz = new ThreadBusiness();

        // GET: Thread?categoryId=1
        public ActionResult Index(int? categoryId)
        {
            if (categoryId == null)
                return RedirectToAction("Index", "Category");

            ViewBag.CategoryId = categoryId.Value;
            ViewBag.CategoryName = _biz.GetCategoryName(categoryId.Value);

            var data = _biz.GetThreadsByCategory(categoryId.Value);
            return View(data);
        }

        // GET: Thread/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var thread = _biz.GetThread(id.Value);
            if (thread == null)
                return HttpNotFound();

            var vm = new ThreadDetailsVM
            {
                Thread = thread,
                Replies = _biz.GetReplies(id.Value)
            };

            return View(vm);
        }

        // GET: Thread/Create?categoryId=1
        public ActionResult Create(int? categoryId)
        {
            if (categoryId == null)
                return RedirectToAction("Index", "Category");

            var model = new Thread
            {
                CategoryId = categoryId.Value
            };

            return View(model);
        }

        // POST: Thread/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Thread model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Placeholder mientras no haya login:
            if (model.UserId == 0) model.UserId = 1;

            _biz.CreateThread(model);

            return RedirectToAction("Index", new { categoryId = model.CategoryId });
        }
    }
}