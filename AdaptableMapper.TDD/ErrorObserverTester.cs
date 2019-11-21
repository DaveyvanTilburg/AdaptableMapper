using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class ErrorObserverTester
    {
        [Fact]
        public void ErrorObserverTestMessages()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            mappingConfiguration.Map("", System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"));

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            errorObserver.GetRaisedWarnings().Count.Should().Be(1);
            errorObserver.GetRaisedErrors().Count.Should().Be(1);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var platoonCode = new Mapping(
                new Xml.XmlGetValue("./@code"),
                new Xml.XmlSetValue("./@code")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    platoonCode
                },
                new Xml.XmlGetScope("./army/platoon"),
                new Xml.XmlGetTemplate("./platoons/platoon"),
                new Xml.XmlChildCreator()
            );

            var stolenIntelScope = new MappingScopeRoot(
                new List<MappingScopeComposite>
                {
                    platoonScope
                }
            );

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Xml.XmlTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(stolenIntelScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}