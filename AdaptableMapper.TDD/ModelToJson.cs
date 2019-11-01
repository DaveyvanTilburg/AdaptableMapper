using AdaptableMapper.Model.Language;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class ModelToJson
    {
        [Fact]
        public void ModelToJsonTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();

            ModelBase source = ArmyModelSourceCreator.CreateArmyModel();
            JToken result = mappingConfiguration.Map(source) as JToken;

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\JsonTarget_ArmyExpected.xml");
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
                    "../../Leaders{'PropertyName':'Reference','Value':'{{searchValue}}'}/Name",
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
                new Xml.XmlTargetInstantiator(System.IO.File.ReadAllText(@".\Resources\JsonTarget_ArmyTemplate.xml"))
            );

            var mappingConfiguration = new MappingConfiguration(rootScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}