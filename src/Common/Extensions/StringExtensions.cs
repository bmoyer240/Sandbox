using System.Diagnostics.CodeAnalysis;

namespace Application;

public static class StringExtensions
{
    /// <summary>
    ///     Simple wrapper for <c>IsNullOrWhiteSpace</c>
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEmpty([NotNullWhen(false)] this string? input) =>
        string.IsNullOrWhiteSpace(input);

    /// <summary>
    ///     Simple wrapper for <c>IsNullOrWhiteSpace</c> with a bang!
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNotEmpty([NotNullWhen(true)] this string? input) =>
        string.IsNullOrWhiteSpace(input) == false;

}