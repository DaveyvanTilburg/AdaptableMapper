using FluentAssertions;
using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Traversals.DataStructure;
using Xunit;

namespace MappingFramework.TDD
{
    public class XmlToDataStructure
    {
        [Fact]
        public void XmlToDataStructureTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            string targetInstantiatorSource = CreateDataStructureTargetInstantiatorSource();

            object resultObject = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), targetInstantiatorSource);

            var result = resultObject as Armies.Root;
            result.Should().NotBeNull();

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            errorObserver.GetRaisedWarnings().Count.Should().Be(2);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Organization.Leaders.Count.Should().Be(3);
            result.Organization.Leaders[0].Reference.Should().Be("alpha-bravo-tango-delta");
            result.Organization.Leaders[2].LeaderPerson.Person.Name.Should().Be("John J. Pershing");

            result.Armies.Count.Should().Be(2);
            result.Armies[0].Code.Should().Be("naval");
            result.Armies[1].Platoons.Count.Should().Be(2);
            result.Armies[0].Platoons[0].Members.Count.Should().Be(1);
            result.Armies[0].Platoons[0].Members[0].Name.Should().Be("FlagShip-Alpha");
            result.Armies[1].Platoons[0].Members[1].CrewMembers[0].Name.Should().Be("John");
            result.Armies[1].Platoons[1].LeaderReference.Should().Be("");
            result.Armies[0].Platoons[1].LeaderReference.Should().Be("Ween");
        }

        [Fact]
        public void XmlToDataStructureToString()
        {
            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            mappingConfiguration.ResultObjectConverter = new Configuration.DataStructure.ObjectToJsonResultObjectConverter();
            string dataStructureTargetInstantiatorSource = CreateDataStructureTargetInstantiatorSource();

            object resultObject = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), dataStructureTargetInstantiatorSource);

            var result = resultObject as string;
            result.Should().NotBeNull();

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\ModelTarget_ArmyExpected.txt");
            result.Should().BeEquivalentTo(expectedResult);
        }

        private string CreateDataStructureTargetInstantiatorSource()
        {
            var rootType = typeof(Armies.Root);
            var result = new Configuration.DataStructure.DataStructureTargetInstantiatorSource
            {
                AssemblyFullName = rootType.Assembly.FullName,
                TypeFullName = rootType.FullName
            };

            return JsonSerializer.Serialize(result);
        }

        public static MappingConfiguration GetMappingConfiguration()
        {
            var crewMember = new Mapping(
                new Traversals.Xml.XmlGetThisValueTraversal(),
                new DataStructureSetValueOnPropertyTraversal("Name")
            );

            var crewMemberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMember
                },
                new Traversals.Xml.XmlGetListValueTraversal("./crew/crewMember"),
                new DataStructureGetTemplateTraversal("CrewMembers"),
                new Configuration.DataStructure.DataStructureChildCreator()
            );

            var memberName = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./@name"),
                new DataStructureSetValueOnPropertyTraversal("Name")
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    crewMemberScope
                },
                new List<Mapping>
                {
                    memberName
                },
                new Traversals.Xml.XmlGetListValueTraversal("./members/member"),
                new DataStructureGetTemplateTraversal("Members"),
                new Configuration.DataStructure.DataStructureChildCreator()
            );

            var platoonCode = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./@code"),
                new DataStructureSetValueOnPropertyTraversal("Code")
            );

            var leaderReference = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./leaderReference"),
                new DataStructureSetValueOnPropertyTraversal("LeaderReference")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    memberScope
                },
                new List<Mapping>
                {
                    platoonCode,
                    leaderReference
                },
                new Traversals.Xml.XmlGetListValueTraversal("./platoon"),
                new DataStructureGetTemplateTraversal("Platoons"),
                new Configuration.DataStructure.DataStructureChildCreator()
            );

            var armyCode = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./@code"),
                new DataStructureSetValueOnPropertyTraversal("Code")
            );

            var armyScope = new MappingScopeComposite(
                new List<MappingScopeComposite>
                {
                    platoonScope
                },
                new List<Mapping>
                {
                    armyCode
                },
                new Traversals.Xml.XmlGetListValueTraversal("./army"),
                new DataStructureGetTemplateTraversal("Armies"),
                new Configuration.DataStructure.DataStructureChildCreator()
            );

            var reference = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./@reference"),
                new DataStructureSetValueOnPropertyTraversal("Reference")
            );

            var leaderName = new Mapping(
                new Traversals.Xml.XmlGetThisValueTraversal(),
                new DataStructureSetValueOnPathTraversal("LeaderPerson/Person/Name")
            );

            var leadersScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    reference,
                    leaderName
                },
                new Traversals.Xml.XmlGetListValueTraversal("./leaders/leader"),
                new DataStructureGetTemplateTraversal("Organization/Leaders"),
                new Configuration.DataStructure.DataStructureChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                leadersScope,
                armyScope
            };

            var contextFactory = new ContextFactory(
                new Configuration.Xml.XmlObjectConverter(),
                new Configuration.DataStructure.DataStructureTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}