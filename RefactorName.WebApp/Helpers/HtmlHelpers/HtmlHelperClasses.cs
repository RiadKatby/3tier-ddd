using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcHtmlHelpers
{
    public class MCITagBuilder : TagBuilder
    {
        public MCITagBuilder(string tagName)
            : base(tagName)
        { }

        public virtual MCITagBuilder AppendInnerHtml(string html)
        {
            this.InnerHtml += html;
            return this;
        }

        public MCITagBuilder AppendInnerHtmlFormat(string html, params string[] parameters)
        {
            AppendInnerHtml(string.Format(html, parameters));
            return this;
        }

        public MCITagBuilder AppendInnerHtml(MCITagBuilder tagBuilder)
        {
            AppendInnerHtml(tagBuilder.ToString());
            return this;
        }

        public MvcHtmlString ToMvcHtmlString(TagRenderMode renderMode = TagRenderMode.Normal)
        {
            return new MvcHtmlString(this.ToString(renderMode));
        }

        public virtual new string ToString(TagRenderMode renderMode)
        {
            return base.ToString(renderMode);
        }
    }

    public class MCIDisposableHelper : IDisposable
    {
        protected HtmlHelper _helper;
        MCITagBuilder _tag;

        public MCIDisposableHelper(HtmlHelper helper, MCITagBuilder tag)
        {
            this._helper = helper;
            this._tag = tag;
            _helper.ViewContext.Writer.Write(tag.ToString(TagRenderMode.StartTag));
        }
        public void Dispose()
        {
            _helper.ViewContext.Writer.Write(_tag.ToString(TagRenderMode.EndTag));
        }
    }

    public class MCIItemsCol : MCITagBuilder
    {
        public MCIItemsCol(int spanOf12 = 6, object htmlAttributes = null)
            : base("div")
        {
            RouteValueDictionary customAttributes = htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : new RouteValueDictionary();
            customAttributes["class"] += string.Format(" col-sm-{0}", spanOf12.ToString());

            this.MergeAttributes(customAttributes, true);
        }
    }

    public class MCIFormGroup : MCITagBuilder
    {
        public MCIFormGroup(object htmlAttributes = null)
            : base("div")
        {
            RouteValueDictionary customAttributes = htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : new RouteValueDictionary();
            customAttributes["class"] += " form-group";

            this.MergeAttributes(customAttributes);
        }
    }

    public class MCIFormDisplay : MCITagBuilder
    {
        public MCIFormDisplay(object htmlAttributes = null)
            : base("div")
        {
            RouteValueDictionary customAttributes = htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : new RouteValueDictionary();
            customAttributes["class"] += " form-display";

            this.MergeAttributes(customAttributes);
        }
    }

    public class MCIItemsRow : MCITagBuilder
    {
        public MCIItemsRow()
            : base("div")
        {
        }

        public MCIItemsRow(object htmlAttributes)
            : this()
        {
            RouteValueDictionary customAttributes = htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : new RouteValueDictionary();
            customAttributes["class"] += " row";

            this.MergeAttributes(customAttributes);
        }
    }

    public class MCITabContainer : MCITagBuilder
    {
        public MCITabContainer(string name, NavTabsStyle style, bool inverse)
            : base("div")
        {
            MergeAttribute("id", name);
            if (inverse)
                AddCssClass("nav-tabs-inverse");

            if (style == NavTabsStyle.HorizontalWithBorder)
                AddCssClass("h-tabs-with-border");

            else if (style == NavTabsStyle.HorizontalWithActiveBorder)
                AddCssClass("h-tabs-with-active-border");

            else if (style == NavTabsStyle.VerticalWithBorder)
                AddCssClass("v-tabs-with-border");

            else if (style == NavTabsStyle.VerticalWithActiveBorder)
                AddCssClass("v-tabs-with-active-border");
        }
    }

    public class MCINavBar : MCITagBuilder
    {
        public MCINavBar(string tabName)
            : base("ul")
        {
            this.AddCssClass("nav nav-tabs");
            this.MergeAttribute("role", "tablist");

            if (!string.IsNullOrWhiteSpace(tabName))
                this.MergeAttribute("id", tabName);
        }
    }

    public class MCITabHeader : MCITagBuilder
    {
        public MCITabHeader(string tabName, string innerHtml, bool active = false)
            : base("li")
        {
            if (active)
                this.AddCssClass("active");
            this.MergeAttribute("role", "presentation");

            AppendInnerHtmlFormat("<a href='#{0}' aria-controls='home' role='tab' data-toggle='tab'>{1}</a>", tabName, innerHtml);
        }
    }

    public class MCITabPanel : MCITagBuilder
    {
        public MCITabPanel(string name, bool active, bool withFadeEffect)
            : base("div")
        {
            MergeAttribute("id", name);
            MergeAttribute("role", "tabpanel");
            AddCssClass("tab-pane");
            if (active)
                AddCssClass("active");
            if (withFadeEffect)
                AddCssClass("fade");
            if (active && withFadeEffect)
                AddCssClass("in");
        }
    }

    public class MCITabPanels : MCITagBuilder
    {
        public MCITabPanels(string minHeight)
            : base("div")
        {
            AddCssClass("tab-content");
            if (!string.IsNullOrEmpty(minHeight))
                MergeAttribute("style", string.Format("min-height:{0}", minHeight));
        }
    }

    public class MCIHint : MCITagBuilder
    {
        public MCIHint(string hint)
            : base("i")
        {
            AddCssClass("mci-hint");
            this.SetInnerText(hint);
        }
    }

    public class MCIDisplayTag : MCITagBuilder
    {
        public MCIDisplayTag(string name, object htmlAttributes = null)
            : base("p")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            MergeAttributes(customAttributes);
            MergeAttribute("id", name, false);
        }
        public MCIDisplayTag(string name, string value, object htmlAttributes = null)
            : this(name, htmlAttributes)
        {
            this.SetInnerText(value);
        }
    }

    public class MCIDisplayTagH : MCITagBuilder
    {
        public MCIDisplayTagH(string name, object htmlAttributes = null)
            : base("span")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            MergeAttributes(customAttributes);
            MergeAttribute("id", name, false);
        }

        public MCIDisplayTagH(string name, string value, object htmlAttributes = null)
            : this(name, htmlAttributes)
        {
            this.SetInnerText(value);
        }
    }

    public class MCIInputGroup : MCITagBuilder
    {
        public MCIInputGroup()
            : base("div")
        {
            AddCssClass("input-group");
        }
    }

    public class MCIInputGroupAddon : MCITagBuilder
    {
        public MCIInputGroupAddon(string addon)
            : base("div")
        {
            AddCssClass("input-group-addon");
            InnerHtml = addon;
        }
    }

    public class MCIModalContainer : MCITagBuilder
    {
        public MCIModalContainer(string modalUniqueName, ModalFadeMode fadeMode = ModalFadeMode.Transperent)
            : base("div")
        {
            AddCssClass("modal");
            AddCssClass("fade");
            if (fadeMode == ModalFadeMode.Dark)
                AddCssClass("fade-dark");
            if (fadeMode == ModalFadeMode.Light)
                AddCssClass("fade-light");

            this.MergeAttribute("data-backdrop", "static");
            this.MergeAttribute("id", modalUniqueName);
            this.MergeAttribute("tabindex", "-1");
            this.MergeAttribute("role", "dialog");
            this.MergeAttribute("aria-labelledby", modalUniqueName + "Label");
            this.MergeAttribute("aria-hidden", "true");
        }
    }

    public class MCIModal : MCITagBuilder
    {
        MCITagBuilder modalContent;

        public MCIModal(ModalSize modalSize)
            : base("div")
        {
            AddCssClass("modal-dialog");
            AddCssClass(ParseModalSize(modalSize));

            modalContent = new MCITagBuilder("div");
            modalContent.AddCssClass("modal-content");
        }

        public override string ToString(TagRenderMode renderMode = TagRenderMode.Normal)
        {
            if (renderMode == TagRenderMode.EndTag)
                return modalContent.ToString(TagRenderMode.EndTag) + base.ToString(TagRenderMode.EndTag);

            if (renderMode == TagRenderMode.StartTag)
                return base.ToString(TagRenderMode.StartTag) + modalContent.ToString(TagRenderMode.StartTag);

            else
                return string.Format("{0}{1}{2}", base.ToString(TagRenderMode.StartTag), modalContent.ToString(), base.ToString(TagRenderMode.EndTag));

        }

        public override MCITagBuilder AppendInnerHtml(string html)
        {
            modalContent.AppendInnerHtml(html);
            return this;
        }

        private string ParseModalSize(ModalSize size)
        {
            switch (size)
            {
                case ModalSize.Large:
                    return "modal-lg";
                case ModalSize.Small:
                    return "modal-sm";
                default:
                    return "";
            }
        }
    }

    public class MCIModalHeader : MCITagBuilder
    {
        string closeButtonHtml = "<button type='button' class='close' data-dismiss='modal' aria-hidden='true'>×</button>";
        public MCIModalHeader(bool withCloseButton = true, ModalHeaderStyle modalHeaderStyle = ModalHeaderStyle.Light)
            : base("div")
        {
            AddCssClass("modal-header");
            if (modalHeaderStyle == ModalHeaderStyle.Light)
                AddCssClass("header-light");
            else if (modalHeaderStyle == ModalHeaderStyle.Primary)
                AddCssClass("modal-primary");

            AppendInnerHtml(closeButtonHtml);
        }

        public override string ToString(TagRenderMode renderMode = TagRenderMode.Normal)
        {
            if (renderMode == TagRenderMode.StartTag)
                return base.ToString(TagRenderMode.StartTag) + closeButtonHtml;

            else
                return base.ToString(renderMode);
        }
    }

    public class MCIModalBody : MCITagBuilder
    {
        public MCIModalBody(object htmlAttributes)
            : base("div")
        {
            RouteValueDictionary customAttributes = htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : new RouteValueDictionary();
            customAttributes["class"] += " modal-body";

            this.MergeAttributes(customAttributes);
        }
    }

    public class MCIModalFooter : MCITagBuilder
    {
        public MCIModalFooter(object htmlAttributes)
            : base("div")
        {
            RouteValueDictionary customAttributes = htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : new RouteValueDictionary();
            customAttributes["class"] += " modal-footer";

            this.MergeAttributes(customAttributes);
        }
    }

    public enum ModalSize
    {
        Large,
        Default,
        Small
    }

    public enum ModalFadeMode
    {
        Transperent,
        Dark,
        Light
    }

    public enum ModalHeaderStyle
    {
        Normal,
        Light,
        Primary
    }

    public enum NavTabsStyle
    {
        HorizontalWithBorder,
        HorizontalWithActiveBorder,
        VerticalWithBorder,
        VerticalWithActiveBorder
    }


}