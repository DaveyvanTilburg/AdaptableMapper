using MappingFramework.Compositions;
using MappingFramework.Traversals;
using FluentAssertions;
using System;
using System.Collections.Generic;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Converters;
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
using Xunit;

namespace MappingFramework.TDD.Cases.Converters
{
    public class SerializationCases
    {
        [Fact]
        public void ShouldNotBeAbleToAddIrrelevantTypes()
        {
            var subject = new { };
            Action action = () => ResolvableTypeIdCollection.AddType(subject.GetType());
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void ShouldBeAbleToAddAndResolve()
        {
            ResolvableTypeIdCollection.AddType(typeof(TestResolvableByTypeId));
            Type result = ResolvableTypeIdCollection.GetType("1");

            result.Should().Be(typeof(TestResolvableByTypeId));
        }

        private class TestResolvableByTypeId : ResolvableByTypeId
        {
            public string TypeId => "1";
        }

        [Fact]
        public void JsonTypeIdBasedConverterShouldBeAbleToConvertSerializableByTypeId()
        {
            var subject = new JsonTypeIdBasedConverter();

            subject.CanConvert(typeof(ResolvableByTypeId)).Should().BeTrue();
            subject.CanConvert(typeof(int)).Should().BeFalse();
        }

        [Fact]
        public void CanWriteShouldThrowAnException()
        {
            var subject = new JsonTypeIdBasedConverter();
            Action subjectAction = () => subject.WriteJson(null, null, null);

            subjectAction.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void TypeIdsThatCannotBeFoundShouldThrowException()
        {
            string subject = @"
                {
                  ""TypeId"": ""InvalidTypeId"",
                  ""GetValueTraversalSearchPath"": {
                    ""TypeId"": ""136fe331-e3c2-496d-a7fc-e317b7eb80aa"",
                    ""Value"": ""1""
                  },
                  ""GetValueTraversalSearchValuePath"": {
                    ""TypeId"": ""136fe331-e3c2-496d-a7fc-e317b7eb80aa"",
                    ""Value"": ""2""
                  }
                }
            ";

            Action subjectAction = () => JsonSerializer.Deserialize(typeof(GetValueTraversal), subject);

            subjectAction.Should().Throw<ArgumentException>().WithMessage("Invalid typeId: InvalidTypeId");
        }

        [Theory]
        [MemberData(nameof(Scenarios), MemberType = typeof(SerializationCases))]
        public void SerializeTestForAllResolvableByTypeIdImplementations(Type interfaceType, object subject)
        {
            string serialized = JsonSerializer.Serialize(subject);
            object deserialized = JsonSerializer.Deserialize(interfaceType, serialized);

            deserialized.Should().BeEquivalentTo(subject);
        }

        public static IEnumerable<object[]> Scenarios()
        {
            //Compositions
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new NullObject()
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetNumberOfHits(
                    new List<GetListValueTraversal>
                    {
                        new XmlGetListValueTraversal("path1"),
                        new XmlGetListValueTraversal("path2")
                    }
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetSearchValueTraversal(
                    new GetStaticValue("1"),
                    new GetStaticValue("2")
                )
            };
            yield return new object[]
            {
                typeof(GetListValueTraversal),
                new GetListSearchValueTraversal(
                    new XmlGetListValueTraversal("path"),
                    new GetStaticValue("1")
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetStaticValue("1")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetValueTraversalDaysBetweenDates(
                    new GetStaticValue("1"),
                    new GetStaticValue("2")
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new IfConditionThenAElseBGetValueTraversal(
                    new CompareCondition(
                        new GetStaticValue("1"),
                        CompareOperator.Equals,
                        new GetStaticValue("2")
                    ),
                    new GetStaticValue("3"),
                    new GetStaticValue("4")
                )
            };
            yield return new object[]
            {
                typeof(GetListValueTraversal),
                new GetConditionedListValueTraversal(
                    new GetListSearchValueTraversal(
                        new XmlGetListValueTraversal("path"),
                        new GetStaticValue("1")
                    ),
                    new NotEmptyCondition()
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetConcatenatedByListValueTraversal(
                    new GetListSearchValueTraversal(
                        new XmlGetListValueTraversal("path"),
                        new GetStaticValue("1")
                    ),
                    new GetStaticValue("2"),
                    "-"
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetConcatenatedValueTraversal(
                    new List<GetValueTraversal>
                    {
                        new XmlGetValueTraversal("path"),
                        new XmlGetValueTraversal("path")
                    },
                    "-"
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetMutatedValueTraversal(
                    new GetStaticValue("test"),
                    new TrimValueMutation()
                )
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new SetMutatedValueTraversal(
                    new XmlSetValueTraversal("path"),
                    new TrimValueMutation()
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetAdditionalSourceValue(
                    new GetStaticValue("A"),
                    new GetStaticValue("B")
                )
            };

            //Conditions
            yield return new object[]
            {
                typeof(Condition),
                new CompareCondition(
                    new GetStaticValue("1"),
                    CompareOperator.Equals,
                    new GetStaticValue("2")
                )
            };
            yield return new object[]
            {
                typeof(Condition),
                new ListOfConditions(
                    ListEvaluationOperator.Any,
                    new List<Condition>
                    {
                        new CompareCondition(
                            new GetStaticValue("1"),
                            CompareOperator.Equals,
                            new GetStaticValue("2")
                        ),
                        new CompareCondition(
                            new GetStaticValue("1"),
                            CompareOperator.Equals,
                            new GetStaticValue("2")
                        )
                    }
                )
            };
            yield return new object[]
            {
                typeof(Condition),
                new NotEmptyCondition(
                    new NullObject()
                )
            };

            //Configuration - Dictionary
            yield return new object[]
            {
                typeof(TargetCreator),
                new DictionaryTargetCreator()
            };

            //Configuration - Json
            yield return new object[]
            {
                typeof(ChildCreator),
                new JsonChildCreator()
            };
            yield return new object[]
            {
                typeof(SourceCreator),
                new JsonSourceCreator()
            };
            yield return new object[]
            {
                typeof(TargetCreator),
                new JsonTargetCreator()
            };
            yield return new object[]
            {
                typeof(ResultObjectCreator),
                new JTokenToStringResultObjectCreator()
            };

            //Configuration - DataStructure
            yield return new object[]
            {
                typeof(ChildCreator),
                new DataStructureChildCreator()
            };
            yield return new object[]
            {
                typeof(SourceCreator),
                new DataStructureSourceCreator()
            };
            yield return new object[]
            {
                typeof(TargetCreator),
                new DataStructureTargetCreator()
            };
            yield return new object[]
            {
                typeof(ResultObjectCreator),
                new ObjectToJsonResultObjectCreator()
            };
            yield return new object[]
            {
                typeof(ResultObjectCreator),
                new StringToDataStructureSourceCreator()
            };

            //Configuration - Xml
            yield return new object[]
            {
                typeof(ChildCreator),
                new XmlChildCreator()
            };
            yield return new object[]
            {
                typeof(SourceCreator),
                new XmlSourceCreator()
            };
            yield return new object[]
            {
                typeof(TargetCreator),
                new XmlTargetCreator()
            };
            yield return new object[]
            {
                typeof(ResultObjectCreator),
                new XElementToStringResultObjectCreator()
            };

            //Traversal - Dictionary
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new DictionarySetValueTraversal()
            };

            //Traversal - Json
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new JsonGetListValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new JsonGetTemplateTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new JsonGetValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new JsonSetValueTraversal("path")
            };

            //Traversals - DataStructure
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new DataStructureGetListValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new DataStructureGetTemplateTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new DataStructureGetValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new DataStructureSetValueOnPathTraversal("path")
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new DataStructureSetValueOnPropertyTraversal("path")
            };

            //Traversals - Xml
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new XmlGetListValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new XmlGetTemplateTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new XmlGetThisValueTraversal()
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new XmlGetValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new XmlSetGeneratedIdValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new XmlSetThisValueTraversal()
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new XmlSetValueTraversal("path")
            };

            //ValueMutations - GetValueStringTraversal
            yield return new object[]
            {
                typeof(GetValueStringTraversal),
                new SplitByCharTakePositionStringTraversal('c', 1)
            };

            //ValueMutations
            yield return new object[]
            {
                typeof(ValueMutation),
                new CreateSeparatedRangeFromNumberValueMutation(",")
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new DateValueMutation { FormatTemplate = "d"}
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new DictionaryReplaceValueMutation(
                    new List<DictionaryReplaceValueMutation.ReplaceValue>
                    {
                        new DictionaryReplaceValueMutation.ReplaceValue("x", "y")
                    }
                )
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new ListOfValueMutations(
                    new List<ValueMutation>
                    {
                        new NumberValueMutation(",", 2),
                        new DateValueMutation { FormatTemplate = "d"}
                    }
                )
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new NumberValueMutation(",", 2),
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new PlaceholderValueMutation("")
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new ReplaceValueMutation(
                    new SplitByCharTakePositionStringTraversal('c', 1),
                    new GetStaticValue("1")
                )
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new SubstringValueMutation(new SplitByCharTakePositionStringTraversal('c', 1))
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new ToLowerValueMutation()
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new ToUpperValueMutation()
            };
            yield return new object[]
            {
                typeof(ValueMutation),
                new TrimValueMutation()
            };
        }
    }
}