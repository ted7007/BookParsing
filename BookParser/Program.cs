// i take ALL pages with books, for each take ref to bookInfo, where i take isbn.

using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;

string _igraSlovAddress = "https://igraslov.store/shop/?products-per-page=all";
string _chitayGorodAddress = "https://new.chitai-gorod.ru/search?q=";
var isbns = new List<BookParserAPI.Models.ISBN>();

var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
var document = await context.OpenAsync(_igraSlovAddress);

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
    var pageWithBook = await context.OpenAsync(refValue);
    var isbnTabElements = pageWithBook
        .GetElementsByClassName(
            "woocommerce-product-attributes-item woocommerce-product-attributes-item--attribute_isbnissn");
    if(!isbnTabElements.Any())
        continue;
    var isbnTab = isbnTabElements[0];
    var isbnElements = isbnTab.GetElementsByTagName("p");
    if(!isbnTabElements.Any())
        continue;
    var isbn = new BookParserAPI.Models.ISBN { value = isbnElements[0].TextContent };
     
    isbns.Add(isbn);
    counter++;
    if (counter>1000)
        break;
    Console.WriteLine("isbn created");
}

Console.WriteLine("isbn downloaded");
HttpClient client = new HttpClient();   

foreach (var i in isbns)
{
    try
    {
        var document2 = await context.OpenAsync($"https://www.labirint.ru/search/{i.value}/?stype=0");


        var textWitHResultSearchElements =
            document2.GetElementsByClassName("b-stab-e-slider-item-e-txt-m-small js-search-tab-count");
        if (!(textWitHResultSearchElements.Length > 0))
            continue;
        var refToBookInfo = "https://www.labirint.ru" + document2.GetElementsByClassName("cover")[0].Attributes
            .GetNamedItem("href").Value;
        var bookDocument = await context.OpenAsync(refToBookInfo);

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
            ISBN = i.value,
            Name = bookDocument.GetElementById("product-title").GetElementsByTagName("h1")[0].TextContent,
            PagesCount = pagesCount,
            Year = year
        };

        var bookJson = JsonSerializer.Serialize(book);
        var response = await client.PostAsync("http://ted7007-001-site1.dtempurl.com/api/v1/book",
            new StringContent(bookJson, Encoding.UTF8, "application/json"));
        Console.WriteLine($"Book {book.Name} parsed with code {response.StatusCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{DateTime.Now} : ERROR - {ex.Message}");
    }
}

static string GetValue(IHtmlCollection<IElement> collection)
{
    if (collection.Length > 0)
        return collection[0].TextContent;
    return "";
}
//send book

// private async Task LoadFromChitayGorod()
//     {
//         var isbns = await _isbnService.GetAllAsync();
//         foreach (var i in isbns)
//         {
//             var document = await _context.OpenAsync($"https://old.chitai-gorod.ru/search/result/?q={i.value}&page=1");
//             var textWithEmptyResult =
//                 document.GetElementsByClassName("fs_2 empty_search_result js__empty_search_message")[0].TextContent;
//             
//             var textWitHResultSearch = document.GetElementsByClassName("count-result__value js__searched_goods_count")[0].TextContent;
//             if(textWithEmptyResult.Contains("нет"))
//                             continue;
//             int count;
//             bool res = Int32.TryParse(textWitHResultSearch.Split(' ')[4],out count); // is book found
//             if(!res)
//                 continue;
//             var refToBookInfo = document.GetElementsByClassName("product-card__title")[0].Attributes
//                 .GetNamedItem("href").Value;
//             var bookDocument = await _context.OpenAsync(refToBookInfo);
//             
//             ICollection<Models.Tag> tags = new List<Models.Tag>();
//             bookDocument.GetElementsByClassName("breadcrumbs__item").Select(d =>
//             {
//                 Models.Tag tag = new Models.Tag()
//                 {
//                     Name = d.GetElementsByTagName("span")[0].TextContent
//                 };
//                 tags.Add(tag);
//                 return tag;
//             });
//             
//              var listDetails =
//                 bookDocument.GetElementsByClassName("product-detail-characteristics__item-value");
//             int pagesCount = 0;
//             if (listDetails.Length >= 5)
//             {
//                 Int32.TryParse(listDetails[4].TextContent, out pagesCount);
//             }
//
//             string year = "";
//             if (listDetails.Length >= 3)
//             {
//                 year = listDetails[2].TextContent;
//             }
//
//             CreateBookArgument book = new CreateBookArgument()
//             {
//                 Author = bookDocument.GetElementsByClassName("product-detail-title__author")[0].TextContent,
//                 Description = bookDocument.GetElementsByClassName("product-detail-additional__description")[0]
//                     .TextContent,
//                 Tags = tags,
//                 Genre = tags.Last().Name,
//                 ISBN = i.value,
//                 Name = bookDocument.GetElementsByClassName(
//                     "app-title app-title--mounted product-detail-title__header app-title--header-4")[0].TextContent,
//                 PagesCount = pagesCount,
//                 Year = year
//
//             };
//             await _bookService.CreateAsync(book);
//
//         }
//     }
//     
//     public async Task LoadISBNs()
//     {
//         // i take ALL pages with books, for each take ref to bookInfo, where i take isbn.
//         var document = await _context.OpenAsync(_igraSlovAddress);
//         _logger.LogInformation("got page");
//         var arrayWithBookElements =
//             document.GetElementsByClassName(
//                     "products oceanwp-row clr grid tablet-col tablet-3-col mobile-col mobile-2-col infinite-scroll-wrap")[0]
//                 .Children;
//         int counter = 0;
//         foreach (var i in arrayWithBookElements)
//         {
//             var refToBook = i.GetElementsByClassName("title")[0]
//                 .GetElementsByTagName("a")[0]
//                 .Attributes.GetNamedItem("href");
//             if(refToBook is null)
//                 continue;
//             var refValue = refToBook.Value;
//             var pageWithBook = await _context.OpenAsync(refValue);
//             var isbnTabElements = pageWithBook
//                 .GetElementsByClassName(
//                     "woocommerce-product-attributes-item woocommerce-product-attributes-item--attribute_isbnissn");
//      
//             if(!isbnTabElements.Any())
//                 continue;
//      
//             var isbnTab = isbnTabElements[0];
//      
//             var isbnElements = isbnTab.GetElementsByTagName("p");
//      
//             if(!isbnTabElements.Any())
//                 continue;
//
//             var isbn = new Models.ISBN { value = isbnElements[0].TextContent };
//             
//             await _isbnService.CreateAsync(isbn);
//             counter++;
//             if (counter>1000)
//                 break;
//             _logger.LogInformation("isbn created");
//         }
//         _logger.LogInformation("isbn downloaded");
//
//     }