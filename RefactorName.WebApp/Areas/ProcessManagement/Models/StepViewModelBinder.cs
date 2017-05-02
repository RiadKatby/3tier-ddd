using System;
using System.Web.ModelBinding;
using System.Web.Mvc;

public class StepViewModelBinder : System.Web.Mvc.DefaultModelBinder
{
    protected override object CreateModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext, Type modelType)
    {
        var stepTypeValue = bindingContext.ValueProvider.GetValue("StepType");
        var stepType = Type.GetType((string)stepTypeValue.ConvertTo(typeof(string)), true);
        var step = Activator.CreateInstance(stepType);
        bindingContext.ModelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(() => step, stepType);
        return step;
    }
}