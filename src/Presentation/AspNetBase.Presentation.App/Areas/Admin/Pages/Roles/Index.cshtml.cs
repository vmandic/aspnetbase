using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Presentation.App.Pages.ManageRoles
{
  public class IndexModel : PageModel
  {
    private readonly AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext _context;

    public IndexModel(AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext context)
    {
      _context = context;
    }

    public IQueryable<AppRole> Roles { get; set; }

    public void OnGet()
    {
      Roles = _context.Roles;
    }
  }
}
