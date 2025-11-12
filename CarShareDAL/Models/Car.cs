using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarShareDAL.Models
{
    public class Car
    {
        public int Id { get; set; }

        // Owner Information
        [Required]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        // Basic Information
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        // Car Specifications
        [Required]
        public string CarType { get; set; } // e.g., "Sedan", "SUV", "Truck"

        [Required]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        // Location
        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        // Rental Information
        [Required]
        public string RentalStatus { get; set; } = "Available"; // "Available" | "Rented" | "Unavailable"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000.00)]
        public decimal PricePerDay { get; set; }

        // Availability
        public DateTime? AvailableFrom { get; set; }

        public DateTime? AvailableTo { get; set; }

        // Images
        [StringLength(500)]
        public string MainImageUrl { get; set; }

        [StringLength(2000)]
        public string AdditionalImagesJson { get; set; } // Store as JSON array

        // Approval
        public string ApprovalStatus { get; set; } = "Pending"; // "Pending" | "Approved" | "Rejected"

        public int? ApprovedByAdminId { get; set; }

        public DateTime? ApprovedAt { get; set; }

        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;

    }
}