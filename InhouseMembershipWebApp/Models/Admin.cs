using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InhouseMembershipWebApp.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [Display(Name = "First Name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(60)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email Address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
