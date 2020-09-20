using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InhouseMembershipWebApp.Models
{
    public class Schedule
    {
        public int scheduleId { get; set; }

        [Required(ErrorMessage = "Please Enter the Event Name")]
        [Display(Name = "Event Name")]
        [StringLength(50)]
        public string eventname { get; set; }

        [Required(ErrorMessage = "Please Enter the Event Date")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Event Date")]
        public string eventdate { get; set; }

        [Required(ErrorMessage = "Please Enter the Event Location")]
        [Display(Name = "Event Location")]
        [StringLength(100)]
        public string location { get; set; }

        //Coach model is used here since every event schedule is run by a coach
        //public Coach Coach { get; set; }
    }
}
