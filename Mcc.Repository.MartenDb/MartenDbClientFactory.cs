using Marten;
using Microsoft.Extensions.Configuration;

namespace Mcc.Repository.MartenDb;

public class MartenDbClientFactory
{
    private readonly IConfiguration _configuration;

    public MartenDbClientFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDocumentSession Create()
    {
        var store =  DocumentStore.For(x =>
        {
         x.Connection(_configuration.GetConnectionString("martenDb"));   
        });

        return store.LightweightSession();
    }
}