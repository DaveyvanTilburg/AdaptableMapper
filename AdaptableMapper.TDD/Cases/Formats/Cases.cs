using System;
using System.Collections.Generic;
using AdaptableMapper.Formats;
using AdaptableMapper.Process;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Formats
{
    public class Cases
    {
        [Theory]
        [InlineData("InvalidDateTime", "", "test", "", "w-DateFormatter#1;")]
        [InlineData("ValidDate", "yyyy/MM/dd", "2019-12-01T00:00:00", "2019/12/01")]
        [InlineData("ValidDateISO", "o", "2019-12-01T00:00:00", "2019-12-01T00:00:00.0000000")]
        [InlineData("StrangeFormatTemplate1", "&#$#$", "2019-12-01T00:00:00", "&#$#$")]
        [InlineData("StrangeFormatTemplate2", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{", "2019-12-01T00:00:00", "2019-12-01T00:00:00", "e-DateFormatter#2;")]
        [InlineData("StrangeFormatTemplate3", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{\"", "2019-12-01T00:00:00", "2019345789aw19AUJI0SEU0#&*$Sunday12::{:{")]
        public void DateFormatter(string because, string formatTemplate, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DateFormatter(formatTemplate);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Format(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("ValidDecimal5-7", ".", 5, "3650000", "36.50000")]
        [InlineData("ValidDecimal5-5", ".", 5, "36500", "0.36500")]
        [InlineData("ValidDecimal2-5", ".", 2, "36500", "365.00")]
        [InlineData("ValidDecimal2-3", ".", 2, "365", "3.65")]
        [InlineData("ValidDecimal2-2", ".", 2, "36", "0.36")]
        [InlineData("ValidDecimal2-1", ".", 2, "3", "0.03")]
        [InlineData("ValidDecimal2-0", ".", 2, "abc", "0.00")]
        [InlineData("ValidDecimal2-Comma", ",", 2, "123", "1,23")]
        [InlineData("ValidDecimal2-Gibberish", "abc", 2, "123", "1abc23")]
        [InlineData("ValidDecimalNoSeparator7", "", 7, "abc", "")]
        [InlineData("ValidDecimalNoSeparator0", "", 0, "10", "10")]
        public void NumberFormatter(string because, string formatTemplate, int numberOfDecimals, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new NumberFormatter(formatTemplate, numberOfDecimals);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Format(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }
    }
}