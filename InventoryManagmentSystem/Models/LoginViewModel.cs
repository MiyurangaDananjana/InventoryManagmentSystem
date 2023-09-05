using System.ComponentModel.DataAnnotations;

namespace InventoryManagmentSystem.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "EPF Number is required.")]
        public string EpfNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
