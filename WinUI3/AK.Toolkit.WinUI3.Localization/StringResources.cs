using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AK.Toolkit.WinUI3.Localization;

public record StringResource(string Key, string DependencyPropertyName, string Value);

public class StringResources : ReadOnlyDictionary<string, StringResource>
{
    public StringResources(IDictionary<string, StringResource> dictionary) : base(dictionary)
    {
    }
}