using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseHandler
{
    public static class CompositionRoot
    {
        public static IServiceCollection SetDatabaseHandlerDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDbConnection, DbConnection>();
            serviceCollection.AddTransient<IChunkRepository, ChunkRepository>();
            serviceCollection.AddTransient<IChunkServices, ChunkServices>();
            return serviceCollection;
        }
    }
}