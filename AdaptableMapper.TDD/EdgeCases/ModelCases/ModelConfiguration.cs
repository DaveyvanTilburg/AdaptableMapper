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
        [Fact]
        public void ModelChildCreatorParentInvalidType()
        {
            var subject = new ModelChildCreator();
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = string.Empty, Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#10;" });
        }

        [Fact]
        public void ModelChildCreatorTemplateInvalidType()
        {
            var subject = new ModelChildCreator();
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = new ModelObjects.Simple.Item(), Child = string.Empty }); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#11;" });
        }




        [Fact]
        public void ModelObjectConverterInvalidType()
        {
            var subject = new ModelObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#17;" });
        }

        

        [Fact]
        public void ModelTargetInstantiatorInvalidType()
        {
            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(1); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#25;" });
        }

        [Fact]
        public void ModelTargetInstantiatorInvalidSource()
        {
            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#26;" });
        }



        [Fact]
        public void ModelTargetInstantiatorCannotInstantiateObject()
        {
            var testModel = new ModelTargetInstantiatorSource();
            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);

            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(testSource); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#24;" });
        }

        [Fact]
        public void ModelTargetInstantiatorInstantiatedObjectIsNotOfTypeModelBase()
        {
            ModelTargetInstantiatorSource testModel = CreateModelTargetInstantiatorSource();
            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);

            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(testSource); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#31;" });
        }



        [Fact]
        public void ModelToStringObjectConverterInvalidType()
        {
            var subject = new ModelToStringObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#20;" });
        }



        [Fact]
        public void StringToModelObjectConverterInvalidType()
        {
            var subject = new StringToModelObjectConverter(null);
            List<Information> result = new Action(() => { subject.Convert(0); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#27;" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidSourceType()
        {
            var subject = new StringToModelObjectConverter(new ModelTargetInstantiatorSource());
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#28;" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidSourceStringDeserialize()
        {
            ModelTargetInstantiatorSource testModel = CreateModelTargetInstantiatorSource();
            var subject = new StringToModelObjectConverter(testModel);

            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#29;" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidDeserializedType()
        {
            ModelTargetInstantiatorSource testModel = CreateModelTargetInstantiatorSource();
            var subject = new StringToModelObjectConverter(testModel);

            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);
            List<Information> result = new Action(() => { subject.Convert(testSource); }).Observe();

            result.ValidateResult(new List<string> { "e-MODEL#30;" });
        }

        private static ModelTargetInstantiatorSource CreateModelTargetInstantiatorSource()
        {
            var testType = typeof(ModelObjects.Simple.NoItem);
            var testModel = new ModelTargetInstantiatorSource
            {
                AssemblyFullName = testType.Assembly.FullName,
                TypeFullName = testType.FullName
            };

            return testModel;
        }
    }
}