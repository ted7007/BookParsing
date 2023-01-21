using AutoMapper;
using BookParserAPI.Models;
using BookParserAPI.Repository.Book;
using BookParserAPI.Service.Argument.Book;

namespace BookParserAPI.Service;

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


    public async Task<Book> CreateAsync(CreateBookArgument argument)
    {
        var result = await _repository.GetAsync(argument.ISBN);
        if (!(result is null))
            return result;
        var mappedBook = _mapper.Map<CreateBookArgument, Book>(argument); 
        var bookForCreation = await ProcessBookForCreatingAndGetAsync(mappedBook);
        result = await _repository.CreateAsync(bookForCreation);
        return result;
    }

    public async Task UpdateAsync(UpdateBookArgument argument)
    {
        var mappedBook = _mapper.Map<UpdateBookArgument, Book>(argument);
        await _repository.UpdateAsync(mappedBook);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Book?> GetAsync(string ISBN)
    {
        return await _repository.GetAsync(ISBN);
    }

    /// <summary>
    /// Синхронизация тегов с БД
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    public async Task<Book> ProcessBookForCreatingAndGetAsync(Book book)
    {
        for (int i = 0; i < book.Tags.Count; i++)
        {
            var curTag = book.Tags.First();
            book.Tags.Remove(curTag);
            var existsTag = await _tagService.GetByName(curTag.Name);
            
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