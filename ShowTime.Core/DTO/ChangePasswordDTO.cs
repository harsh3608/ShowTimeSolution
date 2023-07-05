using System.ComponentModel.DataAnnotations;


namespace ShowTime.Core.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? CurrentPassword { get; set; }

        [Required]
        public string? NewPassword { get; set; }
    }
}
