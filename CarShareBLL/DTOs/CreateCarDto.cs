using System.ComponentModel.DataAnnotations;

namespace CarShareBLL.DTOs;
   public class CreateCarDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, MinimumLength = 20)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Car type is required")]
        public string CarType { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(100)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200)]
        public string Location { get; set; }

        [Required(ErrorMessage = "Price per day is required")]
        [Range(0.01, 10000.00)]
        public decimal PricePerDay { get; set; }

        public DateTime? AvailableFrom { get; set; }

        public DateTime? AvailableTo { get; set; }

        public string MainImageUrl { get; set; }

        public List<string> AdditionalImages { get; set; } = new List<string>();
    }