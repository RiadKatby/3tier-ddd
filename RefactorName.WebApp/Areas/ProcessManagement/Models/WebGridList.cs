using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public class WebGridList<T>
    {
        public List<T> List { get; set; }
        public int RowCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}