using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CarLog.Vehicle.Api.Middleware;

public static class GlobalExceptionHandler
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env) 
    {
        app.UseExceptionHandler(appError => 
        {
            appError.Run(async context => 
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null) 
                {
                    var error = new ProblemDetails 
                    {
                        Status = context.Response.StatusCode,
                        Title = "An error occured",
                        Detail = contextFeature.Error.Message,
                        Instance = context.Request.Path
                    };

                    if (env.IsDevelopment()) 
                    {
                        error.Extensions.Add("StackTrace", contextFeature.Error.StackTrace);
                    }

                    var result = JsonSerializer.Serialize(error);

                    await context.Response.WriteAsync(result);
                }
            });
        });
    }
}
