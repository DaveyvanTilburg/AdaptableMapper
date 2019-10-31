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
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            XElement result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml")) as XElement;

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetErrors().Count.Should().Be(2);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        [Fact]
        public void XmlTestIfResultIsString()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            mappingConfiguration.ResultConverter = new Xml.XElementToStringObjectConverter();

            object result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"));

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

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
                new List<Mapping>()
                {
                    crewMemberName
                },
                new Xml.XmlGetScope("./army/platoon/members/member/crew/crewMember"),
                new Xml.XmlTraversal("./crewMemberNames"),
                new Xml.XmlTraversalTemplate("./crewMemberName"),
                new Xml.XmlChildCreator()
            );

            var memberName = new Mapping(
                new Xml.XmlGetValue("./@name"),
                new Xml.XmlSetThisValue()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    memberName
                },
                new Xml.XmlGetScope("./members/member"),
                new Xml.XmlTraversal("./memberNames"),
                new Xml.XmlTraversalTemplate("./memberName"),
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
                new List<MappingScopeComposite>()
                {
                    memberScope
                },
                new List<Mapping>()
                {
                    platoonCode,
                    leaderNameSearch
                },
                new Xml.XmlGetScope("./army/platoon"),
                new Xml.XmlTraversal("./platoons"),
                new Xml.XmlTraversalTemplate("./platoon"),
                new Xml.XmlChildCreator()
            );

            var stolenIntelScope = new MappingScopeRoot(
                new List<MappingScopeComposite>()
                {
                    crewScope,
                    platoonScope
                }
            );

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"))
            );

            var mappingConfiguration = new MappingConfiguration(stolenIntelScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}