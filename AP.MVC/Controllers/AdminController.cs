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

        public ActionResult Users()
        {
            var users = _userBusiness.GetUsers();
            return View(users);
        }

        public ActionResult CreateUser()
        {
            return View(new User
            {
                IsActive = true,
                Role = "Estudiante",
                PhotoUrl = "~/Content/img/default-avatar.png"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User model)
        {
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                model.PasswordHash = "123456";

            if (string.IsNullOrWhiteSpace(model.PhotoUrl))
                model.PhotoUrl = "~/Content/img/default-avatar.png";

            if (!ModelState.IsValid)
                return View(model);

            if (_userBusiness.EmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "Ya existe un usuario registrado con ese correo.");
                return View(model);
            }

            _userBusiness.CreateUser(model);
            TempData["Success"] = "Usuario creado correctamente.";
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

            TempData["Success"] = "Usuario actualizado correctamente.";
            return RedirectToAction("Users");
        }
    }
}
