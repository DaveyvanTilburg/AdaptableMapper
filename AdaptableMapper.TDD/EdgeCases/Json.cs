using System;
using System.Collections.Generic;
using AdaptableMapper.Json;
using AdaptableMapper.Process;
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
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> {"JSON#27"});
        }

        [Fact]
        public void JsonTraversalFirstInListTemplateNoResult()
        {
            var subject = new JsonTraversalFirstInListTemplate();
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "JSON#14", "JSON#28" });
        }

        [Fact]
        public void JsonChildCreatorInvalidTypeParent()
        {
            var subject = new JsonChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#1" });
        }

        [Fact]
        public void JsonChildCreatorInvalidTypeTemplate()
        {
            var subject = new JsonChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(new JArray(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#2" });
        }

        [Fact]
        public void JsonGetScopeInvalidType()
        {
            var subject = new JsonGetScope(string.Empty);
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#3" });
        }

        [Fact]
        public void JsonGetScopeNoResults()
        {
            var subject = new JsonGetScope("abcd");
            List<Information> result = new Action(() => { subject.GetScope(new JObject()); }).Observe();
            result.ValidateResult(new List<string> {"JSON#4"});
        }
    }
}