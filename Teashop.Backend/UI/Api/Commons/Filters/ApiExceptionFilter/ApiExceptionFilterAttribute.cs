using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Teashop.Backend.UI.Api.Commons.Filters.ApiExceptionFilter
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApiExceptionHandler _exceptionHandler;
        private Type _exceptionType;

        public ApiExceptionFilterAttribute()
        {
            _exceptionHandler = new ApiExceptionHandler();
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            LoadExceptionTypeFrom(context);
            if (ExceptionHasHandler())
            {
                HandleKnownException(context);
                return;
            }
            HandleUnknownException(context);
        }

        private void LoadExceptionTypeFrom(ExceptionContext context)
        {
            _exceptionType = context.Exception.GetType();
        }

        private bool ExceptionHasHandler()
        {
            return _exceptionHandler.RegisteredHandlers.ContainsKey(_exceptionType);
        }

        private void HandleKnownException(ExceptionContext context)
        {
            _exceptionHandler.RegisteredHandlers[_exceptionType].Invoke(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            _exceptionHandler.HandleUnknownException(context);
        }
    }
}
