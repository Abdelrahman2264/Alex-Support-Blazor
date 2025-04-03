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
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Category ID")]
        public int CID { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Category Name")]
        [Display(Name = "Category Name")]
        public string ? CategoryName { get; set; }
        public  bool isActive = true;
        public DateTime CreatedDate { get; set; }
        public ICollection<Ticket>? Ticket { get; set; }
    }
}
