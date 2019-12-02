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

            source.FormatTypes.Count.Should().Be(3);
            source.FormatTypes.Should().Contain("None");
            source.FormatTypes.Should().Contain("Date");
            source.FormatTypes.Should().Contain("Decimal2");
        }

        [Theory]
        [InlineData("Empty", "Date", "", "", "w-Format#2;")]
        [InlineData("NoFormatType", "", "test", "", "e-Format#1;")]
        [InlineData("InvalidFormatType", "Date2", "test", "", "e-Format#1;")]
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
        [InlineData("InvalidDateTime", "Date", "", "test", "", "w-Format#3;")]
        [InlineData("ValidDate", "Date", "yyyy/MM/dd", "2019-12-01T00:00:00", "2019/12/01")]
        [InlineData("ValidDateISO", "Date", "o", "2019-12-01T00:00:00", "2019-12-01T00:00:00.0000000")]
        public void DateFormatType(string because, string formatType, string formatTemplate, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new GenericFormatter(formatType, formatTemplate);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Format(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("ValidDecimal2-5", "Decimal2", ".", "36500", "365.00")]
        [InlineData("ValidDecimal2-3", "Decimal2", ".", "365", "3.65")]
        [InlineData("ValidDecimal2-2", "Decimal2", ".", "36", "0.36")]
        [InlineData("ValidDecimal2-1", "Decimal2", ".", "3", "0.03")]
        [InlineData("ValidDecimal2-0", "Decimal2", ".", "abc", "0.00")]
        [InlineData("ValidDecimal2-Comma", "Decimal2", ",", "123", "1,23")]
        [InlineData("ValidDecimal2-Gibberish", "Decimal2", "abc", "123", "1abc23")]
        public void DecimalFormatType(string because, string formatType, string formatTemplate, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new GenericFormatter(formatType, formatTemplate);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Format(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }
    }
}