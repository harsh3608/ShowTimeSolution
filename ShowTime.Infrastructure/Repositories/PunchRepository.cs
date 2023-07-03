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
            var latestpunch = await _context.Punches.LastOrDefaultAsync(x => x.UserId == userId);

            if (latestpunch == null)
            {
                return null;
            }

            return latestpunch.PunchStatus;
        }

        public async Task<List<PunchDTO>> GetAllPunchedInUsers()
        {
            var PunchedInUsers = await _context.Punches.Where(x => x.PunchStatus == true && x.PunchDateTime == DateTime.Today).ToListAsync();

            if (PunchedInUsers == null) { return null; }

            List<PunchDTO> PunchedInUsersList = _mapper.Map<List<PunchDTO>>(PunchedInUsers);

            return PunchedInUsersList;
        }
    }
}
