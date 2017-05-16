using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RefactorName.WebApp.Infrastructure
{
    /// <summary>
    /// Contains classes and properties that are used to create HTML elements.
    /// </summary>
    public class TagBuilderEx : TagBuilder, IDisposable
    {
        private List<TagBuilderEx> innerTagBuilders;

        /// <summary>
        /// Creates a new tag that has a specific tag name.
        /// </summary>
        /// <param name="tagName">The tag name without the "<", "/", or ">" delimiters.</param>
        public TagBuilderEx(string tagName)
            : base(tagName)
        {
            innerTagBuilders = new List<TagBuilderEx>();
        }

        /// <summary>
        /// Create an Inner Tag that is associated to this Tag.
        /// </summary>
        /// <param name="tagName">The tag name without the "<", "/", or ">" delimiters.</param>
        /// <returns></returns>
        public TagBuilderEx CreateInnerTag(string tagName)
        {
            TagBuilderEx innerBuilder = new TagBuilderEx(tagName);
            innerTagBuilders.Add(innerBuilder);

            return innerBuilder;
        }

        /// <summary>
        /// Adds a CSS class to the list of CSS classes in the tag if specific condition is satisfied.
        /// </summary>
        /// <param name="builder">TagBuilder object to add class to.</param>
        /// <param name="condition">condition to be checked.</param>
        /// <param name="trueValue">use this value when condition satisfied.</param>
        /// <param name="falseValue">use this value when condition unsatisfied.</param>
        public void AddCssClassIf(bool condition, string trueValue, string falseValue)
        {
            if (condition)
                AddCssClass(trueValue);
            else
                AddCssClass(falseValue);
        }

        public void Dispose()
        {
            StringBuilder sb = new StringBuilder(InnerHtml);

            foreach (var builder in innerTagBuilders)
                sb.Append(builder.ToString());

            InnerHtml = sb.ToString();
        }

        /// <summary>
        /// Adds a CSS classes to the list of CSS classes in the tag.
        /// </summary>
        /// <param name="classes">The CSS classes to add</param>
        public void AddCssClasses(params string[] classes)
        {
            foreach (var clazz in classes)
                AddCssClass(clazz);
        }
    }
}