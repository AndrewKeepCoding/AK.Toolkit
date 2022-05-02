# 🧰 AK.Toolkit

The AK.Toolkit will be a collection of controls, helpers, etc... stuff that I needed to use but couldn't find somewhere else.

## 🔵 WinUI 3

### 💻 AutoCompleteTextBox

A TextBox control that shows a suggestion based on input.
AutoCompleteTextBox shows a suggestion **inside** the TextBox control.

```xaml
<toolkit:AutoCompleteTextBox
    IsSuggestionCaseSensitive="false"
    SuggestionForeground="HotPink"
    SuggestionPrefix="..."
    SuggestionSuffix=" ? [Press Right]"
    SuggestionsSource="{x:Bind Suggestions, Mode=OneWay}" />
```

![AutoCompleteTextBox Screenshot](Assets/auto-complete-textbox-sample-screenshot.png)

## 🛠️ Utilities

### 🧩 RandomStringGenerator

A static class that generates random strings.

| OutputType               | Source                                                         |
| ------------------------ | -------------------------------------------------------------- |
| `Numbers`                | 0123456789                                                     |
| `Alphabets`              | ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz           |
| `LowerCaseAlphabets`     | abcdefghijklmnopqrstuvwxyz                                     |
| `UpperCaseAlphabets`     | ABCDEFGHIJKLMNOPQRSTUVWXYZ                                     |
| `AlphaNumerics`          | ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 |
| `LowerCaseAlphaNumerics` | abcdefghijklmnopqrstuvwxyz0123456789                           |
| `UpperCaseAlphaNumerics` | ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789                           |

It's easy to use.

```csharp
string randomString = RandomStringGenerator.GenerateString(
    OutputType.AlphaNumerics,
    minLength: 3,
    maxLength: 10);
```

## 🎬 YouTube

- [AutoCompleteTextBox and RandomStringGenerator](https://youtu.be/G17jbGSXLnk)
