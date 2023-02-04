using AutoMapper;
using BookParserAPI.Dto.Input.User;
using BookParserAPI.Dto.Output.User;
using BookParserAPI.Models;
using BookParserAPI.Security;
using BookParserAPI.Service.Argument.User;
using BookParserAPI.Service.User;
using Microsoft.AspNetCore.Mvc;

namespace BookParserAPI.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IAuthenticationService authService, IMapper mapper)
    {
        _userService = userService;
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Register(RegisterModel model)
    {
        var mappedArgument = _mapper.Map<RegisterModel, CreateUserArgument>(model);
        var result = await _userService.CreateAsync(mappedArgument);
        var mappedResult = _mapper.Map<User, UserDto>(result);
        return new OkObjectResult(mappedResult);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> LogIn(LoginModel model)
    {
        var token = await _authService.GetTokenAsync(model);
        if (token is null)
            return new UnauthorizedResult();
        return new OkObjectResult(token);
    }
}