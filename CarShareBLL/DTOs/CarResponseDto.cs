namespace CarShareBLL.DTOs
{
    public class CarResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CarDto Car { get; set; }
    }
}