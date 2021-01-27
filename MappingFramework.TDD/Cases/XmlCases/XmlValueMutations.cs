using System.Xml.Linq;
using System.Xml.XPath;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.ValueMutations;
using FluentAssertions;
using MappingFramework.Languages.Xml;
using MappingFramework.Languages.Xml.Traversals;
using Xunit;

namespace MappingFramework.TDD.Cases.XmlCases
{
    public class XmlValueMutations
    {
        [Theory]
        [InlineData("./test", "2019-12-01T00:00:10Z", XmlInterpretation.Default, "yyyy/MM/dd", "2019/12/01")]
        public void XmlSetValueTraversalWithDateFormatter(string path, string value, XmlInterpretation xmlInterpretation, string formatTemplate, string expectedResult)
        {
            ValueMutation valueMutation = new DateValueMutation { FormatTemplate = formatTemplate };
            var subject = new SetMutatedValueTraversal(new XmlSetValueTraversal(path) { XmlInterpretation = xmlInterpretation }, valueMutation);
            var context = new Context(null, XElement.Parse("<root><test></test></root>"), null);

            subject.SetValue(context, value);

            context.Information().Count.Should().Be(0);
            
            var xElementResult = (XElement)context.Target;
            XElement result = xElementResult.XPathSelectElement("./test");

            result?.Value.Should().Be(expectedResult);
        }
    }
}