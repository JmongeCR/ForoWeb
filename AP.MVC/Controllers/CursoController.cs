using AP.Business;
using AP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AP.MVC.Controllers
{
    public class CursoController : Controller
    {
        private readonly CourseBusiness _business = new CourseBusiness();

        private bool IsAdmin() => Session["UserRole"]?.ToString() == "Administrador";

        private List<SelectListItem> CursoFieldOptions() => new List<SelectListItem>
        {
            new SelectListItem { Value = "Todos",       Text = "Todos los campos" },
            new SelectListItem { Value = "Nombre",      Text = "Nombre" },
            new SelectListItem { Value = "Descripcion", Text = "Descripcion" }
        };

        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            ViewBag.CurrentCriteria = "";
            ViewBag.CurrentField    = "Todos";
            ViewBag.FieldOptions    = CursoFieldOptions();
            return View(_business.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string criteria, string field)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var cursos = _business.GetAll();

            if (!string.IsNullOrWhiteSpace(criteria))
            {
                var q = criteria.Trim().ToLower();
                switch (field)
                {
                    case "Nombre":
                        cursos = cursos.Where(c => c.Name.ToLower().Contains(q)).ToList();
                        break;
                    case "Descripcion":
                        cursos = cursos.Where(c => c.Description != null && c.Description.ToLower().Contains(q)).ToList();
                        break;
                    default:
                        cursos = cursos.Where(c =>
                            c.Name.ToLower().Contains(q) ||
                            (c.Description != null && c.Description.ToLower().Contains(q))).ToList();
                        break;
                }
            }

            ViewBag.CurrentCriteria = criteria;
            ViewBag.CurrentField    = field;
            ViewBag.FieldOptions    = CursoFieldOptions();
            return View("Index", cursos);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, string description)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "El nombre del curso es obligatorio.";
                return View(new Course { Name = name, Description = description });
            }

            _business.Create(new Course { Name = name.Trim(), Description = description?.Trim() });
            TempData["Success"] = "Curso creado exitosamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var curso = _business.GetById(id);
            if (curso == null) return HttpNotFound();
            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string name, string description)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "El nombre del curso es obligatorio.";
                return View(new Course { CourseId = id, Name = name, Description = description });
            }

            _business.Update(new Course { CourseId = id, Name = name.Trim(), Description = description?.Trim() });
            TempData["Success"] = "Curso actualizado.";
            return RedirectToAction("Index");
        }
    }
}
