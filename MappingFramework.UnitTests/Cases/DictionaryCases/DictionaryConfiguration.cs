using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Dictionary;
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

        [Fact]
        public void SerializationTest()
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