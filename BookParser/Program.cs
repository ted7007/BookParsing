// i take ALL pages with books, for each take ref to bookInfo, where i take isbn.

using BookParser;
using BookParser = BookParser.BookParser;

global::BookParser.BookParser parser = new global::BookParser.BookParser();

parser.StartParsingAsync(1000);
