using System;
using System.Collections.Generic;
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
        public void GenerateIdValueTraversal()
        {
            var subject = new GenerateIdValueTraversal { Number = 3 };

            string firstResult = subject.GetValue(null);
            firstResult.Should().Be("3");

            string secondResult = subject.GetValue(null);
            secondResult.Should().Be("4");

            string thirdResult = subject.GetValue(null);
            thirdResult.Should().Be("5");
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
                                new AdaptableMapper.Traversals.GenerateIdValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetValueTraversal("./Id")
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
                                new AdaptableMapper.Traversals.GenerateIdValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetValueTraversal("./Id")
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
                                new AdaptableMapper.Traversals.GenerateIdValueTraversal(),
                                new AdaptableMapper.Traversals.Xml.XmlSetValueTraversal("./Id")
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