﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;
using ShowTime.Infrastructure.DatabaseContext;
using ShowTime.Infrastructure.IRepositories;

namespace ShowTime.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LeaveRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LeaveDTO> AddLeaveRequest(LeaveAddRequest request)
        {
            Leave leave = new Leave();

            _mapper.Map(request, leave);
            leave.Id = Guid.NewGuid();
            leave.DateOfRequest = DateTime.Now;

            await _context.Leaves.AddAsync(leave);
            await _context.SaveChangesAsync();

            LeaveDTO leaveDTO = new LeaveDTO();
            _mapper.Map(leave, leaveDTO);
            return leaveDTO;
        }


        public async Task<LeaveDTO> DeleteLeaveRequest(Guid leaveId)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(l => l.Id == leaveId);

            if (leave == null) { return null; }

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();

            LeaveDTO leaveDTO = new LeaveDTO();
            _mapper.Map(leave, leaveDTO);
            return leaveDTO;
        }

        public async Task<IEnumerable<LeaveDTO>> GetAllLeaveRequests()
        {
            var leaves = await _context.Leaves.OrderByDescending(x=>x.DateOfRequest).ToListAsync();
            var result = _mapper.Map<List<LeaveDTO>>(leaves);
            return result;
        }


        public async Task<LeaveDTO> GetLeaveRequest(Guid leaveId)
        {
            var leaveRequest = await _context.Leaves.OrderBy(x => x.DateOfRequest).FirstOrDefaultAsync(leave => leave.Id == leaveId);


            LeaveDTO leaveDTO = new LeaveDTO();
            _mapper.Map(leaveRequest, leaveDTO);
            return leaveDTO;
        }

        public async Task<IEnumerable<LeaveDTO>> GetUserAllLeaves(Guid UserId)
        {
            var leaves = await _context.Leaves.Where(x => x.UserId == UserId).OrderByDescending(x => x.DateOfRequest).ToListAsync();

            var result = _mapper.Map<List<LeaveDTO>>(leaves);
            return result;
        }

        public async Task<LeaveDTO> ToggleLeaveStatus(Guid leaveId, int value)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(x => x.Id == leaveId);

            if (leave != null)
            {
                leave.Status = value;
                await _context.SaveChangesAsync();

                LeaveDTO leaveDTO = new LeaveDTO();
                _mapper.Map(leave, leaveDTO);
                return leaveDTO;
            }
            else
            {
                return null;
            }

        }


    }
}
