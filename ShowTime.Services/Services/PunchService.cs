﻿using ShowTime.Core.DTO;
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

            var punchResponse = await _punchRepository.AddPunch(punch);
            
            return punchResponse;
        }

        public async Task<string> GetPunchStatus(Guid userId)
        {
            return await _punchRepository.GetPunchStatus(userId);
        }
    }
}
