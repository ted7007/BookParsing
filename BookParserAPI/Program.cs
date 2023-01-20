using System.Data.Common;
using System.Reflection;
using BookParserAPI.Models;
using BookParserAPI.Repository;
using Microsoft.EntityFrameworkCore;
using BookParserAPI.Config;
using BookParserAPI.Repository.Book;
using BookParserAPI.Repository.Tag;
using BookParserAPI.Service;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);


var connectionString =  GetConnectionString(builder.Environment.EnvironmentName, builder);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySql(connectionString,
                     ServerVersion.AutoDetect(connectionString),
                     options => options.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: System.TimeSpan.FromSeconds(30),
                         errorNumbersToAdd: null)));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddControllers();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => $"{app.Environment.EnvironmentName}");

app.Run();

string GetConnectionString(string stage, WebApplicationBuilder hostBuilder)
{
    
    var dataBaseOptions =
        hostBuilder.Configuration.GetSection(DataBaseOptions.OptionName)
            .Get<DataBaseOptions>();
    if (dataBaseOptions is null)
        throw new ArgumentNullException(nameof(dataBaseOptions));
    DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
    connectionStringBuilder.Add("server", dataBaseOptions.Server);
    connectionStringBuilder.Add("port", dataBaseOptions.Port);
    connectionStringBuilder.Add("uid", dataBaseOptions.UserName);
    connectionStringBuilder.Add("database", dataBaseOptions.DatabaseName);
    connectionStringBuilder.Add("pwd", dataBaseOptions.Password);
    var result = connectionStringBuilder.ConnectionString;
    if (string.IsNullOrEmpty(result))
        throw new InvalidCastException("connection string is null");
    return connectionStringBuilder.ConnectionString;


}
/*  todo
 
 * global logging
 * global exception handler
 * authorization & authentification
 
 
*/

