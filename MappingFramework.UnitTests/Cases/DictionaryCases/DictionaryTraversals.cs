using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Dictionary.Traversals;
using Xunit;

namespace MappingFramework.UnitTests.Cases.DictionaryCases
{
    public class DictionaryTraversals
    {
        [Theory]
        [InlineData("Key", DictionaryValueTypes.String, "1", "1", 0)]
        [InlineData("Key", DictionaryValueTypes.Integer, "1", 1, 0)]
        [InlineData("", DictionaryValueTypes.Integer, "1", 1, 1)]
        [InlineData("Key", DictionaryValueTypes.Integer, "a", null, 1)]
        public void DictionarySetValueTraversal(string key, DictionaryValueTypes dictionaryValueTypes, string value, object expectedValue, int informationCount)
        {
            var subject = new DictionarySetValueTraversal(key, dictionaryValueTypes);
            Context context = new Context(null, new Dictionary<string, object>(), null);

            subject.SetValue(context, value);

            if (informationCount > 0)
                context.Information().Count.Should().Be(informationCount);
            else
                ((Dictionary<string, object>)context.Target)[key].Should().BeEquivalentTo(expectedValue);
        }
    }
}