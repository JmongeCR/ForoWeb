using System.Net;
using System.Web.Mvc;
using AP.Business;
using AP.Models;
using AP.MVC.Models;

namespace AP.MVC.Controllers
{
    // SOLID: SRP - este controller solo se ocupa de las vistas del foro
    // SOLID: DIP - se comunica con ThreadBusiness y no con los repos directamente
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

        // POST: Thread/Close/5  (solo Administrador)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Close(int id)
        {
            var role = Session["UserRole"]?.ToString();
            if (role != "Administrador")
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            _biz.SetClosed(id, true);
            return RedirectToAction("Details", new { id });
        }

        // POST: Thread/Reopen/5  (solo Administrador)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reopen(int id)
        {
            var role = Session["UserRole"]?.ToString();
            if (role != "Administrador")
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            _biz.SetClosed(id, false);
            return RedirectToAction("Details", new { id });
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