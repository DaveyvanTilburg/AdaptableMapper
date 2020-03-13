using System;
using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.Configuration.Json;
using MappingFramework.Process;
using MappingFramework.Traversals.Json;
using MappingFramework.ValueMutations;
using Xunit;

namespace MappingFramework.TDD.Cases.AdditionalSources
{
    public class AdditionalSourceCases
    {
        [Theory]
        [InlineData("Values", "Identifier", "0123")]
        [InlineData("Valuesy", "Identifier", "", "w-AdditionalSourceValues#2;")]
        [InlineData("Valuesy", "Identifiery", "", "w-AdditionalSourceValues#2;")]
        [InlineData("Values", "Identifiery", "", "w-AdditionalSourceValues#3;")]
        [InlineData("", "", "", "e-GetStaticValueTraversal#1;", "w-GetAdditionalSourceValue#3;")]
        [InlineData("", "Identifier", "", "e-GetStaticValueTraversal#1;", "w-GetAdditionalSourceValue#3;")]
        [InlineData("Values", "", "", "e-GetStaticValueTraversal#1;", "w-GetAdditionalSourceValue#4;")]
        public void GetAdditionalSourceValue(string name, string key, string value, params string[] expectedErrorCodes)
        {
            var subject = new GetAdditionalSourceValue(
                new GetStaticValue(name),
                new GetStaticValue(key));

            var additionalSources = new List<AdditionalSource>
            {
                new TestAdditionalSource(),
                new TestExtraAdditionalSource()
            };

            var contextFactory = new ContextFactory(new JsonObjectConverter(), new JsonTargetInstantiator(), additionalSources);
            Context context = contextFactory.Create("{}", "{}");

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.GetValue(context); }).Observe();
            information.ValidateResult(expectedErrorCodes);

            result.Should().BeEquivalentTo(value);
        }

        [Fact]
        public void GetAdditionalSourceValueNullChecks()
        {
            var subject = new GetAdditionalSourceValue(null, null);
            List<Information> information = new Action(() => { subject.GetValue(new Context(null, null, null)); }).Observe();

            information.ValidateResult(new List<string> { "e-GetAdditionalSourceValue#1;", "e-GetAdditionalSourceValue#2;" });
        }

        [Fact]
        public void GetAdditionalSourceValueDoubleRegistration()
        {
            var additionalSources = new List<AdditionalSource>
            {
                new TestAdditionalSource(),
                new TestAdditionalSource()
            };

            var contextFactory = new ContextFactory(new JsonObjectConverter(), new JsonTargetInstantiator(), additionalSources);
            List<Information> information = new Action(() => { contextFactory.Create("{}", "{}"); }).Observe();
            information.ValidateResult(new List<string> { "e-AdditionalSourceValues#1;" });
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

            var contextFactory = new ContextFactory(new JsonObjectConverter(), new JsonTargetInstantiator(), additionalSources);
            var mappingConfiguration = new MappingConfiguration(new List<MappingScopeComposite> { mappingScopeComposite }, mappings, contextFactory, new JTokenToStringObjectConverter());


            string result = string.Empty;
            List<Information> information = new Action(() => {
                result = mappingConfiguration.Map(
                    System.IO.File.ReadAllText("./Cases/AdditionalSources/AdditionalSourceValueFullSource.json"),
                    System.IO.File.ReadAllText("./Cases/AdditionalSources/AdditionalSourceValueFullTarget.json")) as string;
            }).Observe();
            information.Should().BeEmpty();

            string expectedResult = System.IO.File.ReadAllText("./Cases/AdditionalSources/AdditionalSourceValueFullExpectedResult.json");
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
