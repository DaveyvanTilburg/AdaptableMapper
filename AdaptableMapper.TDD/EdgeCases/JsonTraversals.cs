using System;
using System.Collections.Generic;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class JsonTraversals
    {
        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-JSON#3;")]
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, "w-JSON#4;")]
        public void JsonGetScopeTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonGetScopeTraversal(path);
            object context = CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetScope(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", "", ContextType.EmptyString, "e-JSON#5;")]
        [InlineData("EmptySearchValuePath", "", "", ContextType.EmptyObject, "e-JSON#31;")]
        [InlineData("NoResultOnSearchValuePath", "", "abcd", ContextType.EmptyObject, "e-JSON#6;")]
        [InlineData("NoResultOnActualPath", "", "$.SimpleItems[0].SimpleItem.Id", ContextType.TestObject, "e-JSON#6;")]
        [InlineData("EmptySearchPathValue", "", "$.SimpleItems[0].SimpleItem.SurName", ContextType.TestObject, "w-JSON#7;")]
        [InlineData("EmptyActualPath", "$.SimpleItems[0].SimpleItem.SurName", "$.SimpleItems[0].SimpleItem.Id", ContextType.TestObject, "w-JSON#8;")]
        public void JsonGetSearchValueTraversal_InvalidType(string because, string path, string searchPath, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonGetSearchValueTraversal(path, searchPath);
            object context = CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-JSON#18;")]
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, "w-JSON#30;")]
        [InlineData("InvalidPath", "[]", ContextType.EmptyObject, "e-JSON#29;")]
        public void JsonSetValueTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonSetValueTraversal(path);
            object context = CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.SetValue(context, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-JSON#10;")]
        [InlineData("NoResult", "", ContextType.EmptyObject, "e-JSON#6;")]
        [InlineData("EmptyResult", "$.SimpleItems[0].SimpleItem.SurName", ContextType.TestObject, "w-JSON#11;")]
        public void JsonGetValueTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonGetValueTraversal(path);
            object context = CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }



        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-JSON#23;")]
        [InlineData("NoParentCheck", "$", ContextType.EmptyObject, "e-JSON#9;")]
        [InlineData("InvalidPath", "abcd", ContextType.EmptyObject, "w-JSON#24;")]
        [InlineData("InvalidParentPath", "ab/cd", ContextType.EmptyObject, "e-JSON#15;", "w-JSON#24;")] //Preferred cascade, the error is an extra notification of something wrong with the path
        [InlineData("NoParent", "../", ContextType.EmptyObject, "w-JSON#24;")]
        [InlineData("InvalidCharacters", "[]", ContextType.EmptyObject, "e-JSON#28;", "w-JSON#24;")] //Preferred cascade, the error is an extra notification of something wrong with the path
        public void JsonGetTemplateTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonGetTemplateTraversal(path);
            object context = CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Get(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        private object CreateTarget(ContextType contextType)
        {
            switch (contextType)
            {
                case ContextType.EmptyString:
                    return string.Empty;
                case ContextType.EmptyObject:
                    return new JObject();
                case ContextType.TestObject:
                    return CreateTestData();
                default:
                    throw new NotImplementedException();
            }
        }

        private JToken CreateTestData()
            => JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));
    }
}
