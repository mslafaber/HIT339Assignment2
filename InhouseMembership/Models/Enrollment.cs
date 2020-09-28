using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InhouseMembership.Models
{
    public class Enrollment
    {
        public string EnrollmentId { get; set; }

        public string ScheduleId { get; set; }

        public string MemberId { get; set; }

        public Schedule Schedule { get; set; }

        public ApplicationUser Member { get; set; }
    }
}