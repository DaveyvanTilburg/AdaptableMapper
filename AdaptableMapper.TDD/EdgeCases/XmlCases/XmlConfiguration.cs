using AdaptableMapper.Process;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Configuration.Xml;
using AdaptableMapper.Traversals;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases.XmlCases
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
        public void XElementToStringObjectConverterInvalidType()
        {
            var subject = new XElementToStringObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-XML#9;" });
        }
    }
}