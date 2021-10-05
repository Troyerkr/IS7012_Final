using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dibs.Data;
using Dibs.Models;

namespace Dibs.Pages.Meetings
{
    public class IndexModel : PageModel
    {
        private readonly Dibs.Data.DibsContext _context;

        public IndexModel(Dibs.Data.DibsContext context)
        {
            _context = context;
        }

        public IList<Meeting> Meeting { get;set; }

        public async Task OnGetAsync()
        {
            Meeting = await _context.Meeting
                .Include(m => m.MeetingUser).ToListAsync();
            Meeting = await _context.Meeting
                .Include(m => m.Room).ToListAsync();
        }
    }
}
