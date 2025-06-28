using StockApp.API.Middleware;
namespace StockApp.API.Middleware
{
    public static class ErrorHandlerMiddlewareExtensions
    {
       
            public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ErrorHandlerMiddleware>();
            }
        }
    }

