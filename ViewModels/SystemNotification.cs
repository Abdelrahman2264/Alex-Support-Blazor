using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexSupport.ViewModels
{
    [Table("Notifications", Schema = "dbo")]
    public class SystemNotification
    {
        [Key]
        [Column(TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Notification ID")]
        public int NID { get; set; }

        [Required(ErrorMessage = "Sender user is required")]
        [ForeignKey("FromUser")]
        [DisplayName("Sender ID")]
        public int FromUserId { get; set; }

        [Required(ErrorMessage = "Recipient user is required")]
        [ForeignKey("ToUser")]
        [DisplayName("Recipient ID")]
        public int ToUserId { get; set; }

        [Required(ErrorMessage = "Notification message is required")]
        [Column(TypeName = "NVARCHAR(500)")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Message must be between 5 and 500 characters")]
        [DisplayName("Notification Message")]
        public string Message { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Sent At")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [DisplayName("Read At")]
        public DateTime? ReadAt { get; set; }

        [Required]
        [DisplayName("Is Read")]
        [DefaultValue(false)]
        public bool IsRead { get; set; } = false;



        [Column(TypeName = "NVARCHAR(100)")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        [DisplayName("Notification Title")]
        public string? Title { get; set; }



        // Navigation properties
        [DisplayName("Sender")]
        public virtual AppUser FromUser { get; set; }

        [DisplayName("Recipient")]
        public virtual AppUser ToUser { get; set; }
    }
}