using ShowTime.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShowTime.Core.Models
{
    public class UserRegistrationRequest
    {
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender can't be blank")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number can't be blank")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.Employee;

        [Required(ErrorMessage = "Job Role can't be blank")]
        public string? JobRole { get; set; }
    }
}
