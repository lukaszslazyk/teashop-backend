using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.UI.Api.Error.Models;

namespace Teashop.Backend.UI.Api.Error.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IDictionary<Type, Func<Exception, ApiError>> _exceptionHandlers;
        private Exception _exception;

        public ErrorController()
        {
            _exceptionHandlers = new Dictionary<Type, Func<Exception, ApiError>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
            };
        }

        [Route("error")]
        public ApiError Error()
        {
            LoadException();
            if (ExceptionHasHandler())
                HandleException();

            return HandleUnexpectedException();
        }

        private void LoadException()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            _exception = context?.Error;
        }

        private bool ExceptionHasHandler()
        {
            return _exceptionHandlers.ContainsKey(_exception.GetType());
        }

        private ApiError HandleException()
        {
            return _exceptionHandlers[_exception.GetType()].Invoke(_exception);
        }

        private ApiError HandleValidationException(Exception exception)
        {
            Response.StatusCode = 400;
            return new ApiError
            {
                ErrorType = "Validation",
                Message = exception.Message,
                Details = (exception as ValidationException).Errors
            };
        }

        private ApiError HandleNotFoundException(Exception exception)
        {
            Response.StatusCode = 404;
            return new ApiError
            {
                ErrorType = "NotFound",
                Message = exception.Message,
            };
        }

        private ApiError HandleUnexpectedException()
        {
            Response.StatusCode = 500;
            return new ApiError
            {
                ErrorType = "Unexpected",
                Message = "Unexpected error has occurred."
            };
        }
    }
}
