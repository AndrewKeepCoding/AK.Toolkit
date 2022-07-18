using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AK.Toolkit.WinUI3.Localization;

public record StringResource(string Key, string DependencyPropertyName, string Value);

public class StringResourceList : List<StringResource>
{
}

public class StringResourceListDictionary : ReadOnlyDictionary<string, StringResourceList>
{
    public StringResourceListDictionary(IDictionary<string, StringResourceList> dictionary) : base(dictionary)
    {
    }
}