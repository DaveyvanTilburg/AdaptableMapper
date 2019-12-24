using System;
using System.Collections.Generic;
using AdaptableMapper.Conditions;
using AdaptableMapper.Configuration;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Traversals
{
    public class TraversalsCases
    {
        [Theory]
        [InlineData("Valid", "value", "value")]
        [InlineData("Invalid", "", "", "e-GetStaticValueTraversal#1;")]
        public void GetStaticValueTraversal(string because, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new GetStaticValueTraversal(value);

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue((string)null); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Fact]
        public void GetNothingValueTraversal()
        {
            var subject = new GetNothingValueTraversal();

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(null); }).Observe();

            information.Count.Should().Be(0);

            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData("", "B", "C", "C")]
        [InlineData("A", "B", "C", "B")]
        public void IfConditionThenAElseBGetValueTraversal(string valueA, string valueB, string valueC, string expectedValue)
        {
            var subject = new IfConditionThenAElseBGetValueTraversal(new NotEmptyCondition(new GetStaticValueTraversal(valueA)), new GetStaticValueTraversal(valueB), new GetStaticValueTraversal(valueC));

            string result = subject.GetValue(new Context(null, null));
            result.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false, "e-IfConditionThenAElseBGetValueTraversal#3;")]
        [InlineData(true, false, true, "e-IfConditionThenAElseBGetValueTraversal#2;")]
        [InlineData(false, false, false, "e-IfConditionThenAElseBGetValueTraversal#1;", "e-IfConditionThenAElseBGetValueTraversal#2;", "e-IfConditionThenAElseBGetValueTraversal#3;")]
        public void IfConditionThenAElseBGetValueTraversalValidation(bool valueA, bool valueB, bool valueC, params string[] expectedErrors)
        {
            Condition condition = valueA ? new NotEmptyCondition(new GetStaticValueTraversal("A")) : null;
            GetValueTraversal getValueTraversalA = valueB ? new GetStaticValueTraversal("B") : null;
            GetValueTraversal getValueTraversalB = valueC ? new GetStaticValueTraversal("C") : null;

            var subject = new IfConditionThenAElseBGetValueTraversal(condition, getValueTraversalA, getValueTraversalB);

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.GetValue(null); }).Observe();
            information.ValidateResult(expectedErrors, "Validation");
            if (valueA == false || valueB == false || valueC == false)
                result.Should().BeEmpty();
            else
                result.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("RangeWithLastDate", "2019/01/01", "2019/01/04", true, "4")]
        [InlineData("RangeWithoutLastDate", "2019/01/01", "2019/01/04", false, "3")]
        [InlineData("invalid first path", "a", "2019/01/04", false, "", "w-GetValueTraversalDaysBetweenDates#1;")]
        [InlineData("invalid first path", "2019/01/01", "a", false, "", "w-GetValueTraversalDaysBetweenDates#2;")]
        public void GetValueTraversalDaysBetweenDates(string because, string firstDate, string lastDate, bool includeLastDay, string expectedResult, params string[] expectedCodes)
        {
            var subject = new GetValueTraversalDaysBetweenDates(new GetStaticValueTraversal(firstDate), new GetStaticValueTraversal(lastDate))
            {
                IncludeLastDay = includeLastDay
            };

            string value = string.Empty;
            List<Information> result = new Action(() => { value = subject.GetValue(new Context(null, null)); }).Observe();

            result.ValidateResult(new List<string>(expectedCodes), because);
            if (expectedCodes.Length == 0)
                value.Should().Be(expectedResult);
        }

        [Fact]
        public void GenerateIdValueTraversalMultipleScopesSameTemplate()
        {
            string source = System.IO.File.ReadAllText("./Resources/GenerateIdValueTraversalMultipleScopesSameTemplate/MultipleScopesSource.xml");
            string template = System.IO.File.ReadAllText("./Resources/GenerateIdValueTraversalMultipleScopesSameTemplate/CarTemplate.xml");
            string expectedResult = System.IO.File.ReadAllText("./Resources/GenerateIdValueTraversalMultipleScopesSameTemplate/CarExpactedResult.xml");

            var mappingConfiguration = new MappingConfiguration(
                new List<MappingScopeComposite>
                {
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new AdaptableMapper.Traversals.GetNothingValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new AdaptableMapper.Traversals.Xml.XmlGetThisValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new AdaptableMapper.Traversals.Xml.XmlGetScopeTraversal("./Wheels/Wheel"),
                        new AdaptableMapper.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new AdaptableMapper.Configuration.Xml.XmlChildCreator()
                    ),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new AdaptableMapper.Traversals.GetNothingValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new AdaptableMapper.Traversals.Xml.XmlGetThisValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new AdaptableMapper.Traversals.Xml.XmlGetScopeTraversal("./Doors/Door"),
                        new AdaptableMapper.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new AdaptableMapper.Configuration.Xml.XmlChildCreator()
                    ),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new AdaptableMapper.Traversals.GetNothingValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new AdaptableMapper.Traversals.Xml.XmlGetThisValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new AdaptableMapper.Traversals.Xml.XmlGetScopeTraversal("./Windows/Window"),
                        new AdaptableMapper.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new AdaptableMapper.Configuration.Xml.XmlChildCreator()
                    )
                },
                new ContextFactory(
                    new AdaptableMapper.Configuration.Xml.XmlObjectConverter(),
                    new AdaptableMapper.Configuration.Xml.XmlTargetInstantiator()
                ),
                new AdaptableMapper.Configuration.Xml.XElementToStringObjectConverter()
            );

            string result = mappingConfiguration.Map(source, template) as string;

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}