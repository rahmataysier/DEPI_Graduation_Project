using AutoMapper;
using CarShareBLL.DTOs.AdminDTOs;
using CarShareBLL.Interfaces;
using CarShareDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareBLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public AdminService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarOwnerPendingDto>> GetPendingCarOwnersAsync()
        {
            var owners = await _userRepo.GetPendingOwnersAsync();
            return _mapper.Map<IEnumerable<CarOwnerPendingDto>>(owners);
        }

        public async Task<CarOwnerApprovalResponseDto?> ApproveOrRejectCarOwnerAsync(CarOwnerApprovalRequestDto request)
        {
            var owner = await _userRepo.GetByIdAsync(request.CarOwnerId);
            if (owner == null || owner.Role != "Owner")
                return null;

            owner.IsApproved = request.IsApproved;
            await _userRepo.UpdateAsync(owner);

            return new CarOwnerApprovalResponseDto
            {
                CarOwnerId = owner.Id,
                FullName = owner.FullName,
                IsApproved = owner.IsApproved,
                AdminComment = request.AdminComment,
                ActionDate = DateTime.UtcNow
            };
        }

        public async Task<IEnumerable<CarOwnerPendingDto>> GetAllCarOwnersAsync()
        {
            var owners = await _userRepo.GetAllOwnersAsync();
            return _mapper.Map<IEnumerable<CarOwnerPendingDto>>(owners);
        }
    }
}
