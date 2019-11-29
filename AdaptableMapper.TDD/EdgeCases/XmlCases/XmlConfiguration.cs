using AdaptableMapper.Process;
using System;
using System.Collections.Generic;
using AdaptableMapper.Configuration.Xml;
using AdaptableMapper.Traversals;
using AdaptableMapper.Xml;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases.XmlCases
{
    public class XmlConfiguration
    {
        [Theory]
        [InlineData("InvalidParentType", ContextType.EmptyString, "e-XML#10;")]
        [InlineData("InvalidTemplateType", ContextType.EmptyObject, "e-XML#11;")]
        public void XmlChildCreator(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlChildCreator();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = context, Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "e-XML#18;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "e-XML#19;")]
        public void XmlObjectConverter(string because, ContextType contextType, XmlInterpretation xmlInterpretation, params string[] expectedErrors)
        {
            var subject = new XmlObjectConverter { XmlInterpretation = xmlInterpretation };
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "e-XML#24;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, "e-XML#6;")]
        public void XmlTargetInstantiator(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlTargetInstantiator();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Create(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "e-XML#32;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, "e-XML#33;")]
        public void XmlTargetInstantiatorRemovesNamespace(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XmlTargetInstantiatorRemovesNamespace();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Create(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "e-XML#9;")]
        public void XElementToStringObjectConverter(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new XElementToStringObjectConverter();
            object context = Xml.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }
    }
}