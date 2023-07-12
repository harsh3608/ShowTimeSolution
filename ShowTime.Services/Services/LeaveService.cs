using ShowTime.Core.DTO;
using ShowTime.Core.Models;
using ShowTime.Infrastructure.IRepositories;
using ShowTime.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Services.Services
{
    public class LeaveService: ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }


        public async Task<LeaveDTO> AddLeaveRequest(LeaveAddRequest request)
        {
            var leave = await _leaveRepository.AddLeaveRequest(request);
            return leave;
        }

        public async Task<LeaveDTO> DeleteLeaveRequest(Guid leaveId)
        {
            var leave = await _leaveRepository.DeleteLeaveRequest(leaveId);
            return leave;
        }

        public async Task<IEnumerable<LeaveDTO>> GetAllLeaveRequests()
        {
            var leaves = await _leaveRepository.GetAllLeaveRequests();
            return leaves;
        }

        public async Task<LeaveDTO> GetLeaveRequest(Guid leaveId)
        {
            var leave = await _leaveRepository.GetLeaveRequest(leaveId);
            return leave;
        }

        public async Task<IEnumerable<LeaveDTO>> GetUserAllLeaves(Guid UserId)
        {
            var leaves = await _leaveRepository.GetUserAllLeaves(UserId);
            return leaves;
        }
    }
}
