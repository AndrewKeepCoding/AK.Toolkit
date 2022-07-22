using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AK.Toolkit.Uwp.Localization
{
    public class StringResource
    {
        public StringResource(string key, string depenencyPropertyName, string value)
        {
            Key = key;
            DependencyPropertyName = depenencyPropertyName;
            Value = value;
        }

        public string Key { get; }

        public string DependencyPropertyName { get; }

        public string Value { get; }
    }

    public class StringResourceList : List<StringResource>
    {
    }

    public class StringResourceListDictionary : ReadOnlyDictionary<string, StringResourceList>
    {
        public StringResourceListDictionary(IDictionary<string, StringResourceList> dictionary) : base(dictionary)
        {
        }
    }
}