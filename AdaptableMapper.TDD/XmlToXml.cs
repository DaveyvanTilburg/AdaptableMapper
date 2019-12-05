using FluentAssertions;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Conditions;
using AdaptableMapper.Configuration;
using AdaptableMapper.Xml;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class XmlToXml
    {
        [Fact]
        public void XmlToXmlTest()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();

            XElement result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml")) as XElement;

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            string expectedResult = System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyExpected.xml");
            XElement xExpectedResult = XElement.Parse(expectedResult);

            errorObserver.GetRaisedWarnings().Count.Should().Be(0);
            errorObserver.GetRaisedErrors().Count.Should().Be(0);
            errorObserver.GetRaisedOtherTypes().Count.Should().Be(0);

            result.Should().BeEquivalentTo(xExpectedResult);
        }

        [Fact]
        public void XmlTestIfResultIsString()
        {
            var errorObserver = new TestErrorObserver();
            Process.ProcessObservable.GetInstance().Register(errorObserver);

            MappingConfiguration mappingConfiguration = GetMappingConfiguration();
            mappingConfiguration.ResultObjectConverter = new Configuration.Xml.XElementToStringObjectConverter();

            object result = mappingConfiguration.Map(System.IO.File.ReadAllText(@".\Resources\XmlSource_ArmyComposition.xml"), System.IO.File.ReadAllText(@".\Resources\XmlTarget_ArmyTemplate.xml"));

            Process.ProcessObservable.GetInstance().Unregister(errorObserver);

            XElement resultXElement = result as XElement;
            resultXElement.Should().BeNull();

            string resultString = result as string;
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
                new Traversals.Xml.XmlGetScopeTraversal("./army/platoon/members/member/crew/crewMember"),
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
                new Traversals.Xml.XmlGetScopeTraversal("./members/member"),
                new Traversals.Xml.XmlGetTemplateTraversal("./memberNames/memberName"),
                new Configuration.Xml.XmlChildCreator()
            );

            var platoonCode = new Mapping(
                new Traversals.Xml.XmlGetValueTraversal("./@code"),
                new Traversals.Xml.XmlSetValueTraversal("./@code")
            );

            var leaderNameSearch = new Mapping(
                new Traversals.Xml.XmlGetSearchValueTraversal(
                    "../../leaders/leader[@reference='{{searchValue}}']",
                    "./leaderReference"
                )
                    { XmlInterpretation = XmlInterpretation.Default },
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
                new Traversals.Xml.XmlGetScopeTraversal("./army/platoon"),
                new Traversals.Xml.XmlGetTemplateTraversal("./platoons/platoon") { XmlInterpretation = XmlInterpretation.Default },
                new Configuration.Xml.XmlChildCreator()
            )
            {
                Condition = new CompareCondition(new Traversals.Xml.XmlGetValueTraversal("./@deployed"), new Traversals.GetStaticValueTraversal("True"), CompareOperator.Equals)
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