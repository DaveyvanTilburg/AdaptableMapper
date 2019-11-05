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

        [Given(@"I add a ModelObjectConverter to the contextFactory")]
        public void GivenIAddAModelObjectConverterToTheContextFactory()
        {
            _builder.AddModelObjectConverterToContextFactory();
        }

        [Given(@"I add a XmlTargetInitiator with an empty string to the contextFactory")]
        public void GivenIAddAXmlTargetInitiatorWithAnEmptyStringToTheContextFactory()
        {
            _builder.AddXmlTargetInitiatorToContextFactory(string.Empty);
        }

        [Given(@"I add a XElementToStringObjectConverter for mappingConfiguration")]
        public void GivenIAddAXElementToStringObjectConverterForMappingConfiguration()
        {
            _builder.AddXelementToStringObjectConverter();
        }

        [Given(@"I add a JsonObjectConverter to the contextFactory")]
        public void GivenIAddAJsonObjectConverterToTheContextFactory()
        {
            _builder.AddJsonObjectConverterToContextFactory();
        }

        [Given(@"I add a ModelTargetInitiator with an empty string to the contextFactory")]
        public void GivenIAddAModelTargetInitiatorWithAnEmptyStringToTheContextFactory()
        {
            _builder.ModelTargetInitiatorToContextFactory(string.Empty, string.Empty);
        }

        [Given(@"I add a ModelToStringObjectConverter for mappingConfiguration")]
        public void GivenIAddAModelToStringObjectConverterForMappingConfiguration()
        {
            _builder.AddModelToStringObjectConverter();
        }

        [Given(@"I add a XmlObjectConverter to the contextFactory")]
        public void GivenIAddAXmlObjectConverterToTheContextFactory()
        {
            _builder.AddXmlObjectConverterToContextFactory();
        }

        [Given(@"I add a JsonTargetInitiator with an empty string to the contextFactory")]
        public void GivenIAddAJsonTargetInitiatorWithAnEmptyStringToTheContextFactory()
        {
            _builder.AddJsonTargetInitiatorToContextFactory(string.Empty);
        }

        [Given(@"I add a JtokenToStringObjectConverter for mappingConfiguration")]
        public void GivenIAddAJtokenToStringObjectConverterForMappingConfiguration()
        {
            _builder.AddJTokenToStringObjectConverter();
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