using RefactorName.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using RefactorName.WebApp.Infrastructure;
using System.Text;

namespace RefactorName.WebApp.Helpers
{
    /// <summary>
    /// Expose <see cref="HtmlHelper"/> extensions to show Snackbars area.
    /// </summary>
    public static class SnackbarExtensions
    {
        /// <summary>
        /// Renders special area that shows Snackbars (Alert Message).
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="snackbars">Snackbars message to be shown.</param>
        /// <param name="isFluid"></param>
        /// <returns></returns>
        public static MvcHtmlString SnackbarsArea(this HtmlHelper helper, IEnumerable<SnackbarViewModel> snackbars, bool isFluid = false)
        {
            TagBuilderEx container;
            using (container = new TagBuilderEx("div"))
            {
                container.AddCssClass("message-box-container");
                container.AddCssClassIf(isFluid, "container-fluid", "container");

                using (TagBuilderEx row = container.CreateInnerTag("div"))
                using (TagBuilderEx messageBox = row.CreateInnerTag("div"))
                {
                    messageBox.AddCssClass("message-box");
                    messageBox.AddCssClasses("col-md-9", "col-md-offset-3", "col-xs-11", "col-xs-offset-1");

                    foreach (var snackbar in snackbars)
                        RenderSnackbar(snackbar, messageBox);
                }
            }

            return new MvcHtmlString(container.ToString());
        }

        private static void RenderSnackbar(SnackbarViewModel model, TagBuilderEx parent)
        {
            using (TagBuilderEx alert = parent.CreateInnerTag("div"))
            {
                alert.AddCssClasses("center-block", "alert", "alert-dismissible");
                alert.AddCssClass("alert-" + model.Type.ToString());
                alert.MergeAttribute("data-snackbar-timeout", model.Timeout.ToString());
                alert.MergeAttribute("role", "alert");

                using (TagBuilderEx content = alert.CreateInnerTag("span"))
                {
                    content.AddCssClass("alert-content");
                    content.InnerHtml = model.Message;
                }
            }
        }
    }
}