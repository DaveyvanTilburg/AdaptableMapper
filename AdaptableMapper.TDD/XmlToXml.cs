using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
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
                new Xml.XmlGetThisValue(),
                new Xml.XmlSetThisValue()
            );

            var crewScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                new Xml.XmlGetScope("./army/platoon/members/member/crew/crewMember"),
                new Xml.XmlGetTemplate("./crewMemberNames/crewMemberName"),
                new Xml.XmlChildCreator()
            );

            var memberName = new Mapping(
                new Xml.XmlGetValue("./@name"),
                new Xml.XmlSetThisValue()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                new Xml.XmlGetScope("./members/member"),
                new Xml.XmlGetTemplate("./memberNames/memberName"),
                new Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new Xml.XmlGetValue("./@code"),
                new Xml.XmlSetValue("./@code")
            );

            var leaderNameSearch = new Mapping(
                new Xml.XmlGetSearchValue(
                    "../../leaders/leader[@reference='{{searchValue}}']",
                    "./leaderReference"
                ),
                new Xml.XmlSetValue("./leaderName")
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
                new Xml.XmlGetScope("./army/platoon"),
                new Xml.XmlGetTemplate("./platoons/platoon"),
                new Xml.XmlChildCreator()
            );

            var stolenIntelScope = new MappingScopeRoot(
                new List<MappingScopeComposite>
                {
                    crewScope,
                    platoonScope
                }
            );

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Xml.XmlTargetInstantiatorRemovesNamespace()
            );

            var mappingConfiguration = new MappingConfiguration(contextFactory, new NullObjectConverter());
            mappingConfiguration.MappingScope = stolenIntelScope;

            return mappingConfiguration;
        }
    }
}