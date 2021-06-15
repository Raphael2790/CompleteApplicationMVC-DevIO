using Microsoft.AspNetCore.Mvc;
using RSS.CompleteApp.Extensions.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSS.CompleteApp.Extensions
{
    public static class MvcOptionsConfig
    {
        public static void MessageProviderConfiguration(MvcOptions options)
        {
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "O valor preenchido é inválido para este campo");
            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "Este campo precisa ser preenchido");
            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Este campo precisa ser preenchido");
            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "É necessário que o corpo da requisição não esteja vazio");
            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) => "O valor preenchido para este campo é inválido");
            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor preenchido para este campo é inválido");
            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser numérico");
            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => "O valor preenchido é inválido para esse campo");
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => "O valor preenchido é inválido para esse campo");
            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => "o campo deve ser numérico");
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "Este campo precisa ser preenchido");

            //Habilitando de forma global a validação do AntiForgeryToken
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            options.Filters.Add(typeof(LogAccessFilter));
        }
    }
}
