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

namespace Dibs.Pages.MeetingUsers
{
    public class RSVPModel : PageModel
    {
        private readonly Dibs.Data.DibsContext _context;

        public RSVPModel(Dibs.Data.DibsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Attendee Attendee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Attendee = await _context.Attendee
                .Include(a => a.Meeting)
                .Include(a => a.MeetingUser).FirstOrDefaultAsync(m => m.Id == id);

            if (Attendee == null)
            {
                return NotFound();
            }
           ViewData["MeetingId"] = new SelectList(_context.Meeting, "Id", "Title");
           ViewData["MeetingUserId"] = new SelectList(_context.MeetingUser, "Id", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Attendee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendeeExists(Attendee.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            
            return RedirectToPage("./Details", new { id = Attendee.MeetingUserId });
        }

        private bool AttendeeExists(int id)
        {
            return _context.Attendee.Any(e => e.Id == id);
        }
    }
}
