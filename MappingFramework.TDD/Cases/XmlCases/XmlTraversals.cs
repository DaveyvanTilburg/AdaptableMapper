using MappingFramework.Process;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.Configuration.Xml;
using MappingFramework.Traversals.Xml;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;
using MappingFramework.Xml;
using FluentAssertions;
using Xunit;
using MappingFramework.Traversals;
using System.Xml.XPath;
using System.Linq;
using MappingFramework.Compositions;

namespace MappingFramework.TDD.Cases.XmlCases
{
    public class XmlTraversals
    {
        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-XML#12;")]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, "e-XML#28;", "w-XML#5;")] //Preferred cascade, 28 contains extra info
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, "w-XML#5;")]
        public void XmlGetListValueTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlGetListValueTraversal(path) { XmlInterpretation = XmlInterpretation.Default };
            Context context = new Context(Xml.CreateTarget(contextType), null, null);
            List<Information> result = new Action(() => { subject.GetValues(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void XmlGetThisValueTraversalInvalidType()
        {
            var subject = new XmlGetThisValueTraversal();
            var context = new Context(null, Xml.CreateTarget(ContextType.EmptyString), null);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#16;" }, "InvalidType");
        }

        [Fact]
        public void XmlGetThisValueTraversalValid()
        {
            var subject = new XmlGetThisValueTraversal();
            object context = Xml.CreateTarget(ContextType.TestObject);

            var traversal = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem[@Id='1']/Name");
            Template name = traversal.GetTemplate(context, new MappingCaches());

            string value = string.Empty;
            List<Information> result = new Action(() => { value = subject.GetValue(new Context(name.Child, null, null)); }).Observe();
            result.ValidateResult(new List<string>(), "Valid");

            value.Should().BeEquivalentTo("Davey");
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, XmlInterpretation.Default, "", "e-XML#17;")]
        [InlineData("EmptyPath", "", ContextType.EmptyObject, XmlInterpretation.WithoutNamespace, "", "e-XML#29;")]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, XmlInterpretation.Default, "", "e-XML#29;")]
        [InlineData("InvalidPathWithoutNamespace", "::", ContextType.EmptyObject, XmlInterpretation.WithoutNamespace, "", "w-XML#30;")]
        [InlineData("EmptyString", "//SimpleItems/SimpleItem/SurName", ContextType.TestObject, XmlInterpretation.Default, "")]
        [InlineData("ValidNamespaceless", "//SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey")]
        [InlineData("ValidNamespacelessDot", "./SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey")]
        [InlineData("ValidNamespacelessDifferentPrefix", "./SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey")]
        [InlineData("GetProcessingInstruction", "/processing-instruction('thing')", ContextType.Alternative2TestObject, XmlInterpretation.Default, "value1|value2|value3")]
        [InlineData("GetNamespaceFromRoot", "./@count", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "2")]
        [InlineData("CountChildNodesWithoutNamespace", "count(./SimpleItems/SimpleItem)", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "2")]
        [InlineData("CountChildNodes", "count(./SimpleItems/SimpleItem)", ContextType.AlternativeTestObject, XmlInterpretation.Default, "0")]
        [InlineData("ValidNamespaceless", "concat(./SimpleItems/SimpleItem[1]/Name,',',./SimpleItems/SimpleItem[2]/Name)", ContextType.TestObject, XmlInterpretation.Default, "Davey,Joey")]
        public void XmlGetValueTraversal(string because, string path, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedValue, params string[] expectedErrors)
        {
            var subject = new XmlGetValueTraversal(path) { XmlInterpretation = xmlInterpretation };
            var context = new Context(Xml.CreateTarget(contextType), null, null);

            string value = null;
            List<Information> result = new Action(() => { value = subject.GetValue(context); }).Observe();

            result.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
                value.Should().Be(expectedValue);
        }

        [Fact]
        public void XmlSetThisValueTraversal()
        {
            var subject = new XmlSetThisValueTraversal();
            var context = new Context(null, Xml.CreateTarget(ContextType.EmptyString), null);
            List<Information> result = new Action(() => { subject.SetValue(context, null, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#20;" }, "InvalidType");
        }

        [Fact]
        public void XmlSetThisValueTraversalValid()
        {
            var subject = new XmlSetThisValueTraversal();
            object context = Xml.CreateTarget(ContextType.TestObject);

            var traversal = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem[@Id='1']/Name");
            Template name = traversal.GetTemplate(context, new MappingCaches());

            var setContext = new Context(null, name.Child, null);

            List<Information> result = new Action(() => { subject.SetValue(setContext, null, "Test"); }).Observe();
            result.ValidateResult(new List<string>(), "Valid");

            string value = new XmlGetThisValueTraversal().GetValue(new Context(setContext.Target, null, null));

            value.Should().BeEquivalentTo("Test");
        }

        [Theory]
        [InlineData("InvalidType", "", "", ContextType.EmptyString, XmlInterpretation.Default, "", "e-XML#21;")]
        [InlineData("ValidNamespaceless", "//SimpleItems/SimpleItem[@Id='1']/SurName", "van Tilburg", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "./Resources/SimpleNamespaceExpectedResult.xml")]
        [InlineData("ValidNamespacelessAttribute", "//SimpleItems/SimpleItem[@Id='1']/@Id", "3", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "./Resources/SimpleNamespaceExpectedResultAttribute.xml")]
        public void XmlSetValueTraversal(string because, string path, string value, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultPath, params string[] expectedErrors)
        {
            var subject = new XmlSetValueTraversal(path) { XmlInterpretation = xmlInterpretation };
            var context = new Context(null, Xml.CreateTarget(contextType), null);

            List<Information> result = new Action(() => { subject.SetValue(context, null, value); }).Observe();

            result.ValidateResult(new List<string>(expectedErrors), because);
            if (expectedErrors.Length == 0)
            {
                XElement xElementResult = context.Target as XElement;

                var converter = new XElementToStringObjectConverter();
                var convertedResult = converter.Convert(xElementResult);
                convertedResult.Should().BeEquivalentTo(System.IO.File.ReadAllText(expectedResultPath));
            }
        }

        [Fact]
        public void XmlSetValueTraversalOnAttributes()
        {
            var subject = new XmlSetValueTraversal("//SimpleItems/SimpleItem/@Id") { XmlInterpretation = XmlInterpretation.Default };
            var context = new Context(null, Xml.CreateTarget(ContextType.TestObject), null);

            List<Information> result = new Action(() => { subject.SetValue(context, null, "3"); }).Observe();

            string value = new XmlGetValueTraversal("//SimpleItems/SimpleItem[@Id='3']/Name").GetValue(new Context(context.Target, null, null));
            value.Should().BeEquivalentTo("Davey");
        }

        [Fact]
        public void XmlSetValueTraversalCData()
        {
            var target = XDocument.Load("./Resources/XmlCData/CDataTemplate.xml").Root;
            var subject = new XmlSetValueTraversal("./item") { SetAsCData = true };

            subject.SetValue(new Context(null, target, null), null, "Test");

            var expectedResult = System.IO.File.ReadAllText("./Resources/XmlCData/CDataExpectedResult.xml");
            var result = new XElementToStringObjectConverter().Convert(target);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void XmlSetThisValueTraversalCData()
        {
            var context = XDocument.Load("./Resources/XmlCData/CDataTemplate.xml").Root;
            var targets = context.XPathEvaluate("./item") as IEnumerable;
            var target = targets.Cast<XObject>().FirstOrDefault() as XElement;

            var subject = new XmlSetThisValueTraversal { SetAsCData = true };

            subject.SetValue(new Context(null, target, null), null, "Test");

            var expectedResult = System.IO.File.ReadAllText("./Resources/XmlCData/CDataExpectedResult.xml");
            var result = new XElementToStringObjectConverter().Convert(target);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-XML#23;")]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, "e-XML#27;")]
        [InlineData("NoResult", "abcd", ContextType.EmptyObject, "w-XML#2;")]
        [InlineData("test", "//SimpleItems/SimpleItem/@Id", ContextType.TestObject, "e-XML#8;")]
        [InlineData("ResultHasNoParent", "/", ContextType.TestObject, "e-XML#8;")]
        public void XmlGetTemplateTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlGetTemplateTraversal(path) { XmlInterpretation = XmlInterpretation.Default };
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetTemplate(context, new MappingCaches()); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void XmlSetGeneratedIdValueTraversalInvalidType()
        {
            var subject = new XmlSetGeneratedIdValueTraversal("") { XmlInterpretation = XmlInterpretation.Default, SetAsCData = false };

            List<Information> information = new Action(() => { subject.SetValue(new Context(null, 1, null), null, ""); }).Observe();
            information.ValidateResult(new List<string> { "e-XmlSetGeneratedIdValueTraversal#1;" });
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

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(new Context(source, null, null)); }).Observe();
            information.ValidateResult(new List<string> { "e-XML#28;", "w-XML#5;", "w-XML#5;" });

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
                        new XmlGetListValueTraversal("./Students/Student"),
                        new XmlGetTemplateTraversal("./People/Person"),
                        new XmlChildCreator())
                },
                new ContextFactory(
                    new XmlObjectConverter(),
                    new XmlTargetInstantiator()
                ),
                new XElementToStringObjectConverter()
            );

            string source = System.IO.File.ReadAllText("./Resources/MultipleScopesSource.xml");
            string template = System.IO.File.ReadAllText("./Resources/MultipleScopesTemplate.xml");
            string expectedResult = System.IO.File.ReadAllText("./Resources/MultipleScopesExpectedResult.xml");

            string result = mappingConfiguration.Map(source, template) as string;

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

            List<Information> result = new Action(() => { mapping.Map(context, null); }).Observe();

            result.Count.Should().Be(0);
            XElement xElementResult = context.Target as XElement;

            var converter = new XElementToStringObjectConverter();
            var convertedResult = converter.Convert(xElementResult);
            convertedResult.Should().BeEquivalentTo(System.IO.File.ReadAllText("./Resources/SimpleProcessingInstructionExpectedResult.xml"));
        }
    }
}