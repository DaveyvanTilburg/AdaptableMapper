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
        
        [When(@"I run Map with a null parameter")]
        public void WhenIRunMapWithANullParameter()
        {
            TestErrorObserver testErrorObserver = new TestErrorObserver();
            ProcessObservable.GetInstance().Register(testErrorObserver);

            MappingConfiguration mappingConfiguration = _builder.GetResult();
            _result = mappingConfiguration.Map(null);

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
    }
}