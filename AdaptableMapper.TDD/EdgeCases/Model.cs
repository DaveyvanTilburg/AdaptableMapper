using System;
using System.Collections.Generic;
using AdaptableMapper.Model;
using AdaptableMapper.Process;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class Model
    {
        [Fact]
        public void ModelChildCreatorParentInvalidType()
        {
            var subject = new ModelChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#10" });
        }

        [Fact]
        public void ModelChildCreatorTemplateInvalidType()
        {
            var subject = new ModelChildCreator();
            List<Information> result = new Action(() => { subject.CreateChildOn(new ModelObjects.Simple.Item(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#11" });
        }

        [Fact]
        public void ModelGetScopeInvalidType()
        {
            var subject = new ModelGetScope("");
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#12" });
        }

        [Fact]
        public void ModelGetScopeEmptyPath()
        {
            var subject = new ModelGetScope("");
            var model = new ModelObjects.Simple.Item();
            List<Information> result = new Action(() => { subject.GetScope(model); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#2" });
        }

        [Fact]
        public void ModelGetScopeInvalidButAcceptedRoot()
        {
            var subject = new ModelGetScope("/");
            var model = new ModelObjects.Simple.Item();
            List<Information> result = new Action(() => { subject.GetScope(model); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#2" });
        }

        [Fact]
        public void ModelGetSearchValueInvalidType()
        {
            var subject = new ModelGetSearchValue(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#13" });
        }

        [Fact]
        public void ModelGetSearchValueNoResultForSearchPath()
        {
            var subject = new ModelGetSearchValue(string.Empty, "dummySearch");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#14" });
        }

        [Fact]
        public void ModelGetSearchValueNoResultForActualPath()
        {
            var subject = new ModelGetSearchValue("ab/cd", "Items{'PropertyName':'Code','Value':'1'}/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#7", "MODEL#8", "MODEL#5", "MODEL#15" });
        }

        [Fact]
        public void ModelGetSearchValueNoResultActualSearch()
        {
            var subject = new ModelGetSearchValue("ab/cd", "Items{'PropertyName':'Code','Value':'3'}/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#4", "MODEL#14" });
        }

        [Fact]
        public void ModelGetSearchValueInvalidFilterMarkup()
        {
            var subject = new ModelGetSearchValue("ab/cd", "Items{'PropertyName':'Code','Value':'1'/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#7", "MODEL#8", "MODEL#5", "MODEL#9", "MODEL#14", "MODEL#32" });
        }

        [Fact]
        public void ModelGetValueInvalidType()
        {
            var subject = new ModelGetValue(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#16" });
        }

        [Fact]
        public void ModelGetValueEmptyList()
        {
            var subject = new ModelGetValue("Items/Code");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#5", "MODEL#9" });
        }

        [Fact]
        public void ModelGetValueNoParent()
        {
            var subject = new ModelGetValue("../");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#3", "MODEL#9" });
        }

        [Fact]
        public void ModelObjectConverterInvalidType()
        {
            var subject = new ModelObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#17" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalidType()
        {
            var subject = new ModelSetValueOnPath(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#18" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalidChildType()
        {
            var subject = new ModelSetValueOnPath("NoItems/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.Mix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#23", "MODEL#9" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalid()
        {
            var subject = new ModelSetValueOnPath("NoItem/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.Mix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#1", "MODEL#9" });
        }

        [Fact]
        public void ModelSetValueOnPropertyInvalidType()
        {
            var subject = new ModelSetValueOnProperty(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#19" });
        }

        [Fact]
        public void ModelSetValueOnPathInvalidTypeAlongTheWay()
        {
            var subject = new ModelSetValueOnPath("Mixes/NoItem/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.DeepMix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#9", "MODEL#1", "MODEL#9" });
        }

        [Fact]
        public void ModelTargetInstantiatorInvalidType()
        {
            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(1); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#25" });
        }

        [Fact]
        public void ModelTargetInstantiatorInvalidSource()
        {
            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#26" });
        }

        [Fact]
        public void ModelTargetInstantiatorCannotInstantiateObject()
        {
            var testModel = new ModelTargetInstantiatorSource();

            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);

            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(testSource); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#24" });
        }

        [Fact]
        public void ModelTargetInstantiatorInstantiatedObjectIsNotOfTypeModelBase()
        {
            ModelTargetInstantiatorSource testModel = CreateModelTargetInstantiatorSource();
            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);

            var subject = new ModelTargetInstantiator();
            List<Information> result = new Action(() => { subject.Create(testSource); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#31" });
        }

        [Fact]
        public void ModelToStringObjectConverterInvalidType()
        {
            var subject = new ModelToStringObjectConverter();
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#20" });
        }

        [Fact]
        public void ModelTraversalInvalidType()
        {
            var subject = new ModelTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#21" });
        }

        [Fact]
        public void ModelTraversalTemplateInvalidType()
        {
            var subject = new ModelTraversalTemplate(string.Empty);
            List<Information> result = new Action(() => { subject.Traverse(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#22" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidType()
        {
            var subject = new StringToModelObjectConverter(null);
            List<Information> result = new Action(() => { subject.Convert(1); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#27" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidSourceType()
        {
            var subject = new StringToModelObjectConverter(new ModelTargetInstantiatorSource());
            List<Information> result = new Action(() => { subject.Convert(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#28" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidSourceStringDeserialize()
        {
            ModelTargetInstantiatorSource testModel = CreateModelTargetInstantiatorSource();
            var subject = new StringToModelObjectConverter(testModel);

            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "MODEL#29" });
        }

        [Fact]
        public void StringToModelObjectConverterInvalidDeserializedType()
        {
            ModelTargetInstantiatorSource testModel = CreateModelTargetInstantiatorSource();
            var subject = new StringToModelObjectConverter(testModel);

            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testModel);
            List<Information> result = new Action(() => { subject.Convert(testSource); }).Observe();

            result.ValidateResult(new List<string> { "MODEL#30" });
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