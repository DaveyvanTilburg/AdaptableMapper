using FluentAssertions;
using MappingFramework.Languages.Xml.Interpretation;
using Xunit;

namespace MappingFramework.UnitTests.Cases.XmlCases
{
    public class XmlInterpretationsCases
    {
        [Theory]
        [InlineData(".//month/day", ".//*[local-name()='month']/*[local-name()='day']")]
        [InlineData("//month/day", "//*[local-name()='month']/*[local-name()='day']")]
        [InlineData("./month/day", "./*[local-name()='month']/*[local-name()='day']")]
        [InlineData("//month//day", "//*[local-name()='month']//*[local-name()='day']")]
        [InlineData("./*", "./*")]
        [InlineData("count(./SimpleItems/SimpleItem)", "count(./*[local-name()='SimpleItems']/*[local-name()='SimpleItem'])")]
        [InlineData("concat(//SimpleItem[1]/Name,',',//SimpleItem[2]/Name)", "concat(//*[local-name()='SimpleItem'][1]/*[local-name()='Name'],',',//*[local-name()='SimpleItem'][2]/*[local-name()='Name'])")]
        [InlineData("concat(count(//day), '-days')", "concat(count(//*[local-name()='day']), '-days')")]
        [InlineData("concat(count(//month//day), ' days this year, and ', count(//month//day[@dow='sunday']), ' sundays')", "concat(count(//*[local-name()='month']//*[local-name()='day']), ' days this year, and ', count(//*[local-name()='month']//*[local-name()='day'][@dow='sunday']), ' sundays')")]
        public void Test(string path, string expectedResult)
        {
            string result = path.ConvertToInterpretation(XmlInterpretation.WithoutNamespace);
            result.Should().Be(expectedResult);
        }
    }
}