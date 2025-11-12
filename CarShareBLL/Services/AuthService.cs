using Microsoft.Extensions.Logging;
using CarShareBLL.DTOs;
using CarShareDAL.Models;
using CarShareDAL.Repositories;

namespace CarShareBLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IRepository<User> userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (await _userRepository.EmailExistsAsync(registerDto.Email))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    };
                }
                var userRole = registerDto.Role == "CarOwner" ? "CarOwner" : "Renter";

                var user = new User
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email.ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    PhoneNumber = registerDto.PhoneNumber,
                    Role = userRole,
                    IsApproved = userRole == "CarOwner" ? false : true,
                    CreatedAt = DateTime.UtcNow,
                    LicenseNumber = registerDto.LicenseNumber,
                    NationalId = registerDto.NationalId,
                    DocumentUrl = registerDto.DocumentUrl
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                _logger.LogInformation($"New user registered: {user.Email}");

                var message = userRole == "CarOwner"
                    ? "Registration successful! Your account is pending admin approval."
                    : "Registration successful! You can now log in.";

                return new AuthResponseDto
                {
                    Success = true,
                    Message = message,
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "An error occurred during registration"
                };
            }
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(loginDto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                if (!user.IsApproved)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Your account has not been approved yet"
                    };
                }

                _logger.LogInformation($"User logged in: {user.Email}");

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "An error occurred during login"
                };
            }
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    _logger.LogInformation($"User logged out: {user.Email}");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return false;
            }
        }



        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? MapToUserDto(user) : null;
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsApproved = user.IsApproved,
                CreatedAt = user.CreatedAt,
                LicenseNumber = user.LicenseNumber,
                NationalId = user.NationalId,
                DocumentUrl = user.DocumentUrl
            };
        }
    }  
}
