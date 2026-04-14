using System.IO;
using System.Web;
using System.Web.Mvc;
using AP.Business;
using AP.Models;
using AP.MVC.Models;
using System.Collections.Generic;

namespace AP.MVC.Controllers
{
    // SOLID: SRP - separamos ver el perfil (Index) de actualizar la foto (UpdatePhoto)
    // para que cada metodo tenga una sola responsabilidad
    // SOLID: DIP - usamos UserBusiness y ClaseBusiness, no los repos directamente
    public class ProfiledController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();
        private readonly ClaseBusiness _claseBusiness = new ClaseBusiness();

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Access");

            int userId = (int)Session["UserId"];
            var user = _userBusiness.GetUserById(userId);

            if (user == null)
                return RedirectToAction("Login", "Access");

            List<Clase> clases;
            if (user.Role == "Estudiante")
                clases = _claseBusiness.GetByStudent(userId);
            else if (user.Role == "Profesor")
                clases = _claseBusiness.GetByProfessor(userId);
            else
                clases = new List<Clase>();

            var vm = new ProfileVM
            {
                UserId   = user.UserId,
                FullName = user.FullName,
                Email    = user.Email,
                Role     = user.Role,
                PhotoUrl = string.IsNullOrWhiteSpace(user.PhotoUrl)
                    ? "~/Content/img/default-avatar.png"
                    : user.PhotoUrl,
                Clases   = clases
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
