using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.TDD.DataStructureExamples.Simple;
using MappingFramework.Traversals.DataStructure;
using Xunit;

namespace MappingFramework.TDD.Cases.DataStructureCases
{
    public class DataStructureTraversals
    {
        [Theory]
        [InlineData("EmptyPath", "", ContextType.EmptyObject, "item", 1)]
        [InlineData("NodeIsNotAModelBase", "Items/Code/test", ContextType.TestObject, "item", 2)]
        [InlineData("InvalidButAcceptedRoot", "/", ContextType.EmptyObject, "item", 1)]
        [InlineData("InvalidEndNode", "Items/Code", ContextType.TestObject, "item", 2)]
        [InlineData("Valid", "Mixes{'PropertyName':'Code','Value':'1'}/Items", ContextType.TestObject, "deepmix", 0)]
        public void DataStructureGetScopeTraversal(string because, string path, ContextType contextType, string createType, int informationCount)
        {
            var subject = new DataStructureGetListValueTraversal(path);
            Context context = new Context(DataStructure.Stub(contextType, createType), null, null);

            subject.GetValues(context);

            context.Information().Count.Should().Be(informationCount, because);
        }

        [Theory]
        [InlineData("EmptyList", "Items/Code", ContextType.EmptyObject, "item", 1)]
        [InlineData("NoParent", "../", ContextType.EmptyObject, "item", 1)]
        public void DataStructureGetValueTraversal(string because, string path, ContextType contextType, string createType, int informationCount)
        {
            var subject = new DataStructureGetValueTraversal(path);
            var context = new Context(DataStructure.Stub(contextType, createType), null, null);

            subject.GetValue(context);

            context.Information().Count.Should().Be(informationCount, because);
        }

        [Fact]
        public void DataStructureGetValueTraversalHasParent()
        {
            var subject = new DataStructureGetValueTraversal("../Items/Code");
            object source = DataStructure.Stub(ContextType.TestObject, "item");
            var context = new Context(((Item)source).Items[0], null, null);

            subject.GetValue(context);

            context.Information().Count.Should().Be(0);
        }

        [Theory]
        [InlineData("InvalidChildType", "NoItems/Code", ContextType.EmptyObject, "mix", 1)]
        [InlineData("InvalidPath", "NoItem/Code", ContextType.EmptyObject, "mix", 1)]
        [InlineData("InvalidTypeAlongTheWay", "Mixes/NoItem/Code", ContextType.EmptyObject, "deepmix", 1)]
        public void DataStructureSetValueOnPathTraversal(string because, string path, ContextType contextType, string createType, int informationCount)
        {
            var subject = new DataStructureSetValueOnPathTraversal(path);
            var context = new Context(null, DataStructure.Stub(contextType, createType), null);

            subject.SetValue(context, null, string.Empty);

            context.Information().Count.Should().Be(informationCount, because);
        }
    }
}