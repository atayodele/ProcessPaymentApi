using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace CheckoutApp.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                                    .Keys
                                    .SelectMany(key => context.ModelState[key].Errors
                                    .Select(x => x.ErrorMessage))
                                    .ToList();
                if (errors.Count > 0)
                {
                    throw new Exception(errors.FirstOrDefault());
                }
            }
        }
    }
}
