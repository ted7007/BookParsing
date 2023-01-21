namespace BookParserAPI.Repository.Tag;

public interface ITagRepository
{
    public Task<Models.Tag?> GetByNameAsync(string name);

    public Task<Models.Tag> CreateAsync(Models.Tag tag);
}