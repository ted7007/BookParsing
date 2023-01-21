using AutoMapper;
using BookParserAPI.Models;
using BookParserAPI.Repository.Tag;

namespace BookParserAPI.Service;

public class TagService : ITagService
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public TagService(ITagRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<Models.Tag?> GetByName(string name)
    {
        var result = await _repository.GetByName(name);
        return result;
    }

    public async Task<Models.Tag> Create(Tag tag)
    {
        var existsTag = await _repository.GetByName(tag.Name);
        if (existsTag != null)
            return existsTag;
        var result = await _repository.Create(tag);
        return result;
    }
}