using CatalogAPI.Models;
using CatalogAPI.Queries;
using CatalogAPI.Repositories;
using GraphQL;
using GraphQL.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
 *mutation($catalog:CatalogInput){
  insertCatalog(catalog:$catalog){
    catalogName
    
  }
}
{
  "catalog": {
    "catalogName": "Computer Accessories"
  }
}

mutation($catalogId:Long!){
  deleteCatalog(catalogId:$catalogId)
}

{
  "catalogId": 4
}
 */


namespace CatalogAPI.GraphqlMutations
{
    public class CatalogMutation : ObjectGraphType
    {
      

     
        private readonly ICatalogRepo CatalogRepository;
       
        public CatalogMutation(ICatalogRepo _CatalogRepository)
        {
            
            CatalogRepository = _CatalogRepository;
            Field<CatalogGQLQuery>("insertCatalog",
             arguments: new QueryArguments(
           
                 new QueryArgument<CatalogGQLInputType> { Name = "catalog" }),
               
             resolve: context =>
             {
                
                 var catalog = context.GetArgument<Catalog>("catalog");
                
                 return InsertCatalog( catalog);
             });

            //Field<CatalogGQLQuery>("insertCatalog",
            //   arguments: new QueryArguments(new QueryArgument<CatalogGQLInputType> { Name = "catalog" }),
            //   resolve: context =>
            //   {
            //       return CatalogRepository.AddCatalog(context.GetArgument<Catalog>("catalog"));
            //   });

            Field<StringGraphType>(
                "DeleteCatalog",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<LongGraphType>>
                { Name = "CatalogId" }),
                resolve: context =>
                {
                    var catalogId = context.GetArgument<long>("CatalogId");
                    
                    CatalogRepository.DeleteCatalog(catalogId);
                    return $"CatalogId {catalogId} is successfully deleted";
                }
            );
        }
        private Task<Catalog> InsertCatalog(Catalog Catalog)
        {
            if (Catalog== null)
                return null;
            else
            {
                return CatalogRepository.AddCatalog(Catalog);
            }
        }
    }
}