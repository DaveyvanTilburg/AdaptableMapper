using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Conditions;
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

            errorObserver.GetRaisedWarnings().Count.Should().Be(0);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        private static MappingConfiguration GetFakedMappingConfiguration()
        {
            var memberName = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Name"),
                new Traversals.Xml.XmlSetThisValueTraversal()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                new Traversals.Model.ModelGetScopeTraversal("Members"),
                new Traversals.Xml.XmlGetTemplateTraversal("./memberNames/memberName"),
                new Configuration.Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Code"),
                new Traversals.Xml.XmlSetValueTraversal("./@code")
            );

            var leaderName = new Mapping(
                new Compositions.GetSearchValueTraversal(
                    new Traversals.Model.ModelGetValueTraversal("../../Organization/Leaders{'PropertyName':'Reference','Value':'{{searchValue}}'}/LeaderPerson/Person/Name"),
                    new Traversals.Model.ModelGetValueTraversal("LeaderReference")
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
                new Traversals.Model.ModelGetScopeTraversal("Armies/Platoons"),
                new Traversals.Xml.XmlGetTemplateTraversal("./platoons/platoon"),
                new Configuration.Xml.XmlChildCreator()
            )
            {
                Condition = new CompareCondition(
                    new Traversals.Model.ModelGetValueTraversal("Deployed"), 
                    CompareOperator.Equals, 
                    new Compositions.GetStaticValueTraversal("True"))
            };

            var crewMemberName = new Mapping(
                new Traversals.Model.ModelGetValueTraversal("Name"),
                new Traversals.Xml.XmlSetThisValueTraversal()
            );

            var crewMemberNamesScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                new Traversals.Model.ModelGetScopeTraversal("Armies/Platoons/Members/CrewMembers"),
                new Traversals.Xml.XmlGetTemplateTraversal("./crewMemberNames/crewMemberName"),
                new Configuration.Xml.XmlChildCreator()
            );

            var scopes = new List<MappingScopeComposite>
            {
                crewMemberNamesScope,
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new Configuration.Model.ModelObjectConverter(),
                new Configuration.Xml.XmlTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}