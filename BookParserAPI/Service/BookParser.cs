using AngleSharp;
using BookParserAPI.Service.Book;
using BookParserAPI.Service.ISBN;

namespace BookParserAPI.Service;

public class BookParser : IBookParser
{
    private readonly IBookService _bookService;
    private readonly IISBNService _isbnService;
    private readonly string _igraSlovAddress = "https://igraslov.store/shop/?products-per-page=all";//"https://igraslov.store/shop/page/1";
    private readonly IBrowsingContext _context;

    public BookParser(IBookService bookService, IISBNService isbnService)
    {
        _bookService = bookService;
        _isbnService = isbnService;
        var config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
    }

    public Task LoadFromChitayGorod()
    {
        throw new NotImplementedException();
    }

    public async Task LoadISBNs()
    {
        var document = await _context.OpenAsync(_igraSlovAddress);
        var arrayWithBookElements =
            document.GetElementsByClassName(
                    "products oceanwp-row clr grid tablet-col tablet-3-col mobile-col mobile-2-col infinite-scroll-wrap")[0]
                .Children;
        foreach (var i in arrayWithBookElements)
        {
            var refToBook = i.GetElementsByClassName("title")[0]
                .GetElementsByTagName("a")[0]
                .Attributes.GetNamedItem("href");
            if(refToBook is null)
                continue;
            var refValue = refToBook.Value;
            var pageWithBook = await _context.OpenAsync(refValue);
            var isbnTabElements = pageWithBook
                .GetElementsByClassName(
                    "woocommerce-product-attributes-item woocommerce-product-attributes-item--attribute_isbnissn");
     
            if(!isbnTabElements.Any())
                continue;
     
            var isbnTab = isbnTabElements[0];
     
            var isbnElements = isbnTab.GetElementsByTagName("p");
     
            if(!isbnTabElements.Any())
                continue;

            var isbn = new Models.ISBN { value = isbnElements[0].TextContent };
            await _isbnService.CreateAsync(isbn);

        }

    }
    
    
    
    
    
}