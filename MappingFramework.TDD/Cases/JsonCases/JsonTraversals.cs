using MappingFramework.Configuration;
using MappingFramework.Traversals.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MappingFramework.TDD.Cases.JsonCases
{
    public class JsonTraversals
    {
        [Theory]
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, 1)]
        public void JsonGetScopeTraversal(string because, string path, ContextType contextType, int informationCount)
        {
            var subject = new JsonGetListValueTraversal(path);
            Context context = new Context(Json.Stub(contextType), null, null);

            subject.GetValues(context);
            context.Information().Count.Should().Be(informationCount, because);
        }

        [Theory]
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, 1)]
        [InlineData("InvalidPath", "[]", ContextType.EmptyObject, 1)]
        [InlineData("Valid", "$.SimpleItems[0].SurName", ContextType.TestObject, 0)]
        public void JsonSetValueTraversal(string because, string path, ContextType contextType, int informationCount)
        {
            var subject = new JsonSetValueTraversal(path);
            var context = new Context(null, Json.Stub(contextType), null);

            subject.SetValue(context, null, string.Empty);
            context.Information().Count.Should().Be(informationCount, because);
        }

        [Theory]
        [InlineData("NoResult", "", ContextType.EmptyObject, 1)]
        [InlineData("EmptyResult", "$.SimpleItems[0].SurName", ContextType.TestObject, 0)]
        public void JsonGetValueTraversal(string because, string path, ContextType contextType, int informationCount)
        {
            var subject = new JsonGetValueTraversal(path);
            var context = new Context(Json.Stub(contextType), null, null);

            subject.GetValue(context);
            context.Information().Count.Should().Be(informationCount, because);
        }

        [Theory]
        [InlineData("NoParentCheck", "$", ContextType.EmptyObject, 1)]
        [InlineData("InvalidPath", "abcd", ContextType.EmptyObject, 1)]
        [InlineData("InvalidParentPath", "ab/cd", ContextType.EmptyObject, 2)]
        [InlineData("NoParent", "../", ContextType.EmptyObject, 1)]
        [InlineData("InvalidCharacters", "[]", ContextType.EmptyObject, 2)]
        [InlineData("Valid", "$.SimpleItems[0]", ContextType.TestObject, 0)]
        [InlineData("Did not end in an element that has a parent", "$", ContextType.TestObject, 1)]
        public void JsonGetTemplateTraversal(string because, string path, ContextType contextType, int informationCount)
        {
            var subject = new JsonGetTemplateTraversal(path);
            object source = Json.Stub(contextType);
            var context = new Context();

            subject.GetTemplate(context, source, new MappingCaches());
            context.Information().Count.Should().Be(informationCount, because);
        }
    }
}