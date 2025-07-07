using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexSupport.ViewModels
{
    [Table("Departments", Schema = "dbo")]
    public class Department
    {
        public Department()
        {
            Users = new HashSet<AppUser>();
            CreateDate = DateTime.UtcNow; // Initialize with UTC time
        }

        [Key]
        [Column(TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Department ID")]
        public int DID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Department name is required")]
        [Display(Name = "Department Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Department name must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-&]+$",
            ErrorMessage = "Department name can only contain letters, numbers, spaces, hyphens, and ampersands")]
        public string DepartmentName { get; set; }

        [Required]
        [Display(Name = "Active Status")]
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column(TypeName = "datetime2(0)")]
        [Display(Name = "Creation Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }






        public virtual ICollection<AppUser> Users { get; set; }
    }
}