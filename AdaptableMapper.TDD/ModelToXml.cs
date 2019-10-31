using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using AdaptableMapper.Model.Language;
using ModelObjects.Armies;

namespace AdaptableMapper.TDD
{
    public class ModelToXml
    {
        [Fact]
        public void ModelToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Errors.ErrorObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetFakedMappingConfiguration();

            ModelBase source = GetSource();
            XElement result = mappingConfiguration.Map(source) as XElement;

            Errors.ErrorObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetErrors().Count.Should().Be(1);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private Root GetSource()
        {
            var leader1 = new Leader() { Name = "Christopher columbus", Reference = "alpha-bravo-tango-delta" };
            var leader2 = new Leader() { Name = "Ocean man", Reference = "Ween" };
            var leader3 = new Leader() { Name = "John J. Pershing", Reference = "Pershing" };

            Army army1 = CreateArmy1();
            Army army2 = CreateArmy2();

            var root = new Root()
            {
                Armies = new List<Army>()
                {
                    army1,
                    army2
                },
                Leaders = new List<Leader>()
                {
                    leader1,
                    leader2,
                    leader3
                }
            };
            army1.Parent = root;
            army2.Parent = root;
            leader1.Parent = root;
            leader2.Parent = root;
            leader3.Parent = root;

            return root;
        }

        private static Army CreateArmy1()
        {
            var army1platoon2member1crewMmeber1 = new CrewMember() { Name = "Natasha" };
            var army1platoon2member1crewMmeber2 = new CrewMember() { Name = "Yuri" };

            var army1platoon2member1 = new Member()
            {
                Name = "Sub-Zero",
                CrewMembers = new List<CrewMember>()
                {
                    army1platoon2member1crewMmeber1,
                    army1platoon2member1crewMmeber2
                }
            };
            army1platoon2member1crewMmeber1.Parent = army1platoon2member1;
            army1platoon2member1crewMmeber2.Parent = army1platoon2member1;


            var army1platoon2 = new Platoon()
            {
                Members = new List<Member>()
                {
                    army1platoon2member1
                },
                Code = "clean-floors",
                LeaderReference = "Ween"
            };
            army1platoon2member1.Parent = army1platoon2;

            var army1platoon1member1crewMmeber1 = new CrewMember() { Name = "John" };
            var army1platoon1member1crewMmeber2 = new CrewMember() { Name = "Jane" };

            var army1platoon1member1 = new Member()
            {
                Name = "FlagShip-Alpha",
                CrewMembers = new List<CrewMember>()
                {
                    army1platoon1member1crewMmeber1,
                    army1platoon1member1crewMmeber2
                }
            };
            army1platoon1member1crewMmeber1.Parent = army1platoon1member1;
            army1platoon1member1crewMmeber2.Parent = army1platoon1member1;


            var army1platoon1 = new Platoon()
            {
                Members = new List<Member>()
                {
                    army1platoon1member1
                },
                Code = "black-sky",
                LeaderReference = "alpha-bravo-tango-delta"
            };
            army1platoon1member1.Parent = army1platoon1;

            var army1 = new Army()
            {
                Code = "navel",
                Platoons = new List<Platoon>()
                {
                    army1platoon1,
                    army1platoon2
                }
            };
            army1platoon1.Parent = army1;
            army1platoon2.Parent = army1;
            return army1;
        }

        private static Army CreateArmy2()
        {
            var army1platoon2member1 = new Member()
            {
                Name = "Pharah",
                CrewMembers = new List<CrewMember>()
            };

            var army2platoon2 = new Platoon()
            {
                Members = new List<Member>()
                {
                    army1platoon2member1
                },
                Code = "air-soldier",
                LeaderReference = ""
            };
            army1platoon2member1.Parent = army2platoon2;

            var army2platoon1member2crewMmeber1 = new CrewMember() { Name = "John" };

            var army2platoon1member2 = new Member()
            {
                Name = "Boeing B-17",
                CrewMembers = new List<CrewMember>()
                {
                    army2platoon1member2crewMmeber1
                }
            };

            var army2platoon1member1crewMmeber1 = new CrewMember() { Name = "Hans" };

            var army2platoon1member1 = new Member()
            {
                Name = "Messerschmitt Bf 109",
                CrewMembers = new List<CrewMember>()
                {
                    army2platoon1member1crewMmeber1
                }
            };
            army2platoon1member1crewMmeber1.Parent = army2platoon1member1;


            var army2platoon1 = new Platoon()
            {
                Members = new List<Member>()
                {
                    army2platoon1member1,
                    army2platoon1member2
                },
                Code = "death-rains-from-above",
                LeaderReference = "Pershing"
            };
            army2platoon1member1.Parent = army2platoon1;

            var army2 = new Army()
            {
                Code = "thunder-struck",
                Platoons = new List<Platoon>()
                {
                    army2platoon1,
                    army2platoon2
                }
            };
            army2platoon1.Parent = army2;
            army2platoon2.Parent = army2;
            return army2;
        }

        private MappingConfiguration GetFakedMappingConfiguration()
        {
            var memberName = new Mapping(
                new Model.ModelGetValue("Name"),
                new Xml.XmlSetThisValue()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>()
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
                new List<MappingScopeComposite>()
                {
                    memberScope
                },
                new List<Mapping>()
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
                new List<Mapping>()
                {
                    crewMemberName,
                },
                new Model.ModelGetScope("Armies/Platoons/Members/CrewMembers"),
                new Xml.XmlTraversal("./crewMemberNames"),
                new Xml.XmlTraversalTemplate("./crewMemberName"),
                new Xml.XmlChildCreator()
            );

            var rootScope = new MappingScopeRoot(
                new List<MappingScopeComposite>()
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