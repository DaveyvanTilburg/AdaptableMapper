using System;
using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.ValueMutations;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using FluentAssertions;
using Xunit;
using AdaptableMapper.ValueMutations.Traversals;

namespace AdaptableMapper.TDD.Cases.ValueMutations
{
    public class ValueMutationsCases
    {
        [Theory]
        [InlineData("InvalidDateTime", "", "test", "", "w-DateValueMutation#1;")]
        [InlineData("ValidDate", "yyyy/MM/dd", "2019-12-01T00:00:00", "2019/12/01")]
        [InlineData("ValidDate", "yyyy|MM|dd", "2019/12/19T00:00:00", "2019|12|19")]
        [InlineData("ValidDateISO", "o", "2019-12-01T00:00:00", "2019-12-01T00:00:00.0000000")]
        [InlineData("StrangeFormatTemplate1", "&#$#$", "2019-12-01T00:00:00", "&#$#$")]
        [InlineData("StrangeFormatTemplate2", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{", "2019-12-01T00:00:00", "2019-12-01T00:00:00", "e-DateValueMutation#2;")]
        [InlineData("StrangeFormatTemplate3", "yyyy345789awytUJIHSEFUH#&*$ddddMM:\":{:{\"", "2019-12-01T00:00:00", "2019345789aw19AUJI0SEU0#&*$Sunday12::{:{")]
        public void DateValueMutation(string because, string formatTemplate, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DateValueMutation(formatTemplate);

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
                new GetStaticValueTraversal(valueToReplace), 
                new GetStaticValueTraversal(newValue)
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null), value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("Valid", "this is an old message", "this is a new message")]
        [InlineData("ValidMultipleHits", "this is an old message, through luxery", "this is a new message, through hard work")]
        [InlineData("No hit", "this is an-old message", "this is an-old message")]
        [InlineData("Empty", "", "", "w-DictionaryReplaceValueMutation#2;")]
        public void DictionaryReplaceValueMutation(string because, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DictionaryReplaceValueMutation(
                new Dictionary<string, string>
                {
                    ["an old"] = "a new",
                    ["luxery"] = "hard work"
                }
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null), value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);
            if (expectedInformation.Length == 0)
                result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("Valid", '|', 2, "value1|value2|value3", "value1|silver|value3")]
        [InlineData("No hit", '|', 4, "value1|value2|value3", "value1|value2|value3", "w-SplitByCharTakePositionStringTraversal#2;")]
        public void DictionaryReplaceValueMutationTraversal(string because, char separator, int position, string value, string expectedResult, params string[] expectedInformation)
        {
            var subject = new DictionaryReplaceValueMutation(
                new Dictionary<string, string>
                {
                    ["value1"] = "bronze",
                    ["value2"] = "silver",
                    ["value3"] = "gold"
                }
            )
            {
                GetValueStringTraversal = new SplitByCharTakePositionStringTraversal(separator, position)
            };

            string result = null;
            List<Information> information = new Action(() => { result = subject.Mutate(new Context(null, null), value); }).Observe();

            information.ValidateResult(new List<string>(expectedInformation), because);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DictionaryReplaceValueMutationNoReplacesSet()
        {
            var subject = new DictionaryReplaceValueMutation(
                new Dictionary<string, string>()
            );

            List<Information> information = new Action(() => { subject.Mutate(new Context(null, null), string.Empty); }).Observe();

            information.ValidateResult(new List<string> { "e-DictionaryReplaceValueMutation#1;" });
        }
    }
}