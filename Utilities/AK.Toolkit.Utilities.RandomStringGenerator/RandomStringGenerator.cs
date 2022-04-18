using System.ComponentModel;
using System.Text;

namespace AK.Toolkit.Utilities;

public static class RandomStringGenerator
{
    public enum OutputType
    {
        /// <summary>
        /// 0123456789
        /// </summary>
        Numbers,
        /// <summary>
        /// ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz
        /// </summary>
        Alphabets,
        /// <summary>
        /// abcdefghijklmnopqrstuvwxyz
        /// </summary>
        LowerCaseAlphabets,
        /// <summary>
        /// ABCDEFGHIJKLMNOPQRSTUVWXYZ
        /// </summary>
        UpperCaseAlphabets,
        /// <summary>
        /// ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789
        /// </summary>
        AlphaNumerics,
        /// <summary>
        /// abcdefghijklmnopqrstuvwxyz0123456789
        /// </summary>
        LowerCaseAlphaNumerics,
        /// <summary>
        /// ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789
        /// </summary>
        UpperCaseAlphaNumerics,
    }

    public static string GenerateString(OutputType outputType, int length)
    {
        string source = GetSource(outputType);

        Random random = new();
        StringBuilder stringBuilder = new();

        for (int i = 0; i < length; i++)
        {
            _ = stringBuilder.Append(source[random.Next(source.Length)]);
        }

        return stringBuilder.ToString();
    }

    public static string GenerateString(OutputType outputType, int minLength, int maxLength)
    {
        Random random = new();
        int length = random.Next(minLength, maxLength + 1);
        return GenerateString(outputType, length);
    }

    private static string GetSource(OutputType outputType)
        => outputType switch
        {
            OutputType.Numbers => "0123456789",
            OutputType.Alphabets => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz",
            OutputType.LowerCaseAlphabets => "abcdefghijklmnopqrstuvwxyz",
            OutputType.UpperCaseAlphabets => "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            OutputType.AlphaNumerics => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789",
            OutputType.LowerCaseAlphaNumerics => "abcdefghijklmnopqrstuvwxyz0123456789",
            OutputType.UpperCaseAlphaNumerics => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            _ => throw new InvalidEnumArgumentException("outputType", (int)outputType, typeof(OutputType))
        };
}
