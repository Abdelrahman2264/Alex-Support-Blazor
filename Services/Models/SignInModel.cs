using System.ComponentModel.DataAnnotations;

namespace AlexSupport.Services.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage ="Username or Email is required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
