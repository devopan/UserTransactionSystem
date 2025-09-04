using AutoMapper;
using UserTransactionSystem.Domain.Entities;
using UserTransactionSystem.Services.DTOs;

namespace UserTransactionSystem.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // Transaction mappings
            CreateMap<CreateTransactionDto, Domain.Entities.Transaction>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}