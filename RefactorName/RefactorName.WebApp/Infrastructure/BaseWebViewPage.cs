using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RefactorName.WebApp.Models;

namespace RefactorName.WebApp.Infrastructure
{
    /// <summary>
    /// Provides additional properties that may used throughout writing Views in Razor.
    /// </summary>
    public abstract class BaseWebViewPage : WebViewPage
    {
        /// <summary>
        /// Gets the LayoutModel property that associated with _Layout.cshtml views.
        /// </summary>
        public LayoutViewModel LayoutModel
        {
            get
            {
                if (ViewBag.LayoutModel != null)
                    return ViewBag.LayoutModel;
                else
                {
                    var model = TempData[BaseController.LayoutModelKey] as LayoutViewModel;
                    TempData[BaseController.LayoutModelKey] = model;
                    return model;
                }
            }
        }
    }

    /// <summary>
    /// Provides additional properties that may used throughout writing Views in Razor.
    /// </summary>
    public abstract class BaseWebViewPage<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Gets the LayoutModel property that associated with _Layout.cshtml views.
        /// </summary>
        public LayoutViewModel LayoutModel
        {
            get
            {
                var model = TempData[BaseController.LayoutModelKey] as LayoutViewModel;

                if (model != null)
                    return model;
                else
                    return ViewBag.LayoutModel;
            }
        }
    }
}