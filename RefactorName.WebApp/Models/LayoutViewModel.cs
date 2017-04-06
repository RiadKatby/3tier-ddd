using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.Web.Models
{
    public class LayoutViewModel
    {
        public string MetaKeywords { get; set; }

        public string Title { get; set; }

        public string ReturnUrl { get; set; }

        public string StatusMessage { get; set; }

        public bool HasLocalPassword { get; set; }

        public bool ShowRemoveButton { get; set; }

        public string Message { get; set; }

        public string ProviderDisplayName { get; set; }
    }
}