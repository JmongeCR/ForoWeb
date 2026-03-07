using System;
using System.Web.Mvc;
using AP.Business;
using AP.Models;

namespace AP.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();

        private bool IsAdmin()
        {
            return Session["UserRole"] != null &&
                   Session["UserRole"].ToString() == "Administrador";
        }

        public ActionResult Users()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var users = _userBusiness.GetUsers();
            return View(users);
        }

        public ActionResult CreateUser()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

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
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrWhiteSpace(model.PasswordHash))
                model.PasswordHash = "123456";

            if (string.IsNullOrWhiteSpace(model.PhotoUrl))
                model.PhotoUrl = "~/Content/img/default-avatar.png";

            _userBusiness.CreateUser(model);
            return RedirectToAction("Users");
        }

        public ActionResult EditUser(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            var user = _userBusiness.GetUserById(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User model)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(model);

            _userBusiness.UpdateUser(model);
            return RedirectToAction("Users");
        }
    }
}