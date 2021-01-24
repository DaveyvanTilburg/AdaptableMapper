using System.Xml.Linq;
using MappingFramework.Configuration.Xml;
using MappingFramework.Xml;
using Xunit;
using FluentAssertions;
using MappingFramework.Configuration;

namespace MappingFramework.TDD.Cases.XmlCases
{
    public class XmlConfiguration
    {
        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "", 1)]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "", 1)]
        [InlineData("Valid", ContextType.ValidAlternativeSource, XmlInterpretation.WithoutNamespace, "./Resources/SimpleRemovedNamespaceExpectedResult.xml", 0)]
        public void XmlObjectConverter(string because, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultFile, int informationCount)
        {
            var subject = new XmlObjectConverter { XmlInterpretation = xmlInterpretation };
            object source = Xml.Stub(contextType);
            var context = new Context();

            object value = subject.Convert(context, source);
            value.Should().NotBeNull(because);

            context.Information().Count.Should().Be(informationCount, because);
            
            if (informationCount == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;

                var converter = new XElementToStringObjectConverter();
                var convertedResult = converter.Convert(xElementValue);
                convertedResult.Should().Be(expectedResult, because);
            }
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "", 1)]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "", 1)]
        [InlineData("Valid", ContextType.ValidAlternativeSource, XmlInterpretation.WithoutNamespace, "./Resources/SimpleRemovedNamespaceExpectedResult.xml", 0)]
        public void XmlTargetInstantiator(string because, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultFile, int informationCount)
        {
            var subject = new XmlTargetInstantiator { XmlInterpretation = xmlInterpretation };
            object source = Xml.Stub(contextType);
            var context = new Context();

            object value = subject.Create(context, source);
            value.Should().NotBeNull(because);

            context.Information().Count.Should().Be(informationCount, because);

            if (informationCount == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;

                var converter = new XElementToStringObjectConverter();
                var convertedResult = converter.Convert(xElementValue);
                convertedResult.Should().Be(expectedResult, because);
            }
        }
    }
}