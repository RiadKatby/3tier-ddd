using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RefactorName.WebApp
{
    public enum Test
    {
        [Display(Name = "معلومات")]
        Info = 1,
        Danger,
        Success,
        Warning
    }
}