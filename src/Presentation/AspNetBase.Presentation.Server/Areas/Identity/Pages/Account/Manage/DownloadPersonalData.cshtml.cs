using System.Text;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity.AccountManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
  {
    private readonly IManageAccountService _manageAccountService;

    public DownloadPersonalDataModel(IManageAccountService manageAccountService)
    {
      _manageAccountService = manageAccountService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var personalData = await _manageAccountService.GetPersonalData(User);

      Response.Headers.Add(
        "Content-Disposition",
        "attachment; filename=PersonalData.json");

      return new FileContentResult(
        Encoding.UTF8.GetBytes(
          JsonConvert.SerializeObject(personalData)), "text/json");
    }
  }
}
