using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;

namespace AspNetBase.Presentation.Server.Pages.ManageUsers
{
    public class DetailsModel : PageModel
    {
        private readonly AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext _context;

        public DetailsModel(AspNetBase.Infrastructure.DataAccess.EntityFramework.AppDbContext context)
        {
            _context = context;
        }

        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (AppUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
