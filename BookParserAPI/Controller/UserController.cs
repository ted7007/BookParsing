using AutoMapper;
using BookParserAPI.Dto.Input.User;
using BookParserAPI.Dto.Output.User;
using BookParserAPI.Models;
using BookParserAPI.Service.Argument.User;
using BookParserAPI.Service.User;
using Microsoft.AspNetCore.Mvc;

namespace BookParserAPI.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
    {
        var mappedArgument = _mapper.Map<RegisterDto, CreateUserArgument>(dto);
        var result = await _userService.CreateAsync(mappedArgument);
        var mappedResult = _mapper.Map<User, UserDto>(result);
        return new OkObjectResult(mappedResult);
    }
}