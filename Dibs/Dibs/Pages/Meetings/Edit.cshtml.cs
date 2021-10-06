using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dibs.Data;
using Dibs.Models;

namespace Dibs.Pages.Meetings
{
    public class EditModel : PageModel
    {
        private readonly Dibs.Data.DibsContext _context;

        public EditModel(Dibs.Data.DibsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Meeting Meeting { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Meeting = await _context.Meeting
                .Include(m => m.MeetingUser)
                .Include(m => m.Room).FirstOrDefaultAsync(m => m.Id == id);

            if (Meeting == null)
            {
                return NotFound();
            }
           ViewData["MeetingUserId"] = new SelectList(_context.MeetingUser, "Id", "Email");
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "RoomNum");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["MeetingUserId"] = new SelectList(_context.MeetingUser, "Id", "Email");
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "RoomNum");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Number of invites can not exceed room capacity
            var GetRoomCapacity = _context.Room.Where(r => r.Id == Meeting.RoomId).Select(x => x.Capacity).ToList();
            int RoomCapacity = GetRoomCapacity[0];
            int InvitesRequested = Meeting.NumOfInvites;
            if (InvitesRequested + 1 > RoomCapacity)
            {
                ModelState.AddModelError("Meeting.NumOfInvites", "Number of invites exceeds room capacity");
            }

            //No two meetings are in the same room on the same day -TODO

            //Meeting must be in the future
            DateTime RequestedDate = Meeting.MeetDate.Date;
            DateTime CurrentDate = DateTime.Now.Date;
            if (RequestedDate < CurrentDate)
            {
                ModelState.AddModelError("Meeting.MeetDate", "Meeting must be in the future");
            }


            //No user is hosting more than one meeting on the same day -TODO

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Meeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(Meeting.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MeetingExists(int id)
        {
            return _context.Meeting.Any(e => e.Id == id);
        }
    }
}
