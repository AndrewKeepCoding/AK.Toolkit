# 🧰AK.Toolkit

## 🛠️Utilities
### 🧩RandomStringGenerator
A static class that generates random strings.
- Numbers: 0123456789
- Alphabets: ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz
- LowerCaseAlphabets: abcdefghijklmnopqrstuvwxyz
- UpperCaseAlphabets: ABCDEFGHIJKLMNOPQRSTUVWXYZ
- AlphaNumerics: ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789
- LowerCaseAlphaNumerics: abcdefghijklmnopqrstuvwxyz0123456789
- UpperCaseAlphaNumerics: ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789
It's easy to use.
```csharp
string randomString = RandomStringGenerator.GenerateString(
    OutputType.AlphaNumerics,
    minLength: 3,
    maxLength: 10);
```
