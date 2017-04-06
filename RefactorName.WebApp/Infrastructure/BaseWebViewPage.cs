using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RefactorName.Web.Models;

namespace RefactorName.Web
{
    public abstract class BaseWebViewPage : WebViewPage
    {
        public LayoutViewModel LayoutModel
        {
            get { return (LayoutViewModel)ViewBag.LayoutModel; }
        }
    }
}