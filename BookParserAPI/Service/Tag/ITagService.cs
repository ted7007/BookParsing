using BookParserAPI.Models;

namespace BookParserAPI.Service;

public interface ITagService
{
    public Task<Tag?> GetByName(string name);

    public Task<Tag> Create(Tag argument);
}