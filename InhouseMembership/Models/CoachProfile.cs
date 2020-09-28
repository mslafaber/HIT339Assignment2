using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InhouseMembership.Models
{
    public class CoachProfile
    {
        public int CoachProfileId { get; set; }

        public string CoachId { get; set; }



        [Required(ErrorMessage = "Please Enter Your educatoin background")]
        [Display(Name = "Education")]
        [StringLength(160)]

        public string Education { get; set; }

        [Required(ErrorMessage = "Please Enter Your interests")]
        [Display(Name = "Interests")]
        [StringLength(160)]
        public string Interests { get; set; }

        [Required(ErrorMessage = "Please Enter Your work experience")]
        [Display(Name = "Work Experience")]
        [StringLength(160)]
        public string Experience { get; set; }

        [Required(ErrorMessage = "Please Enter Your skills")]
        [Display(Name = "Skills")]
        [StringLength(160)]
        public string Skills { get; set; }

        [Required(ErrorMessage = "Please write Your biography")]
        [Display(Name = "Biography")]
        [StringLength(800)]
        public string Biography { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }

        public ApplicationUser Coach { get; set; }
    }
}
