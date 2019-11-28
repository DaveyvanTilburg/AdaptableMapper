using System;
using System.Collections.Generic;
using AdaptableMapper.Configuration.Json;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class JsonConfiguration
    {
        [Fact]
        public void JsonChildCreatorInvalidTypeParent()
        {
            var subject = new JsonChildCreator();
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = string.Empty, Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#1;" });
        }

        [Fact]
        public void JsonChildCreatorInvalidType()
        {
            var subject = new JsonChildCreator();
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = new JArray(), Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#2;" });
        }
        
        

        [Fact]
        public void JsonObjectConverterInvalidType()
        {
            var subject = new JsonObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#12;" });
        }

        [Fact]
        public void JsonObjectConverterInvalidSource()
        {
            var subject = new JsonObjectConverter();
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#13;" });
        }

        [Fact]
        public void JsonTargetInstantiatorInvalidType()
        {
            var subject = new JsonTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(0); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#26;" });
        }

        [Fact]
        public void JsonTargetInstantiatorInvalidSource()
        {
            var subject = new JsonTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#20;" });
        }

        

        [Fact]
        public void JTokenToStringObjectConverterInvalidType()
        {
            var subject = new JTokenToStringObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-JSON#25;" });
        }
    }
}