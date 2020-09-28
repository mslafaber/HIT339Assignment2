using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InhouseMembership.Models
{
    public class Schedule
    {

       


            public string ScheduleId { get; set; }


            public string CoachId { get; set; }

            public List<Enrollment> Enrollments { get; set; }

            [Required(ErrorMessage = "Please Enter the Event Name")]
            [Display(Name = "Event Name")]
            [StringLength(50)]
            public string EventName { get; set; }

            [Required(ErrorMessage = "Please Enter the Event Date")]
            [DataType(DataType.DateTime)]
            [Display(Name = "Event Date")]
            public DateTime EventDate { get; set; }

            [Required(ErrorMessage = "Please Enter the Event Location")]
            [Display(Name = "Event Location")]
            [StringLength(100)]
            public string Location { get; set; }

            //Coach model is used here since every event schedule is run by a coach
            //public Coach Coach { get; set; }


        
    }
}
