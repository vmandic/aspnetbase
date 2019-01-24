using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Authorization;

namespace AspNetBase.Presentation.Server.Pages.ManageUsers
{
    public class IndexModel : PageModel
    {
        private readonly AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext _context;

        public IndexModel(AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext context)
        {
            _context = context;
        }

        public IList<AppUser> AppUser { get;set; }

        public async Task OnGetAsync()
        {
            AppUser = await _context.Users.ToListAsync();
        }
    }
}
