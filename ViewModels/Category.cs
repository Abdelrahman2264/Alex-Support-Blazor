using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexSupport.ViewModels
{
    [Table("Categories", Schema = "dbo")]
    public class Category
    {
        [Key]
        [Column(TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Category ID")]
        public int CID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Category Name is required")]
        [Display(Name = "Category Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category Name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$",
            ErrorMessage = "Category Name can only contain letters, numbers, spaces, and hyphens")]
        public string CategoryName { get; set; }

        [Required]
        [DisplayName("Active Status")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column(TypeName = "datetime2(0)")]
        [DisplayName("Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Ticket>? Ticket { get; set; }
        public ICollection<DailyTasks>? DailyTask { get; set; }
    }
}