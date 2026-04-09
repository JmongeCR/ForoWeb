using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AP.Business;
using AP.Models;
using AP.MVC.Filters;

namespace AP.MVC.Controllers
{
    [SessionAuthorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();

        private List<SelectListItem> UserFieldOptions() => new List<SelectListItem>
        {
            new SelectListItem { Value = "Todos",  Text = "Todos los campos" },
            new SelectListItem { Value = "Nombre", Text = "Nombre" },
            new SelectListItem { Value = "Correo", Text = "Correo" },
            new SelectListItem { Value = "Rol",    Text = "Rol" }
        };

        public ActionResult Users()
        {
            ViewBag.CurrentCriteria = "";
            ViewBag.CurrentField    = "Todos";
            ViewBag.FieldOptions    = UserFieldOptions();
            var users = _userBusiness.GetUsers();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchUsers(string criteria, string field)
        {
            var users = _userBusiness.GetUsers();

            if (!string.IsNullOrWhiteSpace(criteria))
            {
                var q = criteria.Trim().ToLower();
                switch (field)
                {
                    case "Nombre":
                        users = users.Where(u => u.FullName.ToLower().Contains(q)).ToList();
                        break;
                    case "Correo":
                        users = users.Where(u => u.Email.ToLower().Contains(q)).ToList();
                        break;
                    case "Rol":
                        users = users.Where(u => u.Role.ToLower().Contains(q)).ToList();
                        break;
                    default:
                        users = users.Where(u =>
                            u.FullName.ToLower().Contains(q) ||
                            u.Email.ToLower().Contains(q) ||
                            u.Role.ToLower().Contains(q)).ToList();
                        break;
                }
            }

            ViewBag.CurrentCriteria = criteria;
            ViewBag.CurrentField    = field;
            ViewBag.FieldOptions    = UserFieldOptions();
            return View("Users", users);
        }

        public ActionResult CreateUser()
        {
            return View(new User { IsActive = true, Role = "Estudiante" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User model)
        {
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                model.PasswordHash = "123456";

            model.PhotoUrl = "~/Content/img/default-avatar.png";

            if (!ModelState.IsValid)
                return View(model);

            if (_userBusiness.EmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "Ya existe un usuario registrado con ese correo.");
                return View(model);
            }

            _userBusiness.CreateUser(model);
            return RedirectToAction("Users");
        }

        public ActionResult EditUser(int id)
        {
            var user = _userBusiness.GetUserById(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User model)
        {
            var currentUser = _userBusiness.GetUserById(model.UserId);
            if (currentUser == null)
                return HttpNotFound();

            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                model.PasswordHash = currentUser.PasswordHash;

            if (string.IsNullOrWhiteSpace(model.PhotoUrl))
                model.PhotoUrl = currentUser.PhotoUrl;

            if (!ModelState.IsValid)
                return View(model);

            if (_userBusiness.EmailExists(model.Email, model.UserId))
            {
                ModelState.AddModelError("Email", "Ya existe otro usuario registrado con ese correo.");
                return View(model);
            }

            _userBusiness.UpdateUser(model);

            if (Session["UserId"] != null && (int)Session["UserId"] == model.UserId)
            {
                Session["UserName"] = model.FullName;
                Session["UserEmail"] = model.Email;
                Session["UserRole"] = model.Role;
                Session["UserPhotoUrl"] = string.IsNullOrWhiteSpace(model.PhotoUrl)
                    ? "~/Content/img/default-avatar.png"
                    : model.PhotoUrl;
            }

            return RedirectToAction("Users");
        }
    }
}
