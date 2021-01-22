using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Process;
using MappingFramework.Traversals;
using MappingFramework.Traversals.Xml;
using FluentAssertions;
using MappingFramework.Traversals.Json;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MappingFramework.TDD.Cases.Conditions
{
    public class ConditionsCases
    {
        [Fact]
        public void IntegrationTest()
        {
            var condition = new Mock<Condition>();
            condition.SetupSequence(c => c.Validate(It.IsAny<Context>()))
                .Returns(false)
                .Returns(false)
                .Returns(true);

            var getScopeTraversal = new Mock<GetListValueTraversal>();
            getScopeTraversal
                .Setup(g => g.GetValues(It.IsAny<Context>()))
                .Returns(new MethodResult<IEnumerable<object>>(new List<object> { 1, 2, 3 }));

            var getTemplateTraversal = new Mock<GetTemplateTraversal>();
            var childCreator = new Mock<ChildCreator>();

            var subject = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping> { new Mapping(new GetStaticValue(""), new XmlSetThisValueTraversal()) },
                null,
                getScopeTraversal.Object,
                getTemplateTraversal.Object,
                childCreator.Object)
            {
                Condition = condition.Object
            };

            subject.Traverse(new Context(null, null, null), new MappingCaches());

            childCreator.Verify(c => c.AddToParent(It.IsAny<Template>(), It.IsAny<object>()), Times.Once);
        }

        [Theory]
        [InlineData("0", CompareOperator.Equals, "0", true)]
        [InlineData("0", CompareOperator.Equals, "1", false)]
        [InlineData("0", CompareOperator.NotEquals, "1", true)]
        [InlineData("1", CompareOperator.NotEquals, "1", false)]
        [InlineData("1", CompareOperator.GreaterThan, "1", false)]
        [InlineData("1", CompareOperator.GreaterThan, "2", false)]
        [InlineData("2", CompareOperator.GreaterThan, "1", true)]
        [InlineData("1", CompareOperator.LessThan, "2", true)]
        [InlineData("2", CompareOperator.LessThan, "1", false)]
        [InlineData("abcd", CompareOperator.Equals, "1", false)]
        [InlineData("0", CompareOperator.NotEquals, "abcd", true)]
        [InlineData("2", CompareOperator.GreaterThan, "a", true, "w-CompareCondition#3;")]
        [InlineData("a", CompareOperator.LessThan, "2", true, "w-CompareCondition#3;")]
        [InlineData("a", CompareOperator.GreaterThan, "2", false, "w-CompareCondition#3;")]
        [InlineData("2", CompareOperator.LessThan, "a", false, "w-CompareCondition#3;")]
        [InlineData("b", CompareOperator.LessThan, "a", false, "w-CompareCondition#3;", "w-CompareCondition#3;")]
        [InlineData("b", CompareOperator.GreaterThan, "a", false, "w-CompareCondition#3;", "w-CompareCondition#3;")]
        [InlineData("abcd", CompareOperator.Contains, "a", true)]
        [InlineData("abcd", CompareOperator.Contains, "e", false)]
        public void CompareConditionStatics(string valueA, CompareOperator compareOperator, string valueB, bool expectedResult, params string[] expectedErrors)
        {
            var subject = new CompareCondition(new GetStaticValue(valueA), compareOperator, new GetStaticValue(valueB));

            bool result = false;
            List<Information> information = new Action(() => { result = subject.Validate(new Context(null, null, null)); }).Observe();

            information.ValidateResult(new List<string>(expectedErrors));
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

            bool result = false;
            List<Information> information = new Action(() => { result = subject.Validate(new Context(string.Empty, string.Empty, null)); }).Observe();

            information.Count.Should().Be(0);
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

            bool result = false;
            List<Information> information = new Action(() => { result = subject.Validate(new Context(source, string.Empty, null)); }).Observe();

            information.Count.Should().Be(0);
            result.Should().Be(expectedResult);
        }
    }
}