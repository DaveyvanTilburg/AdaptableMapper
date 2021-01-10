using System;
using System.Collections.Generic;
using System.Linq;
using MappingFramework.Compositions;
using MappingFramework.Configuration;
using MappingFramework.ValueMutations;
using MappingFramework.Process;
using FluentAssertions;
using Xunit;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework.TDD.Cases.ValueMutations
{
    public class ValueMutationsCases
    {
        [Theory]
        [InlineData("InvalidDateTime", "", "", "test", "", "w-DateValueMutation#1;")]
        [InlineData("ValidDate", "yyyy/MM/dd", "", "2019-12-01T00:00:00", "2019/12/01")]
        [InlineData("ValidDate", "yyyy|MM|dd", "", "2019/12/19T00:00:00", "2019|12|19")]
        [InlineData("ValidDateISO", "o", "", "2019-12-01T00:00:00", "2019-12-01T00:00:00.0000000")]
        [InlineData("S", "s", "", "2019-12-01T00:00:00", "2019-12-01T00:00:00")]
        [InlineData("StrangeFormatTemplate1", "&#$#$", "", "2019-12-01T00:00:00", "&#$#$")]
        [InlineData("StrangeFormatTemplate2", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{", "", "2019-12-01T00:00:00", "2019-12-01T00:00:00", "e-DateValueMutation#2;")]
        [InlineData("StrangeFormatTemplate3", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{\"", "", "2019-12-01T00:00:00", "2019345789aw19aUJI0SEU0#&*$Sunday12::{:{")]
        [InlineData("ValidDateRead", "yyyy/MM/dd", "yyMMdd", "911230", "1991/12/30")]
        [InlineData("ValidDateInvalidRead", "yyyy/MM/dd", "yyMdd", "91230", "911230", "w-DateValueMutation#3;")]
        [InlineData("ValidDateDirectRead", "yyyy/MM/dd", "", "2019-12-01T00:00:00", "2019/12/01")]
        [InlineData("ValidDateDirectReadWrite", "", "", "2019-12-01", "2019-12-01T00:00:00")]

        public void DateValueMutation(string because, string formatTemplate, string readFormatTemplate, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DateValueMutation { FormatTemplate = formatTemplate, ReadFormatTemplate = readFormatTemplate };

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(null, value); }).Observe();

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
        [InlineData("Empty", ".", 5, "", "0.00000")]
        public void NumberValueMutation(string because, string formatTemplate, int numberOfDecimals, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new NumberValueMutation(formatTemplate, numberOfDecimals);

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(null, value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Fact]
        public void ListValueMutation()
        {
            var subject = new ListOfValueMutations();
            subject.ValueMutations.Add(new NumberValueMutation(".", 2));

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(null, "36500"); }).Observe();

            information.Count.Should().Be(0);
            result.Should().Be("365.00");
        }

        [Fact]
        public void ListValueMutationEmpty()
        {
            var subject = new ListOfValueMutations();

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(null, "36500"); }).Observe();

            information.Count.Should().Be(1);
            information.Any(i => i.Message.StartsWith("ListOfValueMutations#1;")).Should().BeTrue();
        }

        [Theory]
        [InlineData("Valid", "an old", "a new", "this is an old message", "this is a new message")]
        [InlineData("Invalid", "an old", "a new", "this is an old message", "this is a new message")]
        [InlineData("InvalidEmpty", "an old", "a new", "", "this is a new message", "w-ReplaceMutation#1;")]
        [InlineData("InvalidEmptyValue", "", "a new", "this is an old message", "this is a new message", "e-GetStaticValueTraversal#1;")]
        [InlineData("InvalidEmptyReplaceValue", "an old", "", "this is an old message", "this is a new message", "e-GetStaticValueTraversal#1;")]
        public void ReplaceValueMutation(string because, string valueToReplace, string newValue, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new ReplaceValueMutation(
                new GetStaticValue(valueToReplace),
                new GetStaticValue(newValue)
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(false, true, "e-ReplaceValueMutation#2;")]
        [InlineData(true, false, "e-ReplaceValueMutation#3;")]
        public void ReplaceValueMutationNullsChecks(bool shouldCreateGetValueStringTraversal, bool shouldCreateGetValueTraversal, params string[] expectedErrorCodes)
        {
            var subject = new ReplaceValueMutation(
                shouldCreateGetValueStringTraversal ? new GetStaticValue(string.Empty) : null,
                shouldCreateGetValueTraversal ? new GetStaticValue(string.Empty) : null
            );

            List<Information> information = new Action(() => { subject.Mutate(new Context(null, null, null), string.Empty); }).Observe();

            information.ValidateResult(new List<string>(expectedErrorCodes), "should check nulls");
        }

        [Theory]
        [InlineData("Valid1", "Old", "New")]
        [InlineData("Valid2", "1", "2")]
        [InlineData("No Hit", "Something", "Something")]
        [InlineData("Empty", "", "", "w-DictionaryReplaceValueMutation#2;")]
        public void DictionaryReplaceValueMutation(string because, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DictionaryReplaceValueMutation(
                new List<DictionaryReplaceValueMutation.ReplaceValue>
                {
                    new DictionaryReplaceValueMutation.ReplaceValue
                    {
                        ValueToReplace = "Old",
                        NewValue = "New"
                    },
                    new DictionaryReplaceValueMutation.ReplaceValue
                    {
                        ValueToReplace = "1",
                        NewValue = "2"
                    }
                }
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("Valid", '|', 2, "value1|value2|value3", "value1|silver|value3")]
        [InlineData("No hit", '|', 4, "value1|value2|value3", "value1|value2|value3", "w-SplitByCharTakePositionStringTraversal#2;")]
        public void DictionaryReplaceValueMutationWithTraversal(string because, char separator, int position, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DictionaryReplaceValueMutation(
                new List<DictionaryReplaceValueMutation.ReplaceValue>
                {
                    new DictionaryReplaceValueMutation.ReplaceValue
                    {
                        ValueToReplace = "value1",
                        NewValue = "bronze"
                    },
                    new DictionaryReplaceValueMutation.ReplaceValue
                    {
                        ValueToReplace = "value2",
                        NewValue = "silver"
                    },
                    new DictionaryReplaceValueMutation.ReplaceValue
                    {
                        ValueToReplace = "value3",
                        NewValue = "gold"
                    }
                }
            )
            {
                GetValueStringTraversal = new SplitByCharTakePositionStringTraversal(separator, position)
            };

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("Valid", '|', 2, "value1|value2|value3", "value2")]
        [InlineData("No hit", '|', 4, "value1|value2|value3", "", "w-SplitByCharTakePositionStringTraversal#2;")]
        public void SubstringValueMutation(string because, char separator, int position, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new SubstringValueMutation
            (
                new SplitByCharTakePositionStringTraversal(separator, position)
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void SubstringValueMutationNotSet()
        {
            var subject = new SubstringValueMutation(null);
            List<Information> information = new Action(() => { subject.Mutate(new Context(null, null, null), ""); }).Observe();
            information.ValidateResult(new List<string> { "e-SubstringValueMutation#1;" });
        }

        [Fact]
        public void DictionaryReplaceValueMutationNoReplacesSet()
        {
            var subject = new DictionaryReplaceValueMutation(
                new List<DictionaryReplaceValueMutation.ReplaceValue>()
            );

            List<Information> information = new Action(() => { subject.Mutate(new Context(null, null, null), string.Empty); }).Observe();

            information.ValidateResult(new List<string> { "e-DictionaryReplaceValueMutation#1;" });
        }

        [Theory]
        [InlineData(",", 0, "6", "0,1,2,3,4,5")]
        [InlineData(",", 2, "6", "2,3,4,5,6,7")]
        [InlineData("sep", 0, "3", "0sep1sep2")]
        [InlineData("|", 0, "abcd", "", "e-CreateSeparatedRangeFromNumberValueMutation#1;")]
        public void CreateSeparatedRangeFromNumberValueMutation(string separator, int startingNumber, string input, string expectedResult, params string[] expectedErrorCodes)
        {
            var subject = new CreateSeparatedRangeFromNumberValueMutation(separator) { StartingNumber = startingNumber };

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), input); }).Observe();

            information.ValidateResult(new List<string>(expectedErrorCodes));
            if (expectedErrorCodes.Length == 0)
                result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("abcd", "ABCD")]
        [InlineData("1", "1")]
        [InlineData("ABCD", "ABCD")]
        public void ToUpperValueMutation(string input, string expectedResult)
        {
            var subject = new ToUpperValueMutation();

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), input); }).Observe();

            information.Count.Should().Be(0);
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

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), input); }).Observe();

            information.Count.Should().Be(0);
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

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), input); }).Observe();

            information.Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(" test ", " ", "test")]
        [InlineData(" test)", ")", " test")]
        [InlineData(" tsest)", " et)", "ses")]
        public void TrimValueMutation(string input, string characters, string expectedResult)
        {
            var subject = new TrimValueMutation(characters);

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null, null), input); }).Observe();

            information.Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }
    }
}