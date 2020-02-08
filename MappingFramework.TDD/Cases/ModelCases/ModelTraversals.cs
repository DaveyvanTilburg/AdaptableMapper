using System;
using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Process;
using MappingFramework.Traversals.Model;
using Xunit;

namespace MappingFramework.TDD.Cases.ModelCases
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
            var subject = new ModelGetListValueTraversal(path);
            Context context = new Context(Model.CreateTarget(contextType, createType), null);
            List<Information> result = new Action(() => { subject.GetValues(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
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