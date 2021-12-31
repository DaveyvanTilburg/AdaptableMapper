using System.Collections.Generic;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Dictionary.Configuration;
using Xunit;

namespace MappingFramework.UnitTests.Cases.DictionaryCases
{
    public class DictionaryConfiguration
    {
        [Theory]
        [InlineData(ContextType.EmptyObject)]
        [InlineData(ContextType.TestObject)]
        [InlineData(ContextType.InvalidObject)]
        public void DictionaryTargetInitiator(ContextType contextType)
        {
            var subject = new DictionaryTargetCreator();
            var source = Dictionary.CreateTarget(contextType);
            var context = new Context();
            
            IDictionary<string, object> result = subject.Create(context, source) as IDictionary<string, object>;

            result.Should().NotBeNull();
            context.Information().Count.Should().Be(0);
        }
    }
}