using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;
using Xunit;

namespace MappingFramework.UnitTests.Cases.ValueMutations
{
    public class ValueMutationsCases
    {
        [Theory]
        [InlineData("InvalidDateTime", "", "", "test", "", 1)]
        [InlineData("ValidDate", "yyyy/MM/dd", "", "2019-12-01T00:00:00", "2019/12/01", 0)]
        [InlineData("ValidDate", "yyyy|MM|dd", "", "2019/12/19T00:00:00", "2019|12|19", 0)]
        [InlineData("ValidDateISO", "o", "", "2019-12-01T00:00:00", "2019-12-01T00:00:00.0000000", 0)]
        [InlineData("S", "s", "", "2019-12-01T00:00:00", "2019-12-01T00:00:00", 0)]
        [InlineData("StrangeFormatTemplate1", "&#$#$", "", "2019-12-01T00:00:00", "&#$#$", 0)]
        [InlineData("StrangeFormatTemplate2", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{", "", "2019-12-01T00:00:00", "2019-12-01T00:00:00", 1)]
        [InlineData("StrangeFormatTemplate3", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{\"", "", "2019-12-01T00:00:00", "2019345789aw19aUJI0SEU0#&*$Sunday12::{:{", 0)]
        [InlineData("ValidDateRead", "yyyy/MM/dd", "yyMMdd", "911230", "1991/12/30", 0)]
        [InlineData("ValidDateInvalidRead", "yyyy/MM/dd", "yyMdd", "91230", "911230", 1)]
        [InlineData("ValidDateDirectRead", "yyyy/MM/dd", "", "2019-12-01T00:00:00", "2019/12/01", 0)]
        [InlineData("ValidDateDirectReadWrite", "", "", "2019-12-01", "2019-12-01T00:00:00", 0)]

        public void DateValueMutation(string because, string formatTemplate, string readFormatTemplate, string value, string expectedResult, int informationCount)
        {
            var subject = new DateValueMutation { FormatTemplate = formatTemplate, ReadFormatTemplate = readFormatTemplate };
            var context = new Context();
            
            string result = subject.Mutate(context, value); ;

            context.Information().Count.Should().Be(informationCount);
            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
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
        [InlineData("Empty", ".", 5, "", "0.00000")]
        public void NumberValueMutation(string because, string formatTemplate, int numberOfDecimals, string value, string expectedResult)
        {
            var subject = new NumberValueMutation(formatTemplate, numberOfDecimals);
            var context = new Context();
            
            string result = subject.Mutate(context, value); ;

            context.Information().Count.Should().Be(0);
            result.Should().Be(expectedResult, because);
        }

        [Fact]
        public void ListValueMutation()
        {
            var subject = new ListOfValueMutations();
            subject.ValueMutations.Add(new NumberValueMutation(".", 2));
            var context = new Context();

            string result = subject.Mutate(context, "36500");

            context.Information().Count.Should().Be(0);
            result.Should().Be("365.00");
        }

        [Fact]
        public void ListValueMutationEmpty()
        {
            var subject = new ListOfValueMutations();
            var context = new Context();
            
            string result = subject.Mutate(context, "36500");

            context.Information().Count.Should().Be(0);
            result.Should().Be("36500");
        }

        [Theory]
        [InlineData("Valid", "an old", "a new", "this is an old message", "this is a new message", 0)]
        [InlineData("Invalid", "an old", "a new", "this is an old message", "this is a new message", 0)]
        [InlineData("EmptyIsValid", "an old", "a new", "", "", 0)]
        public void ReplaceValueMutation(string because, string valueToReplace, string newValue, string value, string expectedResult, int informationCount)
        {
            var subject = new ReplaceValueMutation(
                new GetStaticValue(valueToReplace),
                new GetStaticValue(newValue)
            );
            var context = new Context();

            string result = subject.Mutate(context, value);

            context.Information().Count.Should().Be(informationCount);
            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
        }
        
        [Theory]
        [InlineData("Valid1", "Old", "New", 0)]
        [InlineData("Valid2", "1", "2", 0)]
        [InlineData("No Hit", "Something", "Something", 0)]
        [InlineData("EmptyIsValid", "", "", 0)]
        public void DictionaryReplaceValueMutation(string because, string value, string expectedResult, int informationCount)
        {
            var subject = new DictionaryReplaceValueMutation(
                new List<DictionaryReplaceValueMutation.ReplaceValue>
                {
                    new()
                    {
                        ValueToReplace = "Old",
                        NewValue = "New"
                    },
                    new()
                    {
                        ValueToReplace = "1",
                        NewValue = "2"
                    }
                }
            );
            var context = new Context();

            string result = subject.Mutate(context, value);

            context.Information().Count.Should().Be(informationCount);
            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
        }

        [Theory]
        [InlineData("Valid", '|', 2, "value1|value2|value3", "value1|silver|value3", 0)]
        [InlineData("No hit", '|', 4, "value1|value2|value3", "value1|value2|value3", 1)]
        public void DictionaryReplaceValueMutationWithTraversal(string because, char separator, int position, string value, string expectedResult, int informationCount)
        {
            var subject = new DictionaryReplaceValueMutation(
                new List<DictionaryReplaceValueMutation.ReplaceValue>
                {
                    new()
                    {
                        ValueToReplace = "value1",
                        NewValue = "bronze"
                    },
                    new()
                    {
                        ValueToReplace = "value2",
                        NewValue = "silver"
                    },
                    new()
                    {
                        ValueToReplace = "value3",
                        NewValue = "gold"
                    }
                }
            )
            {
                GetValueStringTraversal = new SplitByCharTakePositionStringTraversal(separator, position)
            };
            var context = new Context();

            string result = subject.Mutate(context, value);

            context.Information().Count.Should().Be(informationCount);
            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
        }

        [Theory]
        [InlineData("Valid", '|', 2, "value1|value2|value3", "value2", 0)]
        [InlineData("No hit", '|', 4, "value1|value2|value3", "", 1)]
        public void SubstringValueMutation(string because, char separator, int position, string value, string expectedResult, int informationCount)
        {
            var subject = new SubstringValueMutation
            (
                new SplitByCharTakePositionStringTraversal(separator, position)
            );
            var context = new Context();

            string result = subject.Mutate(context, value);

            context.Information().Count.Should().Be(informationCount);
            if (informationCount == 0)
                result.Should().Be(expectedResult, because);
        }

        [Fact]
        public void DictionaryReplaceValueMutationNoReplacesSet()
        {
            var subject = new DictionaryReplaceValueMutation(
                new List<DictionaryReplaceValueMutation.ReplaceValue>()
            );
            var context = new Context();

            subject.Mutate(context, string.Empty);

            context.Information().Count.Should().Be(0);
        }

        [Theory]
        [InlineData(",", 0, "6", "0,1,2,3,4,5", 0)]
        [InlineData(",", 2, "6", "2,3,4,5,6,7", 0)]
        [InlineData("sep", 0, "3", "0sep1sep2", 0)]
        [InlineData("|", 0, "abcd", "", 1)]
        public void CreateSeparatedRangeFromNumberValueMutation(string separator, int startingNumber, string value, string expectedResult, int informationCount)
        {
            var subject = new CreateSeparatedRangeFromNumberValueMutation(separator) { StartingNumber = startingNumber };
            var context = new Context();

            string result = subject.Mutate(context, value);

            context.Information().Count.Should().Be(informationCount);
            if (informationCount == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("abcd", "ABCD")]
        [InlineData("1", "1")]
        [InlineData("ABCD", "ABCD")]
        public void ToUpperValueMutation(string input, string expectedResult)
        {
            var subject = new ToUpperValueMutation();
            var context = new Context();
            
            string result = subject.Mutate(context, input);

            context.Information().Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("ABCD", "abcd")]
        [InlineData("1", "1")]
        [InlineData("abcd", "abcd")]
        public void ToLowerValueMutation(string input, string expectedResult)
        {
            var subject = new ToLowerValueMutation();
            var context = new Context();

            string result = subject.Mutate(context, input);

            context.Information().Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("value", "this is a {0}", "this is a value")]
        [InlineData("", "", "")]
        [InlineData("value", "", "")]
        [InlineData("", "this is a", "")]
        public void PlaceholderValueMutation(string input, string placeholder, string expectedResult)
        {
            var subject = new PlaceholderValueMutation(placeholder);
            var context = new Context();

            string result = subject.Mutate(context, input);

            context.Information().Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(" test ", " ", "test")]
        [InlineData(" test)", ")", " test")]
        [InlineData(" tsest)", " et)", "ses")]
        public void TrimValueMutation(string input, string characters, string expectedResult)
        {
            var subject = new TrimValueMutation(characters);
            var context = new Context();

            string result = subject.Mutate(context, input);

            context.Information().Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }
    }
}