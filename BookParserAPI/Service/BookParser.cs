using AngleSharp;
using BookParserAPI.Service.Argument.Book;
using BookParserAPI.Service.Book;
using BookParserAPI.Service.ISBN;

namespace BookParserAPI.Service;

public class BookParser : BackgroundService
{
    private readonly IBookService _bookService;
    private readonly IISBNService _isbnService;
    private readonly ILogger<BookParser> _logger;
    private readonly string _igraSlovAddress ="https://igraslov.store/shop/?products-per-page=all";//"https://igraslov.store/shop/page/1"; 
    private readonly string _chitayGorodAddress = "https://new.chitai-gorod.ru/search?q=";
    private readonly IBrowsingContext _context;

    public BookParser(IBookService bookService, IISBNService isbnService, ILogger<BookParser> logger)
    {
        _bookService = bookService;
        _isbnService = isbnService;
        _logger = logger;
        var config = Configuration.Default.WithDefaultLoader();
        _context = BrowsingContext.New(config);
    }

    public async Task LoadFromChitayGorod()
    {
        var isbns = await _isbnService.GetAllAsync();
        foreach (var i in isbns)
        {
            var document = await _context.OpenAsync($"https://old.chitai-gorod.ru/search/result/?q={i.value}&page=1");
            var textWithEmptyResult =
                document.GetElementsByClassName("fs_2 empty_search_result js__empty_search_message")[0].TextContent;
            
            var textWitHResultSearch = document.GetElementsByClassName("count-result__value js__searched_goods_count")[0].TextContent;
            if(textWithEmptyResult.Contains("нет"))
                            continue;
            int count;
            bool res = Int32.TryParse(textWitHResultSearch.Split(' ')[4],out count); // is book found
            if(!res)
                continue;
            var refToBookInfo = document.GetElementsByClassName("product-card__title")[0].Attributes
                .GetNamedItem("href").Value;
            var bookDocument = await _context.OpenAsync(refToBookInfo);
            
            ICollection<Models.Tag> tags = new List<Models.Tag>();
            bookDocument.GetElementsByClassName("breadcrumbs__item").Select(d =>
            {
                Models.Tag tag = new Models.Tag()
                {
                    Name = d.GetElementsByTagName("span")[0].TextContent
                };
                tags.Add(tag);
                return tag;
            });
            
             var listDetails =
                bookDocument.GetElementsByClassName("product-detail-characteristics__item-value");
            int pagesCount = 0;
            if (listDetails.Length >= 5)
            {
                Int32.TryParse(listDetails[4].TextContent, out pagesCount);
            }

            string year = "";
            if (listDetails.Length >= 3)
            {
                year = listDetails[2].TextContent;
            }

            CreateBookArgument book = new CreateBookArgument()
            {
                Author = bookDocument.GetElementsByClassName("product-detail-title__author")[0].TextContent,
                Description = bookDocument.GetElementsByClassName("product-detail-additional__description")[0]
                    .TextContent,
                Tags = tags,
                Genre = tags.Last().Name,
                ISBN = i.value,
                Name = bookDocument.GetElementsByClassName(
                    "app-title app-title--mounted product-detail-title__header app-title--header-4")[0].TextContent,
                PagesCount = pagesCount,
                Year = year

            };
            await _bookService.CreateAsync(book);

        }
    }
    
    public async Task LoadISBNs()
    {
        // i take ALL pages with books, for each take ref to bookInfo, where i take isbn.
        var document = await _context.OpenAsync(_igraSlovAddress);
        _logger.LogInformation("got page");
        var arrayWithBookElements =
            document.GetElementsByClassName(
                    "products oceanwp-row clr grid tablet-col tablet-3-col mobile-col mobile-2-col infinite-scroll-wrap")[0]
                .Children;
        int counter = 0;
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
            counter++;
            if (counter>1000)
                break;
            _logger.LogInformation("isbn created");
        }
        _logger.LogInformation("isbn downloaded");

    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task first = Task.Run(() => LoadISBNs());
        Task second = Task.Run(
            () =>
            {
                first.Wait();
                LoadFromChitayGorod();
            });
        return Task.WhenAll(second);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("load stopped");
        return Task.CompletedTask;
    }
}