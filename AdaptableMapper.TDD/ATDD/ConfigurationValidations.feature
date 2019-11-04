Feature: ConfigurationValidations

@mytag
Scenario: Empty argument mappingConfiguration
	Given I create a mappingconfiguration
	When I run Map with a null parameter
	Then the result should contain the following errors
	| Type  | Message                                                              |
	| error | TREE#1; Argument cannot be null for MappingConfiguration.Map(string); objects:[] |
	Then result should be null

Scenario: Empty contextFactory mappingConfiguration
	Given I create a mappingconfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors
	| Type  | Message                                                              |
	| error | TREE#2; ContextFactory cannot be null; objects:[] |
	Then result should be null