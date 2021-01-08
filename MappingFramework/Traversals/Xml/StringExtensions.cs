using System.Linq;
using MappingFramework.Xml;

namespace MappingFramework.Traversals.Xml
{
    public static class StringExtensions
    {
        private const char NullChar = '\0';
        private const string DefaultPrefix = "./";

        public static string ConvertToInterpretation(this string path, XmlInterpretation xmlInterpretation)
        {
            if ((path?.Length ?? 0) == 0)
                return string.Empty;

            string result = path;

            switch (xmlInterpretation)
            {
                case XmlInterpretation.WithoutNamespace:
                    string wrappingMethod = GetWrappingMethod(path);
                    if (!string.IsNullOrWhiteSpace(wrappingMethod))
                        path = UnWrap(path, wrappingMethod);

                    result = path.ConvertToNamespacelessPath();

                    if (!string.IsNullOrWhiteSpace(wrappingMethod))
                        result = Wrap(result, wrappingMethod);
                    break;
            }

            return result;
        }

        private static string ConvertToNamespacelessPath(this string path)
        {
            string originalPrefix = path.GetPrefix();

            string trimmedPath = path.TrimStart('/', '.');
            string namespaceLessPath;
            if (path.Contains('/'))
            {
                string[] pathParts = trimmedPath.Split('/');
                namespaceLessPath = string.Concat(pathParts.Select(p => p.ConvertToNamespacelessPart())).TrimStart('/');
            }
            else
                namespaceLessPath = path.ConvertToNamespacelessPart().TrimStart('/');

            return originalPrefix + namespaceLessPath;
        }

        private static string GetPrefix(this string path)
        {
            char firstLetter = path.FirstOrDefault(char.IsLetter);

            if (firstLetter == NullChar)
                return DefaultPrefix;

            int firstLetterIndex = path.IndexOf(firstLetter);
            string originalPrefix = path.Substring(0, firstLetterIndex).Trim('@');
            return originalPrefix;
        }

        private static string ConvertToNamespacelessPart(this string part)
        {
            string result;

            if (part.StartsWith("@"))
                return $"/{part}";

            int indexOfOpeningBracket = part.IndexOf('[');
            if (indexOfOpeningBracket > 0)
            {
                string name = part.Substring(0, indexOfOpeningBracket);
                string filter = part.Substring(indexOfOpeningBracket, part.Length - indexOfOpeningBracket);

                result = $"/*[local-name()='{name}']{filter}";
            }
            else
                result = $"/*[local-name()='{part}']";

            return result;
        }

        private static string GetWrappingMethod(string path)
        {
            if (!(path.Last().Equals(')') && char.IsLetter(path.First())))
                return string.Empty;

            int openingParenthesisIndex = path.IndexOf('(');
            string result = path.Substring(0, openingParenthesisIndex);

            return result;
        }

        private static string UnWrap(string path, string wrappingMethod)
        {
            string result = path.Substring(wrappingMethod.Length, path.Length - wrappingMethod.Length);
            result = result.Trim('(', ')');

            return result;
        }

        private static string Wrap(string path, string wrappingMethod)
            => $"{wrappingMethod}({path})";
    }
}