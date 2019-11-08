using AdaptableMapper.Process;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AdaptableMapper.TDD.ATDD
{
    [Binding]
    public class ConfigurationValidationsSteps
    {
        private MappingConfigurationBuilder _builder;
        private List<Information> _information;
        private object _result;

        [Given(@"I create a mappingconfiguration")]
        public void GivenICreateAMappingconfiguration()
        {
            _builder = new MappingConfigurationBuilder();
            _builder.StartNew();
        }

        [Given(@"I add a contextFactory")]
        public void GivenIAddAContextFactory()
        {
            _builder.AddContextFactory();
        }

        [Given(@"I add a MappingScopeRoot with an empty list")]
        public void GivenIAddAMappingScopeRootWithAnEmptyList()
        {
            _builder.AddMappingScopeRoot();
        }

        [Given(@"I add a '(.*)' ObjectConverter for mappingConfiguration")]
        public void GivenIAddAObjectConverterForMappingConfiguration(string type)
        {
            _builder.AddObjectConverter(type);
        }

        [Given(@"I add a '(.*)' ObjectConverter to the contextFactory")]
        public void GivenIAddAObjectConverterToTheContextFactory(string type)
        {
            _builder.AddObjectConverterToContextFactory(type);
        }

        [Given(@"I add a '(.*)' TargetInitiator to the contextFactory")]
        public void GivenIAddATargetInitiatorToTheContextFactory(string type)
        {
            _builder.AddTargetInitiatorToContextFactory(type);
        }

        [Given(@"I add a Scope to the root")]
        public void GivenIAddAScopeToTheRoot(Table table)
        {
            var scopeCompositeModel = table.CreateInstance<ScopeCompositeModel>();

            _builder.AddScopeToRoot(scopeCompositeModel);
        }

        [Given(@"I add a mapping to the scope")]
        public void GivenIAddAMappingToTheScope(Table table)
        {
            var mapping = table.CreateInstance<MappingModel>();

            _builder.AddMappingToLastScope(mapping);
        }

        [When(@"I run Map with a null parameter")]
        public void WhenIRunMapWithANullParameter()
        {
            Map(null, null);
        }


        [When(@"I run Map with a string parameter '(.*)'")]
        public void WhenIRunMapWithAStringParameter(string p0)
        {
            Map(p0, null);
        }

        private void Map(object input, object targetSource)
        {
            TestErrorObserver testErrorObserver = new TestErrorObserver();
            ProcessObservable.GetInstance().Register(testErrorObserver);

            MappingConfiguration mappingConfiguration = _builder.GetResult();
            _result = mappingConfiguration.Map(input, targetSource);

            ProcessObservable.GetInstance().Unregister(testErrorObserver);
            _information = testErrorObserver.GetInformation();
        }

        [Then(@"the result should contain the following errors")]
        public void ThenTheResultShouldContainTheFollowingErrors(Table table)
        {
            InformationCodesModel expectedInformation = table.CreateInstance<InformationCodesModel>();

            _information.Count.Should().Be(expectedInformation.InformationCodes.Count);

            foreach(string code in expectedInformation.InformationCodes)
                _information.Any(i => i.Message.Contains(code)).Should().BeTrue(code);
        }

        [Then(@"result should be null")]
        public void ThenResultShouldBeNull()
        {
            _result.Should().BeNull();
        }

        [Then(@"result should be '(.*)'")]
        public void ThenResultShouldBe(string p0)
        {
            _result.Should().Be(p0);
        }
    }
}