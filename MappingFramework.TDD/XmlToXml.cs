using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Xml;
using Xunit;

namespace MappingFramework.TDD
{
    public class XmlToXml
    {
        [Fact]
        public void XmlToXmlTest()
        {
            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            MapResult mapResult = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"));
            XElement result = mapResult.Result as XElement;

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            mapResult.Information.Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        [Fact]
        public void XmlTestIfResultIsString()
        {
            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            mappingConfiguration.ResultObjectConverter = new Configuration.Xml.XElementToStringObjectConverter();

            MapResult mapResult = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"));

            XElement resultXElement = mapResult.Result as XElement;
            resultXElement.Should().BeNull();

            string resultString = mapResult.Result as string;
            resultString.Should().NotBeNull();
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var crewMemberName = new Mapping(
                new Traversals.Xml.XmlGetThisValueTraversal(),
                new Traversals.Xml.XmlSetThisValueTraversal()
            );

            var crewScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                null,
                new Traversals.Xml.XmlGetListValueTraversal("./army/platoon/members/member/crew/crewMember"),
                new Traversals.Xml.XmlGetTemplateTraversal("./crewMemberNames/crewMemberName"),
                new Configuration.Xml.XmlChildCreator()
            );

            var memberName = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./@name"),
                new Traversals.Xml.XmlSetThisValueTraversal()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                null,
                new Traversals.Xml.XmlGetListValueTraversal("./members/member"),
                new Traversals.Xml.XmlGetTemplateTraversal("./memberNames/memberName"),
                new Configuration.Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./@code"),
                new Traversals.Xml.XmlSetValueTraversal("./@code")
            );

            var leaderNameSearch = new Mapping(
                new Compositions.GetSearchValueTraversal(
                    new Traversals.Xml.XmlGetValueTraversal("../../leaders/leader[@reference='{{searchValue}}']"),
                    new Traversals.Xml.XmlGetValueTraversal("./leaderReference")
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
                    leaderNameSearch
                },
                null,
                new Traversals.Xml.XmlGetListValueTraversal("./army/platoon"),
                new Traversals.Xml.XmlGetTemplateTraversal("./platoons/platoon") { XmlInterpretation = XmlInterpretation.Default },
                new Configuration.Xml.XmlChildCreator()
            )
            {
                Condition = new CompareCondition(
                    new Traversals.Xml.XmlGetValueTraversal("./@deployed"), 
                    CompareOperator.Equals, 
                    new Compositions.GetStaticValue("True"))
            };

            var scopes = new List<MappingScopeComposite>
            {
                crewScope,
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new Configuration.Xml.XmlObjectConverter(),
                new Configuration.Xml.XmlTargetInstantiator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObjectConverter());

            return mappingConfiguration;
        }
    }
}