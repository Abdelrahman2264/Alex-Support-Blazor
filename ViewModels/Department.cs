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
        }

        [Key]
        [Column(TypeName = "INT")]
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int DID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Department NAme")]
        [Display(Name = "Department Name")]
        public required string DepartmentName { get; set; }


        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Status not defined")]
        [Display(Name = "Status")]
        public required string IsActive { get; set; } //Ctive/Disabled

        [Column(TypeName = "datetime2(0)")]
        [Required(ErrorMessage = "Date not defined")]
        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

 


        public virtual ICollection<AppUser> Users { get; set; }

    }
}
