using AutoMapper;
using BookParserAPI.Repository.User;
using BookParserAPI.Service.Argument.User;

namespace BookParserAPI.Service.User;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<Models.User> CreateAsync(CreateUserArgument argument)
    {
        var mappedUser = _mapper.Map<CreateUserArgument, Models.User>(argument);
        var result = await _repository.CreateAsync(mappedUser);
        return result;
    }

    public Task<Models.User?> GetByLoginAsync(string login)
    {
        var user = _repository.GetByLoginAsync(login);
        return user;
    }
}