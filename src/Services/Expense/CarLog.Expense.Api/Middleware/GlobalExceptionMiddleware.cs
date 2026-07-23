using System.Net;
using System.Text.Json;
using CarLog.Expense.Application.Common.Exceptions;
using CarLog.Expense.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarLog.Expense.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException validationEx => (HttpStatusCode.BadRequest, "Invalid data", string.Join(" | ", validationEx.Errors.Select(e => e.ErrorMessage))),

            DomainException domainEx => (HttpStatusCode.BadRequest, "Business rule violated", domainEx.Message),

            NotFoundException notFoundEx => (HttpStatusCode.NotFound, "Resource not found", notFoundEx.Message),

            _ => (HttpStatusCode.InternalServerError, "An error occured", "An unexpected error occurred. Please try again later.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions.Add("StackTrace", exception.StackTrace);
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(result);
    }
}