using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;
using ShowTime.Infrastructure.DatabaseContext;
using ShowTime.Infrastructure.IRepositories;
using System;

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
                .Where(p => p.UserId == userId && p.PunchDateTime >= fiveDaysAgo && p.PunchDateTime < currentDayEnd)
                .OrderBy(p => p.PunchDateTime)
                .ToListAsync();

            List<WorkingTimeDTO> workingTimes = new List<WorkingTimeDTO>();
            DateTime previousPunchDateTime = DateTime.MinValue;
            double totalWorkingHours = 0;

            foreach (var punch in punches)
            {
                if (previousPunchDateTime != DateTime.MinValue && punch.PunchDateTime.Date != previousPunchDateTime.Date)
                {
                    WorkingTimeDTO workingTime = new WorkingTimeDTO
                    {
                        Date = previousPunchDateTime.Date,
                        WorkingTime = totalWorkingHours
                    };

                    workingTimes.Add(workingTime);
                    totalWorkingHours = 0;
                }

                if (punch.PunchStatus)
                {
                    previousPunchDateTime = punch.PunchDateTime;
                }
                else if (previousPunchDateTime != DateTime.MinValue)
                {
                    totalWorkingHours += (punch.PunchDateTime - previousPunchDateTime).TotalHours;
                }
            }

            // Add the working time for the last day
            if (previousPunchDateTime != DateTime.MinValue)
            {
                WorkingTimeDTO workingTime = new WorkingTimeDTO
                {
                    Date = previousPunchDateTime.Date,
                    WorkingTime = totalWorkingHours
                };

                workingTimes.Add(workingTime);
            }

            return workingTimes;

        }



        public async Task<List<WorkingTimeDTO>> GetAllDaysWorkingTime(Guid userId)
        {
            DateTime currentDate = DateTime.Today;
            DateTime startDate = _context.Punches
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.PunchDateTime)
                .Select(p => p.PunchDateTime.Date)
                .FirstOrDefault();

            if (startDate == default)
            {
                return new List<WorkingTimeDTO>(); // No punches found, return an empty list
            }

            List<WorkingTimeDTO> workingTimes = new List<WorkingTimeDTO>();
            DateTime date = startDate.Date;
            double totalWorkingHours = 0;

            while (date <= currentDate)
            {
                DateTime nextDate = date.AddDays(1);

                var punches = await _context.Punches
                    .Where(p => p.UserId == userId && p.PunchDateTime >= date && p.PunchDateTime < nextDate)
                    .OrderBy(p => p.PunchDateTime)
                    .ToListAsync();

                if (punches.Count == 0)
                {
                    // No punches found for the current date, add a WorkingTimeDTO with zero working hours
                    WorkingTimeDTO workingTime = new WorkingTimeDTO
                    {
                        Date = date,
                        WorkingTime = 0
                    };
                    workingTimes.Add(workingTime);
                }
                else
                {
                    DateTime previousPunchDateTime = punches.First().PunchDateTime;

                    foreach (var punch in punches)
                    {
                        if (punch.PunchStatus)
                        {
                            previousPunchDateTime = punch.PunchDateTime;
                        }
                        else if (previousPunchDateTime != DateTime.MinValue)
                        {
                            totalWorkingHours += (punch.PunchDateTime - previousPunchDateTime).TotalHours;
                            previousPunchDateTime = DateTime.MinValue;
                        }
                    }

                    if (previousPunchDateTime != DateTime.MinValue)
                    {
                        // A punch-out is missing for the last punch-in of the day
                        totalWorkingHours += (nextDate - previousPunchDateTime).TotalHours;
                    }

                    WorkingTimeDTO workingTime = new WorkingTimeDTO
                    {
                        Date = date,
                        WorkingTime = totalWorkingHours
                    };

                    workingTimes.Add(workingTime);
                }

                date = nextDate;
                totalWorkingHours = 0; // Reset totalWorkingHours for the next day
            }

            return workingTimes;
        }




    }
}
