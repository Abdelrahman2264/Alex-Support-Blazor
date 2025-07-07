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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("AppUser ID")]
        public int UID { get; set; }

        [Column(TypeName = "INT")]
        [DisplayName("Fingerprint")]
        public int Fingerprint { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Login Name")]
        [Display(Name = "Login Name")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Login name must be between 4 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Login name can only contain letters, numbers and underscores")]
        public string LoginName { get; set; }

        [Column(TypeName = "NVARCHAR(150)")]
        [Required(ErrorMessage = "Enter Email")]
        [Display(Name = "Email")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@alpinecreations\.com$",
            ErrorMessage = "Email must be a valid @alpinecreations.com address")]
        public string Email { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
        public string Fname { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters")]
        public string Lname { get; set; }

        [Column(TypeName = "NVARCHAR(250)")]
        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "Password")]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character")]
        public string Password { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        [Display(Name = "Job Title")]
        [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters")]
        public string? JobTitle { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        [Display(Name = "Mobile")]
        [Phone(ErrorMessage = "Invalid mobile phone number")]
        [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters")]
        public string? MobilePhone { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        [Display(Name = "InternalPhone")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? Phone { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        [Display(Name = "AppUser Type")]
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Display(Name = "Email Verification")]
        public string? EmailVerified { get; set; }

        [Column(TypeName = "datetime2(0)")]
        [Required(ErrorMessage = "Date not defined")]
        [Display(Name = "Create Date")]
        public DateTime Create_Date { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("Department")]
        [Column(TypeName = "INT")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Choose Department")]
        public int DID { get; set; }

        [Column(TypeName = "VARBINARY(MAX)")]
        [MaxLength(5242880, ErrorMessage = "Image size cannot exceed 5MB")]
        public byte[]? ImageData { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? ImageContentType { get; set; }

        public Department Department { get; set; }
        public ICollection<Ticket> Ticket { get; set; }
        public ICollection<Ticket> AgentTicket { get; set; }
        public virtual ICollection<Tlog> Tlogs { get; set; }
        public virtual ICollection<SystemNotification> SentNotifications { get; set; }
        public virtual ICollection<SystemNotification> ReceivedNotifications { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        public virtual ICollection<SystemLogs> SystemLogs { get; set; }
        public virtual ICollection<DailyTasks> DailyTasks { get; set; }
        public virtual ICollection<DailyTasks> UserDailyTasks { get; set; }
    }
}