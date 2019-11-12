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
            result.ValidateResult(new List<string> { "XML#10" });
        }

        [Fact]
        public void XmlChildCreatorInvalidTemplateType()
        {
            var subject = new XmlChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(new XElement("nullObject"), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "XML#11" });
        }

        [Fact]
        public void XmlGetScopeInvalidType()
        {
            var subject = new XmlGetScope(string.Empty);
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "XML#12" });
        }

        [Fact]
        public void XmlGetSearchValueInvalidType()
        {
            var subject = new XmlGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "XML#13" });
        }

        [Fact]
        public void XmlGetSearchValueEmptySearchPath()
        {
            var subject = new XmlGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "XML#25" });
        }

        [Fact]
        public void XmlGetSearchValueNoSearchResult()
        {
            var subject = new XmlGetSearchValue(string.Empty, "abcd");
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "XML#14", "XML#4" });
        }

        [Fact]
        public void XmlGetSearchValueNoActualPath()
        {
            var subject = new XmlGetSearchValue("abcd:{{searchValue}}", "//SimpleItems/SimpleItem/@Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "XML#15", "XML#4" });
        }

        private XElement CreateTestData()
            => XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));
    }
}