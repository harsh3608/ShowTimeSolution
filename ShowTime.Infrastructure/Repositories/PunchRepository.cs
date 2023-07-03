using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;
using ShowTime.Infrastructure.DatabaseContext;
using ShowTime.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Infrastructure.Repositories
{
    public class PunchRepository: IPunchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public PunchRepository(ApplicationDbContext applicationDbContext, IMapper mapper) 
        {
            applicationDbContext = _context;
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

        public async Task<string?> GetPunchStatus(Guid userId)
        {
            var latestpunch = await _context.Punches.LastOrDefaultAsync(x => x.UserId == userId);

            if (latestpunch == null)
            {
                return null;
            }

            return latestpunch.PunchStatus;
        }
    }
}
