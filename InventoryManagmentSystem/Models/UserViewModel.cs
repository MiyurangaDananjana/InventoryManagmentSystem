using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace InventoryManagmentSystem.Models
{
    public class UserViewModel
    {
       
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "UserName should only contain letters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "EPF Number is required.")]
        public string EPFNumber { get; set; }

        [Required(ErrorMessage = "NIC Number is required.")]
        public string Nic { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Required(ErrorMessage = "Confirm Password is required.")]
        public string ConfirmPassword { get; set; }

        public HttpPostedFileBase Profile { get; set; }
    }
}
