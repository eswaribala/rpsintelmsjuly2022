using CatalogAPIDockerCompose.GraphqlMutations;
using CatalogAPIDockerCompose.Queries;
using GraphQL.Types;



namespace CatalogAPIDockerCompose.Schemas
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
