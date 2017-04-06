using System;
using System.Globalization;
using System.Web.Mvc;

namespace RefactorName.Web.Binders
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        private string[] _customFormat;

        public DateTimeModelBinder(string[] customFormat)
        {
            _customFormat = customFormat;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            DateTime? result;
            try
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (Util.IsGreg(value.AttemptedValue.ToString()))
                    result = DateTime.ParseExact(value.AttemptedValue, _customFormat, new CultureInfo("en-GB"), DateTimeStyles.None);
                else
                {
                    var cul = new CultureInfo("ar-SA");
                    cul.DateTimeFormat.Calendar = new UmAlQuraCalendar();
                    result = DateTime.ParseExact(value.AttemptedValue, _customFormat, cul, DateTimeStyles.None);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}