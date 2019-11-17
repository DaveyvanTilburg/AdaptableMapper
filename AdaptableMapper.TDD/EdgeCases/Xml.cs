using AdaptableMapper.Process;
using AdaptableMapper.Xml;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class Xml
    {
        [Fact]
        public void XmlChildCreatorInvalidParentType()
        {
            var subject = new XmlChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#10" });
        }

        [Fact]
        public void XmlChildCreatorInvalidTemplateType()
        {
            var subject = new XmlChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(new XElement("nullObject"), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#11" });
        }

        [Fact]
        public void XmlGetScopeInvalidType()
        {
            var subject = new XmlGetScope(string.Empty);
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#12" });
        }

        [Fact]
        public void XmlGetScopeInvalidPath()
        {
            var subject = new XmlGetScope("::");
            List<Information> result = new Action(() => { subject.GetScope(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#28" });
        }

        [Fact]
        public void XmlGetScopeNoResults()
        {
            var subject = new XmlGetScope("abcd");
            List<Information> result = new Action(() => { subject.GetScope(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#1" });
        }

        [Fact]
        public void XmlGetSearchValueInvalidType()
        {
            var subject = new XmlGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#13" });
        }

        [Fact]
        public void XmlGetSearchValueEmptySearchPath()
        {
            var subject = new XmlGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#25" });
        }

        [Fact]
        public void XmlGetSearchValueNoSearchResult()
        {
            var subject = new XmlGetSearchValue(string.Empty, "abcd");
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#14", "w-XML#4" });
        }

        [Fact]
        public void XmlGetSearchValueNoActualPath()
        {
            var subject = new XmlGetSearchValue("abcd{{searchValue}}", "//SimpleItems/SimpleItem/@Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#15", "w-XML#4" });
        }

        [Fact]
        public void XmlGetThisValueInvalidType()
        {
            var subject = new XmlGetThisValue();
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#16" });
        }

        [Fact]
        public void XmlGetValueInvalidType()
        {
            var subject = new XmlGetValue(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#17" });
        }

        [Fact]
        public void XmlGetValueInvalidPath()
        {
            var subject = new XmlGetValue("::");
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#29" });
        }

        [Fact]
        public void XmlGetValueNoItems()
        {
            var subject = new XmlGetValue("//SimpleItems/SimpleItems/Name");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#4" });
        }

        [Fact]
        public void XmlObjectConverterInvalidType()
        {
            var subject = new XmlObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#18" });
        }

        [Fact]
        public void XmlObjectConverterInvalidSource()
        {
            var subject = new XmlObjectConverter();
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#19" });
        }

        [Fact]
        public void XmlSetThisValueInvalidType()
        {
            var subject = new XmlSetThisValue();
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#20" });
        }

        [Fact]
        public void XmlSetValueInvalidType()
        {
            var subject = new XmlSetValue(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#21" });
        }

        [Fact]
        public void XmlTargetInstantiatorInvalidType()
        {
            var subject = new XmlTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#24" });
        }

        [Fact]
        public void XmlTargetInstantiatorInvalidTarget()
        {
            var subject = new XmlTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#6" });
        }

        [Fact]
        public void XmlTraversalInvalidType()
        {
            var subject = new XmlTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#22" });
        }

        [Fact]
        public void XmlTraversalInvalidPath()
        {
            var subject = new XmlTraversal("::");
            List<Information> result = new Action(() => { subject.Traverse(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#27" });
        }

        [Fact]
        public void XmlTraversalNoResult()
        {
            var subject = new XmlTraversal("abcd");
            List<Information> result = new Action(() => { subject.Traverse(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#2" });
        }

        [Fact]
        public void XmlTraversalMoreThanOne()
        {
            var subject = new XmlTraversal("//SimpleItems/SimpleItem/name");
            List<Information> result = new Action(() => { subject.Traverse(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#3" });
        }

        [Fact]
        public void XmlTraversalTemplateInvalidType()
        {
            var subject = new XmlTraversalTemplate(string.Empty);
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#23" });
        }

        [Fact]
        public void XmlTraversalTemplateResultHasNoParent()
        {
            var subject = new XmlTraversalTemplate("/");
            List<Information> result = new Action(() => { subject.Traverse(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#26" });
        }

        [Fact]
        public void XElementToStringObjectConverterInvalidType()
        {
            var subject = new XElementToStringObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#9" });
        }

        private XElement CreateTestData()
            => XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));
    }
}