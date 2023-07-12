

using ShowTime.Core.DTO;
using ShowTime.Core.Models;

namespace ShowTime.Infrastructure.IRepositories
{
    public interface ILeaveRepository
    {
        Task<LeaveDTO> AddLeaveRequest(LeaveAddRequest request);

        Task<LeaveDTO> DeleteLeaveRequest(Guid leaveId);
    }
}
