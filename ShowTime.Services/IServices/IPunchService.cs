﻿using ShowTime.Core.DTO;
using ShowTime.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Services.IServices
{
    public interface IPunchService
    {
        Task<PunchDTO> AddPunch(PunchAddRequest punch);

        Task<string> GetPunchStatus(Guid userId);
    }
}
