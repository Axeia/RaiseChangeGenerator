using System;

namespace RaiseChangeGenerator
{
    /// <summary>
    /// Generates a public property with ReactiveUI change notification for this field.
    /// The property name will be the field name in PascalCase without underscores.
    /// </summary>
    /// <example>
    /// <code>
    /// [RaiseChange]
    /// private int _myCounter;
    /// // Generates: public int MyCounter
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class RaiseChangeAttribute : Attribute
    {
    }

    /// <summary>
    /// Generates a proxy property that forwards to a property on the annotated field.<br/>
    /// <b>HIGHLY Recommended</b> to use <c>nameof()</c> for compile-time safety and easier refactoring.
    /// </summary>
    /// <example>
    /// <code>
    /// [RaiseChangeProxy(nameof(RssAutoDownloadingRule.SmartFilter))]
    /// [RaiseChangeProxy(nameof(RssAutoDownloadingRule.PreviouslyMatchedEpisodes), "Episodes")]
    /// private RssAutoDownloadingRule _rule;
    /// // Generates: public string SmartFilter and public List&lt;Episode&gt; Episodes
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class RaiseChangeProxyAttribute(string propertyName, string? customName = null) : Attribute
    {
        public string PropertyName { get; } = propertyName;
        public string? CustomName { get; } = customName;
    }

    /// <summary>
    /// Notifies additional properties when the annotated field changes.<br/>
    /// Use this for computed properties that depend on the annotated field.<br/>
    /// <b>HIGHLY recommended</b> to use nameof() for compile-time safety and easier refactoring.
    /// </summary>
    /// <example>
    /// <code>
    /// [RaiseChange]
    /// [AlsoNotify(nameof(FullName))]
    /// [AlsoNotify(nameof(DisplayText))]
    /// private string _firstName;
    /// 
    /// public string FullName => $"{FirstName} {LastName}";
    /// public string DisplayText => $"User: {FullName}";
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class AlsoNotifyAttribute(string propertyName) : Attribute
    {
        /// <summary>
        /// The name of the property that should also be notified when this field changes.
        /// </summary>
        public string PropertyName { get; } = propertyName;
    }
}