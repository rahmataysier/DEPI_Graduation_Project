namespace CarShareBLL.DTOs;
    public class CarDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CarType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Location { get; set; }
        public string RentalStatus { get; set; }
        public decimal PricePerDay { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public string MainImageUrl { get; set; }
        public List<string> AdditionalImages { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
}