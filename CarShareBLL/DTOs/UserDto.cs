namespace CarShareBLL.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LicenseNumber { get; set; }
        public string NationalId { get; set; }
        public string DocumentUrl { get; set; }
    }
}