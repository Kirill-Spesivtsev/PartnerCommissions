using Microsoft.AspNetCore.Diagnostics;
using PartnerCommissions.Users.Application.Exceptions;

namespace PartnerCommissions.Users.Api.ExceptionHandling;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int? statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => null
        };

        if (statusCode is null)
            return false;

        httpContext.Response.StatusCode = statusCode.Value;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = { Detail = exception.Message }
        });
    }
}
