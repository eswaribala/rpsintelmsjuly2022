using CatalogAPI.GraphqlMutations;
using CatalogAPI.Queries;
using GraphQL.Types;



namespace CatalogAPI.Schemas
{
    public class CatalogSchema:Schema
    {
        public CatalogSchema(IServiceProvider ServiceProvider)
        {
            Query = ServiceProvider.GetRequiredService<CatalogQuery>();
            Mutation = ServiceProvider.GetRequiredService<CatalogMutation>();
        }
    }
}
