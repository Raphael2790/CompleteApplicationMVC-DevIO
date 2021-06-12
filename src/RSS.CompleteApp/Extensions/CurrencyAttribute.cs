using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RSS.CompleteApp.Extensions
{
    //Atributo para ser usado pelo View model após o envio da requisição ao controller
    public class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var currency = Convert.ToDecimal(value, new CultureInfo("pt-BR"));
            }
            catch (Exception)
            {

                return new ValidationResult("Moeda em formato inválido");
            }

            return ValidationResult.Success;
        }
    }

    //Adaptação para o atributo responder no browser
    public class CurrencyAttributeAdapter : AttributeAdapterBase<CurrencyAttribute>
    {
        public CurrencyAttributeAdapter(CurrencyAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-moeda", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Moeda em formato inválido";
        }
    }

    public class CurrencyValidationAttributeProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();
        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            //verifica se o tipo do atributo é currency e cria uma intancia
            if (attribute is CurrencyAttribute currencyAttribute)
                return new CurrencyAttributeAdapter(currencyAttribute, stringLocalizer);

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
