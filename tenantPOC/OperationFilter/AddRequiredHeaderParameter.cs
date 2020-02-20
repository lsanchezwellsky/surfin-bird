using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace tenantPOC.OperationFilter
{
   

    public class AddRequiredHeaderParameter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-tenant",
                In = ParameterLocation.Header,
                Required = false,
                AllowEmptyValue = true,


            });
        }
    }
}