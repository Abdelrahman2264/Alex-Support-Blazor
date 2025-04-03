using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlexSupport.ViewModels
{
    [Table("TicketLogs", Schema = "dbo")]
    public class Tlog
    {

        [Key]
        [Column(TypeName = "INT")]
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("TLOG ID")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Ticket")]
        [Column(TypeName = "INT")]
        [DisplayName("TID")]
        public int TID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Action")]
        public string Action { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(850)")]
        [DisplayName("Description")]
        public string Message { get; set; }

        [Required]
        [Column(TypeName = "datetime2(0)")]
        [DisplayName("Action Time")]
        public DateTime actionTime { get; set; }

        [Required]
        [Column(TypeName = "INT")]
        [ForeignKey("AppUser")]
        public int UID { get; set; }

        public AppUser? AppUser { get; set; }
        public Ticket? Ticket { get; set; }

        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[]? ImageData { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? ImageContentType { get; set; } // e.g. "image/jpeg", "image/png"

    }
}
