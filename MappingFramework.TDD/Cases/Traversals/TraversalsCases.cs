using System;
using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.Process;
using FluentAssertions;
using Xunit;

namespace MappingFramework.TDD.Cases.Traversals
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
                                new MappingFramework.Compositions.GetNothingValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new MappingFramework.Traversals.Xml.XmlGetThisValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new MappingFramework.Traversals.Xml.XmlGetListValueTraversal("./Wheels/Wheel"),
                        new MappingFramework.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new MappingFramework.Configuration.Xml.XmlChildCreator()
                    ),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new MappingFramework.Compositions.GetNothingValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new MappingFramework.Traversals.Xml.XmlGetThisValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new MappingFramework.Traversals.Xml.XmlGetListValueTraversal("./Doors/Door"),
                        new MappingFramework.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new MappingFramework.Configuration.Xml.XmlChildCreator()
                    ),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new MappingFramework.Compositions.GetNothingValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new MappingFramework.Traversals.Xml.XmlGetThisValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new MappingFramework.Traversals.Xml.XmlGetListValueTraversal("./Windows/Window"),
                        new MappingFramework.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new MappingFramework.Configuration.Xml.XmlChildCreator()
                    )
                },
                new ContextFactory(
                    new MappingFramework.Configuration.Xml.XmlObjectConverter(),
                    new MappingFramework.Configuration.Xml.XmlTargetInstantiator()
                ),
                new MappingFramework.Configuration.Xml.XElementToStringObjectConverter()
            );

            string result = mappingConfiguration.Map(source, template) as string;

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}