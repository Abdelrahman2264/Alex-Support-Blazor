using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace AlexSupport.ViewModels
{
    [Table("AppUsers", Schema = "dbo")]
    public class AppUser
    {
        public AppUser()
        {
            Ticket = new HashSet<Ticket>();
            Tlogs = new HashSet<Tlog>();
        }

        [Key]
        [Column(TypeName = "INT")]
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("AppUser ID")]
        public int UID { get; set; }

        [Column(TypeName = "INT")]
        [DataType(DataType.Text)]
        [DisplayName("Fingerprint")]
        public int Fingerprint { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Login Name")]
        [Display(Name = "Login Name")]
        public required string LoginName { get; set; }

        [Column(TypeName = "NVARCHAR(150)")]
        [Required(ErrorMessage = "Enter Email")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "First Name")]
        public required string Fname { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        public required string Lname { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

    

        [Column(TypeName = "NVARCHAR(11)")]
        [Display(Name = "Mobile")]
        public string? MobilePhone { get; set; }

        [Column(TypeName = "NVARCHAR(10)")]
        [Display(Name = "InternalPhone")]
        public string? Phone { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        [Display(Name = "AppUser Type")]
        public string Role { get; set; } // Active/Disabled

        [Column(TypeName = "NVARCHAR(50)")]
        [Display(Name = "Email Verification")]
        public string? EmailVerified { get; set; }

        [Column(TypeName = "datetime2(0)")]
        [Required(ErrorMessage = "Date not defined")]
        [Display(Name = "Create Date")]
        public DateTime Create_Date { get; set; }


        public required bool IsActive { get; set; } = true; // Active/Disabled


        [ForeignKey("Department")]
        [Column(TypeName = "INT")]
        [Required]
        public int DID { get; set; }

        public Department Department { get; set; }
        public  ICollection<Ticket> Ticket { get; set; }
        public  ICollection<Ticket> AgentTicket { get; set; }
        public virtual ICollection<Tlog> Tlogs { get; set; }


    }

}
