using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
