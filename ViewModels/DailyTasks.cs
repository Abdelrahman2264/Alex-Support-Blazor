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
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("ID")]
        public int DTID { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Enter Subject")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        [Required(ErrorMessage = "Choose Priority")]
        [Display(Name = "Priority")]
        public string Priority { get; set; }

        [Column(TypeName = "NVARCHAR(850)")]
        [Required(ErrorMessage = "Enter Your Issue")]
        [Display(Name = "Issue")]
        public string Issue { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Enter Expected Time In Minitues")]
        [Column(TypeName = "INT")]
        [Display(Name = "Due Time")]
        [Range(6, int.MaxValue, ErrorMessage = "Expected Time must be greater than 5 Minutes.")]
        public int? Due_Minutes { get; set; }

        [ForeignKey(nameof(category))]
        [Column(TypeName = "INT")]
        [Display(Name = "Issue Category")]
        public int? CategoryID { get; set; }
        public Category? category { get; set; }

        


    }
}
