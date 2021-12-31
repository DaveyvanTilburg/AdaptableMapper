using System.Xml.Linq;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Xml;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
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

        [Theory]
        [InlineData(".//month/day", ".//*[local-name()='month']/*[local-name()='day']")]
        [InlineData("//month/day", "//*[local-name()='month']/*[local-name()='day']")]
        [InlineData("./month/day", "./*[local-name()='month']/*[local-name()='day']")]
        [InlineData("//month//day", "//*[local-name()='month']//*[local-name()='day']")]
        [InlineData("./*", "./*")]
        [InlineData("count(./SimpleItems/SimpleItem)", "count(./*[local-name()='SimpleItems']/*[local-name()='SimpleItem'])")]
        [InlineData("concat(./SimpleItem[1]/Name,',',./SimpleItem[2]/Name)", "concat(./*[local-name()='SimpleItem'][1]/*[local-name()='Name'],',',./*[local-name()='SimpleItem'][2]/*[local-name()='Name'])")]
        public void Test(string path, string expectedResult)
        {
            string result = path.ConvertToInterpretation(XmlInterpretation.WithoutNamespace);
            result.Should().Be(expectedResult);
        }
    }
}