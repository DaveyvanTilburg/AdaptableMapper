// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.1.0.0
//      SpecFlow Generator Version:3.1.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace AdaptableMapper.TDD.ATDD
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class ConfigurationValidationsFeature : Xunit.IClassFixture<ConfigurationValidationsFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ConfigurationValidations.feature"
#line hidden
        
        public ConfigurationValidationsFeature(ConfigurationValidationsFeature.FixtureData fixtureData, InternalSpecFlow.XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ConfigurationValidations", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Empty argument mappingConfiguration")]
        [Xunit.TraitAttribute("FeatureTitle", "ConfigurationValidations")]
        [Xunit.TraitAttribute("Description", "Empty argument mappingConfiguration")]
        public virtual void EmptyArgumentMappingConfiguration()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Empty argument mappingConfiguration", null, ((string[])(null)));
#line 3
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
 testRunner.Given("I create a mappingConfiguration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 5
 testRunner.When("I run Map with a null parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 6
 testRunner.Then("the result should contain the following errors \'e-TREE#1;\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 7
 testRunner.Then("result should be null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Empty contextFactory mappingConfiguration")]
        [Xunit.TraitAttribute("FeatureTitle", "ConfigurationValidations")]
        [Xunit.TraitAttribute("Description", "Empty contextFactory mappingConfiguration")]
        public virtual void EmptyContextFactoryMappingConfiguration()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Empty contextFactory mappingConfiguration", null, ((string[])(null)));
#line 9
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 10
 testRunner.Given("I create a mappingConfiguration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 11
 testRunner.When("I run Map with a string parameter \'test\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 12
 testRunner.Then("the result should contain the following errors \'e-TREE#2;e-TREE#5;e-TREE#6;\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 13
 testRunner.Then("result should be null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Empty scoperoot empty factory nullconverter mappingConfiguration")]
        [Xunit.TraitAttribute("FeatureTitle", "ConfigurationValidations")]
        [Xunit.TraitAttribute("Description", "Empty scoperoot empty factory nullconverter mappingConfiguration")]
        public virtual void EmptyScoperootEmptyFactoryNullconverterMappingConfiguration()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Empty scoperoot empty factory nullconverter mappingConfiguration", null, ((string[])(null)));
#line 15
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 16
 testRunner.Given("I create a mappingConfiguration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 17
 testRunner.Given("I add a contextFactory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 18
 testRunner.Given("I add a MappingScopeRoot with an empty list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 19
 testRunner.Given("I add a \'Null\' ObjectConverter for mappingConfiguration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 20
 testRunner.When("I run Map with a string parameter \'test\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
 testRunner.Then("the result should contain the following errors \'e-TREE#3;e-TREE#4;\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 22
 testRunner.Then("result should be null", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="MappingConfiguration")]
        [Xunit.TraitAttribute("FeatureTitle", "ConfigurationValidations")]
        [Xunit.TraitAttribute("Description", "MappingConfiguration")]
        [Xunit.InlineDataAttribute("Model-Xml-Xml", "ModelBase", "Xml", "Xml", "e-XML#24;e-MODEL#17;", "<nullObject />", new string[0])]
        [Xunit.InlineDataAttribute("Json-Model-Model", "Json", "Model", "Model", "e-JSON#13;e-MODEL#25;", "{}", new string[0])]
        [Xunit.InlineDataAttribute("Xml-Json-Json", "Xml", "Json", "Json", "e-XML#19;e-JSON#26;", "{}", new string[0])]
        public virtual void MappingConfiguration(string testName, string contextFactoryObjectConverter, string contextFactoryTargetInitiator, string objectConverter, string informationCodes, string result, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("MappingConfiguration", null, exampleTags);
#line 24
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 25
 testRunner.Given("I create a mappingConfiguration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 26
 testRunner.Given("I add a contextFactory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 27
 testRunner.Given(string.Format("I add a \'{0}\' ObjectConverter to the contextFactory", contextFactoryObjectConverter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 28
 testRunner.Given(string.Format("I add a \'{0}\' TargetInitiator to the contextFactory", contextFactoryTargetInitiator), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 29
 testRunner.Given(string.Format("I add a \'{0}\' ObjectConverter for mappingConfiguration", objectConverter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 30
 testRunner.Given("I add a MappingScopeRoot with an empty list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 31
 testRunner.When("I run Map with a string parameter \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 32
 testRunner.Then(string.Format("the result should contain the following errors \'{0}\'", informationCodes), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 33
 testRunner.Then(string.Format("result should be \'{0}\'", result), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="Mapping")]
        [Xunit.TraitAttribute("FeatureTitle", "ConfigurationValidations")]
        [Xunit.TraitAttribute("Description", "Mapping")]
        [Xunit.InlineDataAttribute("All null", "Xml", "<root><testItem>value</testItem></root>", "null", "", "null", "null", "null", "null", "e-TREE#7;e-TREE#9;e-TREE#10;e-XML#24;", "<nullObject />", new string[0])]
        [Xunit.InlineDataAttribute("xml-xml-null-null-null-null", "Xml", "<root><testItem>value</testItem></root>", "xml", "", "null", "null", "null", "null", "e-TREE#9;e-TREE#10;e-XML#24;", "<nullObject />", new string[0])]
        [Xunit.InlineDataAttribute("xml-xml-xml-xml-null-null", "Xml", "<root><testItem>value</testItem></root>", "xml", "./testItem", "xml", "xml", "null", "null", "e-XML#24;e-XML#27;e-XML#26;e-TREE#11;e-TREE#12;", "<nullObject />", new string[0])]
        [Xunit.InlineDataAttribute("xml-xml-xml-xml-xml-xml", "Xml", "<root><testItem>value</testItem></root>", "xml", "./testItem", "xml", "xml", "xml", "xml", "e-XML#24;e-XML#27;e-XML#26;e-XML#29;w-XML#7;w-XML#4;", "<nullObject />", new string[0])]
        [Xunit.InlineDataAttribute("json-json-null-null-null-null", "Json", "{ \"testItem\": [ {\"item\": \"value\"} ]}", "json", "", "null", "null", "null", "null", "e-TREE#9;e-TREE#10;e-JSON#26;", "{}", new string[0])]
        [Xunit.InlineDataAttribute("json-json-json-json-null-null", "Json", "{ \"testItem\": [ {\"item\": \"value\"} ]}", "json", ".testItem", "json", "json", "null", "null", "e-JSON#26;w-JSON#24;e-JSON#1;e-TREE#11;e-TREE#12;", "{}", new string[0])]
        [Xunit.InlineDataAttribute("json-json-json-json-json-json", "Json", "{ \"testItem\": [ {\"item\": \"value\"} ]}", "json", ".testItem", "json", "json", "json", "json", "e-JSON#26;w-JSON#24;e-JSON#1;e-JSON#6;e-JSON#19;w-JSON#11;", "{}", new string[0])]
        [Xunit.InlineDataAttribute("model-model-null-null-null-null", "Model", "{ \"Items\": [{ \"Items\": []}]}", "model", "", "null", "null", "null", "null", "e-TREE#9;e-TREE#10;e-MODEL#25;", "{}", new string[0])]
        [Xunit.InlineDataAttribute("model-model-model-model-null-null", "Model", "{ \"Items\": [{ \"Items\": []}]}", "model", "/Items", "model", "model", "null", "null", "e-MODEL#25;e-TREE#11;e-TREE#12;w-MODEL#2;w-MODEL#9;", "{}", new string[0])]
        [Xunit.InlineDataAttribute("model-model-model-model-model-model", "Model", "{ \"Items\": [{ \"Items\": []}]}", "model", "/Items", "model", "model", "model", "model", "e-MODEL#25;w-MODEL#2;w-MODEL#9;w-MODEL#9;w-MODEL#9;", "{}", new string[0])]
        public virtual void Mapping(string testName, string type, string source, string getScopeTraversal, string getScopeTraversalPath, string traversalToGetTemplate, string childCreator, string getValueTraversal, string setValueTraversal, string informationCodes, string result, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Mapping", null, exampleTags);
#line 41
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 42
 testRunner.Given("I create a mappingConfiguration", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 43
 testRunner.Given("I add a contextFactory", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 44
 testRunner.Given(string.Format("I add a \'{0}\' ObjectConverter to the contextFactory", type), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 45
 testRunner.Given(string.Format("I add a \'{0}\' TargetInitiator to the contextFactory", type), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 46
 testRunner.Given(string.Format("I add a \'{0}\' ObjectConverter for mappingConfiguration", type), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 47
 testRunner.Given("I add a MappingScopeRoot with an empty list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "GetScopeTraversal",
                            "GetScopeTraversalPath",
                            "TraversalToGetTemplate",
                            "ChildCreator"});
                table1.AddRow(new string[] {
                            string.Format("{0}", getScopeTraversal),
                            string.Format("{0}", getScopeTraversalPath),
                            string.Format("{0}", traversalToGetTemplate),
                            string.Format("{0}", childCreator)});
#line 48
 testRunner.Given("I add a Scope to the root", ((string)(null)), table1, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                            "GetValueTraversal",
                            "SetValueTraversal"});
                table2.AddRow(new string[] {
                            string.Format("{0}", getValueTraversal),
                            string.Format("{0}", setValueTraversal)});
#line 51
 testRunner.Given("I add a mapping to the scope", ((string)(null)), table2, "Given ");
#line hidden
#line 54
 testRunner.When(string.Format("I run Map with a string parameter \'{0}\'", source), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 55
 testRunner.Then(string.Format("the result should contain the following errors \'{0}\'", informationCodes), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 56
 testRunner.Then(string.Format("result should be \'{0}\'", result), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ConfigurationValidationsFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ConfigurationValidationsFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
