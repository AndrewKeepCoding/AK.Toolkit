using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Windows.UI.Xaml;

namespace AK.Toolkit.Uwp.Localization
{
    public partial class Localizer
    {
        private static DependencyProperty GetDependencyProperty(UIElement element, string dependencyPropertyName)
        {
            Type type = element.GetType();
            if (type.GetProperty(
                dependencyPropertyName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) is PropertyInfo propertyInfo &&
                propertyInfo.GetValue(null) is DependencyProperty property)
            {
                return property;
            }
            else if (type.GetField(
                dependencyPropertyName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) is FieldInfo fieldInfo &&
                fieldInfo.GetValue(null) is DependencyProperty field)
            {
                return field;
            }

            return null;
        }

        private static (string Key, string Property) GetKeyAndDependencyPropertyName(string name)
        {
            return name.LastIndexOf(".") is int lastSeparatorIndex && lastSeparatorIndex > 1
                ? (name.Substring(0, lastSeparatorIndex), name.Substring(lastSeparatorIndex + 1))
                : (name, string.Empty);
        }

        private static StringResource CreateStringResource(XmlNode xmlNode)
        {
            if ((xmlNode?.Attributes?["name"]?.Value is string name) &&
                (xmlNode?["value"]?.InnerText) is string value)
            {
                (string Key, string DependencyPropertyName) = GetKeyAndDependencyPropertyName(name);
                return new StringResource(Key, DependencyPropertyName + "Property", value);
            }

            return null;
        }

        private static StringResourceListDictionary LoadLanguageResources(string filePath)
        {
            Dictionary<string, StringResourceList> resourceDictionary = new Dictionary<string, StringResourceList>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            if (xmlDoc.SelectNodes("//root/data") is XmlNodeList nodeList)
            {
                foreach (XmlNode node in nodeList)
                {
                    if (CreateStringResource(node) is StringResource resource)
                    {
                        if (resourceDictionary.ContainsKey(resource.Key) is false)
                        {
                            resourceDictionary[resource.Key] = new StringResourceList();
                        }

                        resourceDictionary[resource.Key].Add(resource);
                    }
                }
            }

            return new StringResourceListDictionary(resourceDictionary);
        }
    }
}