using AutoMapper;
using ShowTime.Core.DTO;
using ShowTime.Core.Entities;
using ShowTime.Core.Models;

namespace ShowTime.API.MappingProfile
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {

            CreateMap<Punch, PunchAddRequest>().ReverseMap();

            CreateMap<Punch, PunchDTO>().ReverseMap();


        }
    }
}
