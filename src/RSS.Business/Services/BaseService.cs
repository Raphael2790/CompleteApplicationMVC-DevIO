using FluentValidation;
using FluentValidation.Results;
using RSS.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Business.Services
{
    public abstract class BaseService
    {
        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string message)
        {

        }

        protected bool ExecuteValidation<TV,TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validation.Validate(entity);
            if (validator.IsValid) return true;
            Notify(validator);
            return false;
        }
    }
}
