using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fecomvr1._2.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Usuario"] = User.Identity?.Name ?? "Usuario";
            ViewData["Empresa"] = "FECOM S.A.C.";
            ViewData["Rol"] = User.IsInRole("Admin") ? "Administrador" : "Usuario";

            return View();
        }
    }
}
