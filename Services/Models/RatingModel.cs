using System.ComponentModel.DataAnnotations;

namespace AlexSupport.Services.Models
{
    public class RatingModel
    {
        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5 stars")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string Comment { get; set; } = string.Empty;
    }
}
