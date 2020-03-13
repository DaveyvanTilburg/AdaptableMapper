using System;
using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Process;
using MappingFramework.Traversals.Json;
using FluentAssertions;
using ModelObjects.Simple;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MappingFramework.TDD.Cases.JsonCases
{
    public class JsonTraversals
    {
        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-JSON#3;")]
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, "w-JSON#4;")]
        public void JsonGetScopeTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonGetListValueTraversal(path);
            Context context = new Context(Json.CreateTarget(contextType), null, null);
            List<Information> result = new Action(() => { subject.GetValues(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-JSON#18;")]
        [InlineData("NoResults", "abcd", ContextType.EmptyObject, "w-JSON#30;")]
        [InlineData("InvalidPath", "[]", ContextType.EmptyObject, "e-JSON#29;")]
        [InlineData("Valid", "$.SimpleItems[0].SurName", ContextType.TestObject)]
        public void JsonSetValueTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonSetValueTraversal(path);
            var context = new Context(null, Json.CreateTarget(contextType), null);
            List<Information> result = new Action(() => { subject.SetValue(context, null, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void JsonSetValueTraversalUninitializedValue()
        {
            JToken target = JToken.Parse(JsonConvert.SerializeObject(new SimpleItem()));

            var subject = new JsonSetValueTraversal(".Title");
            var context = new Context(null, target, null);
            List<Information> result = new Action(() => { subject.SetValue(context, null, "test"); }).Observe();
            result.Should().BeEmpty();

            target.TryTraversalGetValue(".Title").Value.Should().BeEquivalentTo("test");
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "e-JSON#10;")]
        [InlineData("NoResult", "", ContextType.EmptyObject, "e-JSON#6;")]
        [InlineData("EmptyResult", "$.SimpleItems[0].SurName", ContextType.TestObject)]
        public void JsonGetValueTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonGetValueTraversal(path);
            var context = new Context(Json.CreateTarget(contextType), null, null);
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
        [InlineData("Valid", "$.SimpleItems[0]", ContextType.TestObject)]
        public void JsonGetTemplateTraversal(string because, string path, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonGetTemplateTraversal(path);
            object context = Json.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.GetTemplate(context, new MappingCaches()); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void DoubleParentTraversal()
        {
            object context = Json.CreateTarget(ContextType.TestObject);
            JToken traversedContext = ((JToken)context).SelectToken("$.SimpleItems[0]");

            var subject = new JsonGetTemplateTraversal("../../");

            List<Information> result = new Action(() => { subject.GetTemplate(traversedContext, new MappingCaches()); }).Observe();
            result.ValidateResult(new List<string>(), "DoubleParent");
        }
    }
}