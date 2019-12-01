using AdaptableMapper.Formats;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Formats
{
    public class NullFormatterCases
    {
        [Theory]
        [InlineData("1")]
        [InlineData("1.00")]
        [InlineData("12/12/123013213")]
        [InlineData("test")]
        [InlineData("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttest")]
        public void NullFormatterFormat(string source)
        {
            var subject = new NullFormatter();

            var result = subject.Format(source);
            result.Should().BeEquivalentTo(source);
        }
    }
}