using FluentAssertions;
using System.Collections.Generic;
using MappingFramework.Configuration;
using Xunit;

namespace MappingFramework.TDD
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
                new Traversals.Xml.XmlGetValueTraversal("./@code"),
                new Traversals.Xml.XmlSetValueTraversal("./@code")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    platoonCode
                },
                new Traversals.Xml.XmlGetListValueTraversal("./army/platoon"),
                new Traversals.Xml.XmlGetTemplateTraversal("./platoons/platoon"),
                new Configuration.Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new Configuration.Xml.XmlObjectConverter(),
                new Configuration.Xml.XmlTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}