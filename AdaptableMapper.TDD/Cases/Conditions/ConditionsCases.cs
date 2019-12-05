using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AdaptableMapper.Conditions;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Conditions
{
    public class ConditionsCases
    {
        [Theory]
        [InlineData("EqualsValid", "Davey", true)]
        [InlineData("EqualsInvalid", "Joey", false)]
        public void CompareConditionXmlComparedToStatic(string because, string staticValue, bool expectedResult)
        {
            var source = XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));

            var condition = new CompareCondition(
                new AdaptableMapper.Traversals.Xml.XmlGetValueTraversal("//SimpleItems/SimpleItem[@Id='1']/Name"),
                new AdaptableMapper.Traversals.GetStaticValueTraversal(staticValue),
                CompareOperator.Equals
                );

            condition.Validate(source).Should().Be(expectedResult, because);
        }

        [Theory]
        [InlineData("EqualsSurName", "$.SimpleItems[0].SurName", "$.SimpleItems[1].SurName", CompareOperator.Equals, true)]
        [InlineData("EqualsName", "$.SimpleItems[0].Name", "$.SimpleItems[1].Name", CompareOperator.Equals, false)]
        [InlineData("NotEqualsName", "$.SimpleItems[0].Name", "$.SimpleItems[1].Name", CompareOperator.NotEquals, true)]
        public void CompareConditionJson(string because, string sourcePath, string targetPath, CompareOperator compareOperator, bool expectedResult)
        {
            var source = JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));

            var condition = new CompareCondition(
                new AdaptableMapper.Traversals.Json.JsonGetValueTraversal(sourcePath),
                new AdaptableMapper.Traversals.Json.JsonGetValueTraversal(targetPath),
                compareOperator
            );

            condition.Validate(source).Should().Be(expectedResult, because);
        }

        [Fact]
        public void CompareConditionNulls()
        {
            var condition = new CompareCondition(
                null, null, CompareOperator.Equals
            );

            condition.Validate(1);
        }

        [Fact]
        public void ListValueMutation()
        {
            var subject = new ListOfConditions();
            subject.Conditions.Add(new CompareCondition(new GetStaticValueTraversal("0"), new GetStaticValueTraversal("0"), CompareOperator.Equals));
            subject.Conditions.Add(new CompareCondition(new GetStaticValueTraversal("0"), new GetStaticValueTraversal("1"), CompareOperator.NotEquals));

            bool result = false;
            List<Information> information = new Action(() => { result = subject.Validate(string.Empty); }).Observe();

            information.Count.Should().Be(0);
            result.Should().Be(true);
        }

        [Fact]
        public void ListValueMutationEmpty()
        {
            var subject = new ListOfConditions();

            bool result = false;
            List<Information> information = new Action(() => { result = subject.Validate(string.Empty); }).Observe();

            information.Count.Should().Be(1);
            information.Any(i => i.Message.StartsWith("ListOfConditions#1;")).Should().BeTrue();
        }
    }
}