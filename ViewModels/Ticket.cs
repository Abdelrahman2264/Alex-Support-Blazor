using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Serialization;

namespace AlexSupport.ViewModels
{

    [Table("Tickets", Schema = "dbo")]
    public class Ticket
    {
        public Ticket()
        {
            Tlogs = new HashSet<Tlog>();

        }

        [Key]
        [Column(TypeName = "INT")]
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int TID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Subject")]
        [Display(Name = "Subject")]
        public  string Subject { get; set; } 
        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Choose Priority")]
        [Display(Name = "Priority")]
        public  string Priority { get; set; }

        [Column(TypeName = "NVARCHAR(850)")]
        [Required(ErrorMessage = "Enter Your Issue")]
        [Display(Name = "Issue")]
        public  string Issue { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Display(Name = "Status")]
        public  string Status { get; set; } //Open/Closed

        [Column(TypeName = "NVARCHAR(50)")]
        [Display(Name = "Result")]
        public string? Result { get; set; }

        [Column(TypeName = "datetime2(0)")]
        [Required(ErrorMessage = "Date not defined")]
        [Display(Name = "Open Date")]
        public DateTime OpenDate { get; set; }

       

        [Column(TypeName = "INT")]
        [Display(Name = "Due Time")]
        [Range(6, int.MaxValue, ErrorMessage = "Expected Time must be greater than 5 Minutes.")]
        public int? Due_Minutes { get; set; }



        [Column(TypeName = "datetime2(0)")]
        [Display(Name = "Close Date")]
        public DateTime? CloseDate { get; set; }

        [Column(TypeName = "NVARCHAR(850)")]
        [Display(Name = "Solution")]
        public string? Solution { get; set; }

        [Column(TypeName = "NVARCHAR(850)")]
        [Display(Name = "Comments")]
        public string? Comments { get; set; }

        [ForeignKey(nameof(Location))]
        [Column(TypeName = "INT")]
        [Display(Name = "Location")]
        public  int LID { get; set; }
        public Location? location { get; set; }

        public int? UserRate { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public int UID { get; set; }

        [ForeignKey(nameof(Agent))]
        [Column(TypeName = "INT")]
        [Display(Name = "Desk Agent")]
        public int? AgentID { get; set; }

        public AppUser? User { get; set; }
        public AppUser? Agent { get; set; }
        public  ICollection<Tlog> Tlogs { get; set; }   
        [ForeignKey(nameof(category))]
        [Column(TypeName = "INT")]
        [Display(Name = "Issue Category")]
        public int? CategoryID { get; set; }
        public Category? category { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Ticket Rate")]

        public decimal? TicketRate { get; set; }
        [Column(TypeName = "datetime2(0)")]

        [Display(Name = "Assigned Date")]
        public DateTime? Assign_Date { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        [Display(Name = "User Approve")]
        public string? UserApprove { get; set; }
        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[]? ImageData { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? ImageContentType { get; set; } // e.g. "image/jpeg", "image/png"
    }
}
