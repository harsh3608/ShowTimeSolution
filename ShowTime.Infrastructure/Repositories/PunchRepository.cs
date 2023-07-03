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
        public PunchRepository(ApplicationDbContext applicationDbContext) 
        {
            applicationDbContext = _context;
        }
    }
}
