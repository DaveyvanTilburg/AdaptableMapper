Feature: ConfigurationValidations

Scenario: Empty argument mappingConfiguration
	Given I create a mappingconfiguration
	When I run Map with a null parameter
	Then the result should contain the following errors
	| Type  | Message                                                                          |
	| error | TREE#1; Argument cannot be null for MappingConfiguration.Map(string); objects:[] |
	Then result should be null

Scenario: Empty contextFactory mappingConfiguration
	Given I create a mappingconfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors
	| Type  | Message                                            |
	| error | TREE#2; ContextFactory cannot be null; objects:[]  |
	| error | TREE#5; MappingScope cannot be null; objects:[]    |
	| error | TREE#6; ObjectConverter cannot be null; objects:[] |
	Then result should be null

Scenario: Empty root empty factory nullconverter mappingConfiguration
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a NullObjectConverter for mappingConfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors
	| Type  | Message                                               |
	| error | TREE#3; ObjectConverter cannot be null; objects:[]    |
	| error | TREE#4; TargetInstantiator cannot be null; objects:[] |
	Then result should be null

Scenario: Empty scope mappingConfiguration input nullobject
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a ModelObjectConverter to the contextFactory
	Given I add a XmlTargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a XElementToStringObjectConverter for mappingConfiguration
	When I run Map with model NullModel
	Then the result should contain the following errors
	| Type  | Message                                                                               |
	| error | XML#6; Template is not valid Xml; objects:["XmlException","Root element is missing."] |
	Then result should be a '<nullObject />'