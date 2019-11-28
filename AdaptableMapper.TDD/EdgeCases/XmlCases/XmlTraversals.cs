using AdaptableMapper.Process;
using System;
using System.Collections.Generic;
using AdaptableMapper.Traversals.Xml;
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
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-XML#17;")]
        [InlineData("InvalidPath", "::", ContextType.EmptyObject, "e-XML#29;")]
        [InlineData("EmptyString", "//SimpleItems/SimpleItem/SurName", ContextType.TestObject, "w-XML#4;")]
        public void XmlGetValueTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlGetValueTraversal(path);
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
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
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-XML#21;")]
        public void XmlSetValueTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlSetValueTraversal(path);
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.SetValue(context, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
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