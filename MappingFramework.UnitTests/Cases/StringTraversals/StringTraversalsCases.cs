using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.ValueMutations.Traversals;
using Xunit;

namespace MappingFramework.UnitTests.Cases.StringTraversals
{
    public class StringTraversalsCases
    {
        [Theory]
        [InlineData("Valid", '|', 2, "value1|value2|value3", "value2", 0)]
        [InlineData("InvalidEmptyString", '|', 2, "", "", 1)]
        [InlineData("InvalidPosition", '|', 5, "value1|value2|value3", "", 1)]
        [InlineData("InvalidPosition", '|', 4, "value1|value2|value3", "", 1)]
        public void SplitByCharTakePositionStringTraversal(string because, char separator, int position, string value, string expectedResult, int informationCount)
        {
            var subject = new SplitByCharTakePositionStringTraversal(separator, position);
            var context = new Context();
            
            string result = subject.GetValue(context, value);

            context.Information().Count.Should().Be(informationCount);

            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
        }
    }
}