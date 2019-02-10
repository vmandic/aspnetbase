using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.Enums;
using AspNetBase.Presentation.App.Controllers.Base;
using AspNetBase.Presentation.App.Models;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBase.Presentation.App.Controllers
{
  public class HomeController : BaseController
  {
    private readonly IEmailSender emailSender;
    private readonly UserManager<AppUser> userManager;

    public HomeController(UserManager<AppUser> userManager, IEmailSender emailSender)
    {
      this.emailSender = emailSender;
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

    public IActionResult TestRiseError()
    {
      HttpContext.RiseError(new Exception("Test error 1.", new Exception("This is an inner exception 1.")));
      return RedirectToAction(nameof(Index));
    }

    public IActionResult TestThrowEx()
    {
      throw new Exception("Test error 2.", new Exception("This is an inner exception 2."));
    }

    public async Task<IActionResult> TestEmail(string message = null)
    {
      await emailSender.SendEmailAsync(
        "mandic.vedran@gmail.com",
        "test msg - " + DateTime.Now,
        message ?? "Test message 123.");

      return RedirectToAction(nameof(Index), new { mailSent = "YES" });
    }
  }
}
