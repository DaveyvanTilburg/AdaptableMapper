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
	Given I add a '<ObjectConverter>' ObjectConverter for mappingConfiguration
	Given I add a MappingScopeRoot with an empty list
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| InformationCodes   |
	| <InformationCodes> |
	Then result should be '<Result>'

	Examples:
	| TestName         | ContextFactoryObjectConverter | ContextFactoryTargetInitiator | ObjectConverter | InformationCodes  | Result         |
	| Model-Xml-Xml    | Model                         | Xml                           | Xml             | XML#24, MODEL#17  | <nullObject /> |
	| Json-Model-Model | Json                          | Model                         | Model           | JSON#13, MODEL#25 | {}             |
	| Xml-Json-Json    | Xml                           | Json                          | Json            | XML#19, JSON#26   | {}             |

Scenario Outline: Mapping
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a 'Xml' ObjectConverter to the contextFactory
	Given I add a 'Xml' TargetInitiator with an empty string to the contextFactory
	Given I add a 'Xml' ObjectConverter for mappingConfiguration
	Given I add a MappingScopeRoot with an empty list
	Given I add a Scope to the root
	| GetScopeTraversal   | GetScopeTraversalPath   | Traversal   | TraversalToGetTemplate   | ChildCreator   |
	| <GetScopeTraversal> | <GetScopeTraversalPath> | <Traversal> | <TraversalToGetTemplate> | <ChildCreator> |
	Given I add a mapping to the scope
	| GetValueTraversal   | SetValueTraversal   |
	| <GetValueTraversal> | <SetValueTraversal> |
	When I run Map with a string parameter '<root><testItem>value</testItem></root>'
	Then the result should contain the following errors
	| InformationCodes   |
	| <InformationCodes> |
	Then result should be '<Result>'

	Examples:
	| TestName                    | GetScopeTraversal | GetScopeTraversalPath | Traversal | TraversalToGetTemplate | ChildCreator | GetValueTraversal | SetValueTraversal | InformationCodes                        | Result         |
	| All null                    | null              |                       | null      | null                   | null         | null              | null              | TREE#7, TREE#8, TREE#9, TREE#10, XML#24 | <nullObject /> |
	| xml-xml-null-null-null-null | xml               |                       | xml       | null                   | null         | null              | null              | TREE#9, TREE#10, XML#24                 | <nullObject /> |
	| xml-xml-xml-xml-null-null   | xml               | ./testItem            | xml       | xml                    | xml          | null              | null              | XML#24, XML#2, XML#2, TREE#11, TREE#12  | <nullObject /> |
	| xml-xml-xml-xml-xml-xml     | xml               | ./testItem            | xml       | xml                    | xml          | xml               | xml               | XML#24, XML#2, XML#2, XML#4, XML#7      | <nullObject /> |