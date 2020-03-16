using System;
using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Process;
using MappingFramework.Traversals.Dictionary;
using Xunit;

namespace MappingFramework.TDD.Cases.DictionaryCases
{
    public class DictionaryTraversals
    {
        [Theory]
        [InlineData("Key", DictionaryValueTypes.String, "1", "1")]
        [InlineData("Key", DictionaryValueTypes.Integer, "1", 1)]
        [InlineData("", DictionaryValueTypes.Integer, "1", 1, "e-DictionarySetValueTraversal#1;")]
        [InlineData("Key", DictionaryValueTypes.Integer, "a", null, "w-DictionarySetValueTraversal#3;")]
        public void DictionarySetValueTraversal(string key, DictionaryValueTypes dictionaryValueTypes, string value, object expectedValue, params string[] expectedErrorCodes)
        {
            var subject = new DictionarySetValueTraversal(key, dictionaryValueTypes);
            Context context = new Context(null, new Dictionary<string, object>(), null);
            List<Information> information = new Action(() => { subject.SetValue(context, null, value); }).Observe();

            if (expectedErrorCodes.Length > 0)
                information.ValidateResult(new List<string>(expectedErrorCodes));
            else
                ((Dictionary<string, object>)context.Target)[key].Should().BeEquivalentTo(expectedValue);
        }

        [Fact]
        public void DictionarySetValueTraversalInvalidType()
        {
            var subject = new DictionarySetValueTraversal("Key");
            Context context = new Context(null, "test", null);
            List<Information> information = new Action(() => { subject.SetValue(context, null, null); }).Observe();

            information.ValidateResult(new List<string> { "e-DictionarySetValueTraversal#2;" });
        }
    }
}