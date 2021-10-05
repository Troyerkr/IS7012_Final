using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dibs.Models
{
    public class Attendee
    {
        //note: I got an error when trying to create the database I found a potential fix here:
        //https://entityframeworkcore.com/knowledge-base/52268985/may-cause-cycles-or-multiple-cascade-paths--specify-on-delete-no-action-or-on-update-no-action--or-modify-other-foreign-key-constraints

        public int Id { get; set; }

        //MeetingUser to Attendee
        [DisplayName("Attendee Email")]
        public MeetingUser MeetingUser { get; set; }
        [DisplayName("Attendee Email")]
        public int? MeetingUserId { get; set; }
        //See note for why it is nullable

        //MeetingUser to Meeting
        [DisplayName("Meeting Title")]
        public Meeting Meeting { get; set; }
        [DisplayName("Meeting Title")]
        public int? MeetingId { get; set; }
        //See note for why it is nullable

        [DisplayName("Attending")]
        [DefaultValue(false)]
        public bool IsAttending { get; set; }

        [StringLength(90)]
        public string Message { get; set; }




    }
}
