using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using AdaptableMapper.Model.Language;

namespace AdaptableMapper.TDD
{
    public class ModelToXml
    {
        [Fact]
        public void ModelToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();

            ModelBase source = ArmyModelSourceCreator.CreateArmyModel();
            XElement result = mappingConfiguration.Map(source) as XElement;

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetRaisedWarnings().Count.Should().Be(1);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private static MappingConfiguration GetFakedMappingConfiguration()
        {
            var memberName = new Mapping(
                new Model.ModelGetValue("Name"),
                new Xml.XmlSetThisValue()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                new Model.ModelGetScope("Members"),
                new Xml.XmlTraversal("./memberNames"),
                new Xml.XmlTraversalTemplate("./memberName"),
                new Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new Model.ModelGetValue("Code"),
                new Xml.XmlSetValue("./@code")
            );

            var leaderName = new Mapping(
                new Model.ModelGetSearchValue(
                    "../../Organization/Leaders{'PropertyName':'Reference','Value':'{{searchValue}}'}/LeaderPerson/Name",
                    "LeaderReference"
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
                    leaderName
                },
                new Model.ModelGetScope("Armies/Platoons"),
                new Xml.XmlTraversal("./platoons"),
                new Xml.XmlTraversalTemplate("./platoon"),
                new Xml.XmlChildCreator()
            );

            var crewMemberName = new Mapping(
                new Model.ModelGetValue("Name"),
                new Xml.XmlSetThisValue()
            );

            var crewMemberNamesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                new Model.ModelGetScope("Armies/Platoons/Members/CrewMembers"),
                new Xml.XmlTraversal("./crewMemberNames"),
                new Xml.XmlTraversalTemplate("./crewMemberName"),
                new Xml.XmlChildCreator()
            );

            var rootScope = new MappingScopeRoot(
                new List<MappingScopeComposite>
                {
                    crewMemberNamesScope,
                    platoonScope
                }
            );

            var contextFactory = new Contexts.ContextFactory(
                new Model.ModelObjectConverter(),
                new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"))
            );

            var mappingConfiguration = new MappingConfiguration(rootScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}