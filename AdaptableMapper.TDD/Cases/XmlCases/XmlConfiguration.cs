using AdaptableMapper.Process;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Configuration.Xml;
using AdaptableMapper.Traversals;
using AdaptableMapper.Xml;
using Xunit;
using FluentAssertions;

namespace AdaptableMapper.TDD.Cases.XmlCases
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
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "", "e-XML#18;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "", "e-XML#19;")]
        [InlineData("Valid", ContextType.ValidAlternativeSource, XmlInterpretation.WithoutNamespace, "./Resources/SimpleRemovedNamespaceExpectedResult.xml")]
        public void XmlObjectConverter(string because, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultFile, params string[] expectedErrors)
        {
            var subject = new XmlObjectConverter { XmlInterpretation = xmlInterpretation };
            object context = Xml.CreateTarget(contextType);

            object value = null;
            List<Information> result = new Action(() => { value = subject.Convert(context); }).Observe();

            result.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;
                xElementValue.ToString().Should().Be(expectedResult);
            }
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "", "e-XML#24;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "", "e-XML#6;")]
        [InlineData("Valid", ContextType.ValidAlternativeSource, XmlInterpretation.WithoutNamespace, "./Resources/SimpleRemovedNamespaceExpectedResult.xml")]
        public void XmlTargetInstantiator(string because, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultFile, params string[] expectedErrors)
        {
            var subject = new XmlTargetInstantiator { XmlInterpretation = xmlInterpretation };
            object context = Xml.CreateTarget(contextType);

            object value = null;
            List<Information> result = new Action(() => { value = subject.Create(context); }).Observe();

            result.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;
                xElementValue.ToString().Should().Be(expectedResult);
            }
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