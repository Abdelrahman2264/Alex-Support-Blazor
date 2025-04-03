using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlexSupport.ViewModels
{
    [Table("Locations", Schema = "dbo")]

    public class Location
    {
        [Key]
        [Column(TypeName = "INT")]
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Location ID")]
        public int LID { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Location Name")]
        [Display(Name = "Location Name")]
        public required string LocationName { get; set; }

        public required bool isActive = true;
        public required DateTime DateTime { get; set; } = DateTime.Now;
       public ICollection<Ticket>? Ticket { get; set; }
    }
}
