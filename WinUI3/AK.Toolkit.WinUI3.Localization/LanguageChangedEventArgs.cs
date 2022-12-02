using System;

namespace AK.Toolkit.WinUI3.Localization;

public class LanguageChangedEventArgs : EventArgs
{
    public LanguageChangedEventArgs(string language)
    {
        Language = language;
    }

    public string Language { get; }
}