using ShowTime.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.Services.IServices
{
    public interface IPunchService
    {
        Task<bool> AddPunch(PunchDTO punch);
    }
}
