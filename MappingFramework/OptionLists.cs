using System;
using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Configuration.DataStructure;
using MappingFramework.Configuration.Dictionary;
using MappingFramework.Configuration.Json;
using MappingFramework.Configuration.Xml;
using MappingFramework.Traversals;
using MappingFramework.Traversals.Dictionary;
using MappingFramework.Traversals.Json;
using MappingFramework.Traversals.Model;
using MappingFramework.Traversals.Xml;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework
{
    public static class OptionLists
    {
        private static List<Type> ComposedGetValueTraversals => new List<Type>
        {
            typeof(GetAdditionalSourceValue),
            typeof(GetConcatenatedByListValueTraversal),
            typeof(GetConcatenatedValueTraversal),
            typeof(GetMutatedValueTraversal),
            typeof(GetNumberOfHits),
            typeof(GetSearchValueTraversal),
            typeof(GetStaticValue),
            typeof(GetValueTraversalDaysBetweenDates),
            typeof(IfConditionThenAElseBGetValueTraversal),
            typeof(NullObject)
        };

        private static List<Type> NewXmlGetValueTraversals => new List<Type>
        {
            typeof(XmlGetValueTraversal),
            typeof(XmlGetThisValueTraversal)
        };

        private static List<Type> NewJsonGetValueTraversals => new List<Type>
        {
            typeof(JsonGetValueTraversal)
        };

        private static List<Type> NewDataStructureGetValueTraversals => new List<Type>
        {
            typeof(ModelGetValueTraversal)
        };

        private static List<Type> XmlGetValueTraversals() => CreateList(NewXmlGetValueTraversals, ComposedGetValueTraversals);
        private static List<Type> JsonGetValueTraversals() => CreateList(NewJsonGetValueTraversals, ComposedGetValueTraversals);
        private static List<Type> DataStructureGetValueTraversals() => CreateList(NewDataStructureGetValueTraversals, ComposedGetValueTraversals);

        private static List<Type> GetValueTraversals(ContentType contentType)
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

        private static List<Type> ComposedSetValueTraversals => new List<Type>
        {
            typeof(SetMutatedValueTraversal)
        };

        private static List<Type> NewXmlSetValueTraversals => new List<Type>
        {
            typeof(XmlSetValueTraversal),
            typeof(XmlSetThisValueTraversal),
            typeof(XmlSetGeneratedIdValueTraversal)
        };

        private static List<Type> NewJsonSetValueTraversals => new List<Type>
        {
            typeof(JsonSetValueTraversal)
        };

        private static List<Type> NewDataStructureSetValueTraversals => new List<Type>
        {
            typeof(ModelSetValueOnPathTraversal),
            typeof(ModelSetValueOnPropertyTraversal)
        };

        private static List<Type> NewDictionarySetValueTraversals => new List<Type>
        {
            typeof(DictionarySetValueTraversal)
        };

        private static List<Type> XmlSetValueTraversals() => CreateList(NewXmlSetValueTraversals, ComposedSetValueTraversals);
        private static List<Type> JsonSetValueTraversals() => CreateList(NewJsonSetValueTraversals, ComposedSetValueTraversals);
        private static List<Type> DataStructureSetValueTraversals() => CreateList(NewDataStructureSetValueTraversals, ComposedSetValueTraversals);
        private static List<Type> DictionaryGetValueTraversals() => CreateList(NewDictionarySetValueTraversals, ComposedSetValueTraversals);

        private static List<Type> SetValueTraversals(ContentType contentType)
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

        private static Type ObjectConverter(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlObjectConverter) :
            contentType == ContentType.Json ? typeof(JsonObjectConverter) :
            contentType == ContentType.DataStructure ? typeof(DataStructureObjectConverter) :
            contentType == ContentType.String ? typeof(StringToDataStructureObjectConverter) : null;

        private static Type TargetInstantiator(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XmlTargetInstantiator) :
            contentType == ContentType.Json ? typeof(JsonTargetInstantiator) :
            contentType == ContentType.DataStructure ? typeof(DataStructureTargetInstantiator) : 
            contentType == ContentType.Dictionary ? typeof(DictionaryTargetInstantiator) : null;

        private static Type ResultObjectConverter(ContentType contentType) =>
            contentType == ContentType.Xml ? typeof(XElementToStringObjectConverter) :
            contentType == ContentType.Json ? typeof(JTokenToStringObjectConverter) :
            contentType == ContentType.DataStructure ? typeof(DataStructureToStringObjectConverter):
            contentType == ContentType.Dictionary ? typeof(DataStructureToStringObjectConverter) : null;

        public static List<Type> List(Type type, ContentType contentType)
        {
            if (!type.IsInterface)
                throw new Exception("List is meant to be called with an interface type as parameter");

            if (type == typeof(ObjectConverter))
                return new List<Type> { ObjectConverter(contentType) };
            if (type == typeof(TargetInstantiator))
                return new List<Type> { TargetInstantiator(contentType) };
            if (type == typeof(ResultObjectConverter))
                return new List<Type> { ResultObjectConverter(contentType) };

            if (type == typeof(Condition))
                return new List<Type>
                {
                    typeof(CompareCondition),
                    typeof(ListOfConditions),
                    typeof(NotEmptyCondition)
                };
            if (type == typeof(ValueMutation))
                return new List<Type>
                {
                    typeof(CreateSeparatedRangeFromNumberValueMutation),
                    typeof(DateValueMutation),
                    typeof(DictionaryReplaceValueMutation),
                    typeof(ListOfValueMutations),
                    typeof(NumberValueMutation),
                    typeof(PlaceholderValueMutation),
                    typeof(ReplaceValueMutation),
                    typeof(SubstringValueMutation),
                    typeof(ToLowerValueMutation),
                    typeof(ToUpperValueMutation),
                    typeof(TrimValueMutation)
                };
            if (type == typeof(GetValueStringTraversal))
                return new List<Type>
                {
                    typeof(SplitByCharTakePositionStringTraversal)
                };
            if (type == typeof(GetValueTraversal))
                return GetValueTraversals(contentType);
            if (type == typeof(SetValueTraversal))
                return SetValueTraversals(contentType);

            throw new Exception($"{type.Name} is not supported");
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