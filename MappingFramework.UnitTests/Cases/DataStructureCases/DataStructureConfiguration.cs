using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.DataStructure;
using MappingFramework.Languages.DataStructure.Configuration;
using Xunit;

namespace MappingFramework.UnitTests.Cases.DataStructureCases
{
    public class DataStructureConfiguration
    {
        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "", 1)]
        [InlineData("Valid", ContextType.EmptyObject, "item", 0)]
        public void DataStructureObjectConverter(string because, ContextType contextType, string createType, int informationCount)
        {
            var subject = new DataStructureSourceCreator();
            object source = DataStructure.Stub(contextType, createType);
            var context = new Context();

            var result = subject.Convert(context, source);
            result.Should().BeAssignableTo<TraversableDataStructure>();

            context.Information().Count.Should().Be(informationCount, because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "", 1)]
        [InlineData("InvalidSource", ContextType.InvalidSource, "", 1)]
        [InlineData("CannotInstantiateObject", ContextType.EmptySourceType, "", 1)]
        [InlineData("InstantiatedObjectIsNotOfTypeDataStructureBase", ContextType.InvalidSourceType, "", 1)]
        [InlineData("Valid", ContextType.ValidSource, "", 0)]
        public void DataStructureTargetInstantiator(string because, ContextType contextType, string createType, int informationCount)
        {
            string source = DataStructure.Stub(contextType, createType) as string;
            var subject = new DataStructureTargetCreator(source);
            var context = new Context();

            var result = subject.Create(context, null);
            result.Should().BeAssignableTo<TraversableDataStructure>();

            context.Information().Count.Should().Be(informationCount, because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "", 1)]
        [InlineData("InvalidSourceType", ContextType.EmptyString, "", 1)]
        public void StringToDataStructureObjectConverterInvalidType(string because, ContextType contextType, string createType, int informationCount)
        {
            var subject = new StringToDataStructureSourceCreator(null);
            object source = DataStructure.Stub(contextType, createType);
            var context = new Context();

            var result = subject.Convert(context, source);
            result.Should().BeAssignableTo<TraversableDataStructure>();

            context.Information().Count.Should().Be(informationCount, because);
        }

        [Fact]
        public void StringToDataStructureObjectConverterInvalidSourceStringDeserialize()
        {
            DataStructureTargetCreatorSource testDataStructure = DataStructure.CreateDataStructureTargetInstantiatorInvalidSource();
            var subject = new StringToDataStructureSourceCreator(testDataStructure);
            var context = new Context();

            var result = subject.Convert(context, "abcd");
            result.Should().BeAssignableTo<TraversableDataStructure>();

            context.Information().Count.Should().Be(1);
        }

        [Fact]
        public void StringToDataStructureObjectConverterInvalidDeserializedType()
        {
            DataStructureTargetCreatorSource testDataStructure = DataStructure.CreateDataStructureTargetInstantiatorInvalidSource();
            var subject = new StringToDataStructureSourceCreator(testDataStructure);
            var context = new Context();

            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testDataStructure);
            var result = subject.Convert(context, testSource);
            result.Should().BeAssignableTo<TraversableDataStructure>();

            context.Information().Count.Should().Be(1);
        }
    }
}