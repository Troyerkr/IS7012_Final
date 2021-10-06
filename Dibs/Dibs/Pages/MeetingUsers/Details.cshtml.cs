using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dibs.Data;
using Dibs.Models;

namespace Dibs.Pages.MeetingUsers
{
    public class DetailsModel : PageModel
    {
        private readonly Dibs.Data.DibsContext _context;

        public DetailsModel(Dibs.Data.DibsContext context)
        {
            _context = context;
        }

        public MeetingUser MeetingUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MeetingUser = await _context.MeetingUser
                .Include(x => x.Attendees).Include(x => x.Meetings)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (MeetingUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
