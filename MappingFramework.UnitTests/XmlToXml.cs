using System.Collections.Generic;
using System.Xml.Linq;
using FluentAssertions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Xml;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
using Xunit;

namespace MappingFramework.UnitTests
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
            mappingConfiguration.ResultObjectCreator = new XElementToStringResultObjectCreator();

            MapResult mapResult = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"));

            XElement resultXElement = mapResult.Result as XElement;
            resultXElement.Should().BeNull();

            string resultString = mapResult.Result as string;
            resultString.Should().NotBeNull();
        }

        private static MappingConfiguration GetMappingConfiguration()
        {
            var crewMemberName = new Mapping(
                new XmlGetThisValueTraversal(),
                new XmlSetThisValueTraversal()
            );

            var crewScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    crewMemberName
                },
                new NullObject(),
                new XmlGetListValueTraversal("./army/platoon/members/member/crew/crewMember"),
                new XmlGetTemplateTraversal("./crewMemberNames/crewMemberName"),
                new XmlChildCreator()
            );

            var memberName = new Mapping(
                new XmlGetValueTraversal("./@name"),
                new XmlSetThisValueTraversal()
            );

            var memberScope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>
                {
                    memberName
                },
                new NullObject(),
                new XmlGetListValueTraversal("./members/member"),
                new XmlGetTemplateTraversal("./memberNames/memberName"),
                new XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new XmlGetValueTraversal("./@code"),
                new XmlSetValueTraversal("./@code")
            );

            var leaderNameSearch = new Mapping(
                new Compositions.GetSearchValueTraversal(
                    new XmlGetValueTraversal("../../leaders/leader[@reference='{{searchValue}}']"),
                    new XmlGetValueTraversal("./leaderReference")
                ),
                new XmlSetValueTraversal("./leaderName")
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
                new NullObject(),
                new XmlGetListValueTraversal("./army/platoon"),
                new XmlGetTemplateTraversal("./platoons/platoon") { XmlInterpretation = XmlInterpretation.Default },
                new XmlChildCreator()
            )
            {
                Condition = new CompareCondition(
                    new XmlGetValueTraversal("./@deployed"), 
                    CompareOperator.Equals, 
                    new Compositions.GetStaticValue("True"))
            };

            var scopes = new List<MappingScopeComposite>
            {
                crewScope,
                platoonScope
            };

            var contextFactory = new ContextFactory(
                new XmlSourceCreator(),
                new XmlTargetCreator()
            );

            var mappingConfiguration = new MappingConfiguration(scopes, contextFactory, new NullObject());

            return mappingConfiguration;
        }
    }
}