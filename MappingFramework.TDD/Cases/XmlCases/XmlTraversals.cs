using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;
using FluentAssertions;
using Xunit;
using MappingFramework.Traversals;
using System.Xml.XPath;
using System.Linq;
using MappingFramework.Compositions;
using MappingFramework.Languages.Xml;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;

namespace MappingFramework.TDD.Cases.XmlCases
{
    public class XmlTraversals
    {
        [Theory]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, 2)]
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, 1)]
        public void XmlGetListValueTraversal(string because, string path, ContextType contextType, int informationCount)
        {
            var subject = new XmlGetListValueTraversal(path) { XmlInterpretation = XmlInterpretation.Default };
            Context context = new Context(Xml.Stub(contextType), null, null);

            subject.GetValues(context);

            context.Information().Count.Should().Be(informationCount, because);
        }

        [Fact]
        public void XmlGetThisValueTraversalValid()
        {
            var subject = new XmlGetThisValueTraversal();
            object source = Xml.Stub(ContextType.TestObject);

            var traversal = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem[@Id='1']/Name");
            Template name = traversal.GetTemplate(new Context(), source);

            var context = new Context(name.Child, null, null);

            string value = subject.GetValue(context);

            context.Information().Count.Should().Be(0);
            value.Should().BeEquivalentTo("Davey");
        }

        [Theory]
        [InlineData("EmptyPath", "", ContextType.EmptyObject, XmlInterpretation.WithoutNamespace, "", 1)]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, XmlInterpretation.Default, "", 1)]
        [InlineData("InvalidPathWithoutNamespace", "::", ContextType.EmptyObject, XmlInterpretation.WithoutNamespace, "", 1)]
        [InlineData("EmptyString", "//SimpleItems/SimpleItem/SurName", ContextType.TestObject, XmlInterpretation.Default, "", 0)]
        [InlineData("ValidNamespaceless", "//SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey", 0)]
        [InlineData("ValidNamespacelessDot", "./SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey", 0)]
        [InlineData("ValidNamespacelessDifferentPrefix", "./SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey", 0)]
        [InlineData("GetProcessingInstruction", "/processing-instruction('thing')", ContextType.Alternative2TestObject, XmlInterpretation.Default, "value1|value2|value3", 0)]
        [InlineData("GetNamespaceFromRoot", "./@count", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "2", 0)]
        [InlineData("CountChildNodesWithoutNamespace", "count(./SimpleItems/SimpleItem)", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "2", 0)]
        [InlineData("CountChildNodes", "count(./SimpleItems/SimpleItem)", ContextType.AlternativeTestObject, XmlInterpretation.Default, "0", 0)]
        [InlineData("ValidNamespaceless", "concat(./SimpleItems/SimpleItem[1]/Name,',',./SimpleItems/SimpleItem[2]/Name)", ContextType.TestObject, XmlInterpretation.Default, "Davey,Joey", 0)]
        public void XmlGetValueTraversal(string because, string path, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedValue, int informationCount)
        {
            var subject = new XmlGetValueTraversal(path) { XmlInterpretation = xmlInterpretation };
            var context = new Context(Xml.Stub(contextType), null, null);

            string value = subject.GetValue(context);

            context.Information().Count.Should().Be(informationCount, because);
            value.Should().Be(expectedValue, because);
        }

        [Fact]
        public void XmlSetThisValueTraversalValid()
        {
            var subject = new XmlSetThisValueTraversal();
            object source = Xml.Stub(ContextType.TestObject);

            var traversal = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem[@Id='1']/Name");
            Template name = traversal.GetTemplate(new Context(), source);

            var context = new Context(null, name.Child, null);

            subject.SetValue(context, "Test");
            context.Information().Count.Should().Be(0);

            string value = new XmlGetThisValueTraversal().GetValue(new Context(context.Target, null, null));
            value.Should().BeEquivalentTo("Test");
        }

        [Theory]
        [InlineData("ValidNamespaceless", "//SimpleItems/SimpleItem[@Id='1']/SurName", "van Tilburg", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "./Resources/SimpleNamespaceExpectedResult.xml")]
        [InlineData("ValidNamespacelessAttribute", "//SimpleItems/SimpleItem[@Id='1']/@Id", "3", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "./Resources/SimpleNamespaceExpectedResultAttribute.xml")]
        public void XmlSetValueTraversal(string because, string path, string value, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultPath)
        {
            var subject = new XmlSetValueTraversal(path) { XmlInterpretation = xmlInterpretation };
            var context = new Context(null, Xml.Stub(contextType), null);

            subject.SetValue(context, value);

            context.Information().Count.Should().Be(0,because);
            
            XElement xElementResult = context.Target as XElement;

            var converter = new XElementToStringResultObjectCreator();
            var convertedResult = converter.Convert(xElementResult);
            convertedResult.Should().BeEquivalentTo(System.IO.File.ReadAllText(expectedResultPath), because);
        }

        [Fact]
        public void XmlSetValueTraversalOnAttributes()
        {
            var subject = new XmlSetValueTraversal("//SimpleItems/SimpleItem/@Id") { XmlInterpretation = XmlInterpretation.Default };
            var context = new Context(null, Xml.Stub(ContextType.TestObject), null);

            subject.SetValue(context, "3");

            string value = new XmlGetValueTraversal("//SimpleItems/SimpleItem[@Id='3']/Name").GetValue(new Context(context.Target, null, null));
            value.Should().BeEquivalentTo("Davey");
        }

        [Fact]
        public void XmlSetValueTraversalCData()
        {
            var target = XDocument.Load("./Resources/XmlCData/CDataTemplate.xml").Root;
            var subject = new XmlSetValueTraversal("./item") { SetAsCData = true };

            subject.SetValue(new Context(null, target, null), "Test");

            var expectedResult = System.IO.File.ReadAllText("./Resources/XmlCData/CDataExpectedResult.xml");
            var result = new XElementToStringResultObjectCreator().Convert(target);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void XmlSetThisValueTraversalCData()
        {
            var source = XDocument.Load("./Resources/XmlCData/CDataTemplate.xml").Root;
            var targets = source.XPathEvaluate("./item") as IEnumerable;
            var target = targets.Cast<XObject>().FirstOrDefault() as XElement;

            var subject = new XmlSetThisValueTraversal { SetAsCData = true };

            subject.SetValue(new Context(null, target, null), "Test");

            var expectedResult = System.IO.File.ReadAllText("./Resources/XmlCData/CDataExpectedResult.xml");
            var result = new XElementToStringResultObjectCreator().Convert(target);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, 1)]
        [InlineData("NoResult", "abcd", ContextType.EmptyObject, 1)]
        [InlineData("test", "//SimpleItems/SimpleItem/@Id", ContextType.TestObject, 1)]
        [InlineData("ResultHasNoParent", "/", ContextType.TestObject, 1)]
        public void XmlGetTemplateTraversal(string because, string path, ContextType contextType, int informationCount)
        {
            var subject = new XmlGetTemplateTraversal(path) { XmlInterpretation = XmlInterpretation.Default };
            object source = Xml.Stub(contextType);
            var context = new Context();
            
            Template template = subject.GetTemplate(context, source);

            template.Parent.Should().BeAssignableTo<XElement>();
            template.Child.Should().BeAssignableTo<XElement>();

            context.Information().Count.Should().Be(informationCount, because);
        }

        [Fact]
        public void XmlGetNumberOfHits()
        {
            XElement source = XDocument.Load("./Resources/XmlGetNumberOfHits/MultipleComments.xml").Root;

            GetValueTraversal subject = new GetNumberOfHits(
                new List<GetListValueTraversal>
                {
                    new XmlGetListValueTraversal("./RoomStay/Comments/Comment"),
                    new XmlGetListValueTraversal("./SpecialRequests/SpecialRequest"),
                    new XmlGetListValueTraversal("./GlobalStuff/GlobalComment"),
                    new XmlGetListValueTraversal(""),
                    new XmlGetListValueTraversal("abcd")
                }
            );
            var context = new Context(source, null, null);

            string result = subject.GetValue(context);

            context.Information().Count.Should().Be(3);
            result.Should().BeEquivalentTo("6");
        }

        [Fact]
        public void MultipleScopes()
        {
            var mappingConfiguration = new MappingConfiguration(
                new List<MappingScopeComposite>
                {
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new XmlGetThisValueTraversal(),
                                new XmlSetThisValueTraversal()
                            )
                        },
                        new NullObject(),
                        new XmlGetListValueTraversal("./InvalidPath"),
                        new XmlGetTemplateTraversal("./People/Person"),
                        new XmlChildCreator()),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new XmlGetThisValueTraversal(),
                                new XmlSetThisValueTraversal()
                            )
                        },
                        new NullObject(),
                        new XmlGetListValueTraversal("./Teachers/Teacher"),
                        new XmlGetTemplateTraversal("./People/Person"),
                        new XmlChildCreator()),
                    new MappingScopeComposite(
                        new List<MappingScopeComposite>(),
                        new List<Mapping>
                        {
                            new Mapping(
                                new XmlGetThisValueTraversal(),
                                new XmlSetThisValueTraversal()
                            )
                        },
                        new NullObject(),
                        new XmlGetListValueTraversal("./Students/Student"),
                        new XmlGetTemplateTraversal("./People/Person"),
                        new XmlChildCreator())
                },
                new ContextFactory(
                    new XmlSourceCreator(),
                    new XmlTargetCreator()
                ),
                new XElementToStringResultObjectCreator()
            );

            string source = System.IO.File.ReadAllText("./Resources/MultipleScopesSource.xml");
            string template = System.IO.File.ReadAllText("./Resources/MultipleScopesTemplate.xml");
            string expectedResult = System.IO.File.ReadAllText("./Resources/MultipleScopesExpectedResult.xml");

            MapResult mapResult = mappingConfiguration.Map(source, template);
            string result = mapResult.Result as string;

            result.Should().BeEquivalentTo(expectedResult);
        }


        [Fact]
        public void ComplexImplementation()
        {
            var listOfValueMutations = new ListOfValueMutations();
            listOfValueMutations.ValueMutations.Add(
                new ReplaceValueMutation(
                    new SplitByCharTakePositionStringTraversal('|', 2),
                    new XmlGetValueTraversal("./SimpleItems/SimpleItem[@Id='1']/Name")
                )
            );
            listOfValueMutations.ValueMutations.Add(
                new DictionaryReplaceValueMutation(
                    new List<DictionaryReplaceValueMutation.ReplaceValue>
                    {
                        new DictionaryReplaceValueMutation.ReplaceValue
                        {
                            ValueToReplace = "value3",
                            NewValue = "SimpleItem"
                        }
                    }
                )
                {
                    GetValueStringTraversal = new SplitByCharTakePositionStringTraversal('|', 3)
                }
            );

            var mapping = new Mapping(
                new XmlGetValueTraversal("/processing-instruction('thing')"),
                new SetMutatedValueTraversal(
                    new XmlSetValueTraversal("/processing-instruction('thing')"),
                    listOfValueMutations
                )
            );

            var context = new Context(
                XDocument.Parse(System.IO.File.ReadAllText("./Resources/SimpleProcessingInstruction.xml")).Root,
                XDocument.Parse(System.IO.File.ReadAllText("./Resources/SimpleProcessingInstructionTemplate.xml")).Root,
                null
            );

            mapping.Map(context);

            context.Information().Count.Should().Be(0);
            XElement xElementResult = context.Target as XElement;

            var converter = new XElementToStringResultObjectCreator();
            var convertedResult = converter.Convert(xElementResult);
            convertedResult.Should().BeEquivalentTo(System.IO.File.ReadAllText("./Resources/SimpleProcessingInstructionExpectedResult.xml"));
        }
    }
}