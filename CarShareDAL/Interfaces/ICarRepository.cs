using CarShareDAL.Models;

namespace CarShareDAL.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<IEnumerable<Car>> GetApprovedCarsAsync();
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
        Task<IEnumerable<Car>> GetCarsByOwnerIdAsync(int ownerId);
        Task<IEnumerable<Car>> SearchCarsAsync(string searchTerm);
        Task<IEnumerable<Car>> FilterCarsAsync(
            string? carType,
            string brand,
            decimal? minPrice,
            decimal? maxPrice,
            string location
        );
        Task<bool> CanDeleteCarAsync(int carId);
        Task<Car> GetCarWithOwnerAsync(int carId);
    }
}
