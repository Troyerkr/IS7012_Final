using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dibs.Models
{
    public class Meeting
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Meeting Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Meeting Day")]
        [DataType(DataType.Date)]
        public DateTime MeetDate { get; set; }

        [Required]
        [DisplayName("Number of People Invited to the Meeting")]
        public int NumOfInvites { get; set; }


        [DisplayName("Meeting Notes")]
        public string Notes { get; set; }
       
        //Meeting To MeetingUser
        [DisplayName("Host Email")]
        public MeetingUser MeetingUser { get; set; }
        [DisplayName("Host Email")]
        public int MeetingUserId { get; set; }

        //Meeting to Attendee
        public List<Attendee> Attendees { get; set; }

        //Meeting to Room
        [DisplayName("Room Number")]
        public Room Room { get; set; }
        [DisplayName("Room Number")]
        public int RoomId { get; set; }

    }
}
