using System.Xml.Linq;
using AdaptableMapper.Conditions;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Conditions
{
    public class Cases
    {
        [Theory]
        [InlineData("Valid", "Davey", true)]
        [InlineData("Invalid", "Joey", false)]
        public void EqualsConditionXmlComparedToStatic(string because, string staticValue, bool expectedResult)
        {
            var source = XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));

            var condition = new EqualsCondition(
                new AdaptableMapper.Traversals.Xml.XmlGetValueTraversal("//SimpleItems/SimpleItem[@Id='1']/Name"),
                new AdaptableMapper.Traversals.GetStaticValueTraversal(staticValue)
                );

            condition.Validate(source).Should().Be(expectedResult, because);
        }

        [Theory]
        [InlineData("ValidSurName", "$.SimpleItems[0].SurName", "$.SimpleItems[1].SurName", true)]
        [InlineData("InvalidName", "$.SimpleItems[0].Name", "$.SimpleItems[1].Name", false)]
        public void EqualsConditionJson(string because, string sourcePath, string targetPath, bool expectedResult)
        {
            var source = JObject.Parse(System.IO.File.ReadAllText("./Resources/Simple.json"));

            var condition = new EqualsCondition(
                new AdaptableMapper.Traversals.Json.JsonGetValueTraversal(sourcePath),
                new AdaptableMapper.Traversals.Json.JsonGetValueTraversal(targetPath)
            );

            condition.Validate(source).Should().Be(expectedResult, because);
        }

        [Fact]
        public void EqualsConditionNulls()
        {
            var condition = new EqualsCondition(
                null, null
            );

            condition.Validate(1);
        }
    }
}