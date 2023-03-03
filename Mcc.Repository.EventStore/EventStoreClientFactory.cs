using EventStore.Client;
using Microsoft.Extensions.Configuration;

namespace Mcc.Repository.EventStore;

public class EventStoreClientFactory
{
    public EventStoreClient Create(IConfiguration configuration)
    {
        var settings = EventStoreClientSettings.Create(configuration.GetConnectionString("Eventstore"));
        settings.ConnectivitySettings.Insecure = true;

        var client = new EventStoreClient(settings);

        return client;
    }
}