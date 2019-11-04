Feature: ConfigurationValidations

@mytag
Scenario: Empty mappingConfiguration
	Given I create a mappingconfiguration
	When I run Map with a null parameter
	Then the result should contain the following errors
	| Type  | Message                                                              |
	| error | TREE#1; Argument cannot be null for MappingConfiguration.Map(string); objects:[] |
	Then result should be null