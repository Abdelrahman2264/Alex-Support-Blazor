using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AlexSupport.ViewModels
{
    [Table("SystemLogs", Schema = "dbo")]

    public class SystemLogs
    {

        [Key]
        [Column(TypeName = "INT")]
        [DataType(DataType.Text)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("SYSTEMLOG ID")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        [DisplayName("Action")]
        public string Action { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Action")]
        public string Type { get; set; }
        [Required]
        [Column(TypeName = "datetime2(0)")]
        [DisplayName("Action Time")]
        public DateTime actionTime { get; set; }
        [Required]
        [Column(TypeName = "INT")]
        [ForeignKey("AppUser")]
        public int UID { get; set; }
        public AppUser? AppUser { get; set; }


    }
}

