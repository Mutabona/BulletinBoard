﻿using System.Net;
using System.Text.Json;
using BulletinBoard.AppServices.Exceptions;
using BulletinBoard.Contracts.Errors;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;

namespace BulletinBoard.API.Middlewares;

/// <summary>
/// Промежуточное ПО для обработки ошибок.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All),
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private const string LogTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode}";
    private readonly RequestDelegate _next;

    /// <summary>
    /// Инициализирует экземпляр <see cref="ExceptionHandlingMiddleware"/>.
    /// </summary>
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Выполняет операцию промежуточного ПО и передаёт управление
    /// </summary>
    public async Task Invoke(
        HttpContext context
        , IHostEnvironment environment
        , IServiceProvider serviceProvider
        , ILogger<ExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var statusCode = GetStatusCode(e);
            
            using (LogContext.PushProperty("Request.TraceId", context.TraceIdentifier))
            using (LogContext.PushProperty("Request.UserName", context.User.Identity?.Name ?? string.Empty))
            using (LogContext.PushProperty("Request.Connection", context.Connection.RemoteIpAddress?.ToString() ?? string.Empty))
            using (LogContext.PushProperty("Request.DisplayUrl", context.Request.GetDisplayUrl()))
            {
                logger.LogError(e, LogTemplate,
                    context.Request.Method,
                    context.Request.Path.ToString(),
                    (int)statusCode);
            }
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var apiError = CreateApiError(e, context, environment);
            await context.Response.WriteAsync(JsonSerializer.Serialize(apiError, JsonSerializerOptions));
        }
    }

    private object CreateApiError(Exception exception, HttpContext context, IHostEnvironment environment)
    {
        return exception switch
        {
            HumanReadableException humanReadableException => new HumanReadableError
            {
                Code = context.Response.StatusCode.ToString(),
                HumanReadableErrorMessage = humanReadableException.HumanReadableMessage,
                Message = humanReadableException.Message,
                TraceId = context.TraceIdentifier,
            },
            EntityNotFoundException => new ApiError
            {
                Code = ((int)HttpStatusCode.NotFound).ToString(),
                Message = "Сущность не была найдена.",
                TraceId = context.TraceIdentifier,
            },
            EmailAlreadyExistsException => new ApiError
            {
                Code = ((int)HttpStatusCode.Conflict).ToString(),
                Message = "Эта почта уже зарегистрирована.",
                TraceId = context.TraceIdentifier,
            },
            InvalidLoginDataException invalidLoginDataException => new ApiError
            {
                Code = ((int)HttpStatusCode.Unauthorized).ToString(),
                Message = invalidLoginDataException.Message,
                TraceId = context.TraceIdentifier,
            },
            ForbiddenException => new ApiError
            {
                Code = ((int)HttpStatusCode.Forbidden).ToString(),
                Message = "Нет доступа.",
                TraceId = context.TraceIdentifier,
            },
            ConflictException => new ApiError()
            {
                Code = ((int)HttpStatusCode.Conflict).ToString(),
                Message = "Конфликт.",
                TraceId = context.TraceIdentifier,
            },
            _ => new ApiError
            {
                Code = ((int)HttpStatusCode.InternalServerError).ToString(),
                Message = "Произошла непредвиденная ошибка.",
                TraceId = context.TraceIdentifier,
            }
        };
    }

    private HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => HttpStatusCode.NotFound,
            InvalidLoginDataException => HttpStatusCode.Unauthorized,
            EmailAlreadyExistsException => HttpStatusCode.Conflict,
            ForbiddenException => HttpStatusCode.Forbidden,
            ConflictException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}