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
using AdaptableMapper.ValueMutations;
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
            var subject = new GetConditionedListValueTraversal(null, (Condition)null);

            List<Information> information = new Action(() => { subject.GetValues(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetConditionedListValueTraversal#1;", "e-GetConditionedListValueTraversal#2;" }, "Empty");
        }

        [Fact]
        public void GetConditionedListValueTraversalBadPath()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem***$%#$") { XmlInterpretation = XmlInterpretation.Default },
                new CompareCondition(
                    new XmlGetValueTraversal("./@Type"),
                    CompareOperator.Equals,
                    new GetStaticValueTraversal("AI")
                )
            );

            MethodResult<IEnumerable<object>> result = new MethodResult<IEnumerable<object>>(null);
            List<Information> information = new Action(() => { result = subject.GetValues(context); }).Observe();

            information.ValidateResult(new List<string> { "e-XML#28;", "w-XML#5;" }, "GetConditionedListValueTraversalBadPath");

            result.IsValid.Should().BeFalse();
            result.Value.Should().BeNull();
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
                    new GetStaticValueTraversal("AI")
                )
            );

            MethodResult<IEnumerable<object>> result = new MethodResult<IEnumerable<object>>(null);
            List<Information> information = new Action(() => { result = subject.GetValues(context); }).Observe();

            information.ValidateResult(new List<string>(), "GetConditionedListValueTraversal");

            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(3);

            result.Value.First().ToString().Should().Contain("Easy");
            result.Value.ToList()[1].ToString().Should().Contain("Medium");
            result.Value.Last().ToString().Should().Contain("Hard");
        }

        [Fact]
        public void GetConditionedListValueTraversalDistinctBy()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                new XmlGetValueTraversal("./@Type")
            );

            MethodResult<IEnumerable<object>> result = new MethodResult<IEnumerable<object>>(null);
            List<Information> information = new Action(() => { result = subject.GetValues(context); }).Observe();

            information.ValidateResult(new List<string>(), "GetConditionedListValueTraversal");

            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(2);

            result.Value.First().ToString().Should().Contain("Davey");
            result.Value.Last().ToString().Should().Contain("Easy");
        }

        [Fact]
        public void GetConditionedListValueTraversalDistinctByConditioned()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConditionedListValueTraversal(
                new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                new CompareCondition(
                    new XmlGetValueTraversal("./@Type"),
                    CompareOperator.Equals,
                    new GetStaticValueTraversal("AI")
                ),
                new XmlGetValueTraversal("./@Type")
            );

            MethodResult<IEnumerable<object>> result = new MethodResult<IEnumerable<object>>(null);
            List<Information> information = new Action(() => { result = subject.GetValues(context); }).Observe();

            information.ValidateResult(new List<string>(), "GetConditionedListValueTraversal");

            result.IsValid.Should().BeTrue();
            result.Value.Count().Should().Be(1);

            result.Value.Last().ToString().Should().Contain("Easy");
        }

        [Fact]
        public void GetConcatenatedByListValueTraversalEmpty()
        {
            var subject = new GetConcatenatedByListValueTraversal(null, null, null);

            List<Information> information = new Action(() => { subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetConcatenatedByListValueTraversal#1;", "e-GetConcatenatedByListValueTraversal#2;" }, "Empty");
        }

        [Fact]
        public void GetConcatenatedByListValueTraversal()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConcatenatedByListValueTraversal(
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

            information.ValidateResult(new List<string>(), "GetConcatenatedByListValueTraversal");

            result.Should().BeEquivalentTo("Davey-Joey");
        }

        [Fact]
        public void GetConcatenatedByListValueTraversalNullSeparator()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConcatenatedByListValueTraversal(
                new GetConditionedListValueTraversal(
                    new XmlGetListValueTraversal("//SimpleItems/SimpleItem") { XmlInterpretation = XmlInterpretation.Default },
                    new CompareCondition(
                        new XmlGetValueTraversal("./@Type"),
                        CompareOperator.Equals,
                        new GetStaticValueTraversal("Person")
                    )
                ),
                new XmlGetValueTraversal("./Name")
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(context); }).Observe();

            information.ValidateResult(new List<string>(), "GetConcatenatedByListValueTraversal");

            result.Should().BeEquivalentTo("DaveyJoey");
        }

        [Fact]
        public void GetConcatenatedValueTraversalEmpty()
        {
            var subject = new GetConcatenatedValueTraversal(null, null);

            List<Information> information = new Action(() => { subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetConcatenatedValueTraversal#1;" }, "Empty");
        }

        [Fact]
        public void GetConcatenatedValueTraversalNullSeparator()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConcatenatedValueTraversal(
                new List<GetValueTraversal>
                {
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[1]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[4]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[6]/Name")
                }
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(context); }).Observe();

            information.ValidateResult(new List<string> { "w-XML#30;" }, "GetConcatenatedValueTraversal");

            result.Should().BeEquivalentTo("DaveyMedium");
        }

        [Fact]
        public void GetConcatenatedValueTraversal()
        {
            Context context = new Context(XDocument.Load("./Resources/SimpleLists.xml").Root, null);

            var subject = new GetConcatenatedValueTraversal(
                new List<GetValueTraversal>
                {
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[1]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[4]/Name"),
                    new XmlGetValueTraversal("/root/SimpleItems/SimpleItem[6]/Name")
                },
                "-"
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(context); }).Observe();

            information.ValidateResult(new List<string> { "w-XML#30;" }, "GetConcatenatedValueTraversal");

            result.Should().BeEquivalentTo("Davey-Medium");
        }

        [Fact]
        public void GetMutatedValueTraversalEmpty()
        {
            var subject = new GetMutatedValueTraversal(null, null);

            List<Information> information = new Action(() => { subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string> { "e-GetMutatedValueTraversal#1;", "e-GetMutatedValueTraversal#2;" }, "Empty");
        }

        [Fact]
        public void GetMutatedValueTraversal()
        {
            var subject = new GetMutatedValueTraversal(
                new GetStaticValueTraversal("test"),
                new PlaceholderValueMutation("({0})")
            );

            string result = null;
            List<Information> information = new Action(() => { result = subject.GetValue(null); }).Observe();

            information.ValidateResult(new List<string>(), "GetMutatedValueTraversal");

            result.Should().BeEquivalentTo("(test)");
        }

        [Fact]
        public void SetMutatedValueTraversalEmpty()
        {
            var subject = new SetMutatedValueTraversal(null, null);

            List<Information> information = new Action(() => { subject.SetValue(null, null, null); }).Observe();

            information.ValidateResult(new List<string> { "e-SetMutatedValueTraversal#1;", "e-SetMutatedValueTraversal#2;" }, "Empty");
        }

        [Fact]
        public void SetMutatedValueTraversal()
        {
            var subject = new SetMutatedValueTraversal(
                new XmlSetThisValueTraversal(),
                new PlaceholderValueMutation("({0})")
            );
            object context = XmlCases.Xml.CreateTarget(ContextType.TestObject);

            var traversal = new XmlGetTemplateTraversal("//SimpleItems/SimpleItem[@Id='1']/Name");
            AdaptableMapper.Traversals.Template name = traversal.GetTemplate(context, new MappingCaches());

            var setContext = new Context(null, name.Child);

            List<Information> result = new Action(() => { subject.SetValue(setContext, null, "Test"); }).Observe();
            result.ValidateResult(new List<string>(), "Valid");

            string value = new XmlGetThisValueTraversal().GetValue(new Context(setContext.Target, null));

            value.Should().BeEquivalentTo("(Test)");
        }
    }
}