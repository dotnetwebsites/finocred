using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace finocred.web.Models
{
    public class ContactUs
    {
        [Display(Name = "First Name")]
        [MaxLength(100)]
        [Required(ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name required")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Required(ErrorMessage = "Mobile no required")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Mobile no must be 10-digit without prefix")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile no must be numeric")]
        [Display(Name = "Mobile No")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Subject required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message required")]
        public string Message { get; set; }
    }
}
