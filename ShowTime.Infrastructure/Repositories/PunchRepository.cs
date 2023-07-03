using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShowTime.Core.DTO;
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


        public PunchRepository(ApplicationDbContext applicationDbContext) 
        {
            applicationDbContext = _context;
        }

        public async Task<PunchDTO> AddPunch(PunchAddRequest punch)
        {

            await _context.Punches.AddAsync(punch);
            await _context.SaveChangesAsync();
        }
    }
}
