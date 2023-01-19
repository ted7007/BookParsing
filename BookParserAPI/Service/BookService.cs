using AutoMapper;
using BookParserAPI.Models;
using BookParserAPI.Repository.Product;
using BookParserAPI.Service.Argument.Book;

namespace BookParserAPI.Service;

public class BookService : IBookService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public BookService(IMapper mapper, IProductRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<Book> CreateAsync(CreateBookArgument argument)
    {
        var mappedProduct = _mapper.Map<CreateBookArgument, Book>(argument);
        var result = await _repository.CreateAsync(mappedProduct);
        return result;
    }

    public async Task UpdateAsync(UpdateBookArgument argument)
    {
        var mappedProduct = _mapper.Map<UpdateBookArgument, Book>(argument);
        await _repository.UpdateAsync(mappedProduct);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Book?> GetAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }
}