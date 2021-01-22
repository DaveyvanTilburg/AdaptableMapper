using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Configuration.Json;
using MappingFramework.Configuration.Xml;
using MappingFramework.Traversals.Json;
using MappingFramework.Traversals.Xml;
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
                                new XmlGetThisValueTraversal(),
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
                        new XmlGetListValueTraversal(""), 
                        new JsonGetTemplateTraversal(""),
                        new JsonChildCreator()
                    )
                },
                new ContextFactory(
                    new XmlObjectConverter(),
                    new JsonTargetInstantiator()
                ),
                new JTokenToStringObjectConverter()
            );
            
            var subject = new CompositionValidationVisitor();
            subject.Visit(composition);

            var result = subject.Feedback();
            result.Count.Should().Be(2);
        }
    }
}