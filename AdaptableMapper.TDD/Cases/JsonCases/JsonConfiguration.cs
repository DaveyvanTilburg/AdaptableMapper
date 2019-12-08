using System;
using System.Collections.Generic;
using AdaptableMapper.Configuration.Json;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using Xunit;

namespace AdaptableMapper.TDD.Cases.JsonCases
{
    public class JsonConfiguration
    {
        [Theory]
        [InlineData("InvalidTypeParent", ContextType.EmptyString, ContextType.EmptyString, "e-JSON#1;")]
        [InlineData("InvalidType", ContextType.InvalidObject, ContextType.EmptyString, "e-JSON#2;")]
        [InlineData("Valid", ContextType.ValidParent, ContextType.TestObject)]
        public void JsonChildCreator(string because, ContextType parentType, ContextType childType, params string[] expectedErrors)
        {
            var subject = new JsonChildCreator();
            object parent = Json.CreateTarget(parentType);
            object child = Json.CreateTarget(childType);
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = parent, Child = child }); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "e-JSON#12;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, "e-JSON#13;")]
        public void JsonObjectConverter(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonObjectConverter();
            object context = Json.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "e-JSON#26;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, "e-JSON#20;")]
        [InlineData("ValidSource", ContextType.ValidSource)]
        public void JsonTargetInstantiatorInvalidType(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JsonTargetInstantiator();
            object context = Json.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Create(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "e-JSON#25;")]
        public void JTokenToStringObjectConverter(string because, ContextType contextType, params string[] expectedErrors)
        {
            var subject = new JTokenToStringObjectConverter();
            object context = Json.CreateTarget(contextType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }
    }
}