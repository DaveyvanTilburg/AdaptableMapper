using System;
using System.Collections.Generic;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Traversals
{
    public class Cases
    {
        [Theory]
        [InlineData("Valid", "value", "value")]
        [InlineData("Invalid", "", "", "e-GetStaticValueTraversal#1;")]
        public void GetStaticValueTraversal(string because, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new GetStaticValueTraversal(value);

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }
    }
}