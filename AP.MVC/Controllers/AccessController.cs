using System;
using System.Web.Mvc;
using AP.Business;

namespace AP.MVC.Controllers
{
    // SOLID: SRP - este controller solo se encarga del login y logout, nada mas
    // SOLID: DIP - usamos UserBusiness en vez del repositorio directamente
    public class AccessController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var user = _userBusiness.ValidateLogin(email, password);

            if (user == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            Session["UserId"] = user.UserId;
            Session["UserName"] = user.FullName;
            Session["UserEmail"] = user.Email;
            Session["UserRole"] = user.Role;
            Session["UserPhotoUrl"] = string.IsNullOrWhiteSpace(user.PhotoUrl)
                ? "~/Content/img/default-avatar.png"
                : user.PhotoUrl;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Access");
        }
    }
}