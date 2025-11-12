using System.ComponentModel.DataAnnotations;

namespace CarShare.BLL.DTOs
{
    public class UpdateCarDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 20)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal PricePerDay { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }
        public DateTime? AvailableFrom { get; set; }

        public DateTime? AvailableTo { get; set; }

        public string MainImageUrl { get; set; }

        public List<string> AdditionalImages { get; set; } = new List<string>();
    }
}