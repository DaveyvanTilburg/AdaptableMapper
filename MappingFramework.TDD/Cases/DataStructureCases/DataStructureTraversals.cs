using System;
using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Process;
using MappingFramework.Traversals.DataStructure;
using Xunit;

namespace MappingFramework.TDD.Cases.DataStructureCases
{
    public class DataStructureTraversals
    {
        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-DataStructure#12;")]
        [InlineData("EmptyPath", "", ContextType.EmptyObject, "item", "w-DataStructure#7;")]
        [InlineData("NodeIsNotAModelBase", "Items/Code/test", ContextType.TestObject, "item", "e-DataStructure#8;", "e-DataStructure#8;")]
        [InlineData("InvalidButAcceptedRoot", "/", ContextType.EmptyObject, "item", "w-DataStructure#7;")]
        [InlineData("InvalidEndNode", "Items/Code", ContextType.TestObject, "item", "w-DataStructure#2;", "w-DataStructure#2;")]
        [InlineData("Valid", "Mixes{'PropertyName':'Code','Value':'1'}/Items", ContextType.TestObject, "deepmix")]
        public void DataStructureGetScopeTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureGetListValueTraversal(path);
            Context context = new Context(DataStructure.CreateTarget(contextType, createType), null, null);
            List<Information> result = new Action(() => { subject.GetValues(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-DataStructure#16;")]
        [InlineData("EmptyList", "Items/Code", ContextType.EmptyObject, "item", "w-DataStructure#5;")]
        [InlineData("NoParent", "../", ContextType.EmptyObject, "item", "w-DataStructure#3;")]
        public void DataStructureGetValueTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureGetValueTraversal(path);
            var context = new Context(DataStructure.CreateTarget(contextType, createType), null, null);
            List<Information> result = new Action(() => { subject.GetValue(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void DataStructureGetValueTraversalHasParent()
        {
            var subject = new DataStructureGetValueTraversal("../Items/Code");
            object context = DataStructure.CreateTarget(ContextType.TestObject, "item");
            var subItem = ((Simple.Item)context).Items[0];

            List<Information> result = new Action(() => { subject.GetValue(new Context(subItem, null, null)); }).Observe();
            result.ValidateResult(new List<string>(), "HasParent");
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-DataStructure#18;")]
        [InlineData("InvalidChildType", "NoItems/Code", ContextType.EmptyObject, "mix", "e-DataStructure#23;")]
        [InlineData("InvalidPath", "NoItem/Code", ContextType.EmptyObject, "mix", "w-DataStructure#1;")]
        [InlineData("InvalidTypeAlongTheWay", "Mixes/NoItem/Code", ContextType.EmptyObject, "deepmix", "w-DataStructure#1;")]
        public void DataStructureSetValueOnPathTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureSetValueOnPathTraversal(path);
            var context = new Context(null, DataStructure.CreateTarget(contextType, createType), null);
            List<Information> result = new Action(() => { subject.SetValue(context, null, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-DataStructure#19;")]
        public void DataStructureSetValueOnPropertyTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureSetValueOnPropertyTraversal(path);
            var context = new Context(null, DataStructure.CreateTarget(contextType, createType), null);
            List<Information> result = new Action(() => { subject.SetValue(context, null, string.Empty); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", "", ContextType.EmptyString, "", "e-DataStructure#22;")]
        public void DataStructureGetTemplateTraversal(string because, string path, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureGetTemplateTraversal(path);
            object context = DataStructure.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.GetTemplate(context, new MappingCaches()); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }
    }
}