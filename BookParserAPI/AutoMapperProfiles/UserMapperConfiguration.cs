using AutoMapper;
using BookParserAPI.Dto.Output.User;
using BookParserAPI.Models;

namespace BookParserAPI.AutoMapperProfiles;

public class UserMapperConfiguration : Profile
{
    public UserMapperConfiguration()
    {
        CreateMap<User, UserDto>();
    }
}