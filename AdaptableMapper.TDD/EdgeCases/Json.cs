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
            result.ValidateResult(new List<string> { "e-JSON#27" });
        }

        [Fact]
        public void JsonTraversalFirstInListTemplateNoResult()
        {
            var subject = new JsonTraversalFirstInListTemplate();
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#28" });
        }

        [Fact]
        public void JsonChildCreatorInvalidTypeParent()
        {
            var subject = new JsonChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#1" });
        }

        [Fact]
        public void JsonChildCreatorInvalidType()
        {
            var subject = new JsonChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(new JArray(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#2" });
        }

        [Fact]
        public void JsonGetScopeInvalidType()
        {
            var subject = new JsonGetScope(string.Empty);
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#3" });
        }

        [Fact]
        public void JsonGetScopeNoResults()
        {
            var subject = new JsonGetScope("abcd");
            List<Information> result = new Action(() => { subject.GetScope(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#4" });
        }

        [Fact]
        public void JsonGetSearchValueInvalidType()
        {
            var subject = new JsonGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#5" });
        }

        [Fact]
        public void JsonGetSearchValueEmptySearchValuePath()
        {
            var subject = new JsonGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#31" });
        }

        [Fact]
        public void JsonGetSearchValueNoResultOnSearchValuePath()
        {
            var subject = new JsonGetSearchValue(string.Empty, "abcd");
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#6", "w-JSON#7" });
        }

        [Fact]
        public void JsonGetSearchValueNoResultOnActualPath()
        {
            var subject = new JsonGetSearchValue(string.Empty, "$.SimpleItems[0].SimpleItem.Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#6", "w-JSON#8" });
        }

        [Fact]
        public void JsonSetValueInvalidType()
        {
            var subject = new JsonSetValue(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#18" });
        }

        [Fact]
        public void JsonSetValueNoResults()
        {
            var subject = new JsonSetValue("abcd");
            List<Information> result = new Action(() => { subject.SetValue(new JObject(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#30" });
        }

        [Fact]
        public void JsonSetValueInvalidPath()
        {
            var subject = new JsonSetValue("[]");
            List<Information> result = new Action(() => { subject.SetValue(new JObject(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#29", "e-JSON#30" });
        }

        [Fact]
        public void JsonGetValueInvalidType()
        {
            var subject = new JsonGetValue(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#10" });
        }

        [Fact]
        public void JsonGetValueNoResult()
        {
            var subject = new JsonGetValue(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#6", "w-JSON#11" });
        }

        [Fact]
        public void JsonObjectConverterInvalidType()
        {
            var subject = new JsonObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#12" });
        }

        [Fact]
        public void JsonObjectConverterInvalidSource()
        {
            var subject = new JsonObjectConverter();
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#13" });
        }

        [Fact]
        public void JsonTargetInstantiatorInvalidType()
        {
            var subject = new JsonTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(0); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#26" });
        }

        [Fact]
        public void JsonTargetInstantiatorInvalidSource()
        {
            var subject = new JsonTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#20" });
        }

        [Fact]
        public void JsonTraversalInvalidType()
        {
            var subject = new JsonTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#21" });
        }

        [Fact]
        public void JsonTraversalInvalidPath()
        {
            var subject = new JsonTraversal("abcd");
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#22" });
        }

        [Fact]
        public void JsonTraversalInvalidCharacters()
        {
            var subject = new JsonTraversal("[]");
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#28", "w-JSON#22" });
        }

        [Fact]
        public void JsonTraversalInvalidParentPath()
        {
            var subject = new JsonTraversal("ab/cd");
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#15", "w-JSON#22" });
        }

        [Fact]
        public void JsonTraversalNoParent()
        {
            var subject = new JsonTraversal("../");
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#22" });
        }

        [Fact]
        public void JsonTraversalTemplateInvalidType()
        {
            var subject = new JsonTraversalTemplate(string.Empty);
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#23" });
        }

        [Fact]
        public void JsonTraversalTemplateInvalidPath()
        {
            var subject = new JsonTraversalTemplate("abcd");
            List<Information> result = new Action(() => { subject.Traverse(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#24" });
        }

        [Fact]
        public void JTokenToStringObjectConverterInvalidType()
        {
            var subject = new JTokenToStringObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#25" });
        }

        private JToken CreateTestData()
            => JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));
    }
}