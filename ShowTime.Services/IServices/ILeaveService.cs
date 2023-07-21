using ShowTime.Core.DTO;
using ShowTime.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Services.IServices
{
    public interface ILeaveService
    {
        Task<LeaveDTO> AddLeaveRequest(LeaveAddRequest request);

        Task<LeaveDTO> DeleteLeaveRequest(Guid leaveId);

        Task<IEnumerable<LeaveDTO>> GetAllLeaveRequests();

        Task<LeaveDTO> GetLeaveRequest(Guid leaveId);

        Task<IEnumerable<LeaveDTO>> GetUserAllLeaves(Guid UserId);

        Task<LeaveDTO> ToggleLeaveStatus(Guid leaveId, int value);
    }
}
