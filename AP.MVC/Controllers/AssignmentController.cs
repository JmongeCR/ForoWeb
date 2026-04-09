using System;
using System.Web.Mvc;
using AP.Business;
using AP.Data;
using AP.Models;
using AP.MVC.Filters;

namespace AP.MVC.Controllers
{
    [SessionAuthorize]
    public class AssignmentController : Controller
    {
        private readonly AssignmentBusiness _assignmentBusiness = new AssignmentBusiness();
        private readonly CategoryRepository _categoryRepository = new CategoryRepository();

        public ActionResult Index()
        {
            var data = _assignmentBusiness.GetAssignments();
            return View(data);
        }

        public ActionResult Details(int id)
        {
            var assignment = _assignmentBusiness.GetAssignmentById(id);
            if (assignment == null)
                return HttpNotFound();

            return View(assignment);
        }

        [SessionAuthorize(Roles = "Administrador,Profesor")]
        public ActionResult Create()
        {
            LoadCategories();
            return View(new Assignment
            {
                DueDate = DateTime.Today.AddDays(7)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize(Roles = "Administrador,Profesor")]
        public ActionResult Create(Assignment model)
        {
            LoadCategories();

            if (model.DueDate.Date < DateTime.Today)
                ModelState.AddModelError("DueDate", "La fecha de entrega no puede ser anterior a hoy.");

            if (!ModelState.IsValid)
                return View(model);

            model.TeacherId = (int)Session["UserId"];
            _assignmentBusiness.CreateAssignment(model);
            TempData["Success"] = "La tarea se registró correctamente.";
            return RedirectToAction("Index");
        }

        private void LoadCategories()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
        }
    }
}
