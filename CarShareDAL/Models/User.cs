using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareDAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = "Renter"; // "Renter" | "Owner" | "Admin"
        public bool IsApproved { get; set; } = false; // owner approval flag
        public string LicenseNumber { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty; // link to uploaded docs
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
