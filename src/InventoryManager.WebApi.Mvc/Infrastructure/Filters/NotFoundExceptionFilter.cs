namespace InventoryManager.WebApi.Mvc.Infrastructure.Filters
{
    using InventoryManager.WebApi.Business.Services.ItemService.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Translates a missing item raised by the business layer into a 404 Not Found,
    /// so controllers do not have to know the HTTP status each domain failure maps to.
    /// </summary>
    public class NotFoundExceptionFilter : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            if (context.Exception is NotFoundException exception)
            {
                context.ExceptionHandled = true;
                context.Result = new NotFoundObjectResult(exception.Message);
            }
        }
    }
}
