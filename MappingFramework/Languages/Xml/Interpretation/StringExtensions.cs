using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MappingFramework.Languages.Xml.Interpretation
{
    public static class StringExtensions
    {
        public static string ConvertToInterpretation(this string path, XmlInterpretation xmlInterpretation)
        {
            if ((path?.Length ?? 0) == 0)
                return string.Empty;

            switch (xmlInterpretation)
            {
                case XmlInterpretation.WithoutNamespace:
                    IXPathComponent test = XPathComponentFactory.Create(path);
                    return test.Compose();
                default:
                    return path;
            }
        }

        public static IEnumerable<(string hit, int index, bool isExpression)> Find(this string value, List<string> searches, bool isExpression)
        {
            foreach (string search in searches)
            {
                int index = value.IndexOf(search, StringComparison.InvariantCulture);
                if (index >= 0)
                    yield return (search, index, isExpression);
            }
        }

        public static IEnumerable<(string hit, int index, bool isExpression)> FindRegex(this string value, List<string> searchExpressions, bool isExpression)
        {
            foreach (string searchExpression in searchExpressions)
            {
                Match match = Regex.Match(value, searchExpression);
                if (match.Success)
                    yield return (match.Value, match.Index, isExpression);
            }
        }
    }
}