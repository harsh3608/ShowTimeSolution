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
    }
}
