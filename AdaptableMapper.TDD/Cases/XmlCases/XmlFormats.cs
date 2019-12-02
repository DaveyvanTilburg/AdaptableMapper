using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using AdaptableMapper.Formats;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals.Xml;
using AdaptableMapper.Xml;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.XmlCases
{
    public class XmlFormats
    {
        [Theory]
        [InlineData("TranslateDateTime", "./test", "2019-12-01T00:00:10Z", XmlInterpretation.Default, "Date", "yyyy/MM/dd", "2019/12/01")]
        [InlineData("TranslateDateTime", "./test", "2019-12-01T00:00:10Z", XmlInterpretation.Default, "Date2", "2019-12-01T00:00:10Z", "", "e-Format#1;")]
        public void XmlSetValueTraversal(string because, string path, string value, XmlInterpretation xmlInterpretation, string format, string formatTemplate, string expectedResult, params string[] expectedErrors)
        {
            Formatter formatter = new GenericFormatter(format, formatTemplate);
            var subject = new XmlSetValueTraversal(path, formatter) { XmlInterpretation = xmlInterpretation };
            object context = XElement.Parse("<root><test></test></root>");

            List<Information> information = new Action(() => { subject.SetValue(context, value); }).Observe();

            information.ValidateResult(new List<string>(expectedErrors), because);
            if (expectedErrors.Length == 0)
            {
                var xElementResult = (XElement)context;
                XElement result = xElementResult.XPathSelectElement("./test");

                result?.Value.Should().Be(expectedResult);
            }
        }

        [Fact]
        public void XmlSetValueSerializeAndDeserialize()
        {
            var source = new XmlSetValueTraversal("", new GenericFormatter("Date", ""));
            string serialized = JsonSerializer.Serialize(source);
            var target = JsonSerializer.Deserialize<XmlSetValueTraversal>(serialized);

            source.Should().BeEquivalentTo(target);
        }
    }
}