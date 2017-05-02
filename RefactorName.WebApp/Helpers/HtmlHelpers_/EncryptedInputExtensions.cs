using IdeasBank.Web.Helpers.HtmlHelpers;
using IdeasBank.Web.Infrastructure;
using IdeasBank.Web.Infrastructure.Encryption;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcHtmlHelpers
{
    //[System.Diagnostics.DebuggerStepThrough]
    public static class EncryptedInputExtensions
    {
        // CheckBox

        public static MvcHtmlString EncryptedCheckBox(this HtmlHelper htmlHelper, string name)
        {
            return EncryptedCheckBox(htmlHelper, name, htmlAttributes: (object)null);
        }

        public static MvcHtmlString EncryptedCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked)
        {
            return EncryptedCheckBox(htmlHelper, name, isChecked, htmlAttributes: (object)null);
        }

        public static MvcHtmlString EncryptedCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, object htmlAttributes)
        {
            return EncryptedCheckBox(htmlHelper, name, isChecked, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString EncryptedCheckBox(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            return EncryptedCheckBox(htmlHelper, name, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString EncryptedCheckBox(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
        {
            return CheckBoxHelper(htmlHelper, metadata: null, name: name, isChecked: null, htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString EncryptedCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, IDictionary<string, object> htmlAttributes)
        {
            return CheckBoxHelper(htmlHelper, metadata: null, name: name, isChecked: isChecked, htmlAttributes: htmlAttributes);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            return EncryptedCheckBoxFor(htmlHelper, expression, htmlAttributes: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            return EncryptedCheckBoxFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            bool? isChecked = null;
            if (metadata.Model != null)
            {
                bool modelChecked;
                if (Boolean.TryParse(metadata.Model.ToString(), out modelChecked))
                {
                    isChecked = modelChecked;
                }
            }

            return CheckBoxHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), isChecked, htmlAttributes);
        }

        private static MvcHtmlString CheckBoxHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, bool? isChecked, IDictionary<string, object> htmlAttributes)
        {
            RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);

            bool explicitValue = isChecked.HasValue;
            if (explicitValue)
            {
                attributes.Remove("checked"); // Explicit value must override dictionary
            }

            return InputHelper(htmlHelper,
                               InputType.CheckBox,
                               metadata,
                               name,
                               value: "true",
                               useViewData: !explicitValue,
                               isChecked: isChecked ?? false,
                               setId: true,
                               isExplicitValue: false,
                               format: null,
                               htmlAttributes: attributes);
        }

        // Hidden

        public static MvcHtmlString EncryptedHidden(this HtmlHelper htmlHelper, string name)
        {
            return EncryptedHidden(htmlHelper, name, value: null, htmlAttributes: null);
        }

        public static MvcHtmlString EncryptedHidden(this HtmlHelper htmlHelper, string name, object value)
        {
            return EncryptedHidden(htmlHelper, name, value, htmlAttributes: null);
        }

        public static MvcHtmlString EncryptedHidden(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return EncryptedHidden(htmlHelper, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString EncryptedHidden(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            return HiddenHelper(htmlHelper,
                                metadata: null,
                                value: value,
                                useViewData: value == null,
                                expression: name,
                                htmlAttributes: htmlAttributes);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return EncryptedHiddenFor(htmlHelper, expression, (IDictionary<string, object>)null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return EncryptedHiddenFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return HiddenHelper(htmlHelper,
                                metadata,
                                metadata.Model,
                                false,
                                ExpressionHelper.GetExpressionText(expression),
                                htmlAttributes);
        }

        private static MvcHtmlString HiddenHelper(HtmlHelper htmlHelper, ModelMetadata metadata, object value, bool useViewData, string expression, IDictionary<string, object> htmlAttributes)
        {
            Binary binaryValue = value as Binary;
            if (binaryValue != null)
            {
                value = binaryValue.ToArray();
            }

            byte[] byteArrayValue = value as byte[];
            if (byteArrayValue != null)
            {
                value = Convert.ToBase64String(byteArrayValue);
            }

            return InputHelper(htmlHelper,
                               InputType.Hidden,
                               metadata,
                               expression,
                               value,
                               useViewData,
                               isChecked: false,
                               setId: true,
                               isExplicitValue: true,
                               format: null,
                               htmlAttributes: htmlAttributes);
        }

        // RadioButton

        public static MvcHtmlString EncryptedRadioButton(this HtmlHelper htmlHelper, string name, object value)
        {
            return EncryptedRadioButton(htmlHelper, name, value, htmlAttributes: (object)null);
        }

        public static MvcHtmlString EncryptedRadioButton(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return EncryptedRadioButton(htmlHelper, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString EncryptedRadioButton(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            // Determine whether or not to render the checked attribute based on the contents of ViewData.
            string valueString = Convert.ToString(value, CultureInfo.CurrentCulture);
            bool isChecked = (!String.IsNullOrEmpty(name)) && (String.Equals(htmlHelper.EvalString(name), valueString, StringComparison.OrdinalIgnoreCase));
            // checked attributes is implicit, so we need to ensure that the dictionary takes precedence.
            RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);
            if (attributes.ContainsKey("checked"))
            {
                return InputHelper(htmlHelper,
                                   InputType.Radio,
                                   metadata: null,
                                   name: name,
                                   value: value,
                                   useViewData: false,
                                   isChecked: false,
                                   setId: true,
                                   isExplicitValue: true,
                                   format: null,
                                   htmlAttributes: attributes);
            }

            return EncryptedRadioButton(htmlHelper, name, value, isChecked, htmlAttributes);
        }

        public static MvcHtmlString EncryptedRadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked)
        {
            return EncryptedRadioButton(htmlHelper, name, value, isChecked, htmlAttributes: (object)null);
        }

        public static MvcHtmlString EncryptedRadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, object htmlAttributes)
        {
            return EncryptedRadioButton(htmlHelper, name, value, isChecked, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString EncryptedRadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, IDictionary<string, object> htmlAttributes)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            // checked attribute is an explicit parameter so it takes precedence.
            RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);
            attributes.Remove("checked");
            return InputHelper(htmlHelper,
                               InputType.Radio,
                               metadata: null,
                               name: name,
                               value: value,
                               useViewData: false,
                               isChecked: isChecked,
                               setId: true,
                               isExplicitValue: true,
                               format: null,
                               htmlAttributes: attributes);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value)
        {
            return EncryptedRadioButtonFor(htmlHelper, expression, value, htmlAttributes: null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes)
        {
            return EncryptedRadioButtonFor(htmlHelper, expression, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString EncryptedRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return RadioButtonHelper(htmlHelper,
                                     metadata,
                                     metadata.Model,
                                     ExpressionHelper.GetExpressionText(expression),
                                     value,
                                     null /* isChecked */,
                                     htmlAttributes);
        }

        private static MvcHtmlString RadioButtonHelper(HtmlHelper htmlHelper, ModelMetadata metadata, object model, string name, object value, bool? isChecked, IDictionary<string, object> htmlAttributes)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);

            bool explicitValue = isChecked.HasValue;
            if (explicitValue)
            {
                attributes.Remove("checked"); // Explicit value must override dictionary
            }
            else
            {
                string valueString = Convert.ToString(value, CultureInfo.CurrentCulture);
                isChecked = model != null &&
                            !String.IsNullOrEmpty(name) &&
                            String.Equals(model.ToString(), valueString, StringComparison.OrdinalIgnoreCase);
            }

            return InputHelper(htmlHelper,
                               InputType.Radio,
                               metadata,
                               name,
                               value,
                               useViewData: false,
                               isChecked: isChecked ?? false,
                               setId: true,
                               isExplicitValue: true,
                               format: null,
                               htmlAttributes: attributes);
        }

        // Helper methods

        private static MvcHtmlString InputHelper(HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string format, IDictionary<string, object> htmlAttributes)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }

            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
            tagBuilder.MergeAttribute("name", string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullName), true);

            string valueParameter = htmlHelper.FormatValue(value, format);
            bool usedModelState = false;

            switch (inputType)
            {
                case InputType.CheckBox:
                    bool? modelStateWasChecked = htmlHelper.GetModelStateValue(fullName, typeof(bool)) as bool?;
                    if (modelStateWasChecked.HasValue)
                    {
                        isChecked = modelStateWasChecked.Value;
                        usedModelState = true;
                    }
                    goto case InputType.Radio;
                case InputType.Radio:
                    if (!usedModelState)
                    {
                        string modelStateValue = htmlHelper.GetModelStateValue(fullName, typeof(string)) as string;
                        if (modelStateValue != null)
                        {
                            isChecked = String.Equals(modelStateValue, valueParameter, StringComparison.Ordinal);
                            usedModelState = true;
                        }
                    }
                    if (!usedModelState && useViewData)
                    {
                        isChecked = htmlHelper.EvalBoolean(fullName);
                    }
                    if (isChecked)
                    {
                        tagBuilder.MergeAttribute("checked", "checked");
                    }
                    tagBuilder.MergeAttribute("value", StringEncrypter.ControlsEncrypter.Encrypt(valueParameter), isExplicitValue);
                    break;
                case InputType.Password:
                    if (value != null)
                    {
                        tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
                    }
                    break;
                default:
                    string attemptedValue = (string)htmlHelper.GetModelStateValue(fullName, typeof(string));
                    if (attemptedValue != null)
                        tagBuilder.MergeAttribute("value", StringEncrypter.ControlsEncrypter.Encrypt(attemptedValue));
                    else
                        tagBuilder.MergeAttribute("value",
                            ((useViewData) ? StringEncrypter.ControlsEncrypter.Encrypt(htmlHelper.EvalString(fullName, format)) : (value == null ? null : StringEncrypter.ControlsEncrypter.Encrypt(valueParameter))),
                            isExplicitValue);
                    break;
            }

            if (setId)
            {
                tagBuilder.GenerateId(string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullName));
            }

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            Dictionary<string, object> unobtrusiveValidationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata) as Dictionary<string, object>;
            //remove attributes which will cause problems for encrypted value types (primitive types attributes)
            //as we experienced till now: (data-val-number)
            unobtrusiveValidationAttributes.Remove("data-val-number");
            tagBuilder.MergeAttributes(unobtrusiveValidationAttributes);

            if (inputType == InputType.CheckBox)
            {
                // Render an additional <input type="hidden".../> for checkboxes. This
                // addresses scenarios where unchecked checkboxes are not sent in the request.
                // Sending a hidden input makes it possible to know that the checkbox was present
                // on the page when the request was submitted.
                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));

                TagBuilder hiddenInput = new TagBuilder("input");
                hiddenInput.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
                hiddenInput.MergeAttribute("name", string.Concat(StringEncrypter.ControlsEncrypter.Prefix, fullName));
                hiddenInput.MergeAttribute("value", StringEncrypter.ControlsEncrypter.Encrypt("false"));
                inputItemBuilder.Append(hiddenInput.ToString(TagRenderMode.SelfClosing));
                return MvcHtmlString.Create(inputItemBuilder.ToString());
            }

            return tagBuilder.ToMvcHtmlString(TagRenderMode.SelfClosing);
        }

        private static RouteValueDictionary ToRouteValueDictionary(IDictionary<string, object> dictionary)
        {
            return dictionary == null ? new RouteValueDictionary() : new RouteValueDictionary(dictionary);
        }
    }
}