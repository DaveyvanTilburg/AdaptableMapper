using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class XmlToModel
    {
        [Fact]
        public void XmlToModelTest()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            object resultObject = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"));

            var result = resultObject as ModelObjects.Armies.Root;

            Errors.ProcessObservable.GetInstance().Unregister(errorObserver);

            errorObserver.GetRaisedWarnings().Count.Should().Be(2);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Leaders.Count.Should().Be(3);
            result.Leaders[0].Reference.Should().Be("alpha-bravo-tango-delta");
            result.Leaders[2].Name.Should().Be("John J. Pershing");

            result.Armies.Count.Should().Be(2);
            result.Armies[0].Code.Should().Be("naval");
            result.Armies[1].Platoons.Count.Should().Be(2);
            result.Armies[0].Platoons[0].Members.Count.Should().Be(1);
            result.Armies[0].Platoons[0].Members[0].Name.Should().Be("FlagShip-Alpha");
            result.Armies[1].Platoons[0].Members[1].CrewMembers[0].Name.Should().Be("John");
            result.Armies[1].Platoons[1].LeaderReference.Should().Be("");
            result.Armies[0].Platoons[1].LeaderReference.Should().Be("Ween");
        }

        public static MappingConfiguration GetMappingConfiguration()
        {
            var crewMember = new Mapping(
                new Xml.XmlGetThisValue(),
                new Model.ModelSetValueOnProperty("Name")
            );

            var crewMemberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    crewMember
                },
                new Xml.XmlGetScope("./crew/crewMember"),
                new Model.ModelTraversalThis(),
                new Model.ModelTraversalTemplate("CrewMembers"),
                new Model.ModelChildCreator()
            );

            var memberName = new Mapping(
                new Xml.XmlGetValue("./@name"),
                new Model.ModelSetValueOnProperty("Name")
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>()
                {
                    crewMemberScope
                },
                new List<Mapping>()
                {
                    memberName
                },
                new Xml.XmlGetScope("./members/member"),
                new Model.ModelTraversalThis(),
                new Model.ModelTraversalTemplate("Members"),
                new Model.ModelChildCreator()
            );

            var platoonCode = new Mapping(
                new Xml.XmlGetValue("./@code"),
                new Model.ModelSetValueOnProperty("Code")
            );

            var leaderReference = new Mapping(
                new Xml.XmlGetValue("./leaderReference"),
                new Model.ModelSetValueOnProperty("LeaderReference")
            );

            var platoonScope = new MappingScopeComposite(
                new List<MappingScopeComposite>()
                {
                    memberScope
                },
                new List<Mapping>()
                {
                    platoonCode,
                    leaderReference
                },
                new Xml.XmlGetScope("./platoon"),
                new Model.ModelTraversalThis(),
                new Model.ModelTraversalTemplate("Platoons"),
                new Model.ModelChildCreator()
            );

            var armyCode = new Mapping(
                new Xml.XmlGetValue("./@code"),
                new Model.ModelSetValueOnProperty("Code")
            );

            var armyScope = new MappingScopeComposite(
                new List<MappingScopeComposite>()
                {
                    platoonScope
                },
                new List<Mapping>()
                {
                    armyCode
                },
                new Xml.XmlGetScope("./army"),
                new Model.ModelTraversalThis(),
                new Model.ModelTraversalTemplate("Armies"),
                new Model.ModelChildCreator()
            );

            var reference = new Mapping(
                new Xml.XmlGetValue("./@reference"),
                new Model.ModelSetValueOnProperty("Reference")
            );

            var leaderName = new Mapping(
                new Xml.XmlGetThisValue(),
                new Model.ModelSetValueOnProperty("Name")
            );

            var leadersScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
                {
                    reference,
                    leaderName
                },
                new Xml.XmlGetScope("./leaders/leader"),
                new Model.ModelTraversalThis(),
                new Model.ModelTraversalTemplate("Leaders"),
                new Model.ModelChildCreator()
            );

            var rootScope = new MappingScopeRoot(
                new List<MappingScopeComposite>()
                {
                    leadersScope,
                    armyScope
                }
            );

            var rootType = typeof(ModelObjects.Armies.Root);

            var contextFactory = new Contexts.ContextFactory(
                new Xml.XmlObjectConverter(),
                new Model.ModelTargetInstantiator(rootType.Assembly.FullName, rootType.FullName)
            );

            var mappingConfiguration = new MappingConfiguration(rootScope, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}