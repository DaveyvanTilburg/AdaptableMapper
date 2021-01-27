using System;
using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Languages.DataStructure.Configuration;
using MappingFramework.Languages.DataStructure.Traversals;
using MappingFramework.Languages.Dictionary.Configuration;
using MappingFramework.Languages.Dictionary.Traversals;
using MappingFramework.Languages.Json.Configuration;
using MappingFramework.Languages.Json.Traversals;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
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
                [DictionaryTargetCreator._typeId] = typeof(DictionaryTargetCreator),

                //Configuration - Json
                [JsonChildCreator._typeId] = typeof(JsonChildCreator),
                [JsonSourceCreator._typeId] = typeof(JsonSourceCreator),
                [JsonTargetCreator._typeId] = typeof(JsonTargetCreator),
                [JTokenToStringResultObjectCreator._typeId] = typeof(JTokenToStringResultObjectCreator),

                //Configuration - DataStructure
                [DataStructureChildCreator._typeId] = typeof(DataStructureChildCreator),
                [DataStructureSourceCreator._typeId] = typeof(DataStructureSourceCreator),
                [DataStructureTargetCreator._typeId] = typeof(DataStructureTargetCreator),
                [ObjectToJsonResultObjectCreator._typeId] = typeof(ObjectToJsonResultObjectCreator),
                [StringToDataStructureSourceCreator._typeId] = typeof(StringToDataStructureSourceCreator),

                //Configuration - Xml
                [XElementToStringResultObjectCreator._typeId] = typeof(XElementToStringResultObjectCreator),
                [XmlChildCreator._typeId] = typeof(XmlChildCreator),
                [XmlSourceCreator._typeId] = typeof(XmlSourceCreator),
                [XmlTargetCreator._typeId] = typeof(XmlTargetCreator),

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