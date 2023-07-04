﻿using AutoMapper;
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


    }
}
