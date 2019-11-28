using FluentAssertions;
using System.Collections.Generic;
using AdaptableMapper.Configuration;
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
                new Xml.XmlGetValueTraversal("./@code"),
                new Xml.XmlSetValueTraversal("./@code")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    platoonCode
                },
                new Xml.XmlGetScopeTraversal("./army/platoon"),
                new Xml.XmlGetTemplateTraversal("./platoons/platoon"),
                new Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new Xml.XmlObjectConverterRemovesNamespace(),
                new Xml.XmlTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}