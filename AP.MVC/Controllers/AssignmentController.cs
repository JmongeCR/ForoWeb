using AP.Business;
using AP.Models;
using AP.MVC.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AP.MVC.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly AssignmentBusiness _biz = new AssignmentBusiness();
        private readonly NotificationBusiness _notifBiz = new NotificationBusiness();

        private bool IsLoggedIn() => Session["UserId"] != null;
        private bool IsProfesor() => Session["UserRole"]?.ToString() == "Profesor";
        private bool IsEstudiante() => Session["UserRole"]?.ToString() == "Estudiante";
        private bool IsAdmin() => Session["UserRole"]?.ToString() == "Administrador";
        private int CurrentUserId() => Convert.ToInt32(Session["UserId"]);

        public ActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Access");

            if (IsProfesor())
            {
                ViewBag.IsProfesor = true;
                return View(_biz.GetByProfessor(CurrentUserId()));
            }
            else if (IsAdmin())
            {
                ViewBag.IsProfesor = true;
                return View(_biz.GetByProfessor(CurrentUserId()));
            }
            else
            {
                ViewBag.IsProfesor = false;
                return View(_biz.GetByStudent(CurrentUserId()));
            }
        }

        public ActionResult Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Access");
            if (!IsProfesor() && !IsAdmin())
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            var vm = new CreateAssignmentVM
            {
                ProfessorClases = _biz.GetProfessorClases(CurrentUserId()),
                DueDate         = DateTime.Today.AddDays(7)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAssignmentVM vm)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Access");
            if (!IsProfesor() && !IsAdmin())
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            if (!ModelState.IsValid || vm.ClaseId == 0)
            {
                vm.ProfessorClases = _biz.GetProfessorClases(CurrentUserId());
                return View(vm);
            }

            var assignment = new Assignment
            {
                ProfessorId = CurrentUserId(),
                ClassId     = vm.ClaseId,
                Title       = vm.Title,
                Description = vm.Description,
                DueDate     = vm.DueDate
            };

            int newId = _biz.Create(assignment);

            // Notifica a cada estudiante inscrito en la clase
            var students = _biz.GetAssignedStudents(newId);
            foreach (var s in students)
            {
                _notifBiz.Notify(
                    s.UserId,
                    $"Nueva tarea: {assignment.Title}",
                    $"/Assignment/Details/{newId}"
                );
            }

            TempData["Success"] = "Tarea creada exitosamente.";
            return RedirectToAction("Details", new { id = newId });
        }

        public ActionResult Details(int? id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Access");
            if (id == null) return RedirectToAction("Index");

            var assignment = _biz.GetById(id.Value);
            if (assignment == null) return HttpNotFound();

            int userId = CurrentUserId();

            if (IsProfesor() && assignment.ProfessorId != userId)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            if (IsEstudiante() && !_biz.IsStudentAssigned(id.Value, userId))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            var vm = new AssignmentDetailsVM
            {
                Assignment       = assignment,
                AssignedStudents = _biz.GetAssignedStudents(id.Value)
            };

            if (IsProfesor() || IsAdmin())
                vm.Submissions = _biz.GetSubmissions(id.Value);
            else
                vm.MySubmission = _biz.GetMySubmission(userId, id.Value);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(int assignmentId, string comment, HttpPostedFileBase file)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Access");
            if (!IsEstudiante())
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            int studentId = CurrentUserId();

            if (!_biz.IsStudentAssigned(assignmentId, studentId))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            string fileUrl = null;

            if (file != null && file.ContentLength > 0)
            {
                string uploadsDir = Server.MapPath("~/Uploads/Submissions/");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                string ext      = Path.GetExtension(file.FileName);
                string fileName = $"{assignmentId}_{studentId}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                file.SaveAs(Path.Combine(uploadsDir, fileName));
                fileUrl = $"~/Uploads/Submissions/{fileName}";
            }

            _biz.Submit(new Submission
            {
                AssignmentId = assignmentId,
                StudentId    = studentId,
                FileUrl      = fileUrl,
                Comment      = comment
            });

            // Notifica al profesor que un estudiante entrego la tarea
            var assignment = _biz.GetById(assignmentId);
            if (assignment != null)
            {
                string studentName = Session["UserName"]?.ToString() ?? "Un estudiante";
                _notifBiz.Notify(
                    assignment.ProfessorId,
                    $"{studentName} entrego la tarea: {assignment.Title}",
                    $"/Assignment/Details/{assignmentId}"
                );
            }

            TempData["Success"] = "Tu entrega fue registrada correctamente.";
            return RedirectToAction("Details", new { id = assignmentId });
        }
    }
}
