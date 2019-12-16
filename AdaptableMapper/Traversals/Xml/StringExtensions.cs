using System.Linq;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public static class StringExtensions
    {
        private const char NullChar = '\0';
        private const string DefaultPrefix = "./";

        public static string ConvertToInterpretation(this string path, XmlInterpretation xmlInterpretation)
        {
            string result = path;

            switch (xmlInterpretation)
            {
                case XmlInterpretation.WithoutNamespace:
                    result = path.ConvertToNamespacelessPath();
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
            string originalPrefix = path.Substring(0, firstLetterIndex);
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
    }
}