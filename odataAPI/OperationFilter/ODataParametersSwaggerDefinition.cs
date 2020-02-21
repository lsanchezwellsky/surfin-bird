using System;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.AspNet.OData;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace odataAPI.OperationFilter
{
    public class ODataParametersSwaggerDefinition : IOperationFilter
    {
        private static readonly Type QueryableType = typeof(IQueryable);

    

        public void Apply(Operation operation, OperationFilterContext context)
        {


            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isOdata= filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is EnableQueryAttribute);
            if (!isOdata) return;
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "$filter",
                Description = "Filter the results using OData syntax.",
                Required = false,
                Type = "string",
                In = "query"
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "$select",
                Description = "Select the fields to show.",
                Required = false,
                Type = "string",
                In = "query"
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "$orderby",
                Description = "Order the results using OData syntax.",
                Required = false,
                Type = "string",
                In = "query"
            });


            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "$skip",
                Description = "The number of results to skip.",
                Required = false,
                Type = "integer",
                In = "query"
            });

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "$top",
                Description = "The number of results to return.",
                Required = false,
                Type = "integer",
                In = "query"
            });


        }
    }
}