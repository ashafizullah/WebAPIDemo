using System;
using System.Drawing;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIDemo.Data;
using WebAPIDemo.Models;
using WebAPIDemo.Models.Repositories;

namespace WebAPIDemo.Filters.ActionFilters
{
	public class Shirt_ValidateCreateShirtFilterAttribute: ActionFilterAttribute
	{
        private readonly ApplicationDbContext db;

        public Shirt_ValidateCreateShirtFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirt = context.ActionArguments["shirt"] as Shirt;

            if(shirt == null)
            {
                context.ModelState.AddModelError("Shirt", "Shirt object is null.");
                var problemdetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemdetails);
            }
            else
            {
                var existingShirt = db.Shirts.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(shirt.Brand) &&
                    !string.IsNullOrWhiteSpace(x.Brand) &&
                    x.Brand.ToLower() == shirt.Brand.ToLower() &&
                    !string.IsNullOrWhiteSpace(shirt.Gender) &&
                    !string.IsNullOrWhiteSpace(x.Gender) &&
                    x.Gender.ToLower() == shirt.Gender.ToLower() &&
                    !string.IsNullOrWhiteSpace(shirt.Color) &&
                    !string.IsNullOrWhiteSpace(x.Color) &&
                    x.Color.ToLower() == shirt.Color.ToLower() &&
                    x.Size == shirt.Size);

                if (existingShirt != null)
                {
                    context.ModelState.AddModelError("Shirt", "Shirt already exists.");
                    var problemdetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemdetails);
                }
            }
        }
    }
}

