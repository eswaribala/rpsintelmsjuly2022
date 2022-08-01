using CatalogAPI.Models;
using GraphQL.Types;


namespace CatalogAPI.Queries
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
