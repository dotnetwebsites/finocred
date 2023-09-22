using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace finocred.web.BusinessLayer.DTOs
{
    public class ApplyDTO
    {
        [Required(ErrorMessage = "Please enter full name")]
        [Display(Name = "Full Name", Prompt = "Full Name*")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter mobile no")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Mobile no must be 10-digit without prefix")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile no must be numeric")]
        [Display(Name = "Mobile No", Prompt = "Mobile Number*")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please enter 6 area pincode")]
        [MaxLength(6, ErrorMessage = "6 digit area pincode required")]
        [MinLength(6, ErrorMessage = "6 digit area pincode required")]
        [Display(Name = "Pincode", Prompt = "Pincode*")]
        public string Pincode { get; set; }


        [Required(ErrorMessage = "Please select apply for")]
        public ApplyFor? ApplyFor { get; set; }
    }
}
