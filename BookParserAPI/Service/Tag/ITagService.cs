namespace BookParserAPI.Service.Tag;

public interface ITagService
{
    public Task<Models.Tag?> GetByNameAsync(string name);

    public Task<Models.Tag> Create(Models.Tag argument);
}