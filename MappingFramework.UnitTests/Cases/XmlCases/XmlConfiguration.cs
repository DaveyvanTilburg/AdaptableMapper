using System.Xml.Linq;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Interpretation;
using Xunit;

namespace MappingFramework.UnitTests.Cases.XmlCases
{
    public class XmlConfiguration
    {
        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, XmlInterpretation.Default, "", 1)]
        [InlineData("InvalidSource", ContextType.InvalidSource, XmlInterpretation.Default, "", 1)]
        [InlineData("Valid", ContextType.ValidAlternativeSource, XmlInterpretation.WithoutNamespace, "./Resources/SimpleRemovedNamespaceExpectedResult.xml", 0)]
        public void XmlObjectConverter(string because, ContextType contextType, XmlInterpretation xmlInterpretation, string expectedResultFile, int informationCount)
        {
            var subject = new XmlSourceCreator { XmlInterpretation = xmlInterpretation };
            object source = Xml.Stub(contextType);
            var context = new Context();

            object value = subject.Convert(context, source);
            value.Should().NotBeNull(because);

            context.Information().Count.Should().Be(informationCount, because);
            
            if (informationCount == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;

                var converter = new XElementToStringResultObjectCreator();
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
            var subject = new XmlTargetCreator { XmlInterpretation = xmlInterpretation };
            object source = Xml.Stub(contextType);
            var context = new Context();

            object value = subject.Create(context, source);
            value.Should().NotBeNull(because);

            context.Information().Count.Should().Be(informationCount, because);

            if (informationCount == 0)
            {
                string expectedResult = System.IO.File.ReadAllText(expectedResultFile);

                XElement xElementValue = value as XElement;

                var converter = new XElementToStringResultObjectCreator();
                var convertedResult = converter.Convert(xElementValue);
                convertedResult.Should().Be(expectedResult, because);
            }
        }
    }
}