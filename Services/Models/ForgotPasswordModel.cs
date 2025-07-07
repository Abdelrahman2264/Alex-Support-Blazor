using System.ComponentModel.DataAnnotations;

namespace AlexSupport.Services.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@alpinecreations\.com$",
            ErrorMessage = "Only company email addresses are allowed")]
        public string Email { get; set; } = string.Empty;


    }
}