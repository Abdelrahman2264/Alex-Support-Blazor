using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlexSupport.ViewModels
{
    [Table("Locations", Schema = "dbo")]
    public class Location
    {
        public Location()
        {
            CreatedDate = DateTime.UtcNow; // Initialize with UTC time
            Ticket = new HashSet<Ticket>();
            Tasks = new HashSet<DailyTasks>();
        }

        [Key]
        [Column(TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Location ID")]
        public int LID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Location name is required")]
        [Display(Name = "Location Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Location name must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-.,()&]+$",
            ErrorMessage = "Location name can contain letters, numbers, spaces, and common punctuation")]
        public string LocationName { get; set; }

        [Required]
        [Display(Name = "Active Status")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column(TypeName = "datetime2(0)")]
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }


        // Navigation properties
        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<DailyTasks> Tasks { get; set; }
    }
}