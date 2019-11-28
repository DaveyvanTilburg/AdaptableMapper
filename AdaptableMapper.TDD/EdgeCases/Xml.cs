using AdaptableMapper.Process;
using AdaptableMapper.Xml;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Traversals;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class Xml
    {
        [Fact]
        public void XmlChildCreatorInvalidParentType()
        {
            var subject = new XmlChildCreator();
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = string.Empty, Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#10;" });
        }

        [Fact]
        public void XmlChildCreatorInvalidTemplateType()
        {
            var subject = new XmlChildCreator();
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = new XElement("nullObject"), Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#11;" });
        }

        [Fact]
        public void XmlGetScopeInvalidType()
        {
            var subject = new XmlGetScopeTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#12;" });
        }

        [Fact]
        public void XmlGetScopeInvalidPath()
        {
            var subject = new XmlGetScopeTraversal("::");
            List<Information> result = new Action(() => { subject.GetScope(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#28;", "w-XML#5;" }); //Preferred cascade, 28 contains extra info
        }

        [Fact]
        public void XmlGetScopeNoResults()
        {
            var subject = new XmlGetScopeTraversal("abcd");
            List<Information> result = new Action(() => { subject.GetScope(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#5;" });
        }

        [Fact]
        public void XmlGetSearchValueInvalidType()
        {
            //Todo, review errors/warnings thrown marker
            var subject = new XmlGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#13;" });
        }

        [Fact]
        public void XmlGetSearchValueEmptySearchPath()
        {
            var subject = new XmlGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#25;" });
        }

        [Fact]
        public void XmlGetSearchValueInvalidSearchPath()
        {
            var subject = new XmlGetSearchValueTraversal(string.Empty, "abcd");
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#30;" });
        }

        [Fact]
        public void XmlGetSearchValueEmptySearchPathValueResult()
        {
            var subject = new XmlGetSearchValueTraversal(string.Empty, "//SimpleItems/SimpleItem/SurName");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#14;" });
        }

        [Fact]
        public void XmlGetSearchValueNoActualPathResult()
        {
            var subject = new XmlGetSearchValueTraversal("//SimpleItems/SimpleItem/SurName", "//SimpleItems/SimpleItem/@Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#15;" });
        }

        [Fact]
        public void XmlGetThisValueInvalidType()
        {
            var subject = new XmlGetThisValueTraversal();
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#16;" });
        }

        [Fact]
        public void XmlGetValueInvalidType()
        {
            var subject = new XmlGetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#17;" });
        }

        [Fact]
        public void XmlGetValueInvalidPath()
        {
            var subject = new XmlGetValueTraversal("::");
            List<Information> result = new Action(() => { subject.GetValue(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#29;" });
        }

        [Fact]
        public void XmlGetValueEmptyString()
        {
            var subject = new XmlGetValueTraversal("//SimpleItems/SimpleItem/SurName");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#4;" });
        }

        [Fact]
        public void XmlObjectConverterRemovesNamespaceInvalidType()
        {
            var subject = new XmlObjectConverterRemovesNamespace();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#30;" });
        }

        [Fact]
        public void XmlObjectConverterRemovesNamespaceInvalidSource()
        {
            var subject = new XmlObjectConverterRemovesNamespace();
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#31;" });
        }

        [Fact]
        public void XmlObjectConverterInvalidType()
        {
            var subject = new XmlObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#18;" });
        }

        [Fact]
        public void XmlObjectConverterInvalidSource()
        {
            var subject = new XmlObjectConverter();
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#19;" });
        }

        [Fact]
        public void XmlSetThisValueInvalidType()
        {
            var subject = new XmlSetThisValueTraversal();
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#20;" });
        }

        [Fact]
        public void XmlSetValueInvalidType()
        {
            var subject = new XmlSetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#21;" });
        }

        [Fact]
        public void XmlTargetInstantiatorInvalidType()
        {
            var subject = new XmlTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#24;" });
        }

        [Fact]
        public void XmlTargetInstantiatorInvalidTarget()
        {
            var subject = new XmlTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#6;" });
        }

        [Fact]
        public void XmlTargetInstantiatorRemovesNamespaceInvalidType()
        {
            var subject = new XmlTargetInstantiatorRemovesNamespace();
            List<Information> result = new Action(() => { subject.Create(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#32;" });
        }

        [Fact]
        public void XmlTargetInstantiatorRemovesNamespaceInvalidTarget()
        {
            var subject = new XmlTargetInstantiatorRemovesNamespace();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#33;" });
        }

        [Fact]
        public void XmlTraversalTemplateInvalidType()
        {
            var subject = new XmlGetTemplateTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Get(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#23;" });
        }

        [Fact]
        public void XmlTraversalTemplateInvalidPath()
        {
            var subject = new XmlGetTemplateTraversal("::");
            List<Information> result = new Action(() => { subject.Get(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#27;" });
        }

        [Fact]
        public void XmlTraversalTemplateNoResult()
        {
            var subject = new XmlGetTemplateTraversal("abcd");
            List<Information> result = new Action(() => { subject.Get(new XElement("nullObject")); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#2;" });
        }

        [Fact]
        public void XmlTraversalTemplateMoreThanOne()
        {
            var subject = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem/Name");
            List<Information> result = new Action(() => { subject.Get(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#3;" });
        }

        [Fact]
        public void XmlTraversalTemplateResultHasNoParent()
        {
            var subject = new XmlGetTemplateTraversal("/");
            List<Information> result = new Action(() => { subject.Get(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-XML#26;" });
        }

        [Fact]
        public void XElementToStringObjectConverterInvalidType()
        {
            var subject = new XElementToStringObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#9;" });
        }

        private XElement CreateTestData()
            => XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));
    }
}