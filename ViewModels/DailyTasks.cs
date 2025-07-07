using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlexSupport.ViewModels
{
    [Table("DailyTasks", Schema = "dbo")]
    public class DailyTasks
    {
        [Key]
        [Column(TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Task ID")]
        public int DTID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Subject is required")]
        [Display(Name = "Subject")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Subject must be between 5 and 50 characters")]
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
        [StringLength(850, MinimumLength = 10, ErrorMessage = "Issue must be between 10 and 850 characters")]
        public string Issue { get; set; }

        [Required]
        [Display(Name = "Active Status")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Expected time is required")]
        [Column(TypeName = "INT")]
        [Display(Name = "Due Time (Minutes)")]
        [Range(5, 1440, ErrorMessage = "Expected time must be between 5 and 1440 minutes (24 hours)")]
        public int Due_Minutes { get; set; }  // Removed nullable as it's required

        [ForeignKey(nameof(Category))]
        [Column(TypeName = "INT")]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int CategoryID { get; set; }  // Removed nullable as it should be required
        public Category? Category { get; set; }

        [Column(TypeName = "INT")]
        [Display(Name = "Recurrence Days")]
        [Required(ErrorMessage = "Recurrence days are required")]
        [Range(1, 365, ErrorMessage = "Recurrence must be between 1 and 365 days")]
        public int RecurrenceDays { get; set; }  // Renamed from TypeName for clarity

        [ForeignKey(nameof(Agent))]
        [Column(TypeName = "INT")]
        [Display(Name = "Assigned Agent")]
        public int? AgentId { get; set; }
        public AppUser? Agent { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;  // Using UTC

        [DataType(DataType.DateTime)]
        [Display(Name = "Last Updated")]
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Location))]
        [Column(TypeName = "INT")]
        [Display(Name = "Location")]
        public int? LocationId { get; set; }
        public Location? Location { get; set; }

        [ForeignKey(nameof(User))]
        [Column(TypeName = "INT")]
        [Display(Name = "Created By")]
        [Required(ErrorMessage = "Creator user is required")]
        public int UID { get; set; }  // Removed nullable as creator should be required
        public AppUser? User { get; set; }
    }
}