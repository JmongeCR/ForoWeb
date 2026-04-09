using System.Web.Mvc;
using AP.Business;

namespace AP.MVC.Controllers
{
    public class AccessController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            email = (email ?? string.Empty).Trim();
            password = password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Debés ingresar el correo y la contraseña.";
                return View();
            }

            var user = _userBusiness.ValidateLogin(email, password);

            if (user == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos, o el usuario está inactivo.";
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
