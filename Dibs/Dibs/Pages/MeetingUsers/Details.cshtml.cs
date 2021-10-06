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
        public List<Room> AllRooms { get; set; }
        public List<Meeting> AllMeetings { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MeetingUser = await _context.MeetingUser
                .Include(m => m.Attendees).Include(m => m.Meetings)
                .FirstOrDefaultAsync(m => m.Id == id);

            AllMeetings = _context.Meeting.ToList();
            AllRooms = _context.Room.ToList();

            if (MeetingUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
