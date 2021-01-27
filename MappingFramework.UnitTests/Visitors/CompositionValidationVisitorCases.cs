using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Json.Configuration;
using MappingFramework.Languages.Json.Traversals;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.Visitors;
using Xunit;

namespace MappingFramework.TDD.Visitors
{
    public class CompositionValidationVisitorCases
    {
        [Fact]
        public void Test()
        {
            var composition = new MappingConfiguration(
                new List<MappingScopeComposite>
                {
                    new MappingScopeComposite(
                        null,
                        new List<Mapping>
                        {
                            new Mapping(
                                new GetSearchValueTraversal(
                                    new XmlGetValueTraversal(""),
                                    new NullObject()
                                ),
                                new SetMutatedValueTraversal(
                                    new JsonSetValueTraversal(""),
                                    new ListOfValueMutations(
                                        new List<ValueMutation>
                                        {
                                            new ReplaceValueMutation(
                                                new GetStaticValue(""),
                                                new JsonGetValueTraversal("")
                                            ),
                                            null
                                        }
                                    )
                                )
                            )
                        },
                        new ListOfConditions(
                            ListEvaluationOperator.All,
                            new List<Condition>
                            {
                                new CompareCondition(
                                    new XmlGetValueTraversal(""),
                                    CompareOperator.Contains,
                                    new XmlGetValueTraversal("")
                                )
                            }
                        ),
                        new GetListSearchValueTraversal(
                            new XmlGetListValueTraversal(""),
                            new NullObject()
                        ), 
                        new JsonGetTemplateTraversal(""),
                        new JsonChildCreator()
                    )
                },
                new ContextFactory(
                    new XmlSourceCreator(),
                    new JsonTargetCreator()
                ),
                new JTokenToStringResultObjectCreator()
            );
            
            var subject = new CompositionValidationVisitor();
            subject.Visit(composition);

            var result = subject.Feedback();
            result.Count.Should().Be(2);
        }
    }
}