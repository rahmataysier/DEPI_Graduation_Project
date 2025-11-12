using CarShare.BLL.DTOs;

namespace CarShareBLL.Services
{

    public interface ICarService
    {
        // CRUD Operations
        Task<CarResponseDto> CreateCarAsync(int ownerId, CreateCarDto createCarDto);
        Task<CarResponseDto> UpdateCarAsync(int ownerId, UpdateCarDto updateCarDto);
        Task<CarResponseDto> DeleteCarAsync(int carId, int ownerId);
        Task<CarDto> GetCarByIdAsync(int carId);
        Task<IEnumerable<CarDto>> GetAllCarsAsync();
        
        // Owner specific
        Task<IEnumerable<CarDto>> GetCarsByOwnerIdAsync(int ownerId);
        
        // Public browsing
        Task<IEnumerable<CarDto>> GetAvailableCarsAsync();
        
        // Search and Filter
        Task<IEnumerable<CarDto>> SearchCarsAsync(string searchTerm);
        Task<IEnumerable<CarDto>> FilterCarsAsync(CarSearchFilterDto filterDto);
        
        // Admin operations
        Task<CarResponseDto> ApproveCarAsync(int carId, int adminId);
        Task<CarResponseDto> RejectCarAsync(int carId, int adminId);
        
        // Rental status
        Task<CarResponseDto> MarkAsRentedAsync(int carId, int ownerId);
        Task<CarResponseDto> MarkAsAvailableAsync(int carId, int ownerId);
    }   
}