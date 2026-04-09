using AP.Business;
using AP.Models;
using AP.MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AP.MVC.Controllers
{
    public class ClaseController : Controller
    {
        private readonly ClaseBusiness _claseBusiness = new ClaseBusiness();
        private readonly CourseBusiness _courseBusiness = new CourseBusiness();

        private bool IsAdmin() => Session["UserRole"]?.ToString() == "Administrador";

        private List<SelectListItem> ClaseFieldOptions() => new List<SelectListItem>
        {
            new SelectListItem { Value = "Todos",    Text = "Todos los campos" },
            new SelectListItem { Value = "Clase",    Text = "Clase" },
            new SelectListItem { Value = "Curso",    Text = "Curso" },
            new SelectListItem { Value = "Profesor", Text = "Profesor" }
        };

        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            ViewBag.CurrentCriteria = "";
            ViewBag.CurrentField    = "Todos";
            ViewBag.FieldOptions    = ClaseFieldOptions();
            return View(_claseBusiness.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string criteria, string field)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var clases = _claseBusiness.GetAll();

            if (!string.IsNullOrWhiteSpace(criteria))
            {
                var q = criteria.Trim().ToLower();
                switch (field)
                {
                    case "Clase":
                        clases = clases.Where(c => c.Name.ToLower().Contains(q)).ToList();
                        break;
                    case "Curso":
                        clases = clases.Where(c => c.CourseName != null && c.CourseName.ToLower().Contains(q)).ToList();
                        break;
                    case "Profesor":
                        clases = clases.Where(c => c.ProfessorName != null && c.ProfessorName.ToLower().Contains(q)).ToList();
                        break;
                    default:
                        clases = clases.Where(c =>
                            c.Name.ToLower().Contains(q) ||
                            (c.CourseName    != null && c.CourseName.ToLower().Contains(q)) ||
                            (c.ProfessorName != null && c.ProfessorName.ToLower().Contains(q))).ToList();
                        break;
                }
            }

            ViewBag.CurrentCriteria = criteria;
            ViewBag.CurrentField    = field;
            ViewBag.FieldOptions    = ClaseFieldOptions();
            return View("Index", clases);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var vm = new CreateClaseVM
            {
                Courses    = _courseBusiness.GetAll(),
                Professors = _claseBusiness.GetAllProfessors()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateClaseVM vm)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (string.IsNullOrWhiteSpace(vm.Name) || vm.CourseId == 0 || vm.ProfessorId == 0)
            {
                vm.Courses    = _courseBusiness.GetAll();
                vm.Professors = _claseBusiness.GetAllProfessors();
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View(vm);
            }

            _claseBusiness.Create(new Clase
            {
                Name        = vm.Name.Trim(),
                CourseId    = vm.CourseId,
                ProfessorId = vm.ProfessorId
            });

            TempData["Success"] = "Clase creada exitosamente.";
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var clase = _claseBusiness.GetById(id);
            if (clase == null) return HttpNotFound();

            var vm = new ClaseDetailsVM
            {
                Clase             = clase,
                Students          = _claseBusiness.GetStudents(id),
                AvailableStudents = _claseBusiness.GetAvailableStudents(id)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(int claseId, int studentId)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            _claseBusiness.AddStudent(claseId, studentId);
            TempData["Success"] = "Estudiante agregado a la clase.";
            return RedirectToAction("Details", new { id = claseId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveStudent(int claseId, int studentId)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            _claseBusiness.RemoveStudent(claseId, studentId);
            TempData["Success"] = "Estudiante eliminado de la clase.";
            return RedirectToAction("Details", new { id = claseId });
        }
    }
}
