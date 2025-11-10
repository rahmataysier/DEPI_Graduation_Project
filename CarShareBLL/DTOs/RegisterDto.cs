namespace CarShareBLL.DTOs
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string LicenseNumber { get; set; }
        public string NationalId { get; set; }
        public string DocumentUrl { get; set; }
    }
}