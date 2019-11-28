using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Configuration;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class XmlToXml
    {
        [Fact]
        public void XmlToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            XElement result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml")) as XElement;

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetRaisedWarnings().Count.Should().Be(1);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        [Fact]
        public void XmlTestIfResultIsString()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            mappingConfiguration.ResultObjectConverter = new Xml.XElementToStringObjectConverter();

            object result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"));

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            XElement resultXElement = result as XElement;
            resultXElement.Should().BeNull();

            string resultString = result as string;
            resultString.Should().NotBeNull();
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var crewMemberName = new Mapping(
                new Xml.XmlGetThisValueTraversal(),
                new Xml.XmlSetThisValueTraversal()
            );

            var crewScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                new Xml.XmlGetScopeTraversal("./army/platoon/members/member/crew/crewMember"),
                new Xml.XmlGetTemplateTraversal("./crewMemberNames/crewMemberName"),
                new Xml.XmlChildCreator()
            );

            var memberName = new Mapping(
                new Xml.XmlGetValueTraversal("./@name"),
                new Xml.XmlSetThisValueTraversal()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                new Xml.XmlGetScopeTraversal("./members/member"),
                new Xml.XmlGetTemplateTraversal("./memberNames/memberName"),
                new Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new Xml.XmlGetValueTraversal("./@code"),
                new Xml.XmlSetValueTraversal("./@code")
            );

            var leaderNameSearch = new Mapping(
                new Xml.XmlGetSearchValueTraversal(
                    "../../leaders/leader[@reference='{{searchValue}}']",
                    "./leaderReference"
                ),
                new Xml.XmlSetValueTraversal("./leaderName")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    memberScope
                },
                new List<Mapping>
                {
                    platoonCode,
                    leaderNameSearch
                },
                new Xml.XmlGetScopeTraversal("./army/platoon"),
                new Xml.XmlGetTemplateTraversal("./platoons/platoon"),
                new Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                crewScope,
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new Xml.XmlObjectConverter(),
                new Xml.XmlTargetInstantiatorRemovesNamespace()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}