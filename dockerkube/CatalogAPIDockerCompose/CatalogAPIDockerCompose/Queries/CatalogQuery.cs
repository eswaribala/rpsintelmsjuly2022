using CatalogAPIDockerCompose.Repositories;
using GraphQL;
using GraphQL.Types;


namespace CatalogAPIDockerCompose.Queries
{
    public class CatalogQuery:ObjectGraphType
    {
        public CatalogQuery(ICatalogRepo CatalogRepository)
        {
            Name = "CatalogQuery";
            //get all catalogs
            Field<ListGraphType<CatalogGQLQuery>>(
              "catalogs",
              resolve: context => CatalogRepository.GetCatalogs()
          ) ;

            //get catalog by id
            Field<CatalogGQLQuery>(
               "catalog",
               arguments: new QueryArguments(new QueryArgument<LongGraphType> { Name = "catalogId" }),
               resolve: context => CatalogRepository.GetCatalogById(context.GetArgument<long>("catalogId"))

               );

        }
    }
}
