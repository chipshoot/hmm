using Hmm.Utility.Validation;
using Hmm.WebConsole.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;

namespace Hmm.WebConsole.Infrastructure.TagHelpers
{
    /// <summary>
    /// The tag helper is for paging section and data set pagination, when working with
    /// Subsystem of the HMM, it's section manage, as for showing data set, then it's pageInfo
    /// </summary>
    [HtmlTargetElement("div", Attributes = "section-mode")]
    public class SectionLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public SectionLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            Guard.Against<ArgumentNullException>(helperFactory == null, nameof(helperFactory));
            _urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public SectionInfo SectionMode { get; set; }

        public string SectionAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "section-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public bool SectionClassesEnabled { get; set; } = false;

        public string SectionClass { get; set; }

        public string SectionClassNormal { get; set; }

        public string SectionClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");
            for (var i = 1; i <= SectionMode.TotalSections; i++)
            {
                var tag = new TagBuilder("a");
                PageUrlValues["SectionPage"] = i;
                tag.Attributes["href"] = urlHelper.Action(SectionAction, PageUrlValues);
                if (SectionClassesEnabled)
                {
                    tag.AddCssClass(SectionClass);
                    tag.AddCssClass(i==SectionMode.CurrentSection? SectionClassSelected: SectionClassNormal);
                }
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);
            }

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}