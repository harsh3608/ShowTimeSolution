﻿using ShowTime.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Infrastructure.IRepositories
{
    public interface IPunchRepository
    {
        Task<bool> AddPunch(PunchDTO punch);
    }
}