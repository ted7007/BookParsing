using AutoMapper;
using BookParserAPI.Dto.Input.Book;
using BookParserAPI.Dto.Output.Book;
using BookParserAPI.Models;
using BookParserAPI.Service;
using BookParserAPI.Service.Argument.Book;
using BookParserAPI.Service.Book;
using BookParserAPI.Service.ISBN;
using Microsoft.AspNetCore.Mvc;

namespace BookParserAPI.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IHostedService _parser;
    private readonly IISBNService _isbnService;
    private readonly IMapper _mapper;
    private readonly ILogger<BookController> _logger;

    public BookController(IBookService bookService, IHostedService parser, IISBNService isbnService, IMapper mapper, ILogger<BookController> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _bookService = bookService;
        _parser = parser;
        _isbnService = isbnService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllAsync()
    {
        var products = await _bookService.GetAllAsync();
        var mappedBooks = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(products);
        return new OkObjectResult(mappedBooks);
    }
    
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateAsync(CreateBookDto argument)
    {
        var mappedArgument = _mapper.Map<CreateBookDto, CreateBookArgument>(argument);
        var result = await _bookService.CreateAsync(mappedArgument);
        var mappedResult = _mapper.Map<Book, BookDto>(result);
        return new OkObjectResult(mappedResult);
    }

    [HttpGet("ISBN")]
    public async Task<IActionResult> GetAllISBN()
    {
        var result = await _isbnService.GetAllAsync();
        return new OkObjectResult(result);
    }

}