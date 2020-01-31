using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using AdaptableMapper.Compositions;
using AdaptableMapper.Conditions;
using AdaptableMapper.Configuration;
using AdaptableMapper.Process;
using AdaptableMapper.Traversals;
using AdaptableMapper.Traversals.Model;
using AdaptableMapper.Traversals.Xml;
using AdaptableMapper.Xml;
using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD.Cases.Compositions
{
    public class CompositionCases
    {


        [Theory]
        [InlineData("", "B", "C", "C")]
        [InlineData("A", "B", "C", "B")]
        public void IfConditionThenAElseBGetValueTraversal(string valueA, string valueB, string valueC, string expectedValue)
        {
            var subject = new AdaptableMapper.Compositions.IfConditionThenAElseBGetValueTraversal(
                new NotEmptyCondition(new GetStaticValueTraversal(valueA)),
                new GetStaticValueTraversal(valueB),
                new GetStaticValueTraversal(valueC));

            string result = subject.GetValue(new Context(null, null));
            result.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false, "e-IfConditionThenAElseBGetValueTraversal#3;")]
        [InlineData(true, false, true, "e-IfConditionThenAElseBGetValueTraversal#2;")]
        [InlineData(false, false, false, "e-IfConditionThenAElseBGetValueTraversal#1;", "e-IfConditionThenAElseBGetValueTraversal#2;", "e-IfConditionThenAElseBGetValueTraversal#3;")]
        public void IfConditionThenAElseBGetValueTraversalValidation(bool valueA, bool valueB, bool valueC, params string[] expectedErrors)
        {
            Condition condition = valueA ? new NotEmptyCondition(new GetStaticValueTraversal("A")) : null;
            GetValueTraversal getValueTraversalA = valueB ? new GetStaticValueTraversal("B") : null;
            GetValueTraversal getValueTraversalB = valueC ? new GetStaticValueTraversal("C") : null;

            var subject = new AdaptableMapper.Compositions.IfConditionThenAElseBGetValueTraversal(condition, getValueTraversalA, getValueTraversalB);

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.GetValue(null); }).Observe();
            information.ValidateResult(expectedErrors, "Validation");
            if (valueA == false || valueB == false || valueC == false)
                result.Should().BeEmpty();
            else
                result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetSearchValueTraversalWithXml()
        {
            var subject = new GetSearchValueTraversal(
                new XmlGetValueTraversal("./SimpleItems/SimpleItem[@Id='{{searchValue}}']/Name"),
                new GetStaticValueTraversal("2"));

            object source = XDocument.Load("./Resources/Simple.xml").Root;
            var context = new Context(source, null);

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.GetValue(context); }).Observe();

            information.Should().BeEmpty();
            result.Should().BeEquivalentTo("Joey");
        }

        [Theory]
        [InlineData("Items{'PropertyName':'Code','Value':'1'/Code", "", "w-MODEL#9;", "e-MODEL#32;")]
        [InlineData("Items{'PropertyName':'Code','Value':'3'}/Code", "", "w-MODEL#4;")]
        [InlineData("Items{'PropertyName':'Code','Value':'1'}/Code", "1")]
        public void GetSearchValueTraversalWithModel(string path, string expectedResult, params string[] expectedErrorCodes)
        {
            var subject = new GetSearchValueTraversal(
                new ModelGetValueTraversal(path),
                new GetStaticValueTraversal("2"));

            object source = ModelCases.Model.CreateTarget(ContextType.TestObject, "item");
            var context = new Context(source, null);

            string result = string.Empty;
            List<Information> information = new Action(() => { result = subject.GetValue(context); }).Observe();

            if (expectedErrorCodes.Length == 0)
            {
                information.Should().BeEmpty();
                result.Should().BeEquivalentTo(expectedResult);
            }
            else
            {
                information.ValidateResult(new List<string>(expectedErrorCodes), "search model");
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public void GetSearchValueTraversalEmpty()
        {
            var subject = new GetSearchValueTraversal(null, null);

            List<Information> information = new Action(() => { subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetSearchValueTraversal#1;", "e-GetSearchValueTraversal#2;" }, "Empty");
        }

        [Fact]
        public void GetSearchValueTraversalInvalidSearchPathType()
        {
            var subject = new GetSearchValueTraversal(new GetNothingValueTraversal(), new GetNothingValueTraversal());

            List<Information> information = new Action(() => { subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetSearchValueTraversal#3;" }, "InvalidSearchPathType");
        }

        [Fact]
        public void GetListSearchValueTraversalXml()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetListSearchValueTraversal(
                new XmlGetListValueTraversal("./SimpleItems/SimpleItem[@Type='{{searchValue}}']"),
                new GetStaticValueTraversal("Person")
            );

            MethodResult<IEnumerable<object>> result = new MethodResult<IEnumerable<object>>(null);
            List<Information> information = new Action(() => { result = subject.GetValues(context); }).Observe();

            information.Should().BeEmpty();
            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(2);
        }

        [Fact]
        public void GetListSearchValueTraversalEmpty()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetListSearchValueTraversal(null, null);

            MethodResult<IEnumerable<object>> result = new MethodResult<IEnumerable<object>>(null);
            List<Information> information = new Action(() => { result = subject.GetValues(context); }).Observe();

            information.ValidateResult(new List<string> { "e-GetListSearchValueTraversal#1;", "e-GetListSearchValueTraversal#2;" }, "Empty");
        }

        [Fact]
        public void GetConditionedListValueTraversalEmpty()
        {
            var subject = new GetConditionedListValueTraversal(null, null);

            List<Information> information = new Action(() => { subject.GetValues(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetConditionedListValueTraversal#1;", "e-GetConditionedListValueTraversal#2;" }, "Empty");
        }

        [Fact]
        public void GetConditionedListValueTraversal()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                new CompareCondition(
                    new XmlGetValueTraversal("./@Type"),
                    CompareOperator.Equals,
                    new GetStaticValueTraversal("Person")
                )
            );

            MethodResult<IEnumerable<object>> result = new MethodResult<IEnumerable<object>>(null);
            List<Information> information = new Action(() => { result = subject.GetValues(context); }).Observe();

            information.ValidateResult(new List<string>(), "GetConditionedListValueTraversal");

            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(2);
        }

        [Fact]
        public void GetConcatenatedValueTraversalEmpty()
        {
            var subject = new GetConcatenatedValueTraversal(null, null, null);

            List<Information> information = new Action(() => { subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetConcatenatedValueTraversal#1;", "e-GetConcatenatedValueTraversal#2;", "e-GetConcatenatedValueTraversal#3;" }, "Empty");
        }

        [Fact]
        public void GetConcatenatedValueTraversal()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConcatenatedValueTraversal(
                new GetConditionedListValueTraversal(
                    new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                    new CompareCondition(
                        new XmlGetValueTraversal("./@Type"),
                        CompareOperator.Equals,
                        new GetStaticValueTraversal("Person")
                    )
                ),
                new XmlGetValueTraversal("./Name"),
                "-"
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(context); }).Observe();

            information.ValidateResult(new List<string>(), "GetConcatenatedValueTraversal");

            result.Should().BeEquivalentTo("Davey-Joey");
        }
    }
}