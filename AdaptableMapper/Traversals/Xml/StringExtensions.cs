using System.Linq;

namespace AdaptableMapper.Traversals.Xml
{
    internal static class StringExtensions
    {
        public static string ConvertToNamespacelessPath(this string path)
        {
            string trimmedPath = path.TrimStart('/', '.');
            string namespaceLessPath;
            if (path.Contains('/'))
            {
                string[] pathParts = trimmedPath.Split('/');
                namespaceLessPath = string.Concat(pathParts.Select(p => p.ConvertToNamespacelessPart()));
            }
            else
                namespaceLessPath = path.ConvertToNamespacelessPart();

            return namespaceLessPath;
        }

        private static string ConvertToNamespacelessPart(this string part)
        {
            string result;

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