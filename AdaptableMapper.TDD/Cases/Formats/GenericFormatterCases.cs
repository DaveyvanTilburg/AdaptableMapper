using System;
using System.Collections.Generic;
using AdaptableMapper.Formats;
using AdaptableMapper.Process;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Formats
{
    public class GenericFormatterCases
    {
        [Fact]
        public void DateTimeFormatterTypes()
        {
            var source = new GenericFormatter("None", "");

            source.FormatTypes.Count.Should().Be(2);
            source.FormatTypes.Should().Contain("None");
            source.FormatTypes.Should().Contain("Date");
        }

        [Theory]
        [InlineData("Empty", "Date", "", "", "w-Format#2;")]
        [InlineData("NoFormatType", "", "test", "", "e-Format#1;")]
        [InlineData("InvalidFormatType", "Date2", "test", "", "e-Format#1;")]
        [InlineData("InvalidDateTime", "Date", "test", "", "w-Format#3;")]
        public void GenericCases(string because, string formatType, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new GenericFormatter(formatType, string.Empty);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Format(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("ValidDate", "Date", "yyyy/MM/dd", "2019-12-01T00:00:00", "2019/12/01")]
        [InlineData("ValidDateISO", "Date", "o", "2019-12-01T00:00:00", "2019-12-01T00:00:00.0000000")]
        public void DateTimeFormatter(string because, string formatType, string format, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new GenericFormatter(formatType, format);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Format(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }
    }
}