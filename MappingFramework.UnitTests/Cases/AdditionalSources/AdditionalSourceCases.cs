using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Json.Configuration;
using MappingFramework.Languages.Json.Traversals;
using MappingFramework.ValueMutations;
using Xunit;

namespace MappingFramework.UnitTests.Cases.AdditionalSources
{
    public class AdditionalSourceCases
    {
        [Theory]
        [InlineData("Values", "Identifier", "0123", 0)]
        [InlineData("Valuesy", "Identifier", "", 1)]
        [InlineData("Valuesy", "Identifiery", "", 1)]
        [InlineData("Values", "Identifiery", "", 1)]
        [InlineData("", "", "", 1)]
        [InlineData("", "Identifier", "", 1)]
        [InlineData("Values", "", "", 1)]
        public void GetAdditionalSourceValue(string name, string key, string value, int informationCount)
        {
            var subject = new GetAdditionalSourceValue(
                new GetStaticValue(name),
                new GetStaticValue(key));

            var additionalSources = new List<AdditionalSource>
            {
                new TestAdditionalSource(),
                new TestExtraAdditionalSource()
            };

            var contextFactory = new ContextFactory(new JsonSourceCreator(), new JsonTargetCreator(), additionalSources);
            Context context = contextFactory.Create("{}", "{}");

            string result = subject.GetValue(context);
            result.Should().BeEquivalentTo(value);
            context.Information().Count.Should().Be(informationCount);
        }

        [Fact]
        public void GetAdditionalSourceValueDoubleRegistration()
        {
            var additionalSources = new List<AdditionalSource>
            {
                new TestAdditionalSource(),
                new TestAdditionalSource()
            };

            var contextFactory = new ContextFactory(new JsonSourceCreator(), new JsonTargetCreator(), additionalSources);
            var context = contextFactory.Create("{}", "{}");

            context.Information().Count.Should().Be(4);
        }

        [Fact]
        public void GetAdditionalSourceValueFullTest()
        {
            var mappingScopeComposite = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    new Mapping(
                        new JsonGetValueTraversal(".Name"),
                        new JsonSetValueTraversal(".Name")
                    ),
                    new Mapping(
                        new GetAdditionalSourceValue(
                            new GetStaticValue("Mapping"),
                    new GetMutatedValueTraversal(
                            new JsonGetValueTraversal(".Name"),
                            new PlaceholderValueMutation("{0}-Profession")
                            )
                        ),
                        new JsonSetValueTraversal(".Profession")
                    ),
                    new Mapping(
                        new GetAdditionalSourceValue(
                            new GetStaticValue("Mapping"),
                            new GetMutatedValueTraversal(
                                new JsonGetValueTraversal(".Name"),
                                new PlaceholderValueMutation("{0}-DOB")
                            )
                        ),
                        new JsonSetValueTraversal(".DOB")
                    )
                },
                new NullObject(),
                new JsonGetListValueTraversal(".People[*]"),
                new JsonGetTemplateTraversal(".People[0]"),
                new JsonChildCreator()
            );

            var mappings = new List<Mapping>
            {
                new Mapping(
                    new GetAdditionalSourceValue(
                        new GetStaticValue("Values"),
                        new GetStaticValue("Identifier")
                    ),
                    new JsonSetValueTraversal(".Id")
                )
            };

            var additionalSources = new List<AdditionalSource>
            {
                new TestAdditionalSource(),
                new TestExtraAdditionalSource()
            };

            var contextFactory = new ContextFactory(new JsonSourceCreator(), new JsonTargetCreator(), additionalSources);
            var mappingConfiguration = new MappingConfiguration(new List<MappingScopeComposite> { mappingScopeComposite }, mappings, contextFactory, new JTokenToStringResultObjectCreator());

            var mapResult = mappingConfiguration.Map(
                System.IO.File.ReadAllText("./Cases/AdditionalSources/AdditionalSourceValueFullSource.json"),
                System.IO.File.ReadAllText("./Cases/AdditionalSources/AdditionalSourceValueFullTarget.json")
            );

            mapResult.Information.Should().BeEmpty();

            string expectedResult = System.IO.File.ReadAllText("./Cases/AdditionalSources/AdditionalSourceValueFullExpectedResult.json");
            mapResult.Result.Should().BeEquivalentTo(expectedResult);
        }
    }
}