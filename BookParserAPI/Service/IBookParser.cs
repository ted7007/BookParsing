namespace BookParserAPI.Service;

public interface IBookParser
{
    public Task LoadFromChitayGorod();

    public Task LoadISBNs();
}