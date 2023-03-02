namespace System.Text;

internal static class StringExtensions
{
        /// <summary>
        ///     Simple wrapper for <c>IsNullOrWhiteSpace</c>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string? input) =>
            string.IsNullOrWhiteSpace(input);

        /// <summary>
        ///     Simple wrapper for <c>IsNullOrWhiteSpace</c> with a bang!
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string? input) =>
            !input.IsEmpty();

}