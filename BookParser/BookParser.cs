using System.Text;
using System.Text.Json;
using AngleSharp;
using AngleSharp.Dom;

namespace BookParser;

public class BookParser
{
    private readonly string _igraSlovAddress = "https://igraslov.store/shop/?products-per-page=all";
    private readonly string _labirintAddress = "https://new.chitai-gorod.ru/search?q=";
    private readonly HttpClient _client;
    private readonly IBrowsingContext _context;


    public BookParser()
    {
        _client = new HttpClient();
        _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
    }
    
    private async Task<List<string>> ParseISBNSAsync(int maxCount)
    {
        var isbns = new List<string>();
        var document = await _context.OpenAsync(_igraSlovAddress);
        Console.WriteLine("got page");
        
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
            var isbn = isbnElements[0].TextContent;
     
            isbns.Add(isbn);
            counter++;
            
            Console.WriteLine("isbn created");
            if (counter>maxCount)
                break;
        }

        Console.WriteLine("isbns downloaded");
        return isbns;
    }

    private async Task ParseLabirintAndSendResultToAPIAsync(List<string> isbns)
    {
        
        foreach (var i in isbns)
        {
            try
            {
                var document2 = await _context.OpenAsync($"https://www.labirint.ru/search/{i}/?stype=0");


                var textWitHResultSearchElements =
                    document2.GetElementsByClassName("b-stab-e-slider-item-e-txt-m-small js-search-tab-count");
                if (!(textWitHResultSearchElements.Length > 0))
                    continue;
                var refToBookInfo = "https://www.labirint.ru" + document2.GetElementsByClassName("cover")[0].Attributes
                    .GetNamedItem("href").Value;
                var bookDocument = await _context.OpenAsync(refToBookInfo);

                ICollection<BookParserAPI.Models.Tag> tags = new List<BookParserAPI.Models.Tag>();
                var elem = bookDocument
                    .GetElementById("thermometer-books")
                    .GetElementsByTagName("span")
                    .Select(d =>
                    {
                        if (!(d.Attributes["itemprop"] is null))
                        {
                            BookParserAPI.Models.Tag tag = new BookParserAPI.Models.Tag()
                            {
                                Name = d.TextContent
                            };
                            tags.Add(tag);
                        }

                        return 1;
                    });

                var pages = GetValue(bookDocument.GetElementsByClassName("pages2")).Split(' ')[1];
                int pagesCount = Int32.Parse(pages);

                string[] yearStr = GetValue(bookDocument.GetElementsByClassName("publisher")).Split(' ');
                string year = "";
                if(yearStr.Length>2)
                    year = yearStr[^2];

                var authorInfo = GetValue(bookDocument.GetElementsByClassName("authors")).Split(' ');
                var author = "No";
                if(authorInfo.Length > 0)
                    author = authorInfo[^2] + " " + authorInfo[^1];

                var genreItem1 = GetValue(bookDocument.GetElementsByClassName("genre"));
                var genreItem2 = GetValue(bookDocument.GetElementsByClassName("collections"));
                var genre = genreItem1 == "" ?  genreItem2 : genreItem1;
                BookParserAPI.Models.Book book = new BookParserAPI.Models.Book()
                {
                    Author = author,
                    Description = bookDocument.GetElementById("fullannotation").GetElementsByTagName("p")[0].TextContent,
                    Tags = tags,
                    Genre =  genre,
                    ISBN = i,
                    Name = bookDocument.GetElementById("product-title").GetElementsByTagName("h1")[0].TextContent,
                    PagesCount = pagesCount,
                    Year = year
                };

                var bookJson = JsonSerializer.Serialize(book);
                var response = await _client.PostAsync("http://ted7007-001-site1.dtempurl.com/api/v1/book",
                    new StringContent(bookJson, Encoding.UTF8, "application/json"));
                Console.WriteLine($"Book {book.Name} parsed with code {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} : ERROR - {ex.Message}");
            }
        }
    }
    
    public async Task StartParsingAsync(int maxCountISBNs)
    {
        var isbns = await ParseISBNSAsync(maxCountISBNs);
        await ParseLabirintAndSendResultToAPIAsync(isbns);
    }
    
    private string GetValue(IHtmlCollection<IElement> collection)
    {
        if (collection.Length > 0)
            return collection[0].TextContent;
        return "";
    }
}