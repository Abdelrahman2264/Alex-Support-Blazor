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
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Notification ID")]
        public int NID { get; set; }
        public int FromUserId { get; set; }
        public AppUser FromUser { get; set; }

        public int ToUserId { get; set; }
        public AppUser ToUser { get; set; }

        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
        public bool IsRead { get; set; } = false;
    }

}
