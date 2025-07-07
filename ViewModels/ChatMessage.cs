namespace AlexSupport.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ChatMessages", Schema = "dbo")]
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Message ID")]
        public int CHID { get; set; }

        [Required(ErrorMessage = "Ticket reference is required")]
        [ForeignKey("Ticket")]
        [DisplayName("Ticket ID")]
        public int TicketId { get; set; }

        [Required]
        [ForeignKey("Sender")]
        [DisplayName("Sender ID")]
        public int SenderId { get; set; }

        [Required(ErrorMessage = "Message text is required")]
        [Column(TypeName = "NVARCHAR(MAX)")]
        [StringLength(5000, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 5000 characters")]
        [DisplayName("Message Content")]
        public string MessageText { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Sent Date")]
        public DateTime SentDate { get; set; } = DateTime.Now;  // Using UTC for consistency

        [Required]
        [DisplayName("Read Status")]
        [DefaultValue(false)]
        public bool? IsRead { get; set; } = false;  // Removed nullable as it has default value

        [Column(TypeName = "VARBINARY(MAX)")]
        [MaxLength(5242880, ErrorMessage = "Image size cannot exceed 5MB")]
        [DisplayName("Attachment Data")]
        public byte[]? ImageData { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        [StringLength(100, ErrorMessage = "Content type cannot exceed 100 characters")]
        [DisplayName("Attachment Type")]
        public string? ImageContentType { get; set; }

        // Navigation properties
        [DisplayName("Related Ticket")]
        public virtual Ticket Ticket { get; set; }

        [DisplayName("Message Sender")]
        public virtual AppUser Sender { get; set; }
    }
}