namespace BookParserAPI.Repository.Tag;

public interface ITagRepository
{
    public Task<Models.Tag?> GetByName(string name);

    public Task<Models.Tag> Create(Models.Tag tag);
}