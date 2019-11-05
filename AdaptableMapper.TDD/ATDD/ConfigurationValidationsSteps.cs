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

        [Given(@"I add a NullObjectConverter for mappingConfiguration")]
        public void GivenIAddNullAObjectConverterForMappingConfiguration()
        {
            _builder.AddNullObjectConverterForMappingConfiguration();
        }

        [Given(@"I add a XElementToStringObjectConverter for mappingConfiguration")]
        public void GivenIAddAXElementToStringObjectConverterForMappingConfiguration()
        {
            _builder.AddXelementToStringObjectConverter();
        }

        [Given(@"I add a ModelToStringObjectConverter for mappingConfiguration")]
        public void GivenIAddAModelToStringObjectConverterForMappingConfiguration()
        {
            _builder.AddModelToStringObjectConverter();
        }

        [Given(@"I add a JtokenToStringObjectConverter for mappingConfiguration")]
        public void GivenIAddAJtokenToStringObjectConverterForMappingConfiguration()
        {
            _builder.AddJTokenToStringObjectConverter();
        }

        [Given(@"I add a Scope to the MappingScopeRoot list")]
        public void GivenIAddAScopeToTheMappingScopeRootList()
        {
            //_builder.AddScopeToMappingScopeRoot();
        }

        [Given(@"I add a '(.*)' ObjectConverter to the contextFactory")]
        public void GivenIAddAObjectConverterToTheContextFactory(string type)
        {
            _builder.AddObjectConverterToContextFactory(type);
        }

        [Given(@"I add a '(.*)' TargetInitiator with an empty string to the contextFactory")]
        public void GivenIAddATargetInitiatorWithAnEmptyStringToTheContextFactory(string type)
        {
            _builder.AddTargetInitiatorToContextFactory(type, string.Empty, string.Empty);
        }

        [When(@"I run Map with a null parameter")]
        public void WhenIRunMapWithANullParameter()
        {
            Map(null);
        }


        [When(@"I run Map with a string parameter '(.*)'")]
        public void WhenIRunMapWithAStringParameter(string p0)
        {
            Map(p0);
        }

        private void Map(object input)
        {
            TestErrorObserver testErrorObserver = new TestErrorObserver();
            ProcessObservable.GetInstance().Register(testErrorObserver);

            MappingConfiguration mappingConfiguration = _builder.GetResult();
            _result = mappingConfiguration.Map(input);

            ProcessObservable.GetInstance().Unregister(testErrorObserver);
            _information = testErrorObserver.GetInformation();
        }

        [Then(@"the result should contain the following errors")]
        public void ThenTheResultShouldContainTheFollowingErrors(Table table)
        {
            List<Information> expectedInformation = table.CreateSet<Information>().ToList();

            _information.Should().BeEquivalentTo(expectedInformation);
        }

        [Then(@"result should be null")]
        public void ThenResultShouldBeNull()
        {
            _result.Should().BeNull();
        }

        [Then(@"result should be a '(.*)'")]
        public void ThenResultShouldBeA(string p0)
        {
            _result.Should().Be(p0);
        }
    }
}