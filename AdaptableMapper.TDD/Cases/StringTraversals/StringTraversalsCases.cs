using System;
using System.Collections.Generic;
using AdaptableMapper.Process;
using AdaptableMapper.ValueMutations.Traversals;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.StringTraversals
{
    public class StringTraversalsCases
    {
        [Theory]
        [InlineData("Valid", '|', 2, "value1|value2|value3", "value2")]
        [InlineData("InvalidEmptyString", '|', 2, "", "", "w-SplitByCharTakePositionStringTraversal#1;")]
        [InlineData("InvalidPosition", '|', 5, "value1|value2|value3", "", "w-SplitByCharTakePositionStringTraversal#2;")]
        public void SplitByCharTakePositionStringTraversal(string because, char separator, int position, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new SplitByCharTakePositionStringTraversal(separator, position);

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }
    }
}