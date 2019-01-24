using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.Enums;
using AspNetBase.Presentation.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBase.Presentation.Server.Controllers
{
  public class HomeController : Controller
  {
    private readonly UserManager<AppUser> userManager;

    public HomeController(UserManager<AppUser> userManager)
    {
      this.userManager = userManager;
    }

    public IActionResult Index()
    {
      return View();
    }

    [Authorize]
    public async Task<IActionResult> About()
    {
      ViewData["Message"] = "Your application description page.";

      var user = await userManager.GetUserAsync(User);
      ViewBag.IsInAdmin = await userManager.IsInRoleAsync(user, Roles.SystemAdministrator.ToString()) ? "YES" : "NO";

      return View();
    }

    public IActionResult Contact()
    {
      ViewData["Message"] = "Your contact page.";

      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
