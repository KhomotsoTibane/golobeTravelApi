using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using GolobeTravelApi.Data;

namespace GolobeTravelApi
{
    public static class Extensions
    {
        public static ModelStateDictionary ToModelStateDictionary(this ValidationResult validationResult)
        {
            var modelState = new ModelStateDictionary();

            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return modelState;
        }

        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
