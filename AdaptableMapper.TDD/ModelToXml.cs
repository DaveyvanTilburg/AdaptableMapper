using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Configuration;
using Xunit;
using AdaptableMapper.Model;

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
            XElement result = mappingConfiguration.Map(source, System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml")) as XElement;

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
                new Traversals.Model.ModelGetValueTraversal("Name"),
                new Xml.XmlSetThisValueTraversal()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                new Traversals.Model.ModelGetScopeTraversal("Members"),
                new Xml.XmlGetTemplateTraversal("./memberNames/memberName"),
                new Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Code"),
                new Xml.XmlSetValueTraversal("./@code")
            );

            var leaderName = new Mapping(
                new Traversals.Model.ModelGetSearchValueTraversal(
                    "../../Organization/Leaders{'PropertyName':'Reference','Value':'{{searchValue}}'}/LeaderPerson/Person/Name",
                    "LeaderReference"
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
                    leaderName
                },
                new Traversals.Model.ModelGetScopeTraversal("Armies/Platoons"),
                new Xml.XmlGetTemplateTraversal("./platoons/platoon"),
                new Xml.XmlChildCreator()
            );

            var crewMemberName = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Name"),
                new Xml.XmlSetThisValueTraversal()
            );

            var crewMemberNamesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                new Traversals.Model.ModelGetScopeTraversal("Armies/Platoons/Members/CrewMembers"),
                new Xml.XmlGetTemplateTraversal("./crewMemberNames/crewMemberName"),
                new Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                crewMemberNamesScope,
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new Configuration.Model.ModelObjectConverter(),
                new Xml.XmlTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}