using System;
using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration.Dictionary;
using MappingFramework.Configuration.Json;
using MappingFramework.Configuration.DataStructure;
using MappingFramework.Configuration.Xml;
using MappingFramework.Traversals.DataStructure;
using MappingFramework.Traversals.Dictionary;
using MappingFramework.Traversals.Json;
using MappingFramework.Traversals.Xml;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework.Converters
{
    public static class ResolvableTypeIdCollection
    {
        private static readonly Dictionary<string, Type> _types;

        static ResolvableTypeIdCollection()
            => _types = new Dictionary<string, Type>
            {
                //Compositions
                [GetSearchValueTraversal._typeId] = typeof(GetSearchValueTraversal),
                [GetStaticValue._typeId] = typeof(GetStaticValue),
                [GetValueTraversalDaysBetweenDates._typeId] = typeof(GetValueTraversalDaysBetweenDates),
                [IfConditionThenAElseBGetValueTraversal._typeId] = typeof(IfConditionThenAElseBGetValueTraversal),
                [GetNumberOfHits._typeId] = typeof(GetNumberOfHits),
                [GetListSearchValueTraversal._typeId] = typeof(GetListSearchValueTraversal),
                [GetConditionedListValueTraversal._typeId] = typeof(GetConditionedListValueTraversal),
                [GetConcatenatedByListValueTraversal._typeId] = typeof(GetConcatenatedByListValueTraversal),
                [GetConcatenatedValueTraversal._typeId] = typeof(GetConcatenatedValueTraversal),
                [GetMutatedValueTraversal._typeId] = typeof(GetMutatedValueTraversal),
                [SetMutatedValueTraversal._typeId] = typeof(SetMutatedValueTraversal),
                [GetAdditionalSourceValue._typeId] = typeof(GetAdditionalSourceValue),

                //Conditions
                [CompareCondition._typeId] = typeof(CompareCondition),
                [ListOfConditions._typeId] = typeof(ListOfConditions),
                [NotEmptyCondition._typeId] = typeof(NotEmptyCondition),

                //Configuration - Dictionary
                [DictionaryTargetInstantiator._typeId] = typeof(DictionaryTargetInstantiator),

                //Configuration - Json
                [JsonChildCreator._typeId] = typeof(JsonChildCreator),
                [JsonObjectConverter._typeId] = typeof(JsonObjectConverter),
                [JsonTargetInstantiator._typeId] = typeof(JsonTargetInstantiator),
                [JTokenToStringObjectConverter._typeId] = typeof(JTokenToStringObjectConverter),

                //Configuration - DataStructure
                [DataStructureChildCreator._typeId] = typeof(DataStructureChildCreator),
                [DataStructureObjectConverter._typeId] = typeof(DataStructureObjectConverter),
                [DataStructureTargetInstantiator._typeId] = typeof(DataStructureTargetInstantiator),
                [ObjectToJsonResultObjectConverter._typeId] = typeof(ObjectToJsonResultObjectConverter),
                [StringToDataStructureObjectConverter._typeId] = typeof(StringToDataStructureObjectConverter),

                //Configuration - Xml
                [XElementToStringObjectConverter._typeId] = typeof(XElementToStringObjectConverter),
                [XmlChildCreator._typeId] = typeof(XmlChildCreator),
                [XmlObjectConverter._typeId] = typeof(XmlObjectConverter),
                [XmlTargetInstantiator._typeId] = typeof(XmlTargetInstantiator),

                //Traversals - Dictionary
                [DictionarySetValueTraversal._typeId] = typeof(DictionarySetValueTraversal),

                //Traversals - Json
                [JsonGetListValueTraversal._typeId] = typeof(JsonGetListValueTraversal),
                [JsonGetTemplateTraversal._typeId] = typeof(JsonGetTemplateTraversal),
                [JsonGetValueTraversal._typeId] = typeof(JsonGetValueTraversal),
                [JsonSetValueTraversal._typeId] = typeof(JsonSetValueTraversal),

                //Traversals - DataStructure
                [DataStructureGetListValueTraversal._typeId] = typeof(DataStructureGetListValueTraversal),
                [DataStructureGetTemplateTraversal._typeId] = typeof(DataStructureGetTemplateTraversal),
                [DataStructureGetValueTraversal._typeId] = typeof(DataStructureGetValueTraversal),
                [DataStructureSetValueOnPathTraversal._typeId] = typeof(DataStructureSetValueOnPathTraversal),
                [DataStructureSetValueOnPropertyTraversal._typeId] = typeof(DataStructureSetValueOnPropertyTraversal),

                //Traversals - Xml
                [XmlGetListValueTraversal._typeId] = typeof(XmlGetListValueTraversal),
                [XmlGetTemplateTraversal._typeId] = typeof(XmlGetTemplateTraversal),
                [XmlGetThisValueTraversal._typeId] = typeof(XmlGetThisValueTraversal),
                [XmlGetValueTraversal._typeId] = typeof(XmlGetValueTraversal),
                [XmlSetGeneratedIdValueTraversal._typeId] = typeof(XmlSetGeneratedIdValueTraversal),
                [XmlSetThisValueTraversal._typeId] = typeof(XmlSetThisValueTraversal),
                [XmlSetValueTraversal._typeId] = typeof(XmlSetValueTraversal),

                //ValueMutations - Traversals
                [SplitByCharTakePositionStringTraversal._typeId] = typeof(SplitByCharTakePositionStringTraversal),

                //ValueMutations
                [CreateSeparatedRangeFromNumberValueMutation._typeId] = typeof(CreateSeparatedRangeFromNumberValueMutation),
                [DateValueMutation._typeId] = typeof(DateValueMutation),
                [DictionaryReplaceValueMutation._typeId] = typeof(DictionaryReplaceValueMutation),
                [ListOfValueMutations._typeId] = typeof(ListOfValueMutations),
                [NumberValueMutation._typeId] = typeof(NumberValueMutation),
                [PlaceholderValueMutation._typeId] = typeof(PlaceholderValueMutation),
                [ReplaceValueMutation._typeId] = typeof(ReplaceValueMutation),
                [SubstringValueMutation._typeId] = typeof(SubstringValueMutation),
                [ToLowerValueMutation._typeId] = typeof(ToLowerValueMutation),
                [ToUpperValueMutation._typeId] = typeof(ToUpperValueMutation),
                [TrimValueMutation._typeId] = typeof(TrimValueMutation),
                
                //Other
                [NullObject._typeId] = typeof(NullObject)
            };

        public static Type GetType(string typeId)
        {
            _types.TryGetValue(typeId, out Type result);

            if (result == null)
                throw new ArgumentException($"Invalid typeId: {typeId}");

            return result;
        }

        private static readonly Type SerializableByTypeIdType = typeof(ResolvableByTypeId);
        public static void AddType(Type type)
        {
            if (!SerializableByTypeIdType.IsAssignableFrom(type))
                throw new Exception($"Types registered need to inherit from {nameof(ResolvableByTypeId)}");

            ResolvableByTypeId temp = Activator.CreateInstance(type) as ResolvableByTypeId;

            _types.Add(temp?.TypeId ?? string.Empty, type);
        }
    }
}