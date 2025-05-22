using AlexSupport.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlexSupport.Services.Models
{
    public class SignUpModel
    {


        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Fingerprint is required")]
        [DisplayName("Fingerprint")]
        public string Fingerprint { get; set; }


        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string Fname { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string Lname { get; set; }

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

        [Column(TypeName = "NVARCHAR(100)")]
        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Jop title is required ")]

        public string? JobTitle { get; set; }

        [Column(TypeName = "NVARCHAR(11)")]
        [Display(Name = "Mobile")]
        public string? MobilePhone { get; set; }

        [Column(TypeName = "NVARCHAR(10)")]
        [Display(Name = "InternalPhone")]
        [Required(ErrorMessage ="Mobile Phone is required")]
        public string? Phone { get; set; }

        [Column(TypeName = "INT")]
        [Range(1, int.MaxValue,ErrorMessage ="Please Select a department")]
        public int DID { get; set; }

    }

}
