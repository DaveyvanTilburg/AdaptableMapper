using System;
using System.Collections.Generic;
using MappingFramework.Configuration.DataStructure;
using MappingFramework.Process;
using MappingFramework.Traversals;
using Xunit;

namespace MappingFramework.TDD.Cases.DataStructureCases
{
    public class DataStructureConfiguration
    {
        [Theory]
        [InlineData("InvalidParentType", ContextType.EmptyString, "", ContextType.EmptyObject, "", "e-DataStructureChildCreator#1;")]
        [InlineData("InvalidChildType", ContextType.EmptyObject, "item", ContextType.EmptyObject, "", "e-DataStructureChildCreator#2;")]
        public void DataStructureChildCreatorCreateChild(string because, ContextType parentType, string parentCreateType, ContextType childType, string childCreateType, params string[] expectedErrors)
        {
            var subject = new DataStructureChildCreator();
            object parent = DataStructure.CreateTarget(parentType, parentCreateType);
            object child = DataStructure.CreateTarget(childType, childCreateType);
            List<Information> result = new Action(() => { subject.CreateChild(new Template { Parent = parent, Child = child }); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidParentType", ContextType.EmptyString, "", ContextType.EmptyString, "", ContextType.EmptyString, "", "e-DataStructureChildCreator#3;")]
        [InlineData("InvalidChildType", ContextType.EmptyObject, "item", ContextType.EmptyString, "", ContextType.EmptyString, "", "e-DataStructureChildCreator#4;")]
        [InlineData("InvalidNewChildType", ContextType.EmptyObject, "item", ContextType.ValidParent, "", ContextType.EmptyString, "", "e-DataStructureChildCreator#5;")]
        public void DataStructureChildCreatorAddToParent(string because, ContextType parentType, string parentCreateType, ContextType childType, string childCreateType, ContextType newChildType, string newChildCreateType, params string[] expectedErrors)
        {
            var subject = new DataStructureChildCreator();
            object parent = DataStructure.CreateTarget(parentType, parentCreateType);
            object child = DataStructure.CreateTarget(childType, childCreateType);
            object newChild = DataStructure.CreateTarget(newChildType, newChildCreateType);
            List<Information> result = new Action(() => { subject.AddToParent(new Template { Parent = parent, Child = child }, newChild); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "", "e-DataStructure#17;")]
        [InlineData("Valid", ContextType.EmptyObject, "item")]
        public void DataStructureObjectConverter(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureObjectConverter();
            object context = DataStructure.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "", "e-DataStructure#25;")]
        [InlineData("InvalidSource", ContextType.InvalidSource, "", "e-DataStructure#26;")]
        [InlineData("CannotInstantiateObject", ContextType.EmptySourceType, "", "e-DataStructure#24;")]
        [InlineData("InstantiatedObjectIsNotOfTypeDataStructureBase", ContextType.InvalidSourceType, "", "e-DataStructure#31;")]
        [InlineData("Valid", ContextType.ValidSource, "")]
        public void DataStructureTargetInstantiator(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureTargetInstantiator();
            object context = DataStructure.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Create(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.EmptyString, "", "e-DataStructure#20;")]
        public void DataStructureToStringObjectConverter(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new DataStructureToStringObjectConverter();
            object context = DataStructure.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Theory]
        [InlineData("InvalidType", ContextType.InvalidType, "", "e-DataStructure#27;")]
        [InlineData("InvalidSourceType", ContextType.EmptyString, "", "e-DataStructure#28;")]
        public void StringToDataStructureObjectConverterInvalidType(string because, ContextType contextType, string createType, params string[] expectedErrors)
        {
            var subject = new StringToDataStructureObjectConverter(null);
            object context = DataStructure.CreateTarget(contextType, createType);
            List<Information> result = new Action(() => { subject.Convert(context); }).Observe();
            result.ValidateResult(new List<string>(expectedErrors), because);
        }

        [Fact]
        public void StringToDataStructureObjectConverterInvalidSourceStringDeserialize()
        {
            DataStructureTargetInstantiatorSource testDataStructure = DataStructure.CreateDataStructureTargetInstantiatorInvalidSource();
            var subject = new StringToDataStructureObjectConverter(testDataStructure);
            List<Information> result = new Action(() => { subject.Convert("abcd"); }).Observe();
            result.ValidateResult(new List<string> { "e-DataStructure#29;" });
        }

        [Fact]
        public void StringToDataStructureObjectConverterInvalidDeserializedType()
        {
            DataStructureTargetInstantiatorSource testDataStructure = DataStructure.CreateDataStructureTargetInstantiatorInvalidSource();
            var subject = new StringToDataStructureObjectConverter(testDataStructure);

            string testSource = Newtonsoft.Json.JsonConvert.SerializeObject(testDataStructure);
            List<Information> result = new Action(() => { subject.Convert(testSource); }).Observe();

            result.ValidateResult(new List<string> { "e-DataStructure#30;" });
        }
    }
}