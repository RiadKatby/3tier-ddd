using System;
using System.Collections.Generic;

namespace RefactorName.WebApp.Models
{
    public class WebGridList<T>
    {
        public List<T> List { get; set; }
        public int RowCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}