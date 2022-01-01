using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Json;
using MappingFramework.Languages.DataStructure.Configuration;
using MappingFramework.Languages.DataStructure.Traversals;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
using MappingFramework.UnitTests.DataStructureExamples.Armies;
using Xunit;

namespace MappingFramework.UnitTests
{
    public class XmlToDataStructure
    {
        [Fact]
        public void XmlToDataStructureTest()
        {
            MappingConfiguration mappingConfiguration = GetMappingConfiguration(CreateDataStructureTargetInstantiatorSource());
            string targetInstantiatorSource = CreateDataStructureTargetInstantiatorSource();

            MapResult mapResult = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), targetInstantiatorSource);

            var result = mapResult.Result as Root;
            result.Should().NotBeNull();

            mapResult.Information.Count.Should().Be(2);

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
            MappingConfiguration mappingConfiguration = GetMappingConfiguration(CreateDataStructureTargetInstantiatorSource());
            mappingConfiguration.ResultObjectCreator = new ObjectToJsonResultObjectCreator();

            MapResult mapResult = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), string.Empty);
            
            var result = mapResult.Result as string;
            result.Should().NotBeNull();

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\ModelTarget_ArmyExpected.txt");
            result.Should().BeEquivalentTo(expectedResult);
        }

        private string CreateDataStructureTargetInstantiatorSource()
        {
            var rootType = typeof(Root);
            var result = new DataStructureTargetCreatorSource
            {
                AssemblyFullName = rootType.Assembly.FullName,
                TypeFullName = rootType.FullName
            };

            return JsonSerializer.Serialize(result);
        }

        public static MappingConfiguration GetMappingConfiguration(string source)
        {
            var crewMember = new Mapping(
                new XmlGetThisValueTraversal(),
                new DataStructureSetValueOnPropertyTraversal("Name")
            );

            var crewMemberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMember
                },
                new NullObject(),
                new XmlGetListValueTraversal("./crew/crewMember"),
                new DataStructureGetTemplateTraversal("CrewMembers"),
                new DataStructureChildCreator()
            );

            var memberName = new Mapping(
                new XmlGetValueTraversal("./@name"),
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
                new NullObject(),
                new XmlGetListValueTraversal("./members/member"),
                new DataStructureGetTemplateTraversal("Members"),
                new DataStructureChildCreator()
            );

            var platoonCode = new Mapping(
                new XmlGetValueTraversal("./@code"),
                new DataStructureSetValueOnPropertyTraversal("Code")
            );

            var leaderReference = new Mapping(
                new XmlGetValueTraversal("./leaderReference"),
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
                new NullObject(),
                new XmlGetListValueTraversal("./platoon"),
                new DataStructureGetTemplateTraversal("Platoons"),
                new DataStructureChildCreator()
            );

            var armyCode = new Mapping(
                new XmlGetValueTraversal("./@code"),
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
                new NullObject(),
                new XmlGetListValueTraversal("./army"),
                new DataStructureGetTemplateTraversal("Armies"),
                new DataStructureChildCreator()
            );

            var reference = new Mapping(
                new XmlGetValueTraversal("./@reference"),
                new DataStructureSetValueOnPropertyTraversal("Reference")
            );

            var leaderName = new Mapping(
                new XmlGetThisValueTraversal(),
                new DataStructureSetValueOnPathTraversal("LeaderPerson/Person/Name")
            );

            var leadersScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    reference,
                    leaderName
                },
                new NullObject(),
                new XmlGetListValueTraversal("./leaders/leader"),
                new DataStructureGetTemplateTraversal("Organization/Leaders"),
                new DataStructureChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                leadersScope,
                armyScope
            };

            var contextFactory = new ContextFactory(
                new XmlSourceCreator(),
                new DataStructureTargetCreator(source)
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObject());

            return mappingConfiguration;
        }
    }
}