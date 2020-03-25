using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.NotificationSystemAdapter.Infrastructure
{
    public class ApiKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isActionFilter = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAsyncActionFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);
            if (isActionFilter && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "api-key",
                    In = "header",
                    Required = true,
                    Type = "string"
                });
            }
        }
    }
}
