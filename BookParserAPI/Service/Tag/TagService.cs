using AutoMapper;
using BookParserAPI.Repository.Tag;

namespace BookParserAPI.Service.Tag;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public TagService(ITagRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<Models.Tag?> GetByNameAsync(string name)
    {
        var result = await _repository.GetByNameAsync(name);
        return result;
    }

    public async Task<Models.Tag> Create(Models.Tag tag)
    {
        var existsTag = await _repository.GetByNameAsync(tag.Name);
        if (existsTag != null)
            return existsTag;
        var result = await _repository.CreateAsync(tag);
        return result;
    }
}