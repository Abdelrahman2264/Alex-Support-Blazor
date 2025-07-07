using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexSupport.ViewModels
{
    [Table("Tickets", Schema = "dbo")]
    public class Ticket
    {
        public Ticket()
        {
            Tlogs = new HashSet<Tlog>();
            ChatMessages = new HashSet<ChatMessage>();
        }

        [Key]
        [Column(TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Ticket ID")]
        public int TID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Subject is required")]
        [Display(Name = "Subject")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Subject must be between 5-50 characters")]
        public string Subject { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Priority is required")]
        [Display(Name = "Priority")]
        [RegularExpression(@"^(Low|Medium|High|Urgent)$",
            ErrorMessage = "Priority must be Low, Medium, High, or Urgent")]
        public string Priority { get; set; }

        [Column(TypeName = "NVARCHAR(850)")]
        [Required(ErrorMessage = "Issue description is required")]
        [Display(Name = "Issue")]
        [StringLength(850, MinimumLength = 10, ErrorMessage = "Issue must be 10-850 characters")]
        public string Issue { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required]
        [Display(Name = "Status")]

        public string Status { get; set; } = "Open";

        [Required]
        [Display(Name = "Solved Status")]
        [DefaultValue(false)]
        public bool IsSolved { get; set; } = false;

        [Required]
        [Column(TypeName = "datetime2(0)")]
        [Display(Name = "Open Date")]
        public DateTime OpenDate { get; set; }

        [Column(TypeName = "INT")]
        [Display(Name = "Due Time (Minutes)")]
        [Range(5, 10080, ErrorMessage = "Due time must be 5-10080 minutes (1 week)")]
        public int? Due_Minutes { get; set; }

        [Column(TypeName = "datetime2(0)")]
        [Display(Name = "Close Date")]
        public DateTime? CloseDate { get; set; }

        [Column(TypeName = "NVARCHAR(850)")]
        [Display(Name = "Solution")]
        [StringLength(850, ErrorMessage = "Solution cannot exceed 850 characters")]
        public string? Solution { get; set; }

        [Column(TypeName = "NVARCHAR(850)")]
        [Display(Name = "Comments")]
        [StringLength(850, ErrorMessage = "Comments cannot exceed 850 characters")]
        public string? Comments { get; set; }

        [ForeignKey(nameof(Location))]
        [Column(TypeName = "INT")]
        [Display(Name = "Location")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a valid location")]
        public int LID { get; set; }
        public Location? Location { get; set; }

        [Column(TypeName = "INT")]
        [Display(Name = "User Rating")]
        [Range(1, 5, ErrorMessage = "Rating must be 1-5 stars")]
        public int? UserRate { get; set; }

        [ForeignKey(nameof(User))]
        [Required(ErrorMessage = "Ticket creator is required")]
        public int UID { get; set; }
        public AppUser? User { get; set; }

        [ForeignKey(nameof(Agent))]
        [Column(TypeName = "INT")]
        [Display(Name = "Assigned Agent")]
        public int? AgentID { get; set; }
        public AppUser? Agent { get; set; }

        [ForeignKey(nameof(Category))]
        [Column(TypeName = "INT")]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a valid category")]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Ticket Rate")]
        [Range(0, 999999.99, ErrorMessage = "Invalid rate value")]
        public decimal? TicketRate { get; set; }

        [Column(TypeName = "datetime2(0)")]
        [Display(Name = "Assigned Date")]
        public DateTime? AssignDate { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Display(Name = "User Approval")]

        public string? UserApproval { get; set; }

        [Column(TypeName = "VARBINARY(MAX)")]
        [MaxLength(5242880, ErrorMessage = "Image cannot exceed 5MB")]
        public byte[]? ImageData { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        [Display(Name = "Image Type")]
        public string? ImageContentType { get; set; }

        // Navigation properties
        public ICollection<Tlog> Tlogs { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}