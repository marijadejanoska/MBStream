using AutoMapper;
using MBStream.Models;
using MBStream.DTOs;
using MBStream.Models;

namespace MBStream.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserResponseDTO>().ReverseMap();

            // Artist mappings
            CreateMap<Artist, ArtistDTO>().ReverseMap();
            CreateMap<Artist, ArtistResponseDTO>().ReverseMap();

        }
    }
}
