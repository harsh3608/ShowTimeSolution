using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Infrastructure.IRepositories
{
    public interface IPunchRepository
    {
        Task<PunchDTO> AddPunch(PunchAddRequest punch);

        Task<bool?> GetPunchStatus(Guid userId);

        Task<List<PunchDTO>> GetAllPunchedInUsers();

        Task<List<PunchDTO>> GetAllUserPunchesForToday(Guid userId);

        Task<TimeSpan> CalculateTotalPunchedInTime(Guid userId);

        Task<List<WorkingTimeDTO>> GetFiveDaysWorkingTime(Guid userId);
    }
}
