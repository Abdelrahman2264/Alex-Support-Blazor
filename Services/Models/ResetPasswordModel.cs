using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlexSupport.Services.Models
{
    public class ResetPasswordModel
    {
        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "confirm Password is required ")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
