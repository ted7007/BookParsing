using AutoMapper;
using BookParserAPI.Dto.Output.Product;
using BookParserAPI.Models;
using BookParserAPI.Service;
using BookParserAPI.Service.Argument.Product;
using Microsoft.AspNetCore.Mvc;

namespace BookParserAPI.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _service;
    private readonly IMapper _mapper;

    public BookController(IBookService service, IMapper mapper)
    {
        _mapper = mapper;
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllAsync()
    {
        var products = await _service.GetAllAsync();
        var mappedProducts = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(products);
        return new OkObjectResult(mappedProducts);
    }

}