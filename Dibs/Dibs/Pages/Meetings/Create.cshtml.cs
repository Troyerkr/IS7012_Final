using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dibs.Data;
using Dibs.Models;

namespace Dibs.Pages.Meetings
{
    public class CreateModel : PageModel
    {
        private readonly Dibs.Data.DibsContext _context;

        public CreateModel(Dibs.Data.DibsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["MeetingUserId"] = new SelectList(_context.MeetingUser, "Id", "FullName");
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "RoomNum");
            return Page();
        }

        [BindProperty]
        public Meeting Meeting { get; set; }

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
            if(InvitesRequested + 1 > RoomCapacity)
            {
                ModelState.AddModelError("Meeting.NumOfInvites", "Number of invites exceeds room capacity");
            }

            //Meeting must be in the future
            DateTime RequestedDate = Meeting.MeetDate.Date;
            DateTime CurrentDate = DateTime.Now.Date;
            if(RequestedDate < CurrentDate)
            {
                ModelState.AddModelError("Meeting.MeetDate", "Meeting must be in the future");
            }


            //No user is hosting more than one meeting on the same day
            int ThisHostId = Meeting.MeetingUserId;
            var AllHostIds = _context.Meeting.Where(h => h.MeetingUserId == ThisHostId).ToList();
            foreach(Meeting Host in AllHostIds)
            {
                if(Host.MeetDate == RequestedDate)
                {
                    ModelState.AddModelError("Meeting.MeetDate", "You have a meeting already scheduled for this date");
                }
            }

            //No two meetings are in the same room on the same day
            int ThisRoomId = Meeting.RoomId;
            DateTime ThisMeetDate = Meeting.MeetDate;
            var AllMeetings = _context.Meeting.ToList();
            foreach(Meeting AnyMeeting in AllMeetings)
            {
                if (ThisRoomId == AnyMeeting.RoomId && ThisMeetDate == AnyMeeting.MeetDate)
                {
                    ModelState.AddModelError("Meeting.MeetDate", "This room is already booked for this date");
                }
            }

            //THEN RE-VALIDATE
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Meeting.Add(Meeting);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Invite", new {id = Meeting.Id });
        }
    }
}
