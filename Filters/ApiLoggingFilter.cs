﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        public readonly ILogger<ApiLoggingFilter> _logger;
        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {

            _logger = logger;

        }
        //executa antes da action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("### Executando -> OnActionExecuting");
            _logger.LogInformation("###################################");
            _logger.LogInformation($"{ DateTime.Now.ToLongTimeString }");
            _logger.LogInformation($"ModalState : {context.ModelState.IsValid}");
            _logger.LogInformation("###################################");

        }

        // executa depois da action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("### Executando -> OnActionExecuted");
            _logger.LogInformation("###################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString}");
            _logger.LogInformation("###################################"); ;
        }
    }
}
