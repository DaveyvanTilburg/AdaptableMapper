using System;
using System.Collections.Generic;
using AdaptableMapper.Configuration;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals.Model;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.ModelCases
{
    public class ModelTraversals
    {
        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-MODEL#12;")]
        [InlineData("EmptyPath", "", ContextType.EmptyObject, "item", "w-MODEL#7;")]
        [InlineData("NodeIsNotAModelBase", "Items/Code/test", ContextType.TestObject, "item", "e-MODEL#8;", "e-MODEL#8;")]
        [InlineData("InvalidButAcceptedRoot", "/", ContextType.EmptyObject, "item", "w-MODEL#7;")]
        [InlineData("InvalidEndNode", "Items/Code", ContextType.TestObject, "item", "w-MODEL#2;", "w-MODEL#2;")]
        [InlineData("Valid", "Mixes{'PropertyName':'Code','Value':'1'}/Items", ContextType.TestObject, "deepmix")]
        public void ModelGetScopeTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelGetScopeTraversal(path);
            object context = Model.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.GetScope(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", "", ContextType.EmptyString, "", "", "e-MODEL#13;")]
        [InlineData("EmptySearchValuePath", "", "", ContextType.EmptyObject, "item", "", "e-MODEL#21;")]
        [InlineData("NoResultForSearchPath", "", "dummySearch", ContextType.EmptyObject, "item", "", "w-MODEL#9;", "w-MODEL#14;")] //Preferred cascade, 9 is extra info
        [InlineData("NoResultForActualPath", "ab/cd", "Items{'PropertyName':'Code','Value':'1'}/Code", ContextType.TestObject, "item", "", "w-MODEL#9;")] //Preferred cascade, 9 is extra info
        [InlineData("NoResultActualSearch", "ab/cd", "Items{'PropertyName':'Code','Value':'3'}/Code", ContextType.TestObject, "item", "", "w-MODEL#4;")]
        [InlineData("InvalidFilterMarkup", "ab/cd", "Items{'PropertyName':'Code','Value':'1'/Code", ContextType.TestObject, "item", "", "w-MODEL#9;", "e-MODEL#32;")] //Preferred cascade, 9 is extra info
        [InlineData("Valid", "Items{'PropertyName':'Code','Value':'{{searchValue}}'}/Name", "Items{'PropertyName':'Code','Value':'1'}/Code", ContextType.TestObject, "item", "Davey")]
        public void ModelGetSearchValueTraversal(string because, string path, string searchPath, ContextType contextType, string createType, string expectedValue, params string[] expectedErrors)
        {
            var subject = new ModelGetSearchValueTraversal(path, searchPath);
            var context = new Context(Model.CreateTarget(contextType, createType), null);

            string value = string.Empty;
            List<Information> result = new Action(() => { value = subject.GetValue(context); }).Observe();

            result.ValidateResult(new List<string>(expectedErrors), because);
            if (expectedErrors.Length == 0)
                value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-MODEL#16;")]
        [InlineData("EmptyList", "Items/Code", ContextType.EmptyObject, "item", "w-MODEL#5;")]
        [InlineData("NoParent", "../", ContextType.EmptyObject, "item", "w-MODEL#3;")]
        public void ModelGetValueTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelGetValueTraversal(path);
            var context = new Context(Model.CreateTarget(contextType, createType), null);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void ModelGetValueTraversalHasParent()
        {
            var subject = new ModelGetValueTraversal("../Items/Code");
            object context = Model.CreateTarget(ContextType.TestObject, "item");
            var subItem = ((ModelObjects.Simple.Item)context).Items[0];

            List<Information> result = new Action(() => { subject.GetValue(new Context(subItem, null)); }).Observe();
            result.ValidateResult(new List<string>(), "HasParent");
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-MODEL#18;")]
        [InlineData("InvalidChildType", "NoItems/Code", ContextType.EmptyObject, "mix", "e-MODEL#23;")]
        [InlineData("InvalidPath", "NoItem/Code", ContextType.EmptyObject, "mix", "w-MODEL#1;")]
        [InlineData("InvalidTypeAlongTheWay", "Mixes/NoItem/Code", ContextType.EmptyObject, "deepmix", "w-MODEL#1;")]
        public void ModelSetValueOnPathTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelSetValueOnPathTraversal(path);
            var context = new Context(null, Model.CreateTarget(contextType, createType));
            List<Information> result = new Action(() => { subject.SetValue(context, null, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-MODEL#19;")]
        public void ModelSetValueOnPropertyTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelSetValueOnPropertyTraversal(path);
            var context = new Context(null, Model.CreateTarget(contextType, createType));
            List<Information> result = new Action(() => { subject.SetValue(context, null, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-MODEL#22;")]
        public void ModelGetTemplateTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new ModelGetTemplateTraversal(path);
            object context = Model.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.GetTemplate(context, new MappingCaches()); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }
    }
}