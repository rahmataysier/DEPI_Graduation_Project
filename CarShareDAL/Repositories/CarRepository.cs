using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarShareDAL.Interfaces;
using CarShareDAL.Models;
using CarShareDAL.Data;

namespace CarShareDAL.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly CarShareDbContext _context;
        public CarRepository(CarShareDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Car>> GetApprovedCarsAsync()
        {
            return await _dbSet
                .Include(c => c.Owner)
                .Where(c => c.ApprovalStatus == "Approved")
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await _dbSet
                .Include(c => c.Owner)
                .Where(c => c.ApprovalStatus == "Approved" 
                         && c.RentalStatus == "Available" )
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByOwnerIdAsync(int ownerId)
        {
            return await _dbSet
                .Include(c => c.Owner)
                .Where(c => c.OwnerId == ownerId && c.ApprovalStatus == "Approved")
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAvailableCarsAsync();
            }

            var lowerSearchTerm = searchTerm.ToLower();

            return await _dbSet
                .Include(c => c.Owner)
                .Where(c => c.ApprovalStatus == "Approved"
                         && c.RentalStatus == "Available"
                         && (c.Title.ToLower().Contains(lowerSearchTerm)
                             || c.Brand.ToLower().Contains(lowerSearchTerm)
                             || c.Model.ToLower().Contains(lowerSearchTerm)
                             || c.Description.ToLower().Contains(lowerSearchTerm)
                             || c.Location.ToLower().Contains(lowerSearchTerm)))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> FilterCarsAsync(
            string? carType,
            string brand,
            decimal? minPrice,
            decimal? maxPrice,
            string location)
        {
            var query = _dbSet
                .Include(c => c.Owner)
                .Where(c => c.ApprovalStatus == "Approved"
                         && c.RentalStatus == "Available")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(carType))
            {
                query = query.Where(c => c.CarType.ToLower() == carType.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(c => c.Brand.ToLower() == brand.ToLower());
            }

            if (minPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(c => c.Location.ToLower().Contains(location.ToLower())
                                      || c.Description.ToLower().Contains(location.ToLower()));
            }

            return await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        public async Task<bool> CanDeleteCarAsync(int carId)
        {
            var car = await GetByIdAsync(carId);
            return car != null && car.RentalStatus != "Rented";
        }

        public async Task<Car> GetCarWithOwnerAsync(int carId)
        {
            return await _dbSet
                .Include(c => c.Owner)
                .FirstAsync(c => c.Id == carId);
        }
    }
}