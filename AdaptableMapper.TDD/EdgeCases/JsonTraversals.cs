using System;
using System.Collections.Generic;
using AdaptableMapper.Json;
using AdaptableMapper.Process;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class JsonTraversals
    {
        [Fact]
        public void JsonGetScopeTraversal_InvalidType()
        {
            var subject = new JsonGetScopeTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#3;" });
        }

        [Fact]
        public void JsonGetScopeTraversal_NoResults()
        {
            var subject = new JsonGetScopeTraversal("abcd");
            List<Information> result = new Action(() => { subject.GetScope(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#4;" });
        }



        [Fact]
        public void JsonGetSearchValueTraversal_InvalidType()
        {
            var subject = new JsonGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#5;" });
        }

        [Fact]
        public void JsonGetSearchValueTraversal_EmptySearchPathValue()
        {
            var subject = new JsonGetSearchValueTraversal(string.Empty, "$.SimpleItems[0].SimpleItem.SurName");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#7;" });
        }

        [Fact]
        public void JsonGetSearchValueTraversal_EmptyActualPath()
        {
            var subject = new JsonGetSearchValueTraversal("$.SimpleItems[0].SimpleItem.SurName", "$.SimpleItems[0].SimpleItem.Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#8;" });
        }

        [Fact]
        public void JsonGetSearchValueTraversal_EmptySearchValuePath()
        {
            var subject = new JsonGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#31;" });
        }

        [Fact]
        public void JsonGetSearchValueTraversal_NoResultOnSearchValuePath()
        {
            var subject = new JsonGetSearchValueTraversal(string.Empty, "abcd");
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#6;" });
        }

        [Fact]
        public void JsonGetSearchValueTraversal_NoResultOnActualPath()
        {
            var subject = new JsonGetSearchValueTraversal(string.Empty, "$.SimpleItems[0].SimpleItem.Id");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#6;" });
        }



        [Fact]
        public void JsonSetValueTraversal_InvalidType()
        {
            var subject = new JsonSetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#18;" });
        }

        [Fact]
        public void JsonSetValueTraversalNoResults()
        {
            var subject = new JsonSetValueTraversal("abcd");
            List<Information> result = new Action(() => { subject.SetValue(new JObject(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#30;" });
        }

        [Fact]
        public void JsonSetValueTraversalInvalidPath()
        {
            var subject = new JsonSetValueTraversal("[]");
            List<Information> result = new Action(() => { subject.SetValue(new JObject(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#29;" });
        }



        [Fact]
        public void JsonGetValueTraversal_InvalidType()
        {
            var subject = new JsonGetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#10;" });
        }

        [Fact]
        public void JsonGetValueTraversal_NoResult()
        {
            var subject = new JsonGetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#6;" });
        }

        [Fact]
        public void JsonGetValueTraversal_EmptyResult()
        {
            var subject = new JsonGetValueTraversal("$.SimpleItems[0].SimpleItem.SurName");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestData()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#11;" });
        }



        [Fact]
        public void JsonGetTemplateTraversal_InvalidType()
        {
            var subject = new JsonGetTemplateTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Get(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#23;" });
        }

        [Fact]
        public void JsonGetTemplateTraversal_NoParentCheck()
        {
            var subject = new JsonGetTemplateTraversal("$");
            List<Information> result = new Action(() => { subject.Get(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#9;" });
        }

        [Fact]
        public void JsonGetTemplateTraversal_InvalidPath()
        {
            var subject = new JsonGetTemplateTraversal("abcd");
            List<Information> result = new Action(() => { subject.Get(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#24;" });
        }

        [Fact]
        public void JsonGetTemplateTraversal_InvalidParentPath()
        {
            var subject = new JsonGetTemplateTraversal("ab/cd");
            List<Information> result = new Action(() => { subject.Get(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#15;", "w-JSON#24;" }); //Preferred cascade, the error is an extra notification of something wrong with the path
        }

        [Fact]
        public void JsonGetTemplateTraversal_NoParent()
        {
            var subject = new JsonGetTemplateTraversal("../");
            List<Information> result = new Action(() => { subject.Get(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "w-JSON#24;" });
        }

        [Fact]
        public void JsonGetTemplateTraversal_InvalidCharacters()
        {
            var subject = new JsonGetTemplateTraversal("[]");
            List<Information> result = new Action(() => { subject.Get(new JObject()); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#28;", "w-JSON#24;" }); //Preferred cascade, the error is an extra notification of something wrong with the path
        }



        private JToken CreateTestData()
            => JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));
    }
}
