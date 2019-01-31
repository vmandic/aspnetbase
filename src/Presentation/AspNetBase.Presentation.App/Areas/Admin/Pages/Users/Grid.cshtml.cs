using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Presentation.App.Pages.ManageUsers
{
  public class GridModel : PageModel
  {
    private readonly AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext _context;

    public GridModel(AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext context)
    {
      _context = context;
    }

    public IQueryable<AppUser> Users { get; set; }

    public void OnGet()
    {
      Users = _context.Users;
    }
  }
}
