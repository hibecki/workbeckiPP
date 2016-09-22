using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PPcore.Helpers
{
    [HtmlTargetElement("input", Attributes = ForAttributeName)]
    public class PPInputTagHelpers : InputTagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName("asp-is-disabled")]
        public bool IsDisabled { set; get; }

        [HtmlAttributeName("asp-is-member")]
        public bool IsMember { set; get; }

        public PPInputTagHelpers(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((IsDisabled) || (IsMember))
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("select", Attributes = ForAttributeName)]
    public class PPSelectTagHelpers : InputTagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName("asp-is-disabled")]
        public bool IsDisabled { set; get; }

        [HtmlAttributeName("asp-is-member")]
        public bool IsMember { set; get; }

        public PPSelectTagHelpers(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((IsDisabled) || (IsMember))
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }
            base.Process(context, output);
        }
    }
}
