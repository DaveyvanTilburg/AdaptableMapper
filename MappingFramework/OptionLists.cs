using System;
using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Traversals;
using MappingFramework.Traversals.Dictionary;
using MappingFramework.Traversals.Json;
using MappingFramework.Traversals.Model;
using MappingFramework.Traversals.Xml;

namespace MappingFramework
{
    public static class OptionLists
    {
        private static readonly List<GetValueTraversal> _composedGetValueTraversals = new List<GetValueTraversal>
        {
            new GetAdditionalSourceValue(),
            new GetConcatenatedByListValueTraversal(),
            new GetConcatenatedValueTraversal(),
            new GetMutatedValueTraversal(),
            new GetNothingValueTraversal(),
            new GetNumberOfHits(),
            new GetSearchValueTraversal(),
            new GetStaticValue(),
            new GetValueTraversalDaysBetweenDates(),
            new IfConditionThenAElseBGetValueTraversal()
        };

        private static readonly List<GetValueTraversal> _xmlGetValueTraversals = new List<GetValueTraversal>
        {
            new XmlGetValueTraversal(),
            new XmlGetThisValueTraversal()
        };

        private static readonly List<GetValueTraversal> _jsonGetValueTraversals = new List<GetValueTraversal>
        {
            new JsonGetValueTraversal()
        };

        private static readonly List<GetValueTraversal> _dataStructureGetValueTraversals = new List<GetValueTraversal>
        {
            new ModelGetValueTraversal()
        };

        private static List<GetValueTraversal> XmlGetValueTraversals() => CreateList(_xmlGetValueTraversals, _composedGetValueTraversals);
        private static List<GetValueTraversal> JsonGetValueTraversals() => CreateList(_jsonGetValueTraversals, _composedGetValueTraversals);
        private static List<GetValueTraversal> DataStructureGetValueTraversals() => CreateList(_dataStructureGetValueTraversals, _composedGetValueTraversals);
        
        public static List<GetValueTraversal> GetValueTraversals(ContentType contentType)
        {
            switch(contentType)
            {
                case ContentType.Xml:
                    return XmlGetValueTraversals();
                case ContentType.Json:
                    return JsonGetValueTraversals();
                case ContentType.DataStructure:
                    return DataStructureGetValueTraversals();
                default:
                    throw new Exception($"{contentType} is unsupported for {nameof(GetValueTraversal)}");
            }
        }



        private static readonly List<SetValueTraversal> _composedSetValueTraversals = new List<SetValueTraversal>
        {
            new SetMutatedValueTraversal()
        };

        private static readonly List<SetValueTraversal> _xmlSetValueTraversals = new List<SetValueTraversal>
        {
            new XmlSetValueTraversal(),
            new XmlSetThisValueTraversal(),
            new XmlSetGeneratedIdValueTraversal()
        };

        private static readonly List<SetValueTraversal> _jsonSetValueTraversals = new List<SetValueTraversal>
        {
            new JsonSetValueTraversal()
        };

        private static readonly List<SetValueTraversal> _dataStructureSetValueTraversals = new List<SetValueTraversal>
        {
            new ModelSetValueOnPathTraversal(),
            new ModelSetValueOnPropertyTraversal()
        };

        private static readonly List<SetValueTraversal> _dictionarySetValueTraversals = new List<SetValueTraversal>
        {
            new DictionarySetValueTraversal()
        };

        private static List<SetValueTraversal> XmlSetValueTraversals() => CreateList(_xmlSetValueTraversals, _composedSetValueTraversals);
        private static List<SetValueTraversal> JsonSetValueTraversals() => CreateList(_jsonSetValueTraversals, _composedSetValueTraversals);
        private static List<SetValueTraversal> DataStructureSetValueTraversals() => CreateList(_dataStructureSetValueTraversals, _composedSetValueTraversals);
        private static List<SetValueTraversal> DictionaryGetValueTraversals() => CreateList(_dictionarySetValueTraversals, _composedSetValueTraversals);

        public static List<SetValueTraversal> SetValueTraversals(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Xml:
                    return XmlSetValueTraversals();
                case ContentType.Json:
                    return JsonSetValueTraversals();
                case ContentType.DataStructure:
                    return DataStructureSetValueTraversals();
                case ContentType.Dictionary:
                    return DictionaryGetValueTraversals();
                default:
                    throw new Exception($"{contentType} is unsupported for {nameof(SetValueTraversal)}");
            }
        }


        private static List<T> CreateList<T>(params IEnumerable<T>[] itemsList)
        {
            var result = new List<T>();
            
            foreach (IEnumerable<T> items in itemsList)
                result.AddRange(items);

            return result;
        }
    }
}