namespace InventoryManager.WebApi.Mvc.Infrastructure.OpenApi
{
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Collections.Generic;
    using System.Linq;

    internal class ApiVersionOperationFilter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiVersion = context.ApiDescription.GetApiVersion();

            if (apiVersion == null)
            {
                return;
            }

            var parameters = operation.Parameters ?? (operation.Parameters = new List<OpenApiParameter>());

            // This API reads its version from an "api-version" query string parameter or from a
            // header of the same name. An API that settled on a single mechanism, or that put the
            // version in the route, would not need this filter at all.

            // A parameter the API explorer already described takes precedence.
            var parameter = parameters.FirstOrDefault(p => p.Name == "api-version");
            if (parameter == null)
            {
                // Otherwise document the query string form.
                parameter = new OpenApiParameter()
                {
                    Name = "api-version",
                    Required = true,
                    In = ParameterLocation.Query
                };
                parameters.Add(parameter);
            }

            parameter.Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString(apiVersion.ToString()) };
            parameter.Description = "The requested API version";
        }


    }
}
