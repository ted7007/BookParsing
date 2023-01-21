using AutoMapper;
using BookParserAPI.Repository.Book;
using BookParserAPI.Service.Argument.Book;
using BookParserAPI.Service.Tag;

namespace BookParserAPI.Service.Book;

public class BookService : IBookService
{
    private readonly IMapper _mapper;
    private readonly IBookRepository _repository;
    private readonly ITagService _tagService;

    public BookService(IMapper mapper, IBookRepository repository, ITagService tagService)
    {
        _mapper = mapper;
        _repository = repository;
        _tagService = tagService;
    }


    public async Task<Models.Book> CreateAsync(CreateBookArgument argument)
    {
        var result = await _repository.GetAsync(argument.ISBN);
        if (!(result is null))
            return result;
        var mappedBook = _mapper.Map<CreateBookArgument, Models.Book>(argument); 
        var bookForCreation = await ProcessBookForCreatingAndGetAsync(mappedBook);
        result = await _repository.CreateAsync(bookForCreation);
        return result;
    }

    public async Task UpdateAsync(UpdateBookArgument argument)
    {
        var mappedBook = _mapper.Map<UpdateBookArgument, Models.Book>(argument);
        await _repository.UpdateAsync(mappedBook);
    }

    public async Task<IEnumerable<Models.Book>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Models.Book?> GetAsync(string ISBN)
    {
        return await _repository.GetAsync(ISBN);
    }

    /// <summary>
    /// Синхронизация тегов с БД
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    public async Task<Models.Book> ProcessBookForCreatingAndGetAsync(Models.Book book)
    {
        for (int i = 0; i < book.Tags.Count; i++)
        {
            var curTag = book.Tags.First();
            book.Tags.Remove(curTag);
            var existsTag = await _tagService.GetByNameAsync(curTag.Name);
            
            if (existsTag is null)
            {
                var newTag = await _tagService.Create(curTag);
                book.Tags.Add(newTag);
                continue;
            }
            
            book.Tags.Add(existsTag);
        }

        return book;
    }
}