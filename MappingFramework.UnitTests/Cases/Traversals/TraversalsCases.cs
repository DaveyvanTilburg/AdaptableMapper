using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
using Xunit;

namespace MappingFramework.UnitTests.Cases.Traversals
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
                    new(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new(
                                new NullObject(),
                                new XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new(
                                new XmlGetThisValueTraversal(),
                                new XmlSetValueTraversal("./Text")
                            )
                        },
                        new NullObject(),
                        new XmlGetListValueTraversal("./Wheels/Wheel"),
                        new XmlGetTemplateTraversal("./Parts/Part"),
                        new XmlChildCreator()
                    ),
                    new(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new(
                                new NullObject(),
                                new XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new(
                                new XmlGetThisValueTraversal(),
                                new XmlSetValueTraversal("./Text")
                            )
                        },
                        new NullObject(),
                        new XmlGetListValueTraversal("./Doors/Door"),
                        new XmlGetTemplateTraversal("./Parts/Part"),
                        new XmlChildCreator()
                    ),
                    new(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new(
                                new NullObject(),
                                new XmlSetGeneratedIdValueTraversal("./Id") { StartingNumber = 1}
                            ),
                            new(
                                new XmlGetThisValueTraversal(),
                                new XmlSetValueTraversal("./Text")
                            )
                        },
                        new NullObject(),
                        new XmlGetListValueTraversal("./Windows/Window"),
                        new XmlGetTemplateTraversal("./Parts/Part"),
                        new XmlChildCreator()
                    )
                },
                new ContextFactory(
                    new XmlSourceCreator(),
                    new XmlTargetCreator()
                ),
                new XElementToStringResultObjectCreator()
            );

            MapResult mapResult = mappingConfiguration.Map(source, template);
            string result = mapResult.Result as string;

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}