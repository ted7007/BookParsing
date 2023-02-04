﻿using AutoMapper;
using BookParserAPI.Dto.Input.User;
using BookParserAPI.Models;
using BookParserAPI.Service.Argument.User;

namespace BookParserAPI.AutoMapperProfiles;

public class CreateUserMapperConfiguration : Profile
{
    public CreateUserMapperConfiguration()
    {
        CreateMap<RegisterDto, CreateUserArgument>();
        CreateMap<CreateUserArgument, User>();
    }
}