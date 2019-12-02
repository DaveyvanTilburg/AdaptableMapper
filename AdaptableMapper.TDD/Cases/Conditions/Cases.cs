using System.Xml.Linq;
using AdaptableMapper.Conditions;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Conditions
{
    public class Cases
    {
        [Theory]
        [InlineData("Davey", true)]
        [InlineData("Joey", false)]
        public void EqualsConditionXmlComparedToStatic(string staticValue, bool expectedResult)
        {
            var source = XElement.Parse(System.IO.File.ReadAllText("./Resources/Simple.xml"));

            var condition = new EqualsCondition(
                new Traversals.Xml.XmlGetValueTraversal("//SimpleItems/SimpleItem[@Id='1']/Name"),
                new Traversals.GetStaticValueTraversal(staticValue)
                );

            condition.Validate(source).Should().Be(expectedResult);
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