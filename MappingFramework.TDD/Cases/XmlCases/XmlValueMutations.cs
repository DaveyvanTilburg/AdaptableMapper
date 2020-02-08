using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.ValueMutations;
using MappingFramework.Process;
using MappingFramework.Traversals.Xml;
using MappingFramework.Xml;
using FluentAssertions;
using Xunit;

namespace MappingFramework.TDD.Cases.XmlCases
{
    public class XmlValueMutations
    {
        [Theory]
        [InlineData("TranslateDateTime", "./test", "2019-12-01T00:00:10Z", XmlInterpretation.Default, "yyyy/MM/dd", "2019/12/01")]
        public void XmlSetValueTraversalWithDateFormatter(string because, string path, string value, XmlInterpretation xmlInterpretation, string formatTemplate, string expectedResult, params string[] expectedErrors)
        {
            ValueMutation valueMutation = new DateValueMutation(formatTemplate);
            var subject = new SetMutatedValueTraversal(new XmlSetValueTraversal(path) { XmlInterpretation = xmlInterpretation }, valueMutation);
            var context = new Context(null, XElement.Parse("<root><test></test></root>"));

            List<Information> information = new Action(() => { subject.SetValue(context, null, value); }).Observe();

            information.ValidateResult(new List<string>(expectedErrors), because);
            if (expectedErrors.Length == 0)
            {
                var xElementResult = (XElement)context.Target;
                XElement result = xElementResult.XPathSelectElement("./test");

                result?.Value.Should().Be(expectedResult);
            }
        }
    }
}