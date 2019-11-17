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

        [Fact]
        public void JsonGetSearchValueInvalidType()
        {
            var subject = new JsonGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#5" });
        }

        [Fact]
        public void JsonGetSearchValueEmptySearchValuePath()
        {
            var subject = new JsonGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "JSON#31" });
        }

        [Fact]
        public void JsonGetSearchValueNoResultOnSearchValuePath()
        {
            var subject = new JsonGetSearchValue(string.Empty, "abcd");
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "JSON#14", "JSON#6", "JSON#7" });
        }

        [Fact]
        public void JsonGetSearchValueNoResultOnActualPath()
        {
            var subject = new JsonGetSearchValue(string.Empty, "$.SimpleItems[0].SimpleItem.Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "JSON#6", "JSON#8" });
        }

        [Fact]
        public void JsonSetValueInvalidType()
        {
            var subject = new JsonSetValue(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#18" });
        }

        [Fact]
        public void JsonSetValueNoResults()
        {
            var subject = new JsonSetValue("abcd");
            List<Information> result = new Action(() => { subject.SetValue(new JObject(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#30" });
        }

        [Fact]
        public void JsonGetValueInvalidType()
        {
            var subject = new JsonGetValue(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#10" });
        }

        [Fact]
        public void JsonGetValueNoResult()
        {
            var subject = new JsonGetValue(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "JSON#6", "JSON#11" });
        }

        [Fact]
        public void JsonObjectConverterInvalidType()
        {
            var subject = new JsonObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "JSON#12" });
        }

        [Fact]
        public void JsonObjectConverterInvalidSource()
        {
            var subject = new JsonObjectConverter();
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "JSON#13" });
        }

        [Fact]
        public void JsonTargetInstantiatorInvalidType()
        {
            var subject = new JsonTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(0); }).Observe();
            result.ValidateResult(new List<string> { "JSON#26" });
        }

        [Fact]
        public void JsonTargetInstantiatorInvalidSource()
        {
            var subject = new JsonTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "JSON#20" });
        }

        [Fact]
        public void JsonTraversalInvalidType()
        {
            var subject = new JsonTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "JSON#21" });
        }

        [Fact]
        public void JsonTraversalInvalidPath()
        {
            var subject = new JsonTraversal("abcd");
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "JSON#22", "JSON#14" });
        }

        private JToken CreateTestData()
            => JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));
    }
}