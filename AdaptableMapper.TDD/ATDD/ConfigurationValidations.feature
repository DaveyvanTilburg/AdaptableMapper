Feature: ConfigurationValidations

Scenario: Empty argument mappingConfiguration
	Given I create a mappingconfiguration
	When I run Map with a null parameter
	Then the result should contain the following errors
	| InformationCodes |
	| TREE#1           |
	Then result should be null

Scenario: Empty contextFactory mappingConfiguration
	Given I create a mappingconfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors
	| InformationCodes       |
	| TREE#2, TREE#5, TREE#6 |
	Then result should be null

Scenario: Empty scoperoot empty factory nullconverter mappingConfiguration
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a 'Null' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors
	| InformationCodes |
	| TREE#3, TREE#4    |
	Then result should be null

Scenario Outline: MappingConfiguration
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a '<ContextFactoryObjectConverter>' ObjectConverter to the contextFactory
	Given I add a '<ContextFactoryTargetInitiator>' TargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a '<ObjectConverter>' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| InformationCodes   |
	| <InformationCodes> |
	Then result should be '<Result>'

	Examples:
	| TestName         | ContextFactoryObjectConverter | ContextFactoryTargetInitiator | ObjectConverter | InformationCodes  | Result         |
	| Model-Xml-Xml    | Model                         | Xml                           | Xml             | XML#6, MODEL#17   | <nullObject /> |
	| Json-Model-Model | Json                          | Model                         | Model           | JSON#13, MODEL#24 | {}             |
	| Xml-Json-Json    | Xml                           | Json                          | Json            | XML#19, JSON#20   | {}             |

Scenario: Empty scope xml to json mappingConfiguration input empty string
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a 'Xml' ObjectConverter to the contextFactory
	Given I add a 'Json' TargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a Scope to the MappingScopeRoot list
	Given I add a 'Json' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| InformationCodes |
	| XML#19, JSON#20  |
	Then result should be '{}'