namespace AlexSupport.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ChatMessages", Schema = "dbo")]
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CHID { get; set; }

        [Required]
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        [Required]
        [ForeignKey("Sender")]  // Changed to match navigation property

        public int SenderId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")] // Allows longer messages
        public string MessageText { get; set; }

        [Required]
        public DateTime SentDate { get; set; } = DateTime.Now;
        [Required]
        public bool ? IsRead { get; set; } = false;

        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[]? ImageData { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? ImageContentType { get; set; } // e.g. "image/jpeg", "image/png"
        // Navigation properties
        public virtual AppUser Sender { get; set; } // Renamed for clarity
    }
}