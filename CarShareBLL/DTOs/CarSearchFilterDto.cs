namespace CarShareBLL.DTOs;
public class CarSearchFilterDto
    {
        public string SearchTerm { get; set; }
        public string CarType { get; set; }
        public string Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Location { get; set; }
        public int? Year { get; set; }
    }