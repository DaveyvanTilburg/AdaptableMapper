using System;
using System.Collections.Generic;
using AdaptableMapper.Configuration.Model;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases.ModelCases
{
    public class ModelConfiguration
    {
        [Theory]
        [InlineData("ParentInvalidType", ContextType.EmptyString, "", "e-MODEL#10;")]
        [InlineData("TemplateInvalidType", ContextType.EmptyObject, "item", "e-MODEL#11;")]
        public void ModelChildCreator(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelChildCreator();
            object context = Model.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = context, Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "", "e-MODEL#17;")]
        public void ModelObjectConverter(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelObjectConverter();
            object context = Model.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "", "e-MODEL#25;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, "", "e-MODEL#26;")]
        [InlineData("CannotInstantiateObject", ContextType.EmptySourceType, "", "e-MODEL#24;")]
        [InlineData("InstantiatedObjectIsNotOfTypeModelBase", ContextType.InvalidSourceType, "", "e-MODEL#31;")]
        public void ModelTargetInstantiator(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelTargetInstantiator();
            object context = Model.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Create(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "", "e-MODEL#20;")]
        public void ModelToStringObjectConverter(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelToStringObjectConverter();
            object context = Model.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "", "e-MODEL#27;")]
        [InlineData("InvalidSourceType", ContextType.EmptyString, "", "e-MODEL#28;")]
        public void StringToModelObjectConverterInvalidType(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new StringToModelObjectConverter(null);
            object context = Model.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void StringToModelObjectConverterInvalidSourceStringDeserialize()
        {
            ModelTargetInstantiatorSource testModel = Model.CreateModelTargetInstantiatorSource();
            var subject = new StringToModelObjectConverter(testModel);
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#29;" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidDeserializedType()
        {
            ModelTargetInstantiatorSource testModel = Model.CreateModelTargetInstantiatorSource();
            var subject = new StringToModelObjectConverter(testModel);

            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);
            List<Information> result = new Action(() => { subject.Convert(testSource); }).Observe();

            result.ValidateResult(new List<string> { "e-MODEL#30;" });
        }
    }
}