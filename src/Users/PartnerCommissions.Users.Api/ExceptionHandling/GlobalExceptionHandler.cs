using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;
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

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails =
            {
                Status = statusCode,
                Title = ReasonPhrases.GetReasonPhrase(statusCode.Value),
                Detail = exception.Message
            }
        };

        if (!await problemDetailsService.TryWriteAsync(context))
            await httpContext.Response.WriteAsJsonAsync(context.ProblemDetails, options: null, contentType: "application/problem+json", cancellationToken);

        return true;
    }
}
