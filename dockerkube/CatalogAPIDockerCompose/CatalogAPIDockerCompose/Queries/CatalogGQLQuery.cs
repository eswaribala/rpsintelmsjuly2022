using CatalogAPIDockerCompose.Models;
using GraphQL.Types;


namespace CatalogAPIDockerCompose.Queries
{
    public class CatalogGQLQuery:ObjectGraphType<Catalog>
    {

        public CatalogGQLQuery()
        {
            Name = "Catalog";
            Field(_ => _.CatalogId).Description("Catalog Id.");
            Field(_ => _.CatalogName).Description("Catalog Name.");
        }
    }
}
