using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;
using ShowTime.Infrastructure.DatabaseContext;
using ShowTime.Infrastructure.IRepositories;

namespace ShowTime.Infrastructure.Repositories
{
    public class PunchRepository : IPunchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public PunchRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _context = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<PunchDTO> AddPunch(PunchAddRequest punchAddRequest)
        {
            Punch punch = new Punch();

            _mapper.Map(punchAddRequest, punch);

            await _context.Punches.AddAsync(punch);
            await _context.SaveChangesAsync();

            PunchDTO punchDTO = new PunchDTO();

            _mapper.Map(punch, punchDTO);
            return punchDTO;
        }

        public async Task<bool?> GetPunchStatus(Guid userId)
        {
            var latestPunchStatus = await _context.Punches
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.PunchDateTime)
            .Select(p => p.PunchStatus)
            .FirstOrDefaultAsync();

            return latestPunchStatus;
        }

        public async Task<List<PunchDTO>> GetAllPunchedInUsers()
        {
            DateTime today = DateTime.Today;

            var punches = await _context.Punches.FromSqlRaw("GetAllPunchedInUsers").ToListAsync();



            var latestPunchedInUsers = _mapper.Map<List<PunchDTO>>(punches);
            return latestPunchedInUsers;
        }

        public async Task<List<PunchDTO>> GetAllUserPunchesForToday(Guid userId)
        {
            var currentDate = DateTime.Today;

            var punchDetails = await _context.Punches
                .Where(p => p.UserId == userId && p.PunchDateTime >= currentDate)
                .OrderByDescending(p => p.PunchDateTime)
                .ToListAsync();

            var punchesForToday = _mapper.Map<List<PunchDTO>>(punchDetails);

            return punchesForToday;
        }


        public async Task<TimeSpan> CalculateTotalPunchedInTime(Guid userId)
        {
            DateTime currentDate = DateTime.Today;
            DateTime currentDayStart = currentDate.Date;
            DateTime currentDayEnd = currentDayStart.AddDays(1);

            var punches = await _context.Punches
                .Where(p => p.UserId == userId && p.PunchStatus && p.PunchDateTime >= currentDayStart && p.PunchDateTime < currentDayEnd)
                .OrderBy(p => p.PunchDateTime)
                .ToListAsync();

            TimeSpan totalPunchedInTime = TimeSpan.Zero;
            DateTime? previousPunchDateTime = null;

            foreach (var punch in punches)
            {
                if (previousPunchDateTime.HasValue)
                {
                    TimeSpan duration = punch.PunchDateTime - previousPunchDateTime.Value;
                    totalPunchedInTime += duration;
                }

                previousPunchDateTime = punch.PunchDateTime;
            }

            return totalPunchedInTime;
        }


        public async Task<List<WorkingTimeDTO>> GetFiveDaysWorkingTime(Guid userId)
        {
            DateTime currentDate = DateTime.Today;
            DateTime fiveDaysAgo = currentDate.AddDays(-5).Date;
            DateTime currentDayStart = currentDate.Date;
            DateTime currentDayEnd = currentDayStart.AddDays(1);

            var punches = await _context.Punches
                .Where(p => p.UserId == userId && p.PunchStatus && p.PunchDateTime >= fiveDaysAgo && p.PunchDateTime < currentDayEnd)
                .OrderBy(p => p.PunchDateTime)
                .ToListAsync();

            List<WorkingTimeDTO> workingTimes = new List<WorkingTimeDTO>();
            DateTime? previousPunchDateTime = null;

            foreach (var punch in punches)
            {
                if (previousPunchDateTime.HasValue)
                {
                    TimeSpan duration = punch.PunchDateTime - previousPunchDateTime.Value;
                    WorkingTimeDTO workingTime = new WorkingTimeDTO
                    {
                        Date = previousPunchDateTime.Value.Date,
                        WorkingTime = duration
                    };

                    if (!workingTimes.Any(w => w.Date == workingTime.Date))
                    {
                        workingTimes.Add(workingTime);
                    }
                }

                previousPunchDateTime = punch.PunchDateTime;
            }

            return workingTimes;
        }

    }
}
