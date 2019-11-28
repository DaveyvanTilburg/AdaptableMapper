using System;
using System.Collections.Generic;
using AdaptableMapper.Configuration.Model;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using AdaptableMapper.Traversals.Model;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class Model
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
        public void ModelGetScopeInvalidType()
        {
            var subject = new ModelGetScopeTraversal("");
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#12;" });
        }

        [Fact]
        public void ModelGetScopeEmptyPath()
        {
            var subject = new ModelGetScopeTraversal("");
            var model = new ModelObjects.Simple.Item();
            List<Information> result = new Action(() => { subject.GetScope(model); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#7;" });
        }

        [Fact]
        public void ModelGetScopeNodeIsNotAModelBase()
        {
            var subject = new ModelGetScopeTraversal("Items/Code/test");
            List<Information> result = new Action(() => { subject.GetScope(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#8;", "e-MODEL#8;" });
        }

        [Fact]
        public void ModelGetScopeInvalidButAcceptedRoot()
        {
            var subject = new ModelGetScopeTraversal("/");
            var model = new ModelObjects.Simple.Item();
            List<Information> result = new Action(() => { subject.GetScope(model); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#7;" });
        }

        [Fact]
        public void ModelGetScopeInvalidEndNode()
        {
            var subject = new ModelGetScopeTraversal("Items/Code");
            List<Information> result = new Action(() => { subject.GetScope(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#2;", "w-MODEL#2;" });
        }

        [Fact]
        public void ModelGetSearchValueInvalidType()
        {
            var subject = new ModelGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#13;" });
        }

        [Fact]
        public void ModelGetSearchValueEmptySearchValuePath()
        {
            var subject = new ModelGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#21;" });
        }

        [Fact]
        public void ModelGetSearchValueNoResultForSearchPath()
        {
            var subject = new ModelGetSearchValueTraversal(string.Empty, "dummySearch");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#9;", "w-MODEL#14;" }); //Preferred cascade, 9 is extra info
        }

        [Fact]
        public void ModelGetSearchValueNoResultForActualPath()
        {
            var subject = new ModelGetSearchValueTraversal("ab/cd", "Items{'PropertyName':'Code','Value':'1'}/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#9;", "w-MODEL#15;" }); //Preferred cascade, 9 is extra info
        }

        [Fact]
        public void ModelGetSearchValueNoResultActualSearch()
        {
            var subject = new ModelGetSearchValueTraversal("ab/cd", "Items{'PropertyName':'Code','Value':'3'}/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#4;" });
        }

        [Fact]
        public void ModelGetSearchValueInvalidFilterMarkup()
        {
            var subject = new ModelGetSearchValueTraversal("ab/cd", "Items{'PropertyName':'Code','Value':'1'/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#9;", "e-MODEL#32;" }); //Preferred cascade, 9 is extra info
        }

        [Fact]
        public void ModelGetValueInvalidType()
        {
            var subject = new ModelGetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#16;" });
        }

        [Fact]
        public void ModelGetValueEmptyList()
        {
            var subject = new ModelGetValueTraversal("Items/Code");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#5;" });
        }

        [Fact]
        public void ModelGetValueNoParent()
        {
            var subject = new ModelGetValueTraversal("../");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#3;" });
        }

        [Fact]
        public void ModelObjectConverterInvalidType()
        {
            var subject = new ModelObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#17;" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalidType()
        {
            var subject = new ModelSetValueOnPathTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#18;" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalidChildType()
        {
            var subject = new ModelSetValueOnPathTraversal("NoItems/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.Mix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#23;" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalid()
        {
            var subject = new ModelSetValueOnPathTraversal("NoItem/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.Mix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#1;" });
        }

        [Fact]
        public void ModelSetValueOnPropertyInvalidType()
        {
            var subject = new ModelSetValueOnPropertyTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#19;" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalidTypeAlongTheWay()
        {
            var subject = new ModelSetValueOnPathTraversal("Mixes/NoItem/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.DeepMix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#1;" });
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
        public void ModelTraversalTemplateInvalidType()
        {
            var subject = new ModelGetTemplateTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Get(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#22;" });
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

        private static ModelObjects.Simple.Item CreateTestItem()
        {
            return new ModelObjects.Simple.Item
            {
                Items = new List<ModelObjects.Simple.Item>
                {
                    new ModelObjects.Simple.Item
                    {
                        Code = "1"
                    },
                    new ModelObjects.Simple.Item
                    {
                        Code = "2"
                    }
                }
            };
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