using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp.Models
{
    /// <summary>
    /// Represents the data model that _Layout.cshtml view need to render the common things.
    /// </summary>
    public class LayoutViewModel
    {
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets Title of User's Browser Tab.
        /// This properties will be read by _Layout.cshtml view.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets all Snackbars (Alert Messages) that will be shown on Snackbar Area.
        /// </summary>
        public List<SnackbarViewModel> Snackbars { get; set; }

        public string ReturnUrl { get; set; }

        public string StatusMessage { get; set; }

        public bool HasLocalPassword { get; set; }

        public bool ShowRemoveButton { get; set; }

        public string Message { get; set; }

        public string ProviderDisplayName { get; set; }

        public LayoutViewModel()
        {
            // DO NOT: Remove this line because HtmlHelper Extensions SnackbarArea will throw NullReferenceException otherwise.
            Snackbars = new List<SnackbarViewModel>();
        }
    }
}