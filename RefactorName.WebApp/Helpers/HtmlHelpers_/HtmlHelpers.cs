using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Ajax;
using System.Linq;
using System.Text;
using System.Web;
using IdeasBank.Web.Models;
using System.Web.Helpers;
using System.Xml.Linq;
using IdeasBank.Web;
using IdeasBank.Web.Infrastructure.Encryption;
using System.Configuration;
using IdeasBank.Web.ModelExtensions;

namespace MvcHtmlHelpers
{
    public static class HtmlHelperExtensions
    {

        #region file upload

        /// <summary>
        /// Return html div string that represents file uploader initialized with finUploader plugin
        /// </summary>
        /// <param name="name">Unique name for the uploader</param>
        /// <param name="controllerName">Upload controller name that contains upload and delete actions</param>
        /// <param name="allowedExtensions">Allowed extensions to upload</param>
        /// <param name="fileMaxSize">Maximum file size to upload</param>
        /// <param name="onSuccessCallback">Javascript function name to call after upload success</param>
        /// <param name="onDeleteCallback">Javascript function name to call after deleting afile from the uploader</param>
        /// <returns>MvcHtmlString of the uploader</returns>
        private static string MCIUploaderHtml(HtmlHelper helper, string name, string controllerName, string allowedExtensions, int fileMaxSize, string onSuccessCallback, string onDeleteCallback)
        {
            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(name);

            StringBuilder result = new StringBuilder();
            string divName = string.Format("{0}_attachement_div", name);

            result.AppendFormat("<div id='{0}' class='uploader'><div class='file_uploader'></div><div class='file_uploader_message'></div></div>", divName);
            result.AppendFormat("<script>$(function () {{initUploader('{0}', '{1}', '{2}', [{3}], {4}, '', '{5}', '{6}');}});</script>", divName, name, controllerName, allowedExtensions, fileMaxSize.ToString(), onSuccessCallback, onDeleteCallback);

            //fill uploader model in session for server side validation
            List<FileUploadModel> fileUploadModels = HttpContext.Current.Session["FileUploadModels"] as List<FileUploadModel>;
            if (fileUploadModels == null)
                HttpContext.Current.Session["FileUploadModels"] = fileUploadModels = new List<FileUploadModel>();

            FileUploadModel model = fileUploadModels.FirstOrDefault(m => m.Name == name);
            if (model == null)
            {
                model = new FileUploadModel();
                fileUploadModels.Add(model);
            }

            model.Name = name;
            model.FileMaxSize = fileMaxSize;
            model.AllowedExtensions = allowedExtensions.Split(',').Select(s => s.Trim(' ', '\'')).ToArray();

            HttpContext.Current.Session["FileUploadModels"] = fileUploadModels;

            return result.ToString();
        }

        /// <summary>
        /// Return file uploader control initialized with finUploader plugin
        /// </summary>
        /// <param name="name">Unique name for the uploader</param>
        /// <param name="controllerName">Upload controller name that contains upload and delete actions</param>
        /// <param name="allowedExtensions">Allowed extensions to upload</param>
        /// <param name="fileMaxSize">Maximum file size to upload</param>
        /// <param name="numberOfFilesToUpload">For multiple files uploader</param>
        /// <param name="onSuccessCallback">Javascript function name to call after upload success</param>
        /// <param name="onDeleteCallback">Javascript function name to call after deleting afile from the uploader</param>
        /// <param name="required">Add required validation?</param>
        /// <param name="validationMessage">Validation message for required validation</param>
        /// <returns>MvcHtmlString for uploader control</returns>
        public static MvcHtmlString MCIFileUploader<TModel>(
            this HtmlHelper<TModel> helper,
            string name,
            string controllerName = "Upload",
            string allowedExtensions = "'jpg', 'jpeg', 'png', 'bmp'",
            int fileMaxSize = 2048,
            string onSuccessCallback = "", string onDeleteCallback = "",
            bool required = true, string validationMessage = "الملف مطلوب")
        {
            StringBuilder result = new StringBuilder();

            TagBuilder newTagbuilder = new TagBuilder("input");
            newTagbuilder.MergeAttribute("id", name);
            newTagbuilder.MergeAttribute("name", name);
            newTagbuilder.MergeAttribute("type", "text");
            newTagbuilder.MergeAttribute("value", "");
            newTagbuilder.MergeAttribute("style", "position:absolute; opacity:0.0; visibility:hidden");
            if (required)
            {
                newTagbuilder.MergeAttribute("data-val", "true");
                newTagbuilder.MergeAttribute("data-val-required", validationMessage);
            }
            result.Append(newTagbuilder.ToString());
            result.Append(MCIUploaderHtml(helper, name, controllerName, allowedExtensions, fileMaxSize, onSuccessCallback, onDeleteCallback));

            return new MvcHtmlString(result.ToString());
        }

        /// <summary>
        /// Return file uploader control initialized with finUploader plugin
        /// </summary>
        /// <param name="expression">Lambda expression for the string property to bind file path to</param>
        /// <param name="controllerName">Upload controller name that contains upload and delete actions</param>
        /// <param name="allowedExtensions">Allowed extensions to upload</param>
        /// <param name="fileMaxSize">Maximum file size to upload</param>
        /// <param name="onSuccessCallback">Javascript function name to call after upload success</param>
        /// <param name="onDeleteCallback">Javascript function name to call after deleting afile from the uploader</param>
        /// <returns>MvcHtmlString for uploader control</returns>
        public static MvcHtmlString MCIFileUploaderFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, string controllerName = "Upload", string allowedExtensions = "'jpg', 'jpeg', 'png', 'bmp'",
            int fileMaxSize = 2048, string onSuccessCallback = "", string onDeleteCallback = "")
        {
            string name = ExpressionHelper.GetExpressionText(expression);

            StringBuilder result = new StringBuilder(System.Web.Mvc.Html.InputExtensions.TextBoxFor(helper, expression, new { @style = "position:absolute;opacity:0.0;visibility:hidden" }).ToString());
            result.Append(MCIUploaderHtml(helper, name, controllerName, allowedExtensions, fileMaxSize, onSuccessCallback, onDeleteCallback));

            return new MvcHtmlString(result.ToString());
        }


        /// <summary>
        /// Return file uploader control full item (with caption) initialized with finUploader plugin
        /// </summary>
        /// <param name="expression">Lambda expression for the string property to bind file path to</param>
        /// <param name="controllerName">Upload controller name that contains upload and delete actions</param>
        /// <param name="allowedExtensions">Allowed extensions to upload</param>
        /// <param name="fileMaxSize">Maximum file size to upload</param>
        /// <param name="withValidation">Add Validation message control?</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="onSuccessCallback">Javascript function name to call after upload success</param>
        /// <param name="onDeleteCallback">Javascript function name to call after deleting afile from the uploader</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString for uploader control with caption label</returns>
        public static MvcHtmlString MCIFileUploadItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, string controllerName = "Upload", string allowedExtensions = "'jpg', 'jpeg', 'png', 'bmp'",
            int fileMaxSize = 2048, bool withValidation = true, int spanOf12 = 6,
            string onSuccessCallback = "", string onDeleteCallback = "", string hint = "")
        {
            string name = ExpressionHelper.GetExpressionText(expression);

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormGroup()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(MCIFileUploaderFor(helper, expression, controllerName, allowedExtensions, fileMaxSize, onSuccessCallback, onDeleteCallback).ToString());
            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());

            return new MvcHtmlString(col.AppendInnerHtml(frmGroup).ToString());
        }

        public static MvcHtmlString MCIFileUploadItemForNoLabel<TModel, TProperty>(
          this HtmlHelper<TModel> helper,
          Expression<Func<TModel, TProperty>> expression, string controllerName = "Upload", string allowedExtensions = "'jpg', 'jpeg', 'png', 'bmp'",
          int fileMaxSize = 2048, bool withValidation = true, int spanOf12 = 6,
          string onSuccessCallback = "", string onDeleteCallback = "", string hint = "")
        {
            string name = ExpressionHelper.GetExpressionText(expression);

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormGroup();
            //    .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(MCIFileUploaderFor(helper, expression, controllerName, allowedExtensions, fileMaxSize, onSuccessCallback, onDeleteCallback).ToString());
            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());

            return new MvcHtmlString(col.AppendInnerHtml(frmGroup).ToString());
        }
        /// <summary>
        /// Return file uploader control full item (with caption) initialized with finUploader plugin
        /// </summary>
        /// <param name="name">Unique name for the uploader</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="labelText">Caption text</param>
        /// <param name="controllerName">Upload controller name that contains upload and delete actions</param>
        /// <param name="allowedExtensions">Allowed extensions to upload</param>
        /// <param name="fileMaxSize">Maximum file size to upload</param>
        /// <param name="required">Add required validation?</param>
        /// <param name="validationMessage">Validation message for required validation</param>
        /// <param name="onSuccessCallback">Javascript function name to call after upload success</param>
        /// <param name="onDeleteCallback">Javascript function name to call after deleting afile from the uploader</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString for uploader control with caption label</returns>
        public static MvcHtmlString MCIFileUploadItem<TModel>(
            this HtmlHelper<TModel> helper,
            string name,
            int spanOf12 = 6,
            string labelText = "",
            string controllerName = "Upload",
            string allowedExtensions = "'jpg', 'jpeg', 'png', 'bmp'",
            int fileMaxSize = 2048,
            bool required = true, string validationMessage = "الملف مطلوب",
            string onSuccessCallback = "", string onDeleteCallback = "", string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(MCIFileUploader(helper, name, controllerName, allowedExtensions, fileMaxSize, onSuccessCallback, onDeleteCallback, required, validationMessage).ToString());

            if (required)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, validationMessage, null, null).ToString());

            return new MvcHtmlString(col.AppendInnerHtml(frmGroup).ToString());
        }

        #endregion

        #region tab

        /// <summary>
        /// return html tag for bootstrab tab container item
        /// </summary>
        /// <param name="name">Unique name for the tab control</param>
        /// <param name="style">direction and border style of the tab control</param>
        /// <param name="inverse">true to inverse color</param>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginTabNavContainer(this HtmlHelper helper, string name, NavTabsStyle style = NavTabsStyle.HorizontalWithActiveBorder, bool inverse = false)
        {
            return new MCIDisposableHelper(helper, new MCITabContainer(name, style, inverse));
        }

        /// <summary>
        /// return html tag for bootstrab tab header item
        /// </summary>
        /// <param name="tabName">Unique name for the header</param>
        /// <param name="innerHtml">Inner html of the header link</param>
        /// <param name="active">Render it as active tab header?</param>
        /// <returns>MvcHtmlString for li tag of bootstrab tab header</returns>
        public static MvcHtmlString MCITabHeader(this HtmlHelper helper, string tabName, string innerHtml, bool active = false)
        {
            return new MCITabHeader(tabName, innerHtml, active).ToMvcHtmlString();
        }

        /// <summary>
        /// begin html tag for ul bootstrab tab nav bar item that contains tab headers
        /// </summary>
        /// <param name="tabName">Unique name for the tab header control</param>
        /// <returns>Begin ul bootstrab tab nav bar item that contains tab headers</returns>
        public static MCIDisposableHelper MCIBeginTabNavBar(this HtmlHelper helper, string tabName = "")
        {
            return new MCIDisposableHelper(helper, new MCINavBar(tabName));
        }

        /// <summary>
        /// Begin tab panel content
        /// </summary>
        /// <param name="name">Unique name for the tab panel (the same name of the corresponding tab header)</param>
        /// <param name="active">Enable focus on this panel by default</param>
        /// <param name="withFadeEffect">Enable fade effect on focus</param>
        /// <returns>begin tab panel div block</returns>
        public static MCIDisposableHelper MCIBeginTabPanel(this HtmlHelper helper, string name, bool active = false, bool withFadeEffect = true)
        {
            return new MCIDisposableHelper(helper, new MCITabPanel(name, active, withFadeEffect));
        }

        /// <summary>
        /// Begins tab panels container for all tab panels
        /// </summary>
        /// <param name="minHeight">Minimum height for the container</param>
        /// <returns>begin tab panels container div block</returns>
        public static MCIDisposableHelper MCIBeginTabPanels(this HtmlHelper helper, string minHeight = "")
        {
            return new MCIDisposableHelper(helper, new MCITabPanels(minHeight));
        }

        #endregion

        #region input form items

        #region row and column

        /// <summary>
        /// Start bootstrap row div tag
        /// </summary>
        /// <param name="htmlAttributes">custom html attributes for the div</param>
        /// <returns>start tag MvcHtmlString</returns>
        public static MvcHtmlString MCIStartFormItemsRow(this HtmlHelper helper, object htmlAttributes = null)
        {
            return new MCIItemsRow(htmlAttributes).ToMvcHtmlString(TagRenderMode.StartTag);
        }

        /// <summary>
        /// Ends bootstrap row div tag
        /// </summary>
        /// <returns>close tag MvcHtmlString</returns>
        public static MvcHtmlString MCIEndFormItemsRow(this HtmlHelper helper)
        {
            return new MCIItemsRow().ToMvcHtmlString(TagRenderMode.EndTag);
        }

        /// <summary>
        /// Writes &lt;div class="row"&gt; to the response
        /// </summary>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginItemsRow(this HtmlHelper helper, object htmlAttributes = null)
        {
            return new MCIDisposableHelper(helper, new MCIItemsRow(htmlAttributes));
        }


        /// <summary>
        /// Start bootstrap row div tag
        /// </summary>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="htmlAttributes">custom html attributes for the div</param>
        /// <returns>start tag MvcHtmlString</returns>
        public static MvcHtmlString MCIStartFormItemsCol(this HtmlHelper helper, int spanOf12 = 6, object htmlAttributes = null)
        {
            return new MCIItemsCol(spanOf12, htmlAttributes).ToMvcHtmlString(TagRenderMode.StartTag);
        }

        /// <summary>
        /// Ends bootstrap row div tag
        /// </summary>
        /// <returns>close tag MvcHtmlString</returns>
        public static MvcHtmlString MCIEndFormItemsCol(this HtmlHelper helper)
        {
            return new MCIItemsCol().ToMvcHtmlString(TagRenderMode.EndTag);
        }

        /// <summary>
        /// Writes &lt;div class="col-sm-[spanOf12]"&gt; to the response
        /// </summary>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginItemsCol(this HtmlHelper helper, int spanOf12 = 6, object htmlAttributes = null)
        {
            return new MCIDisposableHelper(helper, new MCIItemsCol(spanOf12, htmlAttributes));
        }

        #endregion

        #region Display items

        #region Display

        /// <summary>
        /// Returns display value for model property with the display name as caption in bootstrap form item
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, object httmlAttributes = null, bool postBackValue = false, string hint = "")
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormGroup()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(new MCIDisplayTag(name, httmlAttributes)
                .AppendInnerHtml(System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToString()));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.HiddenFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns display value with caption as bootstrap form item
        /// </summary>
        /// <param name="value">Item value to display</param>
        /// <param name="labelText">Item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        /// <param name="name">Unique name for the hidden field</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayItem<TModel>(
         this HtmlHelper<TModel> helper, string value, string labelText = "", int spanOf12 = 6, object httmlAttributes = null, bool postBackValue = false, string name = "postValue", string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormGroup()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(new MCIDisplayTag(name, httmlAttributes)
                .AppendInnerHtml(value));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.Hidden(helper, name, value).ToString());


            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns display value for model property with the display name as caption in bootstrap horizontal form item
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayItemFor_H<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, object httmlAttributes = null, bool postBackValue = false)
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormDisplay()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString())
                .AppendInnerHtml(new MCIDisplayTagH(name, httmlAttributes)
                    .AppendInnerHtml(System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToString()));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.HiddenFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns display value with caption as bootstrap horizontal form item
        /// </summary>
        /// <param name="value">Item value to display</param>
        /// <param name="labelText">Item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        /// <param name="name">Unique name for the hidden field</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayItem_H<TModel>(
         this HtmlHelper<TModel> helper, string value, string labelText = "", int spanOf12 = 6, object httmlAttributes = null, bool postBackValue = false, string name = "postValue")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormDisplay()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText).ToString())
                .AppendInnerHtml(new MCIDisplayTagH(name, value, httmlAttributes));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.Hidden(helper, name, value).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #region truncate display

        /// <summary>
        /// Returns display tag of a value truncated by the given length and use bootstrap pop-over to display the complete value
        /// </summary>
        /// <param name="name">Unique name for the display tag</param>
        /// <param name="value">Value to display</param>
        /// <param name="textMaxLength">Maximum length of the displayed text</param>
        /// <param name="popOverTitle">The pop-over title text</param>
        /// <param name="seeMoreHtml">html text for see more if the text is truncated</param>
        /// <param name="showAllContentInPopover">True if you want to show the complete value in the pop-over. False to display the rest of the value only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayWithTruncate<TModel>(
            this HtmlHelper<TModel> helper, string name, string value, int textMaxLength = 50, string popOverTitle = "", string seeMoreHtml = " المزيد >>",
            bool showAllContentInPopover = false)
        {
            var needTruncate = value.Length > textMaxLength;
            if (needTruncate)//compute real max length with space
            {
                string seperators = " .,-_،()[]&!";
                if (!seperators.Contains(value[textMaxLength]) && !seperators.Contains(value[textMaxLength - 1]))
                    textMaxLength = value.Substring(0, textMaxLength).LastIndexOfAny(seperators.ToCharArray());
            }

            string toDisplayText = needTruncate ? value.Substring(0, textMaxLength) + " ... " : value;
            string popoverText = value;
            if (needTruncate && !showAllContentInPopover)
                popoverText = "..." + value.Substring(textMaxLength, value.Length - textMaxLength);

            string result = toDisplayText;
            if (needTruncate)
            {
                string truncateHtml = string.Format("<a id='{0}_popoverDiv' class='mci-popover' tabindex='0' data-toggle='popover' data-placement='bottom auto' data-trigger='focus' data-container='body' data-title='{1}' data-content='{2}'>{3}</a>", name, popOverTitle, popoverText, seeMoreHtml);
                truncateHtml += string.Format(@"
                    <script>
                        $(function () {{
                            $('#{0}_popoverDiv').popover();
                        }});
                    </script>", name);

                result += truncateHtml;
            }
            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Returns display item of a value with caption truncated by the given length and use bootstrap pop-over to display the complete value
        /// </summary>
        /// <param name="name">Unique name for the display tag</param>
        /// <param name="value">Value to display</param>
        /// <param name="labelText">Item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="textMaxLength">Maximum length of the displayed text</param>
        /// <param name="popOverTitle">The pop-over title text</param>
        /// <param name="seeMoreHtml">html text for see more if the text is truncated</param>
        /// <param name="showAllContentInPopover">True if you want to show the complete value in the pop-over. False to display the rest of the value only</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayItemWithTruncate<TModel>(
            this HtmlHelper<TModel> helper, string name, string value, string labelText = "", int spanOf12 = 6,
            int textMaxLength = 50, string popOverTitle = "", string seeMoreHtml = " المزيد >>", bool showAllContentInPopover = false,
            object httmlAttributes = null, bool postBackValue = false, string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormGroup()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(new MCIDisplayTag(name, httmlAttributes)
                .AppendInnerHtml(MCIDisplayWithTruncate(helper, name, value, textMaxLength, popOverTitle, seeMoreHtml, showAllContentInPopover).ToString()));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.Hidden(helper, name, value).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns display tag of a value truncated by the given length and use bootstrap pop-over to display the complete value
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="textMaxLength">Maximum length of the displayed text</param>
        /// <param name="popOverTitle">The pop-over title text</param>
        /// <param name="seeMoreHtml">html text for see more if the text is truncated</param>
        /// <param name="showAllContentInPopover">True if you want to show the complete value in the pop-over. False to display the rest of the value only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayWithTruncateFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
            int textMaxLength = 50, string popOverTitle = "", string seeMoreHtml = " المزيد >>", bool showAllContentInPopover = false)
        {
            string value = System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToString();
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            string result = MCIDisplayWithTruncate(helper, name, value, textMaxLength, popOverTitle, seeMoreHtml, showAllContentInPopover).ToString();
            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Returns display item of a model value with caption truncated by the given length and use bootstrap pop-over to display the complete value
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="textMaxLength">Maximum length of the displayed text</param>
        /// <param name="popOverTitle">The pop-over title text</param>
        /// <param name="seeMoreHtml">html text for see more if the text is truncated</param>
        /// <param name="showAllContentInPopover">True if you want to show the complete value in the pop-over. False to display the rest of the value only</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>        
        public static MvcHtmlString MCIDisplayItemWithTruncateFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6,
            int textMaxLength = 50, string popOverTitle = "", string seeMoreHtml = " المزيد >>",
            bool showAllContentInPopover = false, object httmlAttributes = null, bool postBackValue = false, string hint = "")
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormGroup()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(new MCIDisplayTag(name, httmlAttributes)
                .AppendInnerHtml(MCIDisplayWithTruncateFor(helper, expression, textMaxLength, popOverTitle, seeMoreHtml, showAllContentInPopover).ToString()));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.HiddenFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        //horizontal

        /// <summary>
        /// Returns horizontal display item of a model value with caption truncated by the given length and use bootstrap pop-over to display the complete value
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="textMaxLength">Maximum length of the displayed text</param>
        /// <param name="popOverTitle">The pop-over title text</param>
        /// <param name="seeMoreHtml">html text for see more if the text is truncated</param>
        /// <param name="showAllContentInPopover">True if you want to show the complete value in the pop-over. False to display the rest of the value only</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        /// <returns>MvcHtmlString</returns>        
        public static MvcHtmlString MCIDisplayItemWithTruncateFor_H<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6,
            int textMaxLength = 50, string popOverTitle = "", string seeMoreHtml = " المزيد >>", bool showAllContentInPopover = false,
            object httmlAttributes = null, bool postBackValue = false)
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormDisplay()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString())
                .AppendInnerHtml(new MCIDisplayTagH(name, httmlAttributes)
                    .AppendInnerHtml(MCIDisplayWithTruncateFor(helper, expression, textMaxLength, popOverTitle, seeMoreHtml, showAllContentInPopover).ToString()));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.HiddenFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns horizontal display item of a value with caption truncated by the given length and use bootstrap pop-over to display the complete value
        /// </summary>
        /// <param name="name">Unique name for the display tag</param>
        /// <param name="value">Value to display</param>
        /// <param name="labelText">Item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="textMaxLength">Maximum length of the displayed text</param>
        /// <param name="popOverTitle">The pop-over title text</param>
        /// <param name="seeMoreHtml">html text for see more if the text is truncated</param>
        /// <param name="showAllContentInPopover">True if you want to show the complete value in the pop-over. False to display the rest of the value only</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the value tag</param>
        /// <param name="postBackValue">If true, value will be posted back in hidden input field</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDisplayItemWithTruncate_H<TModel>(
            this HtmlHelper<TModel> helper, string value, string labelText = "", int spanOf12 = 6,
            int textMaxLength = 50, string popOverTitle = "", string seeMoreHtml = " المزيد >>", bool showAllContentInPopover = false,
            object httmlAttributes = null, bool postBackValue = false, string name = "postValue")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);

            MCITagBuilder frmGroup = new MCIFormDisplay()
                .AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText).ToString())
                .AppendInnerHtml(new MCIDisplayTagH(name, httmlAttributes)
                    .AppendInnerHtml(MCIDisplayWithTruncate(helper, name, value, textMaxLength, popOverTitle, seeMoreHtml, showAllContentInPopover).ToString()));

            if (postBackValue)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.Hidden(helper, name, value).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #endregion

        #region Textbox, Textarea and Password

        /// <summary>
        /// Returns input control item with caption in bootstrap form item
        /// </summary>
        /// <param name="name">Unique name for the input tag</param>
        /// <param name="defaultValue">Input default value</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input control</param>
        /// <param name="format">Input value format</param>
        /// <param name="readOnly">If true, the value will be span styled as disabled text input</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        /// <param name="withValidation">True will add ValidationMessage for the input</param>
        /// <param name="errorMessege">Default error message for the validation</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCITextBoxItem<TModel>(
         this HtmlHelper<TModel> helper, string name, string defaultValue, string labelText = "", int spanOf12 = 6, object httmlAttributes = null, string format = "", bool readOnly = false,
            string postAddon = "", string preAddon = "", bool withValidation = false, string errorMessege = "الرجاء إدخال الحقل", string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            if (readOnly)
            {
                TagBuilder newTagBuilder = new TagBuilder("span");
                newTagBuilder.MergeAttribute("disabled", "");
                if (!customAttributes.ContainsKey("id"))
                    newTagBuilder.MergeAttribute("id", name);
                newTagBuilder.MergeAttributes(customAttributes);
                newTagBuilder.SetInnerText(defaultValue);

                controlHtml = newTagBuilder.ToString();
            }
            else
            {
                controlHtml = System.Web.Mvc.Html.InputExtensions.TextBox(helper, name, defaultValue, format, customAttributes).ToString();

                if (withValidation)
                    controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, errorMessege, null, null);
            }


            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns input control item with caption in bootstrap form item mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input control</param>
        /// <param name="format">Input value format</param>
        /// <param name="readOnly">If true, the value will be span styled as disabled text input</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        /// <param name="withValidation">True will add ValidationMessage for the input</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCITextBoxItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, object httmlAttributes = null, string format = "", bool readOnly = false,
            string postAddon = "", string preAddon = "", bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            if (readOnly)
            {
                TagBuilder newTagBuilder = new TagBuilder("span");
                newTagBuilder.MergeAttribute("disabled", "");
                if (!customAttributes.ContainsKey("id"))
                    newTagBuilder.MergeAttribute("id", helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
                newTagBuilder.MergeAttributes(customAttributes);
                newTagBuilder.SetInnerText(System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToString());

                controlHtml = newTagBuilder.ToString();
            }
            else
            {
                controlHtml = System.Web.Mvc.Html.InputExtensions.TextBoxFor(helper, expression, format, customAttributes).ToString();

                if (withValidation)
                    controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression);
            }

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        public static MvcHtmlString MCITextBoxItemForNoLabel<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, object httmlAttributes = null, string format = "", bool readOnly = false,
            string postAddon = "", string preAddon = "", bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            if (readOnly)
            {
                TagBuilder newTagBuilder = new TagBuilder("span");
                newTagBuilder.MergeAttribute("disabled", "");
                if (!customAttributes.ContainsKey("id"))
                    newTagBuilder.MergeAttribute("id", helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
                newTagBuilder.MergeAttributes(customAttributes);
                newTagBuilder.SetInnerText(System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToString());

                controlHtml = newTagBuilder.ToString();
            }
            else
            {
                controlHtml = System.Web.Mvc.Html.InputExtensions.TextBoxFor(helper, expression, format, customAttributes).ToString();

                if (withValidation)
                    controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression);
            }

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            //frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns multiline input control item with caption in bootstrap form item
        /// </summary>
        /// <param name="name">Unique name for the input tag</param>
        /// <param name="defaultValue">Input default value</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input control</param>
        /// <param name="readOnly">If true, the value will be span styled as disabled text input</param>
        /// <param name="withValidation">True will add ValidationMessage for the input</param>
        /// <param name="errorMessege">Default error message for the validation</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCITextAreaItem<TModel>(
            this HtmlHelper<TModel> helper, string name, string defaultValue, string labelText = "", int spanOf12 = 6, object httmlAttributes = null, bool readOnly = false,
            bool withValidation = false, string errorMessege = "الرجاء إدخال الحقل", string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());


            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            if (readOnly)
            {
                TagBuilder newTagBuilder = new TagBuilder("span");
                newTagBuilder.MergeAttribute("disabled", "");
                if (!customAttributes.ContainsKey("id"))
                    newTagBuilder.MergeAttribute("id", name);
                newTagBuilder.MergeAttributes(customAttributes);
                newTagBuilder.SetInnerText(defaultValue);

                frmGroup.AppendInnerHtml(newTagBuilder.ToString());
            }
            else
            {
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.TextAreaExtensions.TextArea(helper, name, defaultValue, customAttributes).ToString());

                if (withValidation)
                    frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, errorMessege, null, null).ToString());
            }

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns multiline input control item with caption in bootstrap form item mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input control</param>
        /// <param name="readOnly">If true, the value will be span styled as disabled text input</param>
        /// <param name="withValidation">True will add ValidationMessage for the input</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns></returns>
        public static MvcHtmlString MCITextAreaItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, object httmlAttributes = null, bool readOnly = false, bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            if (readOnly)
                frmGroup.AppendInnerHtmlFormat("<span disabled>{0}</span>", System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToString());
            else
            {
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(helper, expression, customAttributes).ToString());

                if (withValidation)
                    frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());
            }

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        public static MvcHtmlString MCITextAreaItemForNoLabel<TModel, TProperty>(
        this HtmlHelper<TModel> helper,
        Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, object httmlAttributes = null, bool readOnly = false, bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            //frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            if (readOnly)
                frmGroup.AppendInnerHtmlFormat("<span disabled>{0}</span>", System.Web.Mvc.Html.DisplayExtensions.DisplayFor(helper, expression).ToString());
            else
            {
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(helper, expression, customAttributes).ToString());

                if (withValidation)
                    frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());
            }

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns password input control item with caption in bootstrap form item mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input control</param>
        /// <param name="withValidation">True will add ValidationMessage for the input</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns></returns>
        public static MvcHtmlString MCIPasswordItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, object httmlAttributes = null, bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";
            customAttributes["autocomplete"] = "off";

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.PasswordFor(helper, expression, customAttributes).ToString());

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns password input control item with caption in bootstrap form item
        /// </summary>
        /// <param name="name">Unique name for the input tag</param>
        /// <param name="defaultValue">Input default value</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input control</param>
        /// <param name="withValidation">True will add ValidationMessage for the input</param>
        /// <param name="errorMessege">Default error message for the validation</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIPasswordItem<TModel>(
            this HtmlHelper<TModel> helper, string name, string defaultValue, string labelText = "", int spanOf12 = 6, object httmlAttributes = null,
            bool withValidation = true, string errorMessege = "الرجاء إدخال الحقل", string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";
            customAttributes["autocomplete"] = "off";

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());


            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.Password(helper, name, defaultValue, customAttributes).ToString());

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, errorMessege, null, null).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #region Dropdownlist

        #region normal Dropdownlist item

        /// <summary>
        /// Returns single-selection select element in bootstrap form item with caption mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDropDownListItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null,
            string postAddon = "", string preAddon = "", bool withValidation = true, string hint = "")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            controlHtml = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(helper, expression, selectList, optionLabel, customAttributes).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression);

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns single-selection encrypted keys select element in bootstrap form item with caption mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedDropDownListItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null,
            string postAddon = "", string preAddon = "", bool withValidation = true, string hint = "")
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            string fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionName);
            string fullId = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(expressionName);
            string displayName = System.Web.Mvc.Html.DisplayNameExtensions.DisplayNameFor(helper, expression).ToString();

            if (selectList == null)
                selectList = new List<SelectListItem>();
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            controlHtml = EncryptedSelectExtensions.EncryptedDropDownListFor(helper, expression, selectList, optionLabel, customAttributes).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullName)).ToString();

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, displayName, new { @for = string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullId) }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns single-selection select element in bootstrap form item with caption
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="defaultValidationMessege">Default error message for the validation</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIDropDownListItem<TModel>(
         this HtmlHelper<TModel> helper, string name, IEnumerable<SelectListItem> selectList,
            string labelText = " ", string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null,
            string postAddon = "", string preAddon = "", bool withValidation = true, string defaultValidationMessege = "الرجاء إدخال الحقل", string hint = "")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            controlHtml = System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, selectList, optionLabel, customAttributes).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, defaultValidationMessege, null, null).ToString();


            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);


            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns single-selection select element with encrypted options in bootstrap form item with caption
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="defaultValidationMessege">Default error message for the validation</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedDropDownListItem<TModel>(
         this HtmlHelper<TModel> helper, string name, IEnumerable<SelectListItem> selectList,
            string labelText = " ", string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null,
            string postAddon = "", string preAddon = "", bool withValidation = true, string defaultValidationMessege = "الرجاء إدخال الحقل", string hint = "")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            controlHtml = EncryptedSelectExtensions.EncryptedDropDownList(helper, name, selectList, optionLabel, customAttributes).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, string.Concat(StringEncrypter.ControlsEncrypter.Prefix, name), defaultValidationMessege, null, null).ToString();


            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);


            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns single-selection select element in bootstrap form item with caption mapped to enum value model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIEnumDropDownListItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression,
            string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null,
            string postAddon = "", string preAddon = "", bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            string controlHtml = "";

            controlHtml = System.Web.Mvc.Html.SelectExtensions.EnumDropDownListFor(helper, expression, optionLabel, customAttributes).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression);

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #region Ajax Dropdownlist

        /// <summary>
        /// Returns single-selection select element with ajax enabled on value changed
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxDropDownList<TModel>(
         this HtmlHelper<TModel> helper, string name, IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", object httmlAttributes = null, string additionalJSONDataFunctionProvider = "")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();

            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";
            if (!customAttributes.ContainsKey("id"))
                customAttributes["id"] = name;

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            var ajaxAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            //merge two attributes
            foreach (var ajaxAttribute in ajaxAttributes)
                customAttributes[ajaxAttribute.Key] = ajaxAttribute.Value;

            if (!string.IsNullOrEmpty(additionalJSONDataFunctionProvider))
                customAttributes["data-ajax-dataprovider"] = additionalJSONDataFunctionProvider;

            string result = System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, selectList, optionLabel, customAttributes).ToString();

            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Returns encrypted single-selection select element with ajax enabled on value changed
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxDropDownList<TModel>(
         this HtmlHelper<TModel> helper, string name, IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", object httmlAttributes = null, string additionalJSONDataFunctionProvider = "")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();

            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";
            if (!customAttributes.ContainsKey("id"))
                customAttributes["id"] = name;

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            var ajaxAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            //merge two attributes
            foreach (var ajaxAttribute in ajaxAttributes)
                customAttributes[ajaxAttribute.Key] = ajaxAttribute.Value;

            if (!string.IsNullOrEmpty(additionalJSONDataFunctionProvider))
                customAttributes["data-ajax-dataprovider"] = additionalJSONDataFunctionProvider;

            string result = EncryptedSelectExtensions.EncryptedDropDownList(helper, name, selectList, optionLabel, customAttributes).ToString();

            return new MvcHtmlString(result);
        }


        /// <summary>
        /// Returns single-selection select element with ajax enabled on value changed mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", object httmlAttributes = null, string additionalJSONDataFunctionProvider = "")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();

            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            var ajaxAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            //merge two attributes
            foreach (var ajaxAttribute in ajaxAttributes)
                customAttributes[ajaxAttribute.Key] = ajaxAttribute.Value;

            if (!string.IsNullOrEmpty(additionalJSONDataFunctionProvider))
                customAttributes["data-ajax-dataprovider"] = additionalJSONDataFunctionProvider;

            string result = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(helper, expression, selectList, optionLabel, customAttributes).ToString();

            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Returns encrypted single-selection select element with ajax enabled on value changed mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", object httmlAttributes = null, string additionalJSONDataFunctionProvider = "")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();

            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            var ajaxAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            //merge two attributes
            foreach (var ajaxAttribute in ajaxAttributes)
                customAttributes[ajaxAttribute.Key] = ajaxAttribute.Value;

            if (!string.IsNullOrEmpty(additionalJSONDataFunctionProvider))
                customAttributes["data-ajax-dataprovider"] = additionalJSONDataFunctionProvider;

            string result = EncryptedSelectExtensions.EncryptedDropDownListFor(helper, expression, selectList, optionLabel, customAttributes).ToString();

            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Returns single-selection select element with ajax enabled on value changed mapped to enum value model property using the Enum for select items
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxEnumDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", object httmlAttributes = null, string additionalJSONDataFunctionProvider = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            var ajaxAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            //merge two attributes
            foreach (var ajaxAttribute in ajaxAttributes)
                customAttributes[ajaxAttribute.Key] = ajaxAttribute.Value;

            if (!string.IsNullOrEmpty(additionalJSONDataFunctionProvider))
                customAttributes["data-ajax-dataprovider"] = additionalJSONDataFunctionProvider;

            string result = System.Web.Mvc.Html.SelectExtensions.EnumDropDownListFor(helper, expression, optionLabel, customAttributes).ToString();

            return new MvcHtmlString(result);
        }

        /// <summary>
        /// Returns encrypted single-selection select element with ajax enabled on value changed mapped to enum value model property using the Enum for select items
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxEnumDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", object httmlAttributes = null, string additionalJSONDataFunctionProvider = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            var ajaxAttributes = ajaxOptions.ToUnobtrusiveHtmlAttributes();

            //merge two attributes
            foreach (var ajaxAttribute in ajaxAttributes)
                customAttributes[ajaxAttribute.Key] = ajaxAttribute.Value;

            if (!string.IsNullOrEmpty(additionalJSONDataFunctionProvider))
                customAttributes["data-ajax-dataprovider"] = additionalJSONDataFunctionProvider;

            string result = EncryptedSelectExtensions.EncryptedEnumDropDownListFor(helper, expression, optionLabel, customAttributes).ToString();

            return new MvcHtmlString(result);
        }


        /// <summary>
        /// Returns single-selection select element with ajax enabled on value changed in bootstrap form item with caption
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="defaultValidationMessege">Default error message for the validation</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxDropDownListItem<TModel>(
            this HtmlHelper<TModel> helper, string name, IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string labelText = " ", string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null, string additionalJSONDataFunctionProvider = "",
            string postAddon = "", string preAddon = "", string hint = "",
            bool withValidation = true, string defaultValidationMessege = "الرجاء إدخال الحقل")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();

            string controlHtml = "";

            controlHtml = MCIAjaxDropDownList(helper, name, selectList, ajaxOptions, optionLabel, httmlAttributes, additionalJSONDataFunctionProvider).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, defaultValidationMessege, null, null).ToString();


            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);


            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns encrypted single-selection select element with ajax enabled on value changed in bootstrap form item with caption
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="defaultValidationMessege">Default error message for the validation</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxDropDownListItem<TModel>(
            this HtmlHelper<TModel> helper, string name, IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string labelText = " ", string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null, string additionalJSONDataFunctionProvider = "",
            string postAddon = "", string preAddon = "", string hint = "",
            bool withValidation = true, string defaultValidationMessege = "الرجاء إدخال الحقل")
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();

            string controlHtml = "";

            controlHtml = EncryptedAjaxDropDownList(helper, name, selectList, ajaxOptions, optionLabel, httmlAttributes, additionalJSONDataFunctionProvider).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, string.Concat(StringEncrypter.ControlsEncrypter.Prefix, name), defaultValidationMessege, null, null).ToString();


            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = string.Concat(StringEncrypter.ControlsEncrypter.Prefix, name) }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);


            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns single-selection select element with ajax enabled on value changed in bootstrap form item with caption mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxDropDownListItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null, string additionalJSONDataFunctionProvider = "",
            string postAddon = "", string preAddon = "", string hint = "", bool withValidation = true)
        {
            if (selectList == null)
                selectList = new List<SelectListItem>();

            string controlHtml = "";

            controlHtml = MCIAjaxDropDownListFor(helper, expression, selectList, ajaxOptions, optionLabel, httmlAttributes, additionalJSONDataFunctionProvider).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression);

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns Encrypted single-selection select element with ajax enabled on value changed in bootstrap form item with caption mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the drop-down list</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxDropDownListItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null, string additionalJSONDataFunctionProvider = "",
            string postAddon = "", string preAddon = "", string hint = "", bool withValidation = true)
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            string fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionName);
            string fullId = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(expressionName);
            string displayName = System.Web.Mvc.Html.DisplayNameExtensions.DisplayNameFor(helper, expression).ToString();

            if (selectList == null)
                selectList = new List<SelectListItem>();

            string controlHtml = "";

            controlHtml = EncryptedAjaxDropDownListFor(helper, expression, selectList, ajaxOptions, optionLabel, httmlAttributes, additionalJSONDataFunctionProvider).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullId)).ToString();

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, displayName, new { @for = string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullId) }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns single-selection select element with ajax enabled on value changed mapped to enum value model property using the Enum for select items. all in bootstrap form item with caption
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxEnumDropDownListItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null, string additionalJSONDataFunctionProvider = "",
            string postAddon = "", string preAddon = "", string hint = "", bool withValidation = true)
        {
            string controlHtml = "";

            controlHtml = MCIAjaxEnumDropDownListFor(helper, expression, ajaxOptions, optionLabel, httmlAttributes).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression);

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns encrypted single-selection select element with ajax enabled on value changed mapped to enum value model property using the Enum for select items. all in bootstrap form item with caption
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="optionLabel">Option Label for the null item</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="additionalJSONDataFunctionProvider">Javascript function that will return Json object to send with drop-down value inside the ajax request</param>
        /// <param name="postAddon">Text to add after input as bootstrap addon</param>
        /// <param name="preAddon">Text to add before input as bootstrap addon</param>
        ///  <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedAjaxEnumDropDownListItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, AjaxOptions ajaxOptions,
            string optionLabel = "اختر", int spanOf12 = 6, object httmlAttributes = null, string additionalJSONDataFunctionProvider = "",
            string postAddon = "", string preAddon = "", string hint = "", bool withValidation = true)
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            string fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionName);
            string fullId = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(expressionName);
            string displayName = System.Web.Mvc.Html.DisplayNameExtensions.DisplayNameFor(helper, expression).ToString();

            string controlHtml = "";

            controlHtml = EncryptedAjaxEnumDropDownListFor(helper, expression, ajaxOptions, optionLabel, httmlAttributes).ToString();

            if (withValidation)
                controlHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullId)).ToString();

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, displayName, new { @for = string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullId) }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            //if there is addon
            if (!string.IsNullOrEmpty(postAddon) || !string.IsNullOrEmpty(preAddon))
            {
                MCITagBuilder inputGroup = new MCIInputGroup();
                //preAddon
                if (!string.IsNullOrEmpty(preAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(preAddon));

                //input control
                inputGroup.AppendInnerHtml(controlHtml);

                //postAddon
                if (!string.IsNullOrEmpty(postAddon))
                    inputGroup.AppendInnerHtml(new MCIInputGroupAddon(postAddon));

                frmGroup.AppendInnerHtml(inputGroup);
            }
            else
                frmGroup.AppendInnerHtml(controlHtml);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #endregion

        #region Checkbox item

        /// <summary>
        /// Returns checkbox input with a label inside form item with caption mapped to boolean model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="cbLabel">Text to display for the checkbox element</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="displayAsButton">If true the checkbox will be displayed as a button using bootstrap button</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCICheckBoxItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, string cbLabel = "", int spanOf12 = 6, bool displayAsButton = false, string hint = "", object httmlAttributes = null)
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            TagBuilder myDiv = new TagBuilder("div");
            if (displayAsButton)
                myDiv.Attributes.Add("data-toggle", "buttons");

            myDiv.InnerHtml = string.Format("<label class='{0}'>", displayAsButton ? "btn btn-default btn-sm" : "checkbox-inline");
            var typedExpression = (System.Linq.Expressions.Expression<System.Func<TModel, bool>>)(object)expression;
            myDiv.InnerHtml += System.Web.Mvc.Html.InputExtensions.CheckBoxFor(helper, typedExpression, httmlAttributes).ToString();
            myDiv.InnerHtml += cbLabel;
            myDiv.InnerHtml += "</label>";

            frmGroup.AppendInnerHtml(myDiv.ToString());

            if (displayAsButton)
            {
                frmGroup.AppendInnerHtmlFormat(
                    @"<script>
                        $(function () {{
                            $('input[name={0}]:checked').parent('.btn').button('toggle');
                        }});
                    </script>", name);
            }

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns checkbox input with a label inside form item with caption 
        /// </summary>
        /// <param name="name">Unique name for the input element</param>
        /// <param name="itemLabel">form item caption text</param>
        /// <param name="cbLabel">Text to display for the checkbox element</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="isChecked">The initial state of the input</param>
        /// <param name="displayAsButton">If true the checkbox will be displayed as a button using bootstrap button</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCICheckBoxItem<TModel>(
            this HtmlHelper<TModel> helper, string name, string itemLabel, string cbLabel = "", int spanOf12 = 6, bool isChecked = false, bool displayAsButton = false, string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtmlFormat("<label for='{2}' {1}>{0}</label>", itemLabel, string.IsNullOrEmpty(itemLabel) ? " class='sr-only'" : "", name);

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            TagBuilder myDiv = new TagBuilder("div");
            if (displayAsButton)
                myDiv.Attributes.Add("data-toggle", "buttons");

            myDiv.InnerHtml += string.Format("<label class='{0}'>", displayAsButton ? "btn btn-default btn-sm" : "checkbox-inline");
            myDiv.InnerHtml += System.Web.Mvc.Html.InputExtensions.CheckBox(helper, name, isChecked).ToString();
            myDiv.InnerHtml += cbLabel;
            myDiv.InnerHtml += "</label>";

            frmGroup.AppendInnerHtml(myDiv.ToString());

            if (displayAsButton)
            {
                frmGroup.AppendInnerHtmlFormat(
                    @"<script>
                        $(function () {{
                            $('input[name={0}]:checked').parent('.btn').button('toggle');
                        }});
                    </script>", name);
            }

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns multiple checkbox elements inside one form item with caption
        /// </summary>
        /// <param name="name">Unique name for the item</param>
        /// <param name="itemList">A collection of System.Web.Mvc.SelectListItem object that are used to populate checkboxes</param>
        /// <param name="itemLabel">form item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="style">Checkboxes style</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCICheckBoxItem_Multi<TModel>(
            this HtmlHelper<TModel> helper,
            string name, IEnumerable<SelectListItem> itemList, string itemLabel = "",
            int spanOf12 = 6, CheckBoxStyles style = CheckBoxStyles.Inline, string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtmlFormat("<label for='{2}' {1}>{0}</label>", itemLabel, string.IsNullOrEmpty(itemLabel) ? " class='sr-only'" : "", name);

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));


            TagBuilder myDiv = new TagBuilder("div");
            if (style == CheckBoxStyles.ButtonGroup || style == CheckBoxStyles.Buttons)
            {
                if (style == CheckBoxStyles.ButtonGroup) myDiv.AddCssClass("btn-group");
                myDiv.Attributes.Add("data-toggle", "buttons");
            }

            string labelClasses = "";
            switch (style)
            {
                case CheckBoxStyles.ButtonGroup:
                case CheckBoxStyles.Buttons:
                    labelClasses = "btn btn-default btn-sm";
                    break;
                case CheckBoxStyles.Inline:
                    labelClasses = "checkbox-inline";
                    break;
                case CheckBoxStyles.Vertical:
                    break;
            }

            int i = 0;
            foreach (var item in itemList)
            {
                if (style == CheckBoxStyles.Vertical) myDiv.InnerHtml += "<div class='checkbox'>";
                myDiv.InnerHtml += string.Format("<label class='{0}' {1}>", labelClasses, style == CheckBoxStyles.Buttons ? "style='margin: 0 0 0 5px'" : "") +
                    System.Web.Mvc.Html.InputExtensions.CheckBox(helper, name, item.Selected, new { id = (name + "_" + i), value = item.Value }).ToString() +
                    item.Text +
                    "</label>";
                if (style == CheckBoxStyles.Vertical) myDiv.InnerHtml += "</div>";
                i++;
            }
            frmGroup.AppendInnerHtml(myDiv.ToString());

            if (style == CheckBoxStyles.ButtonGroup || style == CheckBoxStyles.Buttons)
            {
                frmGroup.AppendInnerHtmlFormat(
                    @"<script>
                        $(function () {{
                            $('input[name={0}]:checked').parent('.btn').button('toggle');
                        }});
                    </script>", name);
            }

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns multiple checkbox elements inside one form item with caption mapped to boolean model properties
        /// </summary>
        /// <param name="itemLabel">form item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="style">Checkboxes style</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <param name="expressions">Lambda expressions for boolean properties</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCICheckBoxItemFor_Multi<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            string itemLabel, int spanOf12, CheckBoxStyles style = CheckBoxStyles.Inline, string hint = "",
            params Expression<Func<TModel, TProperty>>[] expressions)
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtmlFormat("<label{1}>{0}</label>", itemLabel, string.IsNullOrEmpty(itemLabel) ? " class='sr-only'" : "");

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            TagBuilder myDiv = new TagBuilder("div");
            if (style == CheckBoxStyles.ButtonGroup || style == CheckBoxStyles.Buttons)
            {
                if (style == CheckBoxStyles.ButtonGroup) myDiv.AddCssClass("btn-group");
                myDiv.Attributes.Add("data-toggle", "buttons");
            }

            string labelClasses = "";
            switch (style)
            {
                case CheckBoxStyles.ButtonGroup:
                case CheckBoxStyles.Buttons:
                    labelClasses = "btn btn-default btn-sm";
                    break;
                case CheckBoxStyles.Inline:
                    labelClasses = "checkbox-inline";
                    break;
                case CheckBoxStyles.Vertical:
                    break;
            }

            string namesSelector = "";
            string displayName, name;
            ModelMetadata metadata;
            Expression<System.Func<TModel, bool>> typedExpression;
            for (int i = 0; i < expressions.Length; i++)
            {
                //get display name from meta data
                metadata = ModelMetadata.FromLambdaExpression(expressions[i], helper.ViewData);
                displayName = helper.Encode(metadata.DisplayName);
                name = ExpressionHelper.GetExpressionText(expressions[i]).Replace('.', '_');
                if (string.IsNullOrEmpty(displayName)) displayName = name;

                namesSelector += string.Format("{1}input[name={0}]:checked", name, i != 0 ? "," : "");

                if (style == CheckBoxStyles.Vertical) myDiv.InnerHtml += "<div class='checkbox'>";
                myDiv.InnerHtml += string.Format("<label class='{0}' {1}>", labelClasses, style == CheckBoxStyles.Buttons ? "style='margin-left:5px'" : "");
                typedExpression = (System.Linq.Expressions.Expression<System.Func<TModel, bool>>)(object)expressions[i];
                myDiv.InnerHtml += System.Web.Mvc.Html.InputExtensions.CheckBoxFor(helper, typedExpression).ToString() +
                displayName +
                "</label>";

                if (style == CheckBoxStyles.Vertical) myDiv.InnerHtml += "</div>";
            }
            frmGroup.AppendInnerHtml(myDiv.ToString());

            if (style == CheckBoxStyles.ButtonGroup || style == CheckBoxStyles.Buttons)
            {
                frmGroup.AppendInnerHtmlFormat(
                    @"<script>
                        $(function () {{
                            $('{0}').parent('.btn').button('toggle');
                        }});
                    </script>", namesSelector);
            }

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #region Radiobutton item

        /// <summary>
        /// Returns Radio button inputs binded to model property inside form item with caption 
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="itemList">A collection of System.Web.Mvc.SelectListItem object that are used to populate radiobuttons</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="style">Radiobuttons style</param>
        /// <param name="withValidation">True will add ValidationMessage for the item</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIRadioButtonItemFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> itemList,
            int spanOf12 = 6, RadioButtonStyle style = RadioButtonStyle.Inline, bool withValidation = true, string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(MCIRadioButtonFor(helper, expression, itemList, withValidation, style).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns Radio button inputs for one property inside form item with caption
        /// </summary>
        /// <param name="name">Unique name for the property</param>
        /// <param name="itemLabel">form item caption text</param>
        /// <param name="itemList">A collection of System.Web.Mvc.SelectListItem object that are used to populate radiobuttons</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="style">Radiobuttons style</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIRadioButtonItem<TModel>(
            this HtmlHelper<TModel> helper, string name, string itemLabel,
            SelectList itemList,
            int spanOf12 = 6, RadioButtonStyle style = RadioButtonStyle.Inline, string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, itemLabel, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(MCIRadioButtons(helper, name, itemList, style).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns Radio button inputs binded to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="itemList">A collection of System.Web.Mvc.SelectListItem object that are used to populate radiobuttons</param>
        /// <param name="withValidation">True will add ValidationMessage for the item</param>
        /// <param name="style">Radiobuttons style</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIRadioButtonFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> itemList,
            bool withValidation = true, RadioButtonStyle style = RadioButtonStyle.Inline)
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            StringBuilder result = new StringBuilder();

            TagBuilder myDiv = new TagBuilder("div");
            if (style == RadioButtonStyle.ButtonGroup)
            {
                myDiv.AddCssClass("btn-group");
                myDiv.Attributes.Add("data-toggle", "buttons");
            }

            string labelClasses = "";
            switch (style)
            {
                case RadioButtonStyle.ButtonGroup:
                    labelClasses = "btn btn-default btn-sm";
                    break;
                case RadioButtonStyle.Inline:
                    labelClasses = "radio-inline";
                    break;
                case RadioButtonStyle.Vertical:
                    break;
            }

            int i = 0;
            string id;
            foreach (var item in itemList)
            {
                id = name + "_" + i.ToString();
                if (style == RadioButtonStyle.Vertical) result.Append("<div class='radio'>");
                result.AppendFormat("<label class='{0}'>", labelClasses);

                result.Append(System.Web.Mvc.Html.InputExtensions.RadioButtonFor(helper, expression, item.Value, new { @id = id }).ToString());
                result.Append(item.Text);
                result.Append("</label>");
                if (style == RadioButtonStyle.Vertical) result.Append("</div>");
                i++;
            }
            if (withValidation)
                result.Append(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression));
            myDiv.InnerHtml = result.ToString();
            result = new StringBuilder(myDiv.ToString());
            if (style == RadioButtonStyle.ButtonGroup)
            {
                result.AppendFormat(
                    @"<script>
                        $(function () {{
                            $('input[name={0}]:checked').parent().button('toggle');
                        }});
                    </script>", name);
            }
            return new MvcHtmlString(result.ToString());
        }

        /// <summary>
        /// Returns Radio button inputs from select list source
        /// </summary>
        /// <param name="name">Unique name for the property</param>
        /// <param name="itemList">A collection of System.Web.Mvc.SelectListItem object that are used to populate radiobuttons</param>
        /// <param name="style">Radiobuttons style</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIRadioButtons<TModel>(
            this HtmlHelper<TModel> helper, string name,
            SelectList itemList,
            RadioButtonStyle style = RadioButtonStyle.Inline)
        {
            TagBuilder myDiv = new TagBuilder("div");
            if (style == RadioButtonStyle.ButtonGroup)
            {
                myDiv.AddCssClass("btn-group");
                myDiv.Attributes.Add("data-toggle", "buttons");
            }

            itemList = itemList ?? new SelectList(new List<SelectListItem>());

            string labelClasses = "";
            switch (style)
            {
                case RadioButtonStyle.ButtonGroup:
                    labelClasses = "btn btn-default btn-sm";
                    break;
                case RadioButtonStyle.Inline:
                    labelClasses = "radio-inline";
                    break;
                case RadioButtonStyle.Vertical:
                    break;
            }
            int i = 0;
            string id;
            foreach (var item in itemList)
            {
                id = name + "_" + i.ToString();
                if (style == RadioButtonStyle.Vertical) myDiv.InnerHtml += "<div class='radio'>";
                myDiv.InnerHtml += string.Format("<label class='{0}'>", labelClasses) +
                    System.Web.Mvc.Html.InputExtensions.RadioButton(helper, name, item.Value, item.Value == (string)itemList.SelectedValue, new { @id = id }).ToString() +
                    item.Text +
                    "</label>";

                if (style == RadioButtonStyle.Vertical) myDiv.InnerHtml += "</div>";
                i++;
            }

            StringBuilder result = new StringBuilder(myDiv.ToString());

            if (style == RadioButtonStyle.ButtonGroup)
            {
                result.AppendFormat(
                    @"<script>
                        $(function () {{
                            $('input[name={0}]:checked').parent().button('toggle');
                        }});
                    </script>", name);
            }
            return new MvcHtmlString(result.ToString());
        }

        #endregion

        #region Captcha

        /// <summary>
        /// Returns input element with captcha image appended to the input and the input is mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the captcha property</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="withValidation">True will add ValidationMessage for the item</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input element</param>
        /// <param name="imageSource">Url for the image source (Default is ~/shared/_CaptchaImage)</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCICaptchaItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, bool withValidation = true, object httmlAttributes = null, string imageSource = "", string hint = "")
        {
            RouteValueDictionary attr = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            attr["autocomplete"] = "off";
            attr["class"] += " form-control";

            if (string.IsNullOrWhiteSpace(imageSource))
            {
                var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                imageSource = urlHelper.Action("_CaptchaImage", "Shared") + "/" + System.Guid.NewGuid().ToString();
            }

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtmlFormat("<div><div class='col-sm-6'>{0}</div><span class='captcha' style='padding: 0 !important; border: none'><img alt='Captcha' src={1}/></span></div>",
                System.Web.Mvc.Html.InputExtensions.TextBoxFor(helper, expression, attr).ToString(), imageSource);

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();

        }

        /// <summary>
        /// Returns input element with captcha image appended to the input
        /// </summary>
        /// <param name="name">Unique name for the input element</param>
        /// <param name="labelText">form item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="withValidation">True will add ValidationMessage for the item</param>
        /// <param name="errorMessege">Default error message for the validation</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the input element</param>
        /// <param name="imageSource">Url for the image source (Default is ~/shared/_CaptchaImage)</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCICaptchaItem<TModel>(
            this HtmlHelper<TModel> helper,
            string name, string labelText = "", int spanOf12 = 6, bool withValidation = true, string errorMessege = "الرجاء إدخال الحقل", object httmlAttributes = null, string imageSource = "", string hint = "")
        {
            StringBuilder result = new StringBuilder();
            RouteValueDictionary attr = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            attr["autocomplete"] = "off";
            attr["class"] += " form-control";

            if (!attr["class"].ToString().Contains("col-sm-")) attr["class"] += " col-sm-6";

            if (string.IsNullOrWhiteSpace(imageSource))
            {
                var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                imageSource = urlHelper.Action("_CaptchaImage", "Shared") + "/" + System.Guid.NewGuid().ToString();
            }


            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml("<div><div class='col-sm-6'>");
            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.TextBox(helper, name, "", attr).ToString());
            frmGroup.AppendInnerHtml("</div><span class='captcha' style='padding: 0 !important; border: none'>");
            frmGroup.AppendInnerHtmlFormat("<img alt='Captcha' src={0}/></span></div>", imageSource);

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, errorMessege, null, null).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #region signature

        /// <summary>
        /// Returns signature pad binded to input element which is mapped to string model property
        /// </summary>
        /// <param name="expression">Lambda expression for the string property to hold signature base64 string</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="width">Width style value of the signature pad</param>
        /// <param name="height">height style value of the signature pad</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCISignatureItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, int spanOf12 = 6, string width = "300px", string height = "100px", string clearButtonInnerHTML = "مسح التوقيع", string hint = "")
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));


            frmGroup.AppendInnerHtmlFormat("<div class='signature-wraper'><div id='SigDiv_{0}' style='width: {1}; height: {2}'></div></div>", name, width, height);
            frmGroup.AppendInnerHtmlFormat("<button id='btnClearSig_{0}' class='btn btn-default btn-xs' type='button' style='display:block;'>{1}</button>", name, clearButtonInnerHTML);
            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.HiddenFor(helper, expression, new { id = name }).ToString());

            frmGroup.AppendInnerHtmlFormat("<script>$(function () {{$('#SigDiv_{0}').signature({{syncFieldImageURL: '#{0}'}}); $('#SigDiv_{0}').signature('fromImageURL', $('#{0}').val()); $('#btnClearSig_{0}').click(function () {{ $('#SigDiv_{0}').signature('clear'); }});}});</script>", name);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns signature pad binded to input element
        /// </summary>
        /// <param name="name">Unique name for the input property</param>
        /// <param name="labelText">form item caption text</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="width">Width style value of the signature pad</param>
        /// <param name="height">height style value of the signature pad</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCISignatureItem<TModel>(
            this HtmlHelper<TModel> helper,
            string name, string labelText = "", int spanOf12 = 6, string width = "300px", string height = "100px", string clearButtonInnerHTML = "مسح التوقيع", string hint = "")
        {
            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtmlFormat("<div class='signature-wraper'><div id='SigDiv_{0}' style='width: {1}; height: {2}'></div></div>", name, width, height);
            frmGroup.AppendInnerHtmlFormat("<button id='btnClearSig_{0}' class='btn btn-default btn-xs' type='button' style='display:block;'>{1}</button>", name, clearButtonInnerHTML);
            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.Hidden(helper, name, "", new { id = name }).ToString());
            frmGroup.AppendInnerHtml("</div>");

            frmGroup.AppendInnerHtmlFormat("<script>$(function () {{$('#SigDiv_{0}').signature({{syncFieldImageURL: '#{0}'}}); $('#SigDiv_{0}').signature('fromImageURL', $('#{0}').val()); $('#btnClearSig_{0}').click(function () {{ $('#SigDiv_{0}').signature('clear'); }});}});</script>", name);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #region Selectlist items

        /// <summary>
        /// Returns multi-select select element in bootstrap form item with caption mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the list</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIMultiSelectAngularTreeItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            int spanOf12 = 6, object httmlAttributes = null, bool withValidation = true, string hint = "")
        {
            string name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));

            if (selectList == null)
                selectList = new List<SelectListItem>();

            var model = helper.ViewData.Model as UserEditModel;

            var root = new RolesTreeViewModel().ToModel(selectList, model);

            MCITagBuilder col = new MCIItemsCol(spanOf12, new { ng_controller = "billingAppCtrl" });
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml("<multi-select-tree data-input-model='[" + Json.Encode(root) + "]' multi-select='true'" +
                            "data-output-model='selectedItem2' data-default-label='اضغط هنا ...'" +
                            "data-callback='selectOnly1Or2(item, selectedItems)'" +
                            "data-select-only-leafs='false' data-switch-view-callback='switchViewCallback(scopeObj)'" +
                            "data-switch-view-label='test1' data-switch-view='false'></multi-select-tree>" +
                            "<select name='" + name + "' multiple='multiple' ng-hide='true' id='" + name + "' ng-init='selected = " + Json.Encode(selectList) + "' class='form-control select2-offscreen' tabindex='-1'>" +
                                "<option value='{{item.Value}}' ng-selected='itemSelected(item)' ng-repeat='item in selected'>{{item.Text}}</option>" +
                        "</select>");

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns multi-select select element in bootstrap form item with caption mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the list</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIListBoxtItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            int spanOf12 = 6, object httmlAttributes = null, bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            if (selectList == null)
                selectList = new List<SelectListItem>();

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.LabelFor(helper, expression).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.SelectExtensions.ListBoxFor(helper, expression, selectList, customAttributes).ToString());


            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, expression).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }


        /// <summary>
        /// Returns encrypted multi-select select element in bootstrap form item with caption mapped to model property
        /// </summary>
        /// <param name="expression">Lambda expression for the property</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the list</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedListBoxtItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            int spanOf12 = 6, object httmlAttributes = null, bool withValidation = true, string hint = "")
        {
            string expressionName = ExpressionHelper.GetExpressionText(expression);
            string fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionName);
            string fullId = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(expressionName);
            string displayName = System.Web.Mvc.Html.DisplayNameExtensions.DisplayNameFor(helper, expression).ToString();

            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            if (selectList == null)
                selectList = new List<SelectListItem>();

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, displayName, new { @for = string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullId) }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(EncryptedSelectExtensions.EncryptedListBoxFor(helper, expression, selectList, customAttributes).ToString());


            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullName)).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns multi-select select element in bootstrap form item with caption
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the list</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIListBoxtItem<TModel>(
            this HtmlHelper<TModel> helper,
            string name,
            IEnumerable<SelectListItem> selectList,
            string labelText = "",
            int spanOf12 = 6, object httmlAttributes = null, bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            if (selectList == null)
                selectList = new List<SelectListItem>();

            if (!customAttributes.ContainsKey("id"))
                customAttributes["id"] = name;

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = name }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.SelectExtensions.ListBox(helper, name, selectList, customAttributes).ToString());

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, name, "", null, null).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        /// <summary>
        /// Returns Encrypted multi-select select element in bootstrap form item with caption
        /// </summary>
        /// <param name="name">Unique name for the select element</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem object that are used to populate the list</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the select element</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="hint">Optional text to add after the caption label as a hint</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString EncryptedListBoxtItem<TModel>(
            this HtmlHelper<TModel> helper,
            string name,
            IEnumerable<SelectListItem> selectList,
            string labelText = "",
            int spanOf12 = 6, object httmlAttributes = null, bool withValidation = true, string hint = "")
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);
            customAttributes["class"] += " form-control";

            if (selectList == null)
                selectList = new List<SelectListItem>();

            if (!customAttributes.ContainsKey("id"))
                customAttributes["id"] = name;

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (string.IsNullOrWhiteSpace(labelText))
                frmGroup.AppendInnerHtml("<div style='margin-bottom:24px'> </div>");
            else
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText, new { @for = string.Concat(StringEncrypter.ControlsEncrypter.Prefix, name) }).ToString());

            if (!string.IsNullOrEmpty(hint))
                frmGroup.AppendInnerHtml(new MCIHint(hint));

            frmGroup.AppendInnerHtml(EncryptedSelectExtensions.EncryptedListBox(helper, name, selectList, customAttributes).ToString());

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessage(helper, string.Concat(StringEncrypter.ControlsEncrypter.Prefix, name)).ToString());

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion

        #endregion

        #region Buttons

        private static RouteValueDictionary AddConfirmationAttributes(object htmlAttributes, string confirmationTitle, string confirmationMessage)
        {
            RouteValueDictionary result;
            if (htmlAttributes is RouteValueDictionary)
                result = htmlAttributes as RouteValueDictionary;
            else
                result = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            result["class"] += " mci-confirm";
            result["title"] = confirmationTitle;
            result["rel"] = confirmationMessage;

            return result;
        }

        /// <summary>
        /// Returns submit button
        /// </summary>
        /// <param name="innerHtml">Button inner html</param>
        /// <param name="name">Button name</param>
        /// <param name="value">Button value</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the button element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCISubmitButton(this HtmlHelper helper, string innerHtml,
            string name = "", string value = "", object htmlAttributes = null, string onlyForRoles = null)
        {
            //check roles
            if (!string.IsNullOrWhiteSpace(onlyForRoles) &&
                (
                !HttpContext.Current.Request.IsAuthenticated ||
                (HttpContext.Current.User as UserProfilePrincipal) != null && !(HttpContext.Current.User as UserProfilePrincipal).IsInRoles(onlyForRoles))
                )
                return new MvcHtmlString("");

            RouteValueDictionary customAttributes;
            if (htmlAttributes is RouteValueDictionary)
                customAttributes = htmlAttributes as RouteValueDictionary;
            else
                customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            customAttributes["type"] = "submit";
            if (!string.IsNullOrEmpty(name))
                customAttributes["name"] = name;
            if (!string.IsNullOrEmpty(value))
                customAttributes["value"] = value;

            TagBuilder tb = new TagBuilder("button");
            tb.MergeAttributes(customAttributes);
            tb.InnerHtml = innerHtml;

            return new MvcHtmlString(tb.ToString());
        }

        /// <summary>
        /// Returns submit button
        /// </summary>
        /// <param name="innerHtml">Button inner html</param>
        /// <param name="confirmationTitle">bootstrap modal title</param>
        /// <param name="confirmationMessage">bootstrap modal confirmation message</param>
        /// <param name="name">Button name</param>
        /// <param name="value">Button value</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the button element</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCISubmitButtonWithConfirm(this HtmlHelper helper, string innerHtml,
            string confirmationTitle, string confirmationMessage,
            string name = "", string value = "", object htmlAttributes = null, string onlyForRoles = null)
        {
            RouteValueDictionary customHtmlAttributes = AddConfirmationAttributes(htmlAttributes, confirmationTitle, confirmationMessage);

            return MCISubmitButton(helper, innerHtml, name, value, customHtmlAttributes, onlyForRoles);
        }

        /// <summary>
        /// Returns bootstrap loading button element with ajax options with value post capability and custom validation (which is actually anchor tag (ajaxActionLink))
        /// </summary>
        /// <param name="name">Unique name for the button</param>
        /// <param name="btnInnerHtml">Button inner html</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Rout values</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="httmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <param name="jsonDataProviderFunction">Javascript function that will return Json object to send with ajax request</param>
        /// <param name="validateBeforeSend">True if you want to validate the form before send</param>
        /// <param name="validationSelector">JQuery selector for elements to validate before send</param>
        /// <param name="onlyForRoles">comma delimited roles to display the button for</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxButton(this AjaxHelper helper, string name, string btnInnerHtml, string actionName, string controllerName, object routeValues,
            AjaxOptions ajaxOptions, object httmlAttributes = null, string jsonDataProviderFunction = "", bool validateBeforeSend = false, string validationSelector = "", string onlyForRoles = null)
        {
            //check roles
            if (!string.IsNullOrWhiteSpace(onlyForRoles) &&
                (
                !HttpContext.Current.Request.IsAuthenticated ||
                (HttpContext.Current.User as UserProfilePrincipal) != null && !(HttpContext.Current.User as UserProfilePrincipal).IsInRoles(onlyForRoles))
                )
                return new MvcHtmlString("");

            StringBuilder result = new StringBuilder();

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            string imgSrc = urlHelper.Content("~/Content/images/loadings.gif");

            var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(httmlAttributes);

            ajaxOptions = ajaxOptions ?? new AjaxOptions();

            if (!customAttributes.ContainsKey("id"))
                customAttributes["id"] = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(name);

            customAttributes["class"] += " btn";
            if (!customAttributes["class"].ToString().Contains("btn-"))
                customAttributes["class"] += " btn-default";
            customAttributes["data-loading-text"] = string.Format("{0} <img src='{1}' alt='الرجاء الانتظار..'/>", btnInnerHtml, imgSrc);

            string onBegin = ajaxOptions.OnBegin;
            ajaxOptions.OnBegin = string.Format("{0}_onBegin", name);
            ajaxOptions.OnComplete = string.Format("$('#{0}').button('reset'); {1}", customAttributes["id"].ToString(), ajaxOptions.OnComplete);

            result.Append(LinkExtensions.AjaxLinkWithInnerHtml(helper, btnInnerHtml, actionName, controllerName, routeValues, ajaxOptions, customAttributes, null));

            result.AppendFormat(
                @"<script>
                    function {0}_onBegin(xhr , opts){{
                        //execute user onbegin
                        if(!{1})
                            if({2}(xhr , opts)==false) return false;
                        //validate 
                        var validate = {5};
                        var selector = '{6}';
                        var toValidate;
                        var valid = true;
                        if (validate) {{
                            selector = (selector == '') ? 'form' : selector;
                            $toValidate = $(selector);
                            $toValidate.each(function () {{ 
                                var $theForm = $(this).closest('form');
                                if ($theForm.length)                               
                                    valid = $(this).valid() ? valid : false;
                            }});
                        }}

                        if (!valid)
                            return false;

                        $('#{0}').button('loading');

                        //append data if any

                        if(!{3}){{ 
                            var fn = new Function('return {4}();');                           
                            var data = fn();
                            if (data) {{
                                var theUrl = opts.url;
                                theUrl += theUrl.indexOf('?') == -1 ? '?' : '';                                
                                for (var key in data) {{
                                    theUrl += '&';
                                    theUrl += key + '=' + encodeURIComponent(data[key]);
                                }}
                                opts.url = theUrl;
                            }}
                        }}                        
                    }}
                  </script>",
                name, string.IsNullOrWhiteSpace(onBegin).ToString().ToLower(),
                onBegin, string.IsNullOrWhiteSpace(jsonDataProviderFunction).ToString().ToLower(),
                jsonDataProviderFunction, validateBeforeSend.ToString().ToLower(),
                validationSelector);

            return new MvcHtmlString(result.ToString());
        }


        /// <summary>
        /// Returns Exact same as AjaxActionLink but takes inner html instead of inner text
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="ajaxOptions">AjaxOptions</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxLink(this HtmlHelper helper, string linkInnerHtml,
            string actionName, string controllerName = "", object routeValues = null, AjaxOptions ajaxOptions = null, object htmlAttributes = null)
        {

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            if (string.IsNullOrWhiteSpace(controllerName))
                controllerName = helper.ViewContext.RouteData.GetRequiredString("Controller");

            var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            TagBuilder tb = new TagBuilder("a");
            tb.MergeAttributes(customAttributes);
            tb.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            tb.InnerHtml = linkInnerHtml;

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            tb.MergeAttributes(ajaxOptions.ToUnobtrusiveHtmlAttributes());

            return new MvcHtmlString(tb.ToString());
        }

        /// <summary>
        /// Returns ajax anchor element with bootstrap modal confirmation befor request
        /// </summary>
        /// <param name="linkInnerHtml">Anchor inner html</param>
        /// <param name="confirmationTitle">bootstrap modal title</param>
        /// <param name="confirmationMessage">bootstrap modal confirmation message</param>
        /// <param name="actionName">Url Action name</param>
        /// <param name="controllerName">Url Controller name</param>
        /// <param name="routeValues">Url rout values</param>
        /// <param name="htmlAttributes">custom httmlAttributes applied to the anchor element</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIAjaxLinkWithConfirm(this HtmlHelper helper, string linkInnerHtml,
            string confirmationTitle, string confirmationMessage,
            string actionName, string controllerName = "", object routeValues = null, AjaxOptions ajaxOptions = null, object htmlAttributes = null)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            if (string.IsNullOrWhiteSpace(controllerName))
                controllerName = helper.ViewContext.RouteData.GetRequiredString("Controller");

            var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            TagBuilder tb = new TagBuilder("a");
            tb.MergeAttributes(customAttributes);
            tb.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            //tb.SetInnerText(linkText);
            tb.InnerHtml = linkInnerHtml;
            tb.AddCssClass("mci-confirm");
            tb.MergeAttribute("title", confirmationTitle);
            tb.MergeAttribute("rel", confirmationMessage);

            ajaxOptions = (ajaxOptions ?? new AjaxOptions());
            if (string.IsNullOrWhiteSpace(ajaxOptions.OnBegin))
                ajaxOptions.OnBegin = "ReturnFalse";
            tb.MergeAttributes(ajaxOptions.ToUnobtrusiveHtmlAttributes());

            return new MvcHtmlString(tb.ToString());
        }
        #endregion


        #region Modal

        /// <summary>
        /// Returns Bootstrap modal Contaner div start tag 
        /// </summary>
        /// <param name="modalUniqueName">Unique id for the Modal Container</param>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginModalContainer(this HtmlHelper helper, string modalUniqueName, ModalFadeMode fadeMode = ModalFadeMode.Transperent)
        {
            return new MCIDisposableHelper(helper, new MCIModalContainer(modalUniqueName, fadeMode));
        }

        /// <summary>
        /// Returns bootstrap internal bootstrap modal container div which contains modal header,body and footer
        /// </summary>
        /// <param name="modalSize">Modal size (small, default, large)</param>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginModal(this HtmlHelper helper, ModalSize modalSize = ModalSize.Default)
        {
            return new MCIDisposableHelper(helper, new MCIModal(modalSize));
        }

        /// <summary>
        /// Returns bootstrap modal header div
        /// </summary>
        /// <param name="withCloseButton">Display modal close button</param>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginModalHeader(this HtmlHelper helper, bool withCloseButton = true, ModalHeaderStyle modalHeaderStyle = ModalHeaderStyle.Light)
        {
            return new MCIDisposableHelper(helper, new MCIModalHeader(withCloseButton, modalHeaderStyle));
        }

        /// <summary>
        /// Returns bootstrab modal-body div
        /// </summary>
        /// <param name="htmlAttributes">custom html attributes for the div</param>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginModalBody(this HtmlHelper helper, object htmlAttributes = null)
        {
            return new MCIDisposableHelper(helper, new MCIModalBody(htmlAttributes));
        }

        /// <summary>
        /// Returns bootstrab modal-footer div
        /// </summary>
        /// <param name="htmlAttributes">custom html attributes for the div</param>
        /// <returns></returns>
        public static MCIDisposableHelper MCIBeginModalFooter(this HtmlHelper helper, object htmlAttributes = null)
        {
            return new MCIDisposableHelper(helper, new MCIModalFooter(htmlAttributes));
        }

        #endregion

        #region Google Maps

        /// <summary>
        /// Returns google map item and add marker on the map that can be editable and mapped to lat lng model properties.(google maps api loaded dynamically using common.js)
        /// </summary>
        /// <param name="latExpression">Lambda expression for the property holds latitude</param>
        /// <param name="lngExpression">Lambda expression for the property holds longitude</param>
        /// <param name="markerTitle">Title for the marker</param>
        /// <param name="name">Unique name for the item</param>
        /// <param name="editable">True to make marker editable and it's location mapped to hidden inputs to the model</param>
        /// <param name="height">map height</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="withValidation">True will add ValidationMessage for the input element</param>
        /// <param name="callBack">Javascript Initialization callback function will be called after map initialization</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIGoogleMapItemFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> latExpression, Expression<Func<TModel, TProperty>> lngExpression, string markerTitle = "",
            string name = "mciMap", bool editable = true, string height = "400px", int spanOf12 = 6, string labelText = "", bool withValidation = false, string callBack = "")
        {
            string latExpName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(latExpression));
            string lngExpName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(lngExpression));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (!string.IsNullOrEmpty(labelText))
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText).ToString());

            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.TextBoxFor(helper, latExpression, new { @style = "position:absolute;opacity:0.0;visibility:hidden" }).ToString());
            frmGroup.AppendInnerHtml(System.Web.Mvc.Html.InputExtensions.TextBoxFor(helper, lngExpression, new { @style = "position:absolute;opacity:0.0;visibility:hidden" }).ToString());

            if (withValidation)
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(helper, latExpression).ToString());


            frmGroup.AppendInnerHtmlFormat("<div id='{0}_canvas' style='direction: ltr; height:{1}'></div>", name, height);

            frmGroup.AppendInnerHtml("</div>");

            frmGroup.AppendInnerHtmlFormat(@"<script>
                var {0}_marker, {0}_map, {0}_editable, {0}_laditude, {0}_longitude;
                $(function () {{
                    loadGoogleMapsApi('{0}_initialize');
                }});
                
                function {0}_changeMarkerPosition(lat, lng, title, panTo, zoomTo){{

                    if (!{0}_marker) {{
                        {0}_marker = new google.maps.Marker({{
                           position: new google.maps.LatLng(lat, lng),
                           map: {0}_map,
                           title: title,
                           icon: '{4}'
                        }});
                        {0}_marker.setDraggable({5});
                        if ({5}) {{                            
                            google.maps.event.addListener({0}_marker, 'dragend', function (e) {{
                                $('#{2}').val(e.latLng.lat());
                                $('#{3}').val(e.latLng.lng());
                            }});
                        }}                        
                        if(panTo) {0}_map.panTo(new google.maps.LatLng(lat, lng));
                        if(zoomTo) {0}_map.setZoom(zoomTo);
                    }}
                    else
                        changeMarkerPosition({0}_marker, lat, lng, panTo, {0}_map, zoomTo)
                    

                    $('#{2}').val(lat);
                    $('#{3}').val(lng);
                }}

                function {0}_initialize(){{
                    {0}_marker = null;
                    {0}_map = null;
                    var {0}_title = '{1}';
                    {0}_laditude = $('#{2}').val();
                    {0}_longitude = $('#{3}').val();
                    var {0}_zoom = ({0}_laditude && {0}_longitude)? 12 : 5;

                    {0}_laditude = {0}_laditude || 24.7168045;
                    {0}_longitude = {0}_longitude || 45.0968626;
                
                    var location = new google.maps.LatLng({0}_laditude, {0}_longitude);
                    var mapOptions = {{
                        zoom: {0}_zoom,
                        center: location
                    }};
                    {0}_map = new google.maps.Map(document.getElementById('{0}_canvas'), mapOptions);

                    if ($('#{2}').val() && $('#{3}').val()) {{
                        {0}_changeMarkerPosition({0}_laditude, {0}_longitude, {0}_title);                        
                    }}

                    if ({5}) {{
                        google.maps.event.addListener({0}_map, 'click', function (e) {{
                            var lat = e.latLng.lat();
                            var lng = e.latLng.lng();
                            {0}_changeMarkerPosition(lat, lng, {0}_title);                            
                        }});
                    }}; 
                    $('#{0}_canvas').closest('.modal').on('shown.bs.modal', function (e) {{
                        google.maps.event.trigger({0}_map, 'resize');
                        {0}_map.panTo(location);
                    }});

                    var $tab = $('#{0}_canvas').closest('.tab-pane');
                    if ($tab.length) {{
                        $('a[data-toggle=""tab""][href=""#' + $tab.attr('id') + '""]').on('shown.bs.tab', function (e) {{
                        //resize map
                        google.maps.event.trigger(sampleMap_map, 'resize');
                        {0}_map.panTo(location);
                        }});
                    }}

                    if ('{6}'!='') {{
                        var fn = new Function('{6}();');
                        fn();
                    }}
                }}                                 
         </script>", name, markerTitle, latExpName, lngExpName, new UrlHelper(helper.ViewContext.RequestContext).Content("~/Content/images/marker.png"), editable.ToString().ToLower(), callBack);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }


        /// <summary>
        /// Returns google map item and add markers on the map fited cleverly.(google maps api loaded dynamically using common.js)
        /// </summary>
        /// <param name="markers">Collection of GoogleMarker object to draw on the map</param>
        /// <param name="name">Unique name for the item</param>
        /// <param name="height">map height</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="labelText">Item caption</param>
        /// <param name="callBack">Javascript Initialization callback function will be called after map initialization</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIGoogleMapMarkersItem<TModel>(
            this HtmlHelper<TModel> helper,
            IList<GoogleMarker> markers,
            string name = "mciMap", string height = "400px", int spanOf12 = 6, string labelText = "", string callBack = "")
        {
            var markersJson = Newtonsoft.Json.JsonConvert.SerializeObject(markers);

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            if (!string.IsNullOrEmpty(labelText))
                frmGroup.AppendInnerHtml(System.Web.Mvc.Html.LabelExtensions.Label(helper, labelText).ToString());

            frmGroup.AppendInnerHtmlFormat("<div id='{0}_canvas' style='direction: ltr; height:{1}'></div>", name, height);

            frmGroup.AppendInnerHtml("</div>");

            frmGroup.AppendInnerHtmlFormat(@"<script>
                var {0}_markers = [];
                var {0}_map;
                var {0}_bounds;

                $(function () {{
                    loadGoogleMapsApi('{0}_initialize');
                }});

                function {0}_initialize(){{
                    var passedMarkers = jQuery.parseJSON('{2}');
                    
                    var mapOptions = {{
                        zoom: 5,
                        center: new google.maps.LatLng(24.7168045, 45.0968626)
                    }};

                    {0}_map = new google.maps.Map(document.getElementById('{0}_canvas'), mapOptions);
                    {0}_bounds = new google.maps.LatLngBounds();

                    for (i = 0; i < passedMarkers.length; i++) {{
                        if (passedMarkers[i].Lat && passedMarkers[i].Lng) {{
                            var marker = new google.maps.Marker({{
                                name: passedMarkers[i].ID,
                                map: {0}_map,
                                draggable: false,
                                icon: '{1}',
                                title: passedMarkers[i].title,
                                position: new google.maps.LatLng(passedMarkers[i].Lat, passedMarkers[i].Lng)
                            }});
                            {0}_bounds.extend(marker.position);
                            {0}_markers.push(marker);
                        }}
                    }}
                    
                    if({0}_markers.length == 1){{                        
                        {0}_map.panTo({0}_markers[0].position);
                        {0}_map.setZoom(12);
                    }}
                    else if ({0}_markers.length > 1)
                        {0}_map.fitBounds({0}_bounds);

                    $('#{0}_canvas').closest('.modal').on('shown.bs.modal', function (e) {{
                        google.maps.event.trigger({0}_map, 'resize');
                        if({0}_markers.length == 1){{                        
                             {0}_map.panTo({0}_markers[0].position);
                             {0}_map.setZoom(12);
                         }}
                         else if ({0}_markers.length > 1)
                             {0}_map.fitBounds({0}_bounds);
                         else if ({0}_markers.length == 0)
                             {0}_map.panTo(new google.maps.LatLng(24.7168045, 45.0968626));
                    }});
                    if ('{3}'!='') {{
                        var fn = new Function('{3}();');
                        fn();
                    }}
                }}                                 
         </script>", name,
                   new UrlHelper(helper.ViewContext.RequestContext).Content("~/Content/images/marker.png"),
                   markersJson.ToString(),
                   callBack);

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }


        #endregion

        #region Wizard

        /// <summary>
        /// Begins custom MCI wizard steps block that contains wizard steps
        /// </summary>
        /// <param name="name">Unique name for the wizard container</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <returns></returns>
        public static MCIWizard MCIBeginWizard(this HtmlHelper helper, string name = "mciWizard", int spanOf12 = 12)
        {
            return new MCIWizard(helper, name, spanOf12);
        }

        /// <summary>
        /// Returns custom MCI wizard step spot that
        /// </summary>
        /// <param name="stepNumber">Unique name for the step(will be displayed above the step)</param>
        /// <param name="stepTitle">Step title (will be displayed down the step)</param>
        /// <param name="stepStatus">Default status of the step</param>
        /// <param name="width">Style value for width</param>
        /// <param name="stepHRef">Url for the step anchor</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIWizardStep(this HtmlHelper helper, string stepNumber, string stepTitle = "", WizardStepStatus stepStatus = WizardStepStatus.Future, string width = "20%", string stepHRef = "")
        {
            StringBuilder result = new StringBuilder();

            string stepClass = "";
            switch (stepStatus)
            {
                case WizardStepStatus.Active:
                    stepClass = "active";
                    break;
                case WizardStepStatus.Completed:
                    stepClass = "complete";
                    break;
                default:
                    stepClass = "disabled";
                    break;
            }

            string href = !string.IsNullOrEmpty(stepHRef) ? string.Format("href='{0}'", stepHRef) : "";

            result.AppendFormat("<div id='{0}' class='bs-wizard-step {1}' style='width:{2}'>", stepNumber, stepClass, width);
            result.AppendFormat("<div class='bs-wizard-stepnum'>{0}</div>", stepNumber);
            result.Append("<div class='bs-wizard-step-inner'>");
            result.Append("<div class='progress'><div class='progress-bar'></div></div>");
            result.AppendFormat("<a {0} class='bs-wizard-dot'></a></div>", href);
            result.AppendFormat("<div class='bs-wizard-info'>{0}</div></div>", stepTitle);

            return new MvcHtmlString(result.ToString());
        }


        //wizard2
        /// <summary>
        /// Begins custom MCI wizard steps block that contains wizard steps
        /// </summary>
        /// <param name="name">Unique name for the wizard container</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <returns></returns>
        public static MCIWizard2 MCIBeginWizard2(this HtmlHelper helper, string name = "mciWizard", int spanOf12 = 12)
        {
            return new MCIWizard2(helper, name, spanOf12);
        }


        public static MvcHtmlString MCIWizardStep2(this HtmlHelper helper, string stepNumber, string stepTitle = "", string stepHRef = "", bool isCurrent = false, bool incompleteStep = false)
        {
            StringBuilder result = new StringBuilder();

            string stepClass = "";
            if (isCurrent) stepClass += "current";
            if (incompleteStep) stepClass += " not-completed";

            string href = !string.IsNullOrEmpty(stepHRef) ? string.Format("href='{0}'", stepHRef) : "";
            result.AppendFormat("<a id='{2}' {0} class='{1}'><span class='badge'>{2}</span>{3}</a>", href, stepClass, stepNumber, stepTitle);

            return new MvcHtmlString(result.ToString());
        }

        #endregion

        #region Heigh Charts

        /// <summary>
        /// Returns Heigh Chart block of givven chart data. (Pie, map or bar)
        /// </summary>
        /// <param name="chartData">MciChart object</param>
        /// <param name="MciChartType">Chart type(Pie, map or bar)</param>
        /// <param name="name">Unique Name for the chart div</param>
        /// <param name="spanOf12">Number represents bootstrap column span for the item width</param>
        /// <param name="height">Chart height</param>
        /// <param name="options">MciChartOptions object(if null the default values will be used).</param>
        /// <returns></returns>
        public static MvcHtmlString MciChart<TModel>(this HtmlHelper<TModel> helper, object chartData, MciChartType MciChartType, string name = "mciChart", int spanOf12 = 12, string height = "400px", MciChartOptions options = null)
        {
            HtmlString mciChartData = new HtmlString(Json.Encode((MciChart)chartData));

            MCITagBuilder col = new MCIItemsCol(spanOf12, null);
            MCITagBuilder frmGroup = new MCIFormGroup();

            frmGroup.AppendInnerHtmlFormat("<div id='{0}' style='direction: ltr; height: {1}; width:100%;'></div>", name, height);

            if (options == null)
                frmGroup.AppendInnerHtmlFormat(@"<script>$(document).ready(function () {{ Draw{0}('{1}', JSON.parse('{2}')); }});</script>", MciChartType.ToString(), name, mciChartData.ToString());
            else
                frmGroup.AppendInnerHtmlFormat(@"<script>$(document).ready(function () {{ Draw{0}('{1}', JSON.parse('{2}'), JSON.parse('{3}')); }});</script>", MciChartType.ToString(), name, mciChartData.ToString(), Json.Encode(options));

            return col.AppendInnerHtml(frmGroup).ToMvcHtmlString();
        }

        #endregion


        #region Secure Menu Items

        /// <summary>
        /// filter the provided menu for the current logged user
        /// </summary>
        /// <param name="menuPartialViewName">The partialview name contains menu items</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString MCIRenderUserMenuItems(this HtmlHelper helper, string menuPartialViewName)
        {
            string result = System.Web.Mvc.Html.PartialExtensions.Partial(helper, menuPartialViewName).ToString();

            //add temp root
            result = "<root>" + result + "</root>";

            XDocument xdoc = XDocument.Load(new System.IO.StringReader(result));
            var secureItems = (from x in xdoc.Descendants()
                               where x.Attributes().Any(a => a.Name == "secure-roles")
                               select x)
                          .ToArray();

            foreach (var item in secureItems)
            {
                XAttribute secureAttribute = item.Attribute(XName.Get("secure-roles"));
                if (string.IsNullOrWhiteSpace(secureAttribute.Value))
                    continue;
                //remove role attribute
                secureAttribute.Remove();
                //remove if not authenticated
                if (!HttpContext.Current.Request.IsAuthenticated)
                    item.Remove();
                else
                {
                    //check roles
                    if (!(HttpContext.Current.User as UserProfilePrincipal).IsInRoles(secureAttribute.Value))
                        item.Remove();
                }
            }

            //remove temp root
            result = xdoc.ToString().Replace("<root>", "").Replace("</root>", "");

            return new MvcHtmlString(result);
        }

        #endregion

        public static MvcHtmlString RenderHtmlAttributes<TModel>(
        this HtmlHelper<TModel> htmlHelper, object htmlAttributes)
        {
            RouteValueDictionary customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return MvcHtmlString.Create(String.Join(" ",
                customAttributes.Keys.Select(
                    key => String.Format("{0}=\"{1}\"", key,
                    htmlHelper.Encode(customAttributes[key])))));
        }
    }


    #region disposable helpers

    public class MCIWizard : IDisposable
    {
        protected HtmlHelper _helper;

        public MCIWizard(HtmlHelper helper, string name, int spanOf12)
        {
            _helper = helper;
            _helper.ViewContext.Writer.Write(string.Format("<div class='center col-md-{0}'><div id='{1}' class='row bs-wizard'>", spanOf12.ToString(), name));
        }

        public void Dispose()
        {
            _helper.ViewContext.Writer.Write("</div></div>");
        }
    }

    public class MCIWizard2 : IDisposable
    {
        protected HtmlHelper _helper;

        public MCIWizard2(HtmlHelper helper, string name, int spanOf12)
        {
            _helper = helper;
            _helper.ViewContext.Writer.Write(string.Format("<div class='center col-md-{0}'><div id='{1}' class='mci-wizard'>", spanOf12.ToString(), name));
        }

        public void Dispose()
        {
            _helper.ViewContext.Writer.Write("</div></div>");
        }
    }


    #endregion

    #region Enumms

    public enum MciChartType
    {
        PieChart = 1,
        BarChart,
        MapChart
    }


    public enum RadioButtonStyle
    {
        Inline,
        ButtonGroup,
        Vertical
    }

    public enum CheckBoxStyles
    {
        Inline,
        ButtonGroup,
        Buttons,
        Vertical
    }

    public enum WizardStepStatus
    {
        Completed,
        Active,
        Future
    }

    #endregion

}

public class GoogleMarker
{
    public int ID { get; set; }

    public double Lat { get; set; }

    public double Lng { get; set; }

    public string Title { get; set; }
}

