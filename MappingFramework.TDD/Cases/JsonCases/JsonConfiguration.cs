using System;
using System.Collections.Generic;
using MappingFramework.Configuration.Json;
using MappingFramework.Process;
using MappingFramework.Traversals;
using FluentAssertions;
using Xunit;

namespace MappingFramework.TDD.Cases.JsonCases
{
    public class JsonConfiguration
    {
        [Theory]
        [InlineData("Valid", ContextType.ValidParent, ContextType.TestObject)]
        public void JsonChildCreatorCreateChild(string because, ContextType parentType, ContextType childType, params string[] expectedErrors)
        {
            var subject = new JsonChildCreator();
            object parent = Json.CreateTarget(parentType);
            object child = Json.CreateTarget(childType);
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = parent, Child = child }); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("Valid", ContextType.ValidParent, ContextType.TestObject)]
        public void JsonChildCreatorAddToParent(string because, ContextType parentType, ContextType childType, params string[] expectedErrors)
        {
            var subject = new JsonChildCreator();
            object parent = Json.CreateTarget(parentType);
            object child = Json.CreateTarget(childType);
            List<Information> result = new Action(() => { subject.AddToParent(new Template { Parent = parent, Child = child }, child); }).Observe();
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
        [InlineData("LengthCheckUseIndentation", ContextType.TestObject, true, 14)]
        [InlineData("LengthCheckDoNotUseIndentation", ContextType.TestObject, false, 1)]
        public void XElementToStringObjectConverter(string because, ContextType contextType, bool useIndentation, int expectedLines, params string[] expectedErrors)
        {
            var subject = new JTokenToStringObjectConverter { UseIndentation = useIndentation };
            object context = Json.CreateTarget(contextType);

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.Convert(context) as string; }).Observe();

            information.ValidateResult(new List<string>(expectedErrors), because);

            if (expectedErrors.Length == 0)
            {
                string[] lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                lines.Length.Should().Be(expectedLines);
            }
        }
    }
}