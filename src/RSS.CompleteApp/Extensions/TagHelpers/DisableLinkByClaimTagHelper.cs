using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSS.CompleteApp.Extensions.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("a", Attributes = "supress-by-claim-value")]
    public class DisableLinkByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAcessor;

        [HtmlAttributeName("disable-by-claim-name")]
        public string IdentityClaimName { get; set; }
        [HtmlAttributeName("disable-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public DisableLinkByClaimTagHelper(IHttpContextAccessor contextAcessor)
        {
            _contextAcessor = contextAcessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            var hasAccess = CustomAuthorization.ValidateUserClaims(_contextAcessor.HttpContext, IdentityClaimName, IdentityClaimValue);
            if (hasAccess) return;

            output.Attributes.RemoveAll("href");
            output.Attributes.Add(new TagHelperAttribute("style", "cursor:not-allowed"));
            output.Attributes.Add(new TagHelperAttribute("title", "Você não tem permissão!"));
        }
    }
}
