using AutoMapper;
using ToDoLIST.Api.Dtos;
using ToDoLIST.Api.Entities;

namespace ToDoLIST.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserUpdateDto>().ReverseMap();
           
        }
    }
}
