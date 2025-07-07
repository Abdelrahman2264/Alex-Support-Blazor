using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexSupport.Services.Models
{
    public class SignInModel
    {
        //[Column(TypeName = "NVARCHAR(150)")]
        //[Required(ErrorMessage = "Enter Email")]
        //[Display(Name = "Email")]
        //[StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        //[RegularExpression(@"^[a-zA-Z0-9._%+-]+@alpinecreations\.com$",
        //    ErrorMessage = "Email must be a valid @alpinecreations.com address")]
        public string Username { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Password is required")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //[StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be 8-128 characters")]
        public string Password { get; set; } = string.Empty;



    }
}