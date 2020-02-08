using System;
using System.Collections.Generic;
using MappingFramework.Configuration.Model;
using MappingFramework.Process;
using MappingFramework.Traversals;
using Xunit;

namespace MappingFramework.TDD.Cases.ModelCases
{
    public class ModelConfiguration
    {
        [Theory]
        [InlineData("InvalidParentType", ContextType.EmptyString, "", ContextType.EmptyObject, "", "e-ModelChildCreator#1;")]
        [InlineData("InvalidChildType", ContextType.EmptyObject, "item", ContextType.EmptyObject, "", "e-ModelChildCreator#2;")]
        public void ModelChildCreatorCreateChild(string because, ContextType parentType, string parentCreateType, ContextType childType, string childCreateType, params string[] expectedErrors)
        {
            var subject = new ModelChildCreator();
            object parent = Model.CreateTarget(parentType, parentCreateType);
            object child = Model.CreateTarget(childType, childCreateType);
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = parent, Child = child }); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidParentType", ContextType.EmptyString, "", ContextType.EmptyString, "", ContextType.EmptyString, "", "e-ModelChildCreator#3;")]
        [InlineData("InvalidChildType", ContextType.EmptyObject, "item", ContextType.EmptyString, "", ContextType.EmptyString, "", "e-ModelChildCreator#4;")]
        [InlineData("InvalidNewChildType", ContextType.EmptyObject, "item", ContextType.ValidParent, "", ContextType.EmptyString, "", "e-ModelChildCreator#5;")]
        public void ModelChildCreatorAddToParent(string because, ContextType parentType, string parentCreateType, ContextType childType, string childCreateType, ContextType newChildType, string newChildCreateType, params string[] expectedErrors)
        {
            var subject = new ModelChildCreator();
            object parent = Model.CreateTarget(parentType, parentCreateType);
            object child = Model.CreateTarget(childType, childCreateType);
            object newChild = Model.CreateTarget(newChildType, newChildCreateType);
            List<Information> result = new Action(() => { subject.AddToParent(new Template { Parent = parent, Child = child }, newChild); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "", "e-MODEL#17;")]
        [InlineData("Valid", ContextType.EmptyObject, "item")]
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
        [InlineData("Valid", ContextType.ValidSource, "")]
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
            ModelTargetInstantiatorSource testModel = Model.CreateModelTargetInstantiatorInvalidSource();
            var subject = new StringToModelObjectConverter(testModel);
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#29;" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidDeserializedType()
        {
            ModelTargetInstantiatorSource testModel = Model.CreateModelTargetInstantiatorInvalidSource();
            var subject = new StringToModelObjectConverter(testModel);

            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);
            List<Information> result = new Action(() => { subject.Convert(testSource); }).Observe();

            result.ValidateResult(new List<string> { "e-MODEL#30;" });
        }
    }
}