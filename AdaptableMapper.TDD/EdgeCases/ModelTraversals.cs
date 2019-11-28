using System;
using System.Collections.Generic;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals.Model;
using Xunit;

namespace AdaptableMapper.TDD.EdgeCases
{
    public class ModelTraversals
    {
        [Fact]
        public void ModelGetScopeTraversal_InvalidType()
        {
            var subject = new ModelGetScopeTraversal("");
            List<Information> result = new Action(() => { subject.GetScope(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#12;" });
        }

        [Fact]
        public void ModelGetScopeTraversal_EmptyPath()
        {
            var subject = new ModelGetScopeTraversal("");
            var model = new ModelObjects.Simple.Item();
            List<Information> result = new Action(() => { subject.GetScope(model); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#7;" });
        }

        [Fact]
        public void ModelGetScopeTraversal_NodeIsNotAModelBase()
        {
            var subject = new ModelGetScopeTraversal("Items/Code/test");
            List<Information> result = new Action(() => { subject.GetScope(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#8;", "e-MODEL#8;" });
        }

        [Fact]
        public void ModelGetScopeTraversal_InvalidButAcceptedRoot()
        {
            var subject = new ModelGetScopeTraversal("/");
            var model = new ModelObjects.Simple.Item();
            List<Information> result = new Action(() => { subject.GetScope(model); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#7;" });
        }

        [Fact]
        public void ModelGetScopeTraversal_InvalidEndNode()
        {
            var subject = new ModelGetScopeTraversal("Items/Code");
            List<Information> result = new Action(() => { subject.GetScope(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#2;", "w-MODEL#2;" });
        }



        [Fact]
        public void ModelGetSearchValueTraversal_InvalidType()
        {
            var subject = new ModelGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#13;" });
        }

        [Fact]
        public void ModelGetSearchValueTraversal_EmptySearchValuePath()
        {
            var subject = new ModelGetSearchValueTraversal(string.Empty, string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#21;" });
        }

        [Fact]
        public void ModelGetSearchValueTraversal_NoResultForSearchPath()
        {
            var subject = new ModelGetSearchValueTraversal(string.Empty, "dummySearch");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#9;", "w-MODEL#14;" }); //Preferred cascade, 9 is extra info
        }

        [Fact]
        public void ModelGetSearchValueTraversal_NoResultForActualPath()
        {
            var subject = new ModelGetSearchValueTraversal("ab/cd", "Items{'PropertyName':'Code','Value':'1'}/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#9;", "w-MODEL#15;" }); //Preferred cascade, 9 is extra info
        }

        [Fact]
        public void ModelGetSearchValueTraversal_NoResultActualSearch()
        {
            var subject = new ModelGetSearchValueTraversal("ab/cd", "Items{'PropertyName':'Code','Value':'3'}/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#4;" });
        }

        [Fact]
        public void ModelGetSearchValueTraversal_InvalidFilterMarkup()
        {
            var subject = new ModelGetSearchValueTraversal("ab/cd", "Items{'PropertyName':'Code','Value':'1'/Code");
            List<Information> result = new Action(() => { subject.GetValue(CreateTestItem()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#9;", "e-MODEL#32;" }); //Preferred cascade, 9 is extra info
        }



        [Fact]
        public void ModelGetValueTraversal_InvalidType()
        {
            var subject = new ModelGetValueTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.GetValue(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#16;" });
        }

        [Fact]
        public void ModelGetValueTraversal_EmptyList()
        {
            var subject = new ModelGetValueTraversal("Items/Code");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#5;" });
        }

        [Fact]
        public void ModelGetValueTraversal_NoParent()
        {
            var subject = new ModelGetValueTraversal("../");
            List<Information> result = new Action(() => { subject.GetValue(new ModelObjects.Simple.Item()); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#3;" });
        }



        [Fact]
        public void ModelSetValueOnPathTraversal_InvalidType()
        {
            var subject = new ModelSetValueOnPathTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#18;" });
        }

        [Fact]
        public void ModelSetValueOnPathTraversal_InvalidChildType()
        {
            var subject = new ModelSetValueOnPathTraversal("NoItems/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.Mix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#23;" });
        }

        [Fact]
        public void ModelSetValueOnPathTraversal_InvalidPath()
        {
            var subject = new ModelSetValueOnPathTraversal("NoItem/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.Mix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#1;" });
        }



        [Fact]
        public void ModelSetValueOnPropertyTraversal_InvalidType()
        {
            var subject = new ModelSetValueOnPropertyTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.SetValue(string.Empty, string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#19;" });
        }

        [Fact]
        public void ModelSetValueOnPropertyTraversal_TypeAlongTheWay()
        {
            var subject = new ModelSetValueOnPathTraversal("Mixes/NoItem/Code");
            List<Information> result = new Action(() => { subject.SetValue(new ModelObjects.Simple.DeepMix(), string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "w-MODEL#1;" });
        }



        [Fact]
        public void ModelGetTemplateTraversal_InvalidType()
        {
            var subject = new ModelGetTemplateTraversal(string.Empty);
            List<Information> result = new Action(() => { subject.Get(string.Empty); }).Observe();
            result.ValidateResult(new List<string> { "e-MODEL#22;" });
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
    }
}