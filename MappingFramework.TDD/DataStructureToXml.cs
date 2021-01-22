using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.DataStructure;
using MappingFramework.Traversals.DataStructure;
using Xunit;

namespace MappingFramework.TDD
{
    public class DataStructureToXml
    {
        [Fact]
        public void DataStructureToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();

            TraversableDataStructure source = ArmySourceCreator.CreateArmy();
            XElement result = mappingConfiguration.Map(source, System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml")) as XElement;

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetRaisedWarnings().Count.Should().Be(0);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private static MappingConfiguration GetFakedMappingConfiguration()
        {
            var memberName = new Mapping(
                new DataStructureGetValueTraversal("Name"),
                new Traversals.Xml.XmlSetThisValueTraversal()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                null,
                new DataStructureGetListValueTraversal("Members"),
                new Traversals.Xml.XmlGetTemplateTraversal("./memberNames/memberName"),
                new Configuration.Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new DataStructureGetValueTraversal("Code"),
                new Traversals.Xml.XmlSetValueTraversal("./@code")
            );

            var leaderName = new Mapping(
                new Compositions.GetSearchValueTraversal(
                    new DataStructureGetValueTraversal("../../Organization/Leaders{'PropertyName':'Reference','Value':'{{searchValue}}'}/LeaderPerson/Person/Name"),
                    new DataStructureGetValueTraversal("LeaderReference")
                ),
                new Traversals.Xml.XmlSetValueTraversal("./leaderName")
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
                null,
                new DataStructureGetListValueTraversal("Armies/Platoons"),
                new Traversals.Xml.XmlGetTemplateTraversal("./platoons/platoon"),
                new Configuration.Xml.XmlChildCreator()
            )
            {
                Condition = new CompareCondition(
                    new DataStructureGetValueTraversal("Deployed"), 
                    CompareOperator.Equals, 
                    new Compositions.GetStaticValue("True"))
            };

            var crewMemberName = new Mapping(
                new DataStructureGetValueTraversal("Name"),
                new Traversals.Xml.XmlSetThisValueTraversal()
            );

            var crewMemberNamesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                null,
                new DataStructureGetListValueTraversal("Armies/Platoons/Members/CrewMembers"),
                new Traversals.Xml.XmlGetTemplateTraversal("./crewMemberNames/crewMemberName"),
                new Configuration.Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                crewMemberNamesScope,
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new Configuration.DataStructure.DataStructureObjectConverter(),
                new Configuration.Xml.XmlTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}