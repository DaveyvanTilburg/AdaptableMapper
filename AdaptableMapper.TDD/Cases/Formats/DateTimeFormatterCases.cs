using System;
using System.Collections.Generic;
using AdaptableMapper.Formats;
using AdaptableMapper.Process;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Formats
{
    public class DateTimeFormatterCases
    {
        [Fact]
        public void DateTimeFormatterTypes()
        {
            var source = new DateTimeFormatter();

            source.FormatTypes.Count.Should().Be(2);
            source.FormatTypes.Should().Contain("Date");
            source.FormatTypes.Should().Contain("ISO8601");
        }

        [Theory]
        [InlineData("Empty", "Date", "", "", "w-Format#2;")]
        [InlineData("NoFormatType", "", "test", "", "e-Format#1;")]
        [InlineData("InvalidFormatType", "Date2", "test", "", "e-Format#1;")]
        [InlineData("InvalidDateTime", "Date", "test", "", "w-Format#3;")]
        [InlineData("ValidDate", "Date", "2019-12-01T00:00:00", "2019/12/01")]
        [InlineData("ValidDateISO", "ISO8601", "2019-12-01T00:00:00", "2019-12-01T00:00:00.0000000")]
        public void DateTimeFormatter(string because, string formatType, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DateTimeFormatter(formatType);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Format(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }
    }
}