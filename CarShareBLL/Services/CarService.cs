using microsoft.Extensions.Logging;
using CarShareBLL.DTOs;
using CarShareDAL.Models;
using CarShareDAL.Repositories;

namespace CarShareBLL.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CarService> _logger;

          public CarService(
            ICarRepository carRepository,
            IUserRepository userRepository,
            ILogger<CarService> logger)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<CarResponseDto> CreateCarAsync(int ownerId, CreateCarDto createCarDto)
        {
            try
            {
                // Verify owner exists and is a car owner
                var owner = await _userRepository.GetByIdAsync(ownerId);
                if (owner == null || owner.Role != "CarOwner")
                {
                    return new CarResponseDto
                    {
                        Success = false,
                        Message = "Only car owners can create car posts"
                    };
                }

                if (owner.Status != "Approved")
                {
                    return new CarResponseDto
                    {
                        Success = false,
                        Message = "Your account must be approved before creating car posts"
                    };
                }

                // Create car entity
                var car = new Car
                {
                    OwnerId = ownerId,
                    Title = createCarDto.Title,
                    Description = createCarDto.Description,
                    CarType = createCarDto.CarType,
                    Brand = createCarDto.Brand,
                    Model = createCarDto.Model,
                    Year = createCarDto.Year,
                    Location = createCarDto.Location,
                    PricePerDay = createCarDto.PricePerDay,
                    AvailableFrom = createCarDto.AvailableFrom,
                    AvailableTo = createCarDto.AvailableTo,
                    MainImageUrl = createCarDto.MainImageUrl,
                    AdditionalImagesJson = createCarDto.AdditionalImages != null 
                        ? JsonSerializer.Serialize(createCarDto.AdditionalImages) 
                        : null,
                    RentalStatus = "Available",
                    ApprovalStatus = "Pending",
                    CreatedAt = DateTime.UtcNow,
                };

                await _carRepository.AddAsync(car);
                await _carRepository.SaveChangesAsync();

                _logger.LogInformation($"Car created: {car.Title} by owner {ownerId}");

                return new CarResponseDto
                {
                    Success = true,
                    Message = "Car created successfully. Pending admin approval.",
                    Car = MapToCarDto(car, owner)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating car");
                return new CarResponseDto
                {
                    Success = false,
                    Message = "An error occurred while creating the car"
                };
            }
        }

        public async Task<CarResponseDto> UpdateCarAsync(int ownerId, UpdateCarDto updateCarDto)
        {
            try
            {
                var car = await _carRepository.GetCarWithOwnerAsync(updateCarDto.Id);

                if (car == null)
                {
                    return new CarResponseDto { Success = false, Message = "Car not found" };
                }

                if (car.OwnerId != ownerId)
                {
                    return new CarResponseDto { Success = false, Message = "You can only update your own cars" };
                }

                if (car.RentalStatus == "Rented")
                {
                    return new CarResponseDto { Success = false, Message = "Cannot update a rented car" };
                }

                // Update allowed fields
                car.Title = updateCarDto.Title;
                car.Description = updateCarDto.Description;
                car.PricePerDay = updateCarDto.PricePerDay;
                car.Location = updateCarDto.Location;
                car.AvailableFrom = updateCarDto.AvailableFrom;
                car.AvailableTo = updateCarDto.AvailableTo;
                car.MainImageUrl = updateCarDto.MainImageUrl;
                car.AdditionalImagesJson = updateCarDto.AdditionalImages != null 
                    ? JsonSerializer.Serialize(updateCarDto.AdditionalImages) 
                    : null;

                _carRepository.Update(car);
                await _carRepository.SaveChangesAsync();

                _logger.LogInformation($"Car updated: {car.Id}");

                return new CarResponseDto
                {
                    Success = true,
                    Message = "Car updated successfully",
                    Car = MapToCarDto(car, car.Owner)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating car");
                return new CarResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating the car"
                };
            }
        }

        public async Task<CarResponseDto> DeleteCarAsync(int carId, int ownerId)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(carId);

                if (car == null)
                {
                    return new CarResponseDto { Success = false, Message = "Car not found" };
                }

                if (car.OwnerId != ownerId)
                {
                    return new CarResponseDto { Success = false, Message = "You can only delete your own cars" };
                }

                if (car.RentalStatus == "Rented")
                {
                    return new CarResponseDto
                    {
                        Success = false,
                        Message = "Cannot delete a car that is currently rented"
                    };
                }

                _carRepository.Update(car);
                await _carRepository.SaveChangesAsync();

                _logger.LogInformation($"Car deleted: {carId}");

                return new CarResponseDto
                {
                    Success = true,
                    Message = "Car deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting car");
                return new CarResponseDto
                {
                    Success = false,
                    Message = "An error occurred while deleting the car"
                };
            }
        }

        public async Task<CarDto> GetCarByIdAsync(int carId)
        {
            var car = await _carRepository.GetCarWithOwnerAsync(carId);
            return car != null ? MapToCarDto(car, car.Owner) : null;
        }

        public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetApprovedCarsAsync();
            return cars.Select(c => MapToCarDto(c, c.Owner));
        }

        public async Task<IEnumerable<CarDto>> GetCarsByOwnerIdAsync(int ownerId)
        {
            var cars = await _carRepository.GetCarsByOwnerIdAsync(ownerId);
            return cars.Select(c => MapToCarDto(c, c.Owner));
        }

        public async Task<IEnumerable<CarDto>> GetAvailableCarsAsync()
        {
            var cars = await _carRepository.GetAvailableCarsAsync();
            return cars.Select(c => MapToCarDto(c, c.Owner));
        }

        public async Task<IEnumerable<CarDto>> SearchCarsAsync(string searchTerm)
        {
            var cars = await _carRepository.SearchCarsAsync(searchTerm);
            return cars.Select(c => MapToCarDto(c, c.Owner));
        }

        public async Task<IEnumerable<CarDto>> FilterCarsAsync(CarSearchFilterDto filterDto)
        {
        
            var cars = await _carRepository.FilterCarsAsync(
                filterDto.CarType,
                filterDto.Brand,
                filterDto.MinPrice,
                filterDto.MaxPrice,
                filterDto.Location
            );

            return cars.Select(c => MapToCarDto(c, c.Owner));
        }

        public async Task<CarResponseDto> ApproveCarAsync(int carId, int adminId)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(carId);
                if (car == null)
                {
                    return new CarResponseDto { Success = false, Message = "Car not found" };
                }

                var admin = await _userRepository.GetByIdAsync(adminId);
                if (admin == null || admin.Role != "Admin")
                {
                    return new CarResponseDto { Success = false, Message = "Only admins can approve cars" };
                }

                car.ApprovalStatus = "Approved";
                car.ApprovedByAdminId = adminId;
                car.ApprovedAt = DateTime.UtcNow;

                _carRepository.Update(car);
                await _carRepository.SaveChangesAsync();

                _logger.LogInformation($"Car approved: {carId} by admin {adminId}");

                return new CarResponseDto
                {
                    Success = true,
                    Message = "Car approved successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving car");
                return new CarResponseDto { Success = false, Message = "Error approving car" };
            }
        }

        public async Task<CarResponseDto> RejectCarAsync(int carId, int adminId)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(carId);
                if (car == null)
                {
                    return new CarResponseDto { Success = false, Message = "Car not found" };
                }

                var admin = await _userRepository.GetByIdAsync(adminId);
                if (admin == null || admin.Role != "Admin")
                {
                    return new CarResponseDto { Success = false, Message = "Only admins can reject cars" };
                }

                car.ApprovalStatus = "Rejected";

                _carRepository.Update(car);
                await _carRepository.SaveChangesAsync();

                _logger.LogInformation($"Car rejected: {carId} by admin {adminId}");

                return new CarResponseDto
                {
                    Success = true,
                    Message = "Car rejected"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting car");
                return new CarResponseDto { Success = false, Message = "Error rejecting car" };
            }
        }

        public async Task<CarResponseDto> MarkAsRentedAsync(int carId, int ownerId)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(carId);

                if (car == null)
                {
                    return new CarResponseDto { Success = false, Message = "Car not found" };
                }

                if (car.OwnerId != ownerId)
                {
                    return new CarResponseDto { Success = false, Message = "You can only modify your own cars" };
                }

                car.RentalStatus = "Rented";
                car.UpdatedAt = DateTime.UtcNow;

                _carRepository.Update(car);
                await _carRepository.SaveChangesAsync();

                return new CarResponseDto
                {
                    Success = true,
                    Message = "Car marked as rented"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking car as rented");
                return new CarResponseDto { Success = false, Message = "Error updating car status" };
            }
        }

        public async Task<CarResponseDto> MarkAsAvailableAsync(int carId, int ownerId)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(carId);

                if (car == null)
                {
                    return new CarResponseDto { Success = false, Message = "Car not found" };
                }

                if (car.OwnerId != ownerId)
                {
                    return new CarResponseDto { Success = false, Message = "You can only modify your own cars" };
                }

                car.RentalStatus = "Available";
                car.UpdatedAt = DateTime.UtcNow;

                _carRepository.Update(car);
                await _carRepository.SaveChangesAsync();

                return new CarResponseDto
                {
                    Success = true,
                    Message = "Car marked as available"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking car as available");
                return new CarResponseDto { Success = false, Message = "Error updating car status" };
            }
        }

        // Helper method to map Car entity to CarDto
        private CarDto MapToCarDto(Car car, User owner)
        {
            List<string> additionalImages = null;
            if (!string.IsNullOrEmpty(car.AdditionalImagesJson))
            {
                try
                {
                    additionalImages = JsonSerializer.Deserialize<List<string>>(car.AdditionalImagesJson);
                }
                catch { }
            }

            return new CarDto
            {
                Id = car.Id,
                OwnerId = car.OwnerId,
                OwnerName = owner?.FullName ?? "Unknown",
                Title = car.Title,
                Description = car.Description,
                CarType = car.carType,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Location = car.Location,
                RentalStatus = car.RentalStatus,
                PricePerDay = car.PricePerDay,
                AvailableFrom = car.AvailableFrom,
                AvailableTo = car.AvailableTo,
                MainImageUrl = car.MainImageUrl,
                AdditionalImages = additionalImages,
                ApprovalStatus = car.ApprovalStatus,
                CreatedAt = car.CreatedAt,
            };
        }


    }
}