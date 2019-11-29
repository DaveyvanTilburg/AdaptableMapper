using AdaptableMapper.Process;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Traversals.Xml;
using AdaptableMapper.Xml;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases.XmlCases
{
    public class XmlTraversals
    {
        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-XML#12;")]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, "e-XML#28;", "w-XML#5;")] //Preferred cascade, 28 contains extra info
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, "w-XML#5;")]
        public void XmlGetScopeTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlGetScopeTraversal(path);
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetScope(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", "", ContextType.EmptyString, "e-XML#13;")]
        [InlineData("EmptySearchPath", "", "", ContextType.EmptyObject, "e-XML#25;")]
        [InlineData("InvalidSearchPath", "", "abcd", ContextType.EmptyObject, "w-XML#30;")]
        [InlineData("EmptySearchPathValueResult", "", "//SimpleItems/SimpleItem/SurName", ContextType.TestObject, "w-XML#14;")]
        [InlineData("NoActualPathResult", "//SimpleItems/SimpleItem/SurName", "//SimpleItems/SimpleItem/@Id", ContextType.TestObject, "w-XML#15;")]
        public void XmlGetSearchValueTraversal(string because, string path, string searchPath, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlGetSearchValueTraversal(path, searchPath);
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "e-XML#16;")]
        public void XmlGetThisValueTraversal(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlGetThisValueTraversal();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, XmlInterpretation.Default, "", "e-XML#17;")]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, XmlInterpretation.Default, "", "e-XML#29;")]
        [InlineData("InvalidPathWithoutNamespace", "::", ContextType.EmptyObject, XmlInterpretation.WithoutNamespace, "", "w-XML#30;")]
        [InlineData("EmptyString", "//SimpleItems/SimpleItem/SurName", ContextType.TestObject, XmlInterpretation.Default, "", "w-XML#4;")]
        [InlineData("ValidNamespaceless", "//SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey")]
        [InlineData("ValidNamespacelessDifferentPrefix", "./SimpleItems/SimpleItem/Name", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace, "Davey")]
        public void XmlGetValueTraversal(string because, string path, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedValue, params string[] expectedErrors)
        {
            var subject = new XmlGetValueTraversal(path) { XmlInterpretation = xmlInterpretation };
            object context = Xml.CreateTarget(contextType);

            string value = null;
            List<Information> result = new Action(() => { value = subject.GetValue(context); }).Observe();

            result.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
                value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "e-XML#20;")]
        public void XmlSetThisValueTraversal(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlSetThisValueTraversal();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.SetValue(context, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", "", ContextType.EmptyString, XmlInterpretation.Default, "e-XML#21;")]
        [InlineData("ValidNamespaceless", "//SimpleItems/SimpleItem[@Id='1']/SurName", "van Tilburg", ContextType.AlternativeTestObject, XmlInterpretation.WithoutNamespace)]
        public void XmlSetValueTraversal(string because, string path, string value, ContextType contextType, XmlInterpretation xmlInterpretation, params string[] expectedErrors)
        {
            var subject = new XmlSetValueTraversal(path) { XmlInterpretation = xmlInterpretation };
            object context = Xml.CreateTarget(contextType);

            List<Information> result = new Action(() => { subject.SetValue(context, value); }).Observe();

            result.ValidateResult(new List<string>(expectedErrors), because);
            if (expectedErrors.Length == 0)
            {
                XElement xElementResult = context as XElement;
                xElementResult.ToString().Should().BeEquivalentTo(System.IO.File.ReadAllText("./Resources/SimpleNamespaceExpectedResult.xml"));
            }
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-XML#23;")]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, "e-XML#27;")]
        [InlineData("NoResult", "abcd", ContextType.EmptyObject, "w-XML#2;")]
        [InlineData("NoResult", "//SimpleItems/SimpleItem/Name", ContextType.TestObject, "w-XML#3;")]
        [InlineData("ResultHasNoParent", "/", ContextType.TestObject, "e-XML#26;")]
        public void XmlGetTemplateTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlGetTemplateTraversal(path);
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Get(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }
    }
}