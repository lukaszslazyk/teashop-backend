using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.UI.Api.Commons.Models;

namespace Teashop.Backend.UI.Api.Commons.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }
            HandleUnknownException(context);
        }

        private void HandleExceptionWithApiErrorResult(ExceptionContext context, ApiError error, int code)
        {
            context.Result = new ObjectResult(error)
            {
                StatusCode = code
            };
            context.ExceptionHandled = true;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var error = new ApiError
            {
                ErrorType = "Unknown",
                Message = "Unknown error has occurred."
            };
            HandleExceptionWithApiErrorResult(context, error, statusCode);
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status400BadRequest;
            var exception = context.Exception as ValidationException;
            var error = new ApiError
            {
                ErrorType = "Validation",
                Message = exception.Message,
                Details = exception.Errors
            };
            HandleExceptionWithApiErrorResult(context, error, statusCode);
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var statusCode = StatusCodes.Status404NotFound;
            var exception = context.Exception as NotFoundException;
            var error = new ApiError
            {
                ErrorType = "NotFound",
                Message = exception.Message,
            };
            HandleExceptionWithApiErrorResult(context, error, statusCode);
        }
    }
}
