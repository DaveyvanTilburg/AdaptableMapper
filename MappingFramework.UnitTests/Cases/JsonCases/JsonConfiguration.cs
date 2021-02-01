using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Json.Configuration;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MappingFramework.UnitTests.Cases.JsonCases
{
    public class JsonConfiguration
    {
        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, 1)]
        [InlineData("InvalidSource", ContextType.InvalidSource, 1)]
        [InlineData("Valid", ContextType.ValidSource, 0)]
        public void JsonObjectConverter(string because, ContextType contextType, int informationCount)
        {
            var subject = new JsonSourceCreator();
            
            object source = Json.Stub(contextType);
            var context = new Context();

            var result = subject.Convert(context, source);
            context.Information().Count.Should().Be(informationCount, because);
            result.Should().BeAssignableTo<JToken>();
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, 1)]
        [InlineData("InvalidSource", ContextType.InvalidSource, 1)]
        [InlineData("ValidSource", ContextType.ValidSource, 0)]
        public void JsonTargetInstantiatorInvalidType(string because, ContextType contextType, int informationCount)
        {
            var subject = new JsonTargetCreator();
            
            object source = Json.Stub(contextType);
            var context = new Context();

            var result = subject.Create(context, source);
            context.Information().Count.Should().Be(informationCount, because);
            result.Should().BeAssignableTo<JToken>();
        }
    }
}