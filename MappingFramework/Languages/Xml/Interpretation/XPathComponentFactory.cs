using System.Collections.Generic;
using System.Linq;

namespace MappingFramework.Languages.Xml.Interpretation
{
    internal static class XPathComponentFactory
    {
        public static IXPathComponent Create(string path, bool prefixedByExpression = false)
        {
            List<(string, int, bool)> hits = 
                path.Find(KnownFunctions, false).Concat(
                    path.Find(KnownSymbols, false).Concat(
                        path.Find(KnownExpressions, true).Concat(
                            path.FindRegex(FilterExpression, false)
                        )
                    )
                ).OrderBy(h => h.index).ThenByDescending(h => h.hit.Length).ToList();

            if (hits.Count == 0)
            {
                switch (path.Length)
                {
                    case 0:
                        return new XPathHeadPart();
                    case > 0 when prefixedByExpression:
                        return new XPathFilterPart(path, new XPathHeadPart());
                    case > 0:
                        return new XPathPart(path, new XPathHeadPart());
                }
            }

            (string hit, int index, bool isExpression) = hits.First();
            if (index == 0)
                return new XPathPart(hit, Create(path[hit.Length..], isExpression));

            string filterPart = path[..index];
            int skip = hit.Length + filterPart.Length;

            switch (prefixedByExpression)
            {
                case true:
                    return new XPathFilterPart(filterPart,
                        new XPathPart(hit, Create(path[skip..], isExpression))
                    );
                case false:
                    return new XPathPart(filterPart,
                        new XPathPart(hit, Create(path[skip..], isExpression))
                    );
            }
        }

        private static readonly List<string> KnownExpressions = new()
        {
            "//",
            "/"
        };

        private static readonly List<string> KnownFunctions = new()
        {
            "boolean",
            "ceiling",
            "choose",
            "concat",
            "contains",
            "count",
            "current",
            "document",
            "element-available",
            "false",
            "floor",
            "format-number",
            "function-available",
            "generate-id",
            "id",
            "key",
            "lang",
            "last",
            "local-name",
            "name",
            "namespace-uri",
            "normalize-space",
            "not",
            "number",
            "position",
            "round",
            "starts-with",
            "string",
            "string-length",
            "substring",
            "substring-after",
            "substring-before",
            "sum",
            "system-property",
            "translate",
            "true"
        };

        private static readonly List<string> KnownSymbols = new()
        {
            "..",
            ".",
            "(",
            ")",
            "@",
            ",",
            "\"",
            "'",
            "[",
            "]",
            "*"
        };

        private static readonly List<string> FilterExpression = new()
        {
            "(\\[.*?\\])"
        };
    }
}