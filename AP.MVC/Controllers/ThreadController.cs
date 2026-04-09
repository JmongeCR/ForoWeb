using System.Net;
using System.Web.Mvc;
using AP.Business;
using AP.Models;
using AP.MVC.Filters;
using AP.MVC.Models;

namespace AP.MVC.Controllers
{
    [SessionAuthorize]
    public class ThreadController : Controller
    {
        private readonly ThreadBusiness _biz = new ThreadBusiness();

        public ActionResult Index(int? categoryId)
        {
            if (categoryId == null)
                return RedirectToAction("Index", "Category");

            ViewBag.CategoryId = categoryId.Value;
            var data = _biz.GetThreadsByCategory(categoryId.Value);
            return View(data);
        }

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

        public ActionResult Create(int? categoryId)
        {
            if (categoryId == null)
                return RedirectToAction("Index", "Category");

            ViewBag.CategoryId = categoryId.Value;

            var model = new Thread
            {
                CategoryId = categoryId.Value
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Thread model)
        {
            ViewBag.CategoryId = model.CategoryId;

            if (model.CategoryId <= 0)
                ModelState.AddModelError("CategoryId", "La categoría es obligatoria.");

            if (!ModelState.IsValid)
                return View(model);

            model.UserId = (int)Session["UserId"];
            _biz.CreateThread(model);

            return RedirectToAction("Index", new { categoryId = model.CategoryId });
        }
    }
}
