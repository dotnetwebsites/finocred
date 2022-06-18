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
        [Required(ErrorMessage = "Required First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Required Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Required Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Required(ErrorMessage = "Required mobile no")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Mobile no must be 10-digit without prefix")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile no must be numeric")]
        [Display(Name = "Mobile No")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Required message")]
        public string Message { get; set; }
    }
}
