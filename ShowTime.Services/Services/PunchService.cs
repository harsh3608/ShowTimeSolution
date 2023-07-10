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
    public class PunchService: IPunchService
    {
        private readonly IPunchRepository _punchRepository;

        public PunchService(IPunchRepository punchRepository)
        {
            _punchRepository = punchRepository;
        }

        public async Task<PunchDTO> AddPunch(PunchAddRequest punch)
        {
            punch.Id = Guid.NewGuid();

            punch.PunchDateTime = DateTime.Now;

            var punchResponse = await _punchRepository.AddPunch(punch);
            
            return punchResponse;
        }

        public async Task<bool?> GetPunchStatus(Guid userId)
        {
            var status = await _punchRepository.GetPunchStatus(userId);

            return status;
        }

        public async Task<List<PunchDTO>> GetAllPunchedInUsers()
        {
            var punchedInUsers = await _punchRepository.GetAllPunchedInUsers();

            return punchedInUsers;
        }


        public async Task<List<PunchDTO>> GetAllUserPunchesForToday(Guid userId)
        {
            var punchesForToday = await _punchRepository.GetAllUserPunchesForToday(userId);

            return punchesForToday;
        }

        public async Task<TimeSpan> CalculateTotalPunchedInTime(Guid userId)
        {
            var punchedInTime = await _punchRepository.CalculateTotalPunchedInTime(userId);
            return punchedInTime;
        }


        public async Task<List<WorkingTimeDTO>> GetFiveDaysWorkingTime(Guid userId)
        {
            var workingTimes = await _punchRepository.GetFiveDaysWorkingTime(userId);

            return workingTimes;
        }

        public async Task<List<WorkingTimeDTO>> GetAllDaysWorkingTime(Guid userId)
        {
            var workingTimes = await _punchRepository.GetAllDaysWorkingTime(userId);

            return workingTimes;
        }
    }
}
