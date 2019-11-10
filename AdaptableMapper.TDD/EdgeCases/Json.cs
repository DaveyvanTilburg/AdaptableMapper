using System;
using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Json;
using AdaptableMapper.Process;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class Json
    {
        [Fact]
        public void JsonTraversalFirstInListTemplateInvalidType()
        {
            var subject = new JsonTraversalFirstInListTemplate();
            List<Information> result = Observe(() => { subject.Traverse(string.Empty); });
            ValidateResult(result, new List<string> {"JSON#27"});
        }

        [Fact]
        public void JsonTraversalFirstInListTemplateNoResult()
        {
            var subject = new JsonTraversalFirstInListTemplate();
            List<Information> result = Observe(() => { subject.Traverse(new JObject()); });
            ValidateResult(result, new List<string> { "JSON#14", "JSON#28" });
        }

        [Fact]
        public void JsonChildCreatorInvalidTypeParent()
        {
            var subject = new JsonChildCreator();
            List<Information> result = Observe(() => { subject.CreateChildOn(string.Empty, string.Empty); });
            ValidateResult(result, new List<string> { "JSON#1" });
        }

        [Fact]
        public void JsonChildCreatorInvalidTypeTemplate()
        {
            var subject = new JsonChildCreator();
            List<Information> result = Observe(() => { subject.CreateChildOn(new JArray(), string.Empty); });
            ValidateResult(result, new List<string> { "JSON#2" });
        }

        [Fact]
        public void JsonGetScopeInvalidType()
        {
            var subject = new JsonGetScope(string.Empty);
            List<Information> result = Observe(() => { subject.GetScope(string.Empty); });
            ValidateResult(result, new List<string> { "JSON#3" });
        }

        [Fact]
        public void JsonGetScopeNoResults()
        {
            var subject = new JsonGetScope("abcd");
            List<Information> result = Observe(() => { subject.GetScope(new JObject()); });
            ValidateResult(result, new List<string> { "JSON#4" });
        }

        private void ValidateResult(IReadOnlyCollection<Information> information, IReadOnlyCollection<string> expectedCodes)
        {
            information.Count.Should().Be(expectedCodes.Count);

            foreach(string expectedCode in expectedCodes)
                information.Any(i => i.Message.Contains(expectedCode)).Should().BeTrue(expectedCode);
        }

        private List<Information> Observe(Action action)
        {
            var observer = new TestErrorObserver();
            observer.Register();

            action.Invoke();

            observer.Unregister();
            return observer.GetInformation();
        }
    }
}