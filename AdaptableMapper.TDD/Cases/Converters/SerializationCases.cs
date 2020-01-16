using AdaptableMapper.Compositions;
using AdaptableMapper.Traversals;
using FluentAssertions;
using System;
using System.Collections.Generic;
using AdaptableMapper.Conditions;
using AdaptableMapper.Configuration;
using AdaptableMapper.Configuration.Json;
using AdaptableMapper.Configuration.Model;
using AdaptableMapper.Configuration.Xml;
using AdaptableMapper.Converters;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals.Json;
using AdaptableMapper.Traversals.Model;
using AdaptableMapper.Traversals.Xml;
using AdaptableMapper.ValueMutations;
using AdaptableMapper.ValueMutations.Traversals;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Converters
{
    public class SerializationCases
    {
        [Fact]
        public void ShouldNotBeAbleToAddIrrelevantTypes()
        {
            var subject = new {};
            List<Information> result = new Action(() => ResolvableTypeIdCollection.AddType(subject.GetType())).Observe();

            result.ValidateResult(new List<string> { "e-ResolvableTypeIdCollection#1;" }, "IrrelevantTypes");
        }

        [Fact]
        public void ShouldBeAbleToAddAndResolve()
        {
            List<Information> information = new Action(() => ResolvableTypeIdCollection.AddType(typeof(TestResolvableByTypeId))).Observe();
            Type result = ResolvableTypeIdCollection.GetType("1");

            information.Should().BeEmpty();
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
                new GetNothingValueTraversal()
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
                    new GetStaticValueTraversal("1"),
                    new GetStaticValueTraversal("2")
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetStaticValueTraversal("1")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new GetValueTraversalDaysBetweenDates(
                    new GetStaticValueTraversal("1"),
                    new GetStaticValueTraversal("2")
                )
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new IfConditionThenAElseBGetValueTraversal(
                    new CompareCondition(
                        new GetStaticValueTraversal("1"),
                        CompareOperator.Equals,
                        new GetStaticValueTraversal("2")
                    ),
                    new GetStaticValueTraversal("3"),
                    new GetStaticValueTraversal("4")
                )
            };
            
            //Conditions
            yield return new object[]
            {
                typeof(Condition),
                new CompareCondition(
                    new GetStaticValueTraversal("1"),
                    CompareOperator.Equals,
                    new GetStaticValueTraversal("2")
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
                            new GetStaticValueTraversal("1"),
                            CompareOperator.Equals,
                            new GetStaticValueTraversal("2")
                        ),
                        new CompareCondition(
                            new GetStaticValueTraversal("1"),
                            CompareOperator.Equals,
                            new GetStaticValueTraversal("2")
                        )
                    }
                )
            };
            yield return new object[]
            {
                typeof(Condition),
                new NotEmptyCondition(
                    new GetNothingValueTraversal()
                )
            };

            //Configuration - Json
            yield return new object[]
            {
                typeof(ChildCreator),
                new JsonChildCreator()
            };
            yield return new object[]
            {
                typeof(ObjectConverter),
                new JsonObjectConverter()
            };
            yield return new object[]
            {
                typeof(TargetInstantiator),
                new JsonTargetInstantiator()
            };
            yield return new object[]
            {
                typeof(ResultObjectConverter),
                new JTokenToStringObjectConverter()
            };

            //Configuration - Model
            yield return new object[]
            {
                typeof(ChildCreator),
                new ModelChildCreator()
            };
            yield return new object[]
            {
                typeof(ObjectConverter),
                new ModelObjectConverter()
            };
            yield return new object[]
            {
                typeof(TargetInstantiator),
                new ModelTargetInstantiator()
            };
            yield return new object[]
            {
                typeof(ResultObjectConverter),
                new ModelToStringObjectConverter()
            };
            yield return new object[]
            {
                typeof(ResultObjectConverter),
                new StringToModelObjectConverter()
            };

            //Configuration - Xml
            yield return new object[]
            {
                typeof(ChildCreator),
                new XmlChildCreator()
            };
            yield return new object[]
            {
                typeof(ObjectConverter),
                new XmlObjectConverter()
            };
            yield return new object[]
            {
                typeof(TargetInstantiator),
                new XmlTargetInstantiator()
            };
            yield return new object[]
            {
                typeof(ResultObjectConverter),
                new XElementToStringObjectConverter()
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
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new JsonSetThisValueTraversal()
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new JsonGetThisValueTraversal()
            };

            //Traversals - Model
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new ModelGetListValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new ModelGetTemplateTraversal("path")
            };
            yield return new object[]
            {
                typeof(GetValueTraversal),
                new ModelGetValueTraversal("path")
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new ModelSetValueOnPathTraversal("path")
            };
            yield return new object[]
            {
                typeof(SetValueTraversal),
                new ModelSetValueOnPropertyTraversal("path")
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
                new DateValueMutation("d")
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
                        new DateValueMutation("y")
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
                    new GetStaticValueTraversal("1")
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
        }
    }
}