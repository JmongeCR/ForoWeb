using System.Web.Mvc;
using AP.Business;
using AP.MVC.Models;

namespace AP.MVC.Controllers
{
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
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                PhotoUrl = string.IsNullOrWhiteSpace(user.PhotoUrl)
                    ? "~/Content/img/default-avatar.png"
                    : user.PhotoUrl
            };

            return View(vm);
        }
    }
}