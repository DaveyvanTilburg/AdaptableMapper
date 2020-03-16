using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using MappingFramework.Configuration.Dictionary;
using MappingFramework.Dictionary;
using Xunit;

namespace MappingFramework.TDD.Cases.DictionaryCases
{
    public class DictionaryConfiguration
    {
        [Theory]
        [InlineData(ContextType.EmptyObject)]
        [InlineData(ContextType.TestObject)]
        [InlineData(ContextType.InvalidObject)]
        public void DictionaryTargetInitiator(ContextType contextType)
        {
            var subject = new DictionaryTargetInstantiator();

            var context = Dictionary.CreateTarget(contextType);
            IDictionary<string, object> result = null;
            result = subject.Create(context) as IDictionary<string, object>;

            result.Should().NotBeNull();
        }

        [Fact]
        public void Test()
        {
            var subject = new EasyAccessDictionary();
            var formatter = new BinaryFormatter();

            EasyAccessDictionary result;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, subject);
                stream.Position = 0;

                result = (EasyAccessDictionary)formatter.Deserialize(stream);
            }

            result.Should().BeEquivalentTo(subject);
        }
    }
}