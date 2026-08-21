namespace InventoryManager.WebApi.Mvc.Infrastructure.Filters
{
    using InventoryManager.WebApi.Business.Services.ItemService.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Translates a duplicated item name raised by the business layer into a 409 Conflict,
    /// so controllers do not have to know the HTTP status each domain failure maps to.
    /// </summary>
    public class ItemDuplicatedExceptionFilter : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            if (context.Exception is ItemDuplicatedNameException exception)
            {
                context.ExceptionHandled = true;
                context.Result = new ObjectResult(exception.Message) { StatusCode = StatusCodes.Status409Conflict };
            }
        }
    }
}
