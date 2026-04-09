using System.Web.Mvc;
using AP.Business;
using AP.MVC.Filters;
using AP.MVC.Models;

namespace AP.MVC.Controllers
{
    [SessionAuthorize]
    public class ProfiledController : Controller
    {
        private readonly UserBusiness _userBusiness = new UserBusiness();

        public ActionResult Index()
        {
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
