using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using FluentAssertions;
using Xunit;

namespace MappingFramework.TDD.Cases.Traversals
{
    public class TraversalsCases
    {
        [Theory]
        [InlineData("Valid", "value", "value", 0)]
        [InlineData("EmptyIsValid", "", "", 0)]
        public void GetStaticValueTraversal(string because, string value, string expectedResult, int informationCount)
        {
            var subject = new GetStaticValue(value);
            var context = new Context();

            var result = subject.GetValue(context, null);

            context.Information().Count.Should().Be(informationCount);
            
            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
        }

        [Fact]
        public void GetNothingValueTraversal()
        {
            var subject = new NullObject();
            var context = new Context();
            
            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(0);
            result.Should().Be(string.Empty);
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
                                new NullObject(),
                                new MappingFramework.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new MappingFramework.Traversals.Xml.XmlGetThisValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new NullObject(),
                        new MappingFramework.Traversals.Xml.XmlGetListValueTraversal("./Wheels/Wheel"),
                        new MappingFramework.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new MappingFramework.Configuration.Xml.XmlChildCreator()
                    ),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new NullObject(),
                                new MappingFramework.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new MappingFramework.Traversals.Xml.XmlGetThisValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new NullObject(),
                        new MappingFramework.Traversals.Xml.XmlGetListValueTraversal("./Doors/Door"),
                        new MappingFramework.Traversals.Xml.XmlGetTemplateTraversal("./Parts/Part"),
                        new MappingFramework.Configuration.Xml.XmlChildCreator()
                    ),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new NullObject(),
                                new MappingFramework.Traversals.Xml.XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new Mapping(
                                new MappingFramework.Traversals.Xml.XmlGetThisValueTraversal(),
                                new MappingFramework.Traversals.Xml.XmlSetValueTraversal("./Text")
                            )
                        },
                        new NullObject(),
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

            MapResult mapResult = mappingConfiguration.Map(source, template);
            string result = mapResult.Result as string;

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}