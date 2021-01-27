using System.Collections.Generic;
using System.Xml.Linq;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Traversals;
using FluentAssertions;
using MappingFramework.Languages.Json.Traversals;
using MappingFramework.Languages.Xml.Traversals;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MappingFramework.TDD.Cases.Conditions
{
    public class ConditionsCases
    {
        [Theory]
        [InlineData("0", CompareOperator.Equals, "0", true, 0)]
        [InlineData("0", CompareOperator.Equals, "1", false, 0)]
        [InlineData("0", CompareOperator.NotEquals, "1", true, 0)]
        [InlineData("1", CompareOperator.NotEquals, "1", false, 0)]
        [InlineData("1", CompareOperator.GreaterThan, "1", false, 0)]
        [InlineData("1", CompareOperator.GreaterThan, "2", false, 0)]
        [InlineData("2", CompareOperator.GreaterThan, "1", true, 0)]
        [InlineData("1", CompareOperator.LessThan, "2", true, 0)]
        [InlineData("2", CompareOperator.LessThan, "1", false, 0)]
        [InlineData("abcd", CompareOperator.Equals, "1", false, 0)]
        [InlineData("0", CompareOperator.NotEquals, "abcd", true, 0)]
        [InlineData("2", CompareOperator.GreaterThan, "a", true, 1)]
        [InlineData("a", CompareOperator.LessThan, "2", true, 1)]
        [InlineData("a", CompareOperator.GreaterThan, "2", false, 1)]
        [InlineData("2", CompareOperator.LessThan, "a", false, 1)]
        [InlineData("b", CompareOperator.LessThan, "a", false, 2)]
        [InlineData("b", CompareOperator.GreaterThan, "a", false, 2)]
        [InlineData("abcd", CompareOperator.Contains, "a", true, 0)]
        [InlineData("abcd", CompareOperator.Contains, "e", false, 0)]
        public void CompareConditionStatics(string valueA, CompareOperator compareOperator, string valueB, bool expectedResult, int informationCount)
        {
            var subject = new CompareCondition(new GetStaticValue(valueA), compareOperator, new GetStaticValue(valueB));
            var context = new Context();

            bool result = subject.Validate(context);
            context.Information().Count.Should().Be(informationCount);
            
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("EqualsValid", "Davey", true)]
        [InlineData("EqualsInvalid", "Joey", false)]
        public void CompareConditionXmlComparedToStatic(string because, string staticValue, bool expectedResult)
        {
            var source = XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));

            var condition = new CompareCondition(
                new XmlGetValueTraversal("//SimpleItems/SimpleItem[@Id='1']/Name"),
                CompareOperator.Equals,
                new GetStaticValue(staticValue)
                );

            condition.Validate(new Context(source, null, null)).Should().Be(expectedResult, because);
        }

        [Theory]
        [InlineData("EqualsSurName", "$.SimpleItems[0].SurName", "$.SimpleItems[1].SurName", CompareOperator.Equals, true)]
        [InlineData("EqualsName", "$.SimpleItems[0].Name", "$.SimpleItems[1].Name", CompareOperator.Equals, false)]
        [InlineData("NotEqualsName", "$.SimpleItems[0].Name", "$.SimpleItems[1].Name", CompareOperator.NotEquals, true)]
        public void CompareConditionJson(string because, string sourcePath, string targetPath, CompareOperator compareOperator, bool expectedResult)
        {
            var source = JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));

            var condition = new CompareCondition(
                new JsonGetValueTraversal(sourcePath),
                compareOperator,
                new JsonGetValueTraversal(targetPath)
            );

            condition.Validate(new Context(source, null, null)).Should().Be(expectedResult, because);
        }

        [Theory]
        [InlineData(ListEvaluationOperator.All, "0", CompareOperator.Equals, "0", "0", CompareOperator.NotEquals, "0", false)]
        [InlineData(ListEvaluationOperator.All, "0", CompareOperator.Equals, "0", "0", CompareOperator.NotEquals, "1", true)]
        [InlineData(ListEvaluationOperator.Any, "0", CompareOperator.Equals, "0", "0", CompareOperator.NotEquals, "0", true)]
        [InlineData(ListEvaluationOperator.Any, "0", CompareOperator.Equals, "0", "0", CompareOperator.NotEquals, "1", true)]
        [InlineData(ListEvaluationOperator.Any, "0", CompareOperator.Equals, "1", "0", CompareOperator.NotEquals, "0", false)]
        public void ListOfConditions(
            ListEvaluationOperator listEvaluationOperator,
            string entry1ValueA, CompareOperator entry1CompareOperator, string entry1ValueB,
            string entry2ValueA, CompareOperator entry2CompareOperator, string entry2ValueB,
            bool expectedResult)
        {
            var subject = new ListOfConditions(listEvaluationOperator);
            subject.Conditions.Add(new CompareCondition(new GetStaticValue(entry1ValueA), entry1CompareOperator, new GetStaticValue(entry1ValueB)));
            subject.Conditions.Add(new CompareCondition(new GetStaticValue(entry2ValueA), entry2CompareOperator, new GetStaticValue(entry2ValueB)));

            var context = new Context();
            bool result = subject.Validate(context);

            context.Information().Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void ListOfConditionsComplexSetupTest()
        {
            var subject = new ListOfConditions(ListEvaluationOperator.All);

            var subSubject1 = new ListOfConditions(ListEvaluationOperator.All);
            subSubject1.Conditions.Add(new CompareCondition(new GetStaticValue("0"), CompareOperator.Equals, new GetStaticValue("0")));
            subSubject1.Conditions.Add(new CompareCondition(new GetStaticValue("0"), CompareOperator.NotEquals, new GetStaticValue("1")));
            subject.Conditions.Add(subSubject1);

            var subSubject2 = new ListOfConditions(ListEvaluationOperator.Any);
            subSubject2.Conditions.Add(new CompareCondition(new GetStaticValue("0"), CompareOperator.NotEquals, new GetStaticValue("0")));
            subSubject2.Conditions.Add(new CompareCondition(new GetStaticValue("0"), CompareOperator.NotEquals, new GetStaticValue("1")));
            subject.Conditions.Add(subSubject2);

            bool result = subject.Validate(new Context(string.Empty, string.Empty, null));
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("./item/id", true)]
        [InlineData("./item/name", false)]
        public void NotEmptyCondition(string path, bool expectedResult)
        {
            var subject = new NotEmptyCondition(new XmlGetValueTraversal(path));
            var source = XDocument.Load("./Resources/NotEmptyCondition/SimpleSource.xml").Root;
            var context = new Context(source, null, null);

            bool result = subject.Validate(context);

            context.Information().Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }
    }
}