﻿using System;
using System.Collections.Generic;
using AdaptableMapper.Compositions;
using AdaptableMapper.Conditions;
using AdaptableMapper.Configuration.Json;
using AdaptableMapper.Configuration.Model;
using AdaptableMapper.Configuration.Xml;
using AdaptableMapper.Traversals.Json;
using AdaptableMapper.Traversals.Model;
using AdaptableMapper.Traversals.Xml;
using AdaptableMapper.ValueMutations;
using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.Converters
{
    public static class ResolvableTypeIdCollection
    {
        private static readonly Dictionary<string, Type> _types;

        static ResolvableTypeIdCollection()
            => _types = new Dictionary<string, Type>
            {
                //Compositions
                [GetNothingValueTraversal._typeId] = typeof(GetNothingValueTraversal),
                [GetSearchValueTraversal._typeId] = typeof(GetSearchValueTraversal),
                [GetStaticValueTraversal._typeId] = typeof(GetStaticValueTraversal),
                [GetValueTraversalDaysBetweenDates._typeId] = typeof(GetValueTraversalDaysBetweenDates),
                [IfConditionThenAElseBGetValueTraversal._typeId] = typeof(IfConditionThenAElseBGetValueTraversal),
                [GetNumberOfHits._typeId] = typeof(GetNumberOfHits),

                //Conditions
                [CompareCondition._typeId] = typeof(CompareCondition),
                [ListOfConditions._typeId] = typeof(ListOfConditions),
                [NotEmptyCondition._typeId] = typeof(NotEmptyCondition),

                //Configuration - Json
                [JsonChildCreator._typeId] = typeof(JsonChildCreator),
                [JsonObjectConverter._typeId] = typeof(JsonObjectConverter),
                [JsonTargetInstantiator._typeId] = typeof(JsonTargetInstantiator),
                [JTokenToStringObjectConverter._typeId] = typeof(JTokenToStringObjectConverter),

                //Configuration - Model
                [ModelChildCreator._typeId] = typeof(ModelChildCreator),
                [ModelObjectConverter._typeId] = typeof(ModelObjectConverter),
                [ModelTargetInstantiator._typeId] = typeof(ModelTargetInstantiator),
                [ModelToStringObjectConverter._typeId] = typeof(ModelToStringObjectConverter),
                [StringToModelObjectConverter._typeId] = typeof(StringToModelObjectConverter),

                //Configuration - Xml
                [XElementToStringObjectConverter._typeId] = typeof(XElementToStringObjectConverter),
                [XmlChildCreator._typeId] = typeof(XmlChildCreator),
                [XmlObjectConverter._typeId] = typeof(XmlObjectConverter),
                [XmlTargetInstantiator._typeId] = typeof(XmlTargetInstantiator),

                //Traversals - Json
                [JsonGetListValueTraversal._typeId] = typeof(JsonGetListValueTraversal),
                [JsonGetTemplateTraversal._typeId] = typeof(JsonGetTemplateTraversal),
                [JsonGetValueTraversal._typeId] = typeof(JsonGetValueTraversal),
                [JsonSetValueTraversal._typeId] = typeof(JsonSetValueTraversal),

                //Traversals - Model
                [ModelGetListValueTraversal._typeId] = typeof(ModelGetListValueTraversal),
                [ModelGetTemplateTraversal._typeId] = typeof(ModelGetTemplateTraversal),
                [ModelGetValueTraversal._typeId] = typeof(ModelGetValueTraversal),
                [ModelSetValueOnPathTraversal._typeId] = typeof(ModelSetValueOnPathTraversal),
                [ModelSetValueOnPropertyTraversal._typeId] = typeof(ModelSetValueOnPropertyTraversal),

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
                [ToUpperValueMutation._typeId] = typeof(ToUpperValueMutation)
            };

        public static Type GetType(string typeId)
        {
            _types.TryGetValue(typeId, out Type result);

            if (result == null)
                throw new ArgumentException($"Invalid typeId: {typeId}");

            return result;
        }

        private static readonly Type _serializableByTypeIdType = typeof(ResolvableByTypeId);
        public static void AddType(Type type)
        {
            if (!_serializableByTypeIdType.IsAssignableFrom(type))
            {
                Process.ProcessObservable.GetInstance().Raise($"ResolvableTypeIdCollection#1; Type '{type.FullName}' is not assignable from '{_serializableByTypeIdType.Name}'", "error");
                return;
            }

            ResolvableByTypeId temp = Activator.CreateInstance(type) as ResolvableByTypeId;

            _types.Add(temp?.TypeId ?? string.Empty, type);
        }
    }
}