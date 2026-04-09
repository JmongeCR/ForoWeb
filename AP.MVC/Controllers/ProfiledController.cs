using System.IO;
using System.Web;
using System.Web.Mvc;
using AP.Business;
using AP.Models;
using AP.MVC.Models;

namespace AP.MVC.Controllers
{
    // SOLID: Single Responsibility Principle (SRP) - gestiona unicamente el perfil del usuario
    //        autenticado: visualizacion y actualizacion de foto de avatar.
    // DP: Strategy Pattern (implicito) - la logica de guardado de imagen esta encapsulada
    //     en UpdatePhoto, separada de la logica de visualizacion en Index.
    public class ProfiledController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Access");

            int userId = (int)Session["UserId"];
            var user = _userBusiness.GetUserById(userId);

            if (user == null)
                return RedirectToAction("Login", "Access");

            var vm = new ProfileVM
            {
                UserId   = user.UserId,
                FullName = user.FullName,
                Email    = user.Email,
                Role     = user.Role,
                PhotoUrl = string.IsNullOrWhiteSpace(user.PhotoUrl)
                    ? "~/Content/img/default-avatar.png"
                    : user.PhotoUrl
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePhoto(HttpPostedFileBase photo)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Access");

            int userId = (int)Session["UserId"];

            if (photo != null && photo.ContentLength > 0)
            {
                var ext     = Path.GetExtension(photo.FileName).ToLower();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                if (System.Array.IndexOf(allowed, ext) >= 0)
                {
                    var fileName = "avatar_" + userId + ext;
                    var folder   = Server.MapPath("~/Content/img/");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    photo.SaveAs(Path.Combine(folder, fileName));

                    var user = _userBusiness.GetUserById(userId);
                    user.PhotoUrl = "~/Content/img/" + fileName;
                    _userBusiness.UpdateUser(user);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
