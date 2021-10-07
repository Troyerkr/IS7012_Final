using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dibs.Models
{
    public class Room
    {
        public int Id { get; set; }

        [DisplayName("Room Number")]
        [Required]
        
        public string RoomNum { get; set; }

        [Required]
        public int Capacity { get; set; }

        public List<Meeting> Meetings { get; set; }

    }
}
