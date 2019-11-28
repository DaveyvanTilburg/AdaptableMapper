using AdaptableMapper.Process;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Traversals.Xml;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases.XmlCases
{
    public class XmlTraversals
    {
        [Fact]
        public void XmlGetScopeTraversal_InvalidType()
        {
            var subject = new XmlGetScopeTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#12;" });
        }

        [Fact]
        public void XmlGetScopeTraversal_InvalidPath()
        {
            var subject = new XmlGetScopeTraversal("::");
            List<Information> result = new Action(() => { subject.GetScope(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#28;", "w-XML#5;" }); //Preferred cascade, 28 contains extra info
        }

        [Fact]
        public void XmlGetScopeTraversal_NoResults()
        {
            var subject = new XmlGetScopeTraversal("abcd");
            List<Information> result = new Action(() => { subject.GetScope(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#5;" });
        }



        [Fact]
        public void XmlGetSearchValueTraversal_InvalidType()
        {
            var subject = new XmlGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#13;" });
        }

        [Fact]
        public void XmlGetSearchValueTraversal_EmptySearchPath()
        {
            var subject = new XmlGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#25;" });
        }

        [Fact]
        public void XmlGetSearchValueTraversal_InvalidSearchPath()
        {
            var subject = new XmlGetSearchValueTraversal(string.Empty, "abcd");
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#30;" });
        }

        [Fact]
        public void XmlGetSearchValueTraversal_EmptySearchPathValueResult()
        {
            var subject = new XmlGetSearchValueTraversal(string.Empty, "//SimpleItems/SimpleItem/SurName");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#14;" });
        }

        [Fact]
        public void XmlGetSearchValueTraversal_NoActualPathResult()
        {
            var subject = new XmlGetSearchValueTraversal("//SimpleItems/SimpleItem/SurName", "//SimpleItems/SimpleItem/@Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#15;" });
        }

        [Fact]
        public void XmlGetThisValueTraversal_InvalidType()
        {
            var subject = new XmlGetThisValueTraversal();
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#16;" });
        }



        [Fact]
        public void XmlGetValueTraversal_InvalidType()
        {
            var subject = new XmlGetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#17;" });
        }

        [Fact]
        public void XmlGetValueTraversal_InvalidPath()
        {
            var subject = new XmlGetValueTraversal("::");
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#29;" });
        }

        [Fact]
        public void XmlGetValueTraversal_EmptyString()
        {
            var subject = new XmlGetValueTraversal("//SimpleItems/SimpleItem/SurName");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#4;" });
        }



        [Fact]
        public void XmlSetThisValueTraversal_InvalidType()
        {
            var subject = new XmlSetThisValueTraversal();
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#20;" });
        }



        [Fact]
        public void XmlSetValueTraversal_InvalidType()
        {
            var subject = new XmlSetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#21;" });
        }



        [Fact]
        public void XmlGetTemplateTraversal_InvalidType()
        {
            var subject = new XmlGetTemplateTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Get(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#23;" });
        }

        [Fact]
        public void XmlGetTemplateTraversal_InvalidPath()
        {
            var subject = new XmlGetTemplateTraversal("::");
            List<Information> result = new Action(() => { subject.Get(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#27;" });
        }

        [Fact]
        public void XmlGetTemplateTraversal_NoResult()
        {
            var subject = new XmlGetTemplateTraversal("abcd");
            List<Information> result = new Action(() => { subject.Get(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#2;" });
        }

        [Fact]
        public void XmlGetTemplateTraversal_MoreThanOne()
        {
            var subject = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem/Name");
            List<Information> result = new Action(() => { subject.Get(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#3;" });
        }

        [Fact]
        public void XmlGetTemplateTraversal_ResultHasNoParent()
        {
            var subject = new XmlGetTemplateTraversal("/");
            List<Information> result = new Action(() => { subject.Get(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#26;" });
        }



        private XElement CreateTestData()
            => XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));
    }
}