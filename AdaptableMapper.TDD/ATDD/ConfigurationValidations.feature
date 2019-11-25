Feature: ConfigurationValidations

Scenario: Empty argument mappingConfiguration
	Given I create a mappingConfiguration
	When I run Map with a null parameter
	Then the result should contain the following errors 'e-TREE#1;'
	Then result should be null

Scenario: Empty contextFactory mappingConfiguration
	Given I create a mappingConfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors 'e-TREE#2;e-TREE#5;e-TREE#6;'
	Then result should be null

Scenario: Empty scoperoot empty factory nullconverter mappingConfiguration
	Given I create a mappingConfiguration
	Given I add a contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a 'Null' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors 'e-TREE#3;e-TREE#4;'
	Then result should be null

Scenario Outline: MappingConfiguration
	Given I create a mappingConfiguration
	Given I add a contextFactory
	Given I add a '<ContextFactoryObjectConverter>' ObjectConverter to the contextFactory
	Given I add a '<ContextFactoryTargetInitiator>' TargetInitiator to the contextFactory
	Given I add a '<ObjectConverter>' ObjectConverter for mappingConfiguration
	Given I add a MappingScopeRoot with an empty list
	When I run Map with a string parameter ''
	Then the result should contain the following errors '<InformationCodes>'
	Then result should be '<Result>'

	Examples:
	| TestName         | ContextFactoryObjectConverter | ContextFactoryTargetInitiator | ObjectConverter | InformationCodes      | Result         |
	| Model-Xml-Xml    | ModelBase                     | Xml                           | Xml             | e-XML#24;e-MODEL#17;  | <nullObject /> |
	| Json-Model-Model | Json                          | Model                         | Model           | e-JSON#13;e-MODEL#25; | {}             |
	| Xml-Json-Json    | Xml                           | Json                          | Json            | e-XML#19;e-JSON#26;   | {}             |

Scenario Outline: Mapping
	Given I create a mappingConfiguration
	Given I add a contextFactory
	Given I add a '<Type>' ObjectConverter to the contextFactory
	Given I add a '<Type>' TargetInitiator to the contextFactory
	Given I add a '<Type>' ObjectConverter for mappingConfiguration
	Given I add a MappingScopeRoot with an empty list
	Given I add a Scope to the root
	| GetScopeTraversal   | GetScopeTraversalPath   | TraversalToGetTemplate   | ChildCreator   |
	| <GetScopeTraversal> | <GetScopeTraversalPath> | <TraversalToGetTemplate> | <ChildCreator> |
	Given I add a mapping to the scope
	| GetValueTraversal   | SetValueTraversal   |
	| <GetValueTraversal> | <SetValueTraversal> |
	When I run Map with a string parameter '<Source>'
	Then the result should contain the following errors '<InformationCodes>'
	Then result should be '<Result>'

	Examples:
	| TestName                            | Type  | Source                                  | GetScopeTraversal | GetScopeTraversalPath | TraversalToGetTemplate | ChildCreator | GetValueTraversal | SetValueTraversal | InformationCodes                                           | Result         |
	| All null                            | Xml   | <root><testItem>value</testItem></root> | null              |                       | null                   | null         | null              | null              | e-TREE#7;e-TREE#9;e-TREE#10;e-XML#24;                      | <nullObject /> |
	| xml-xml-null-null-null-null         | Xml   | <root><testItem>value</testItem></root> | xml               |                       | null                   | null         | null              | null              | e-TREE#9;e-TREE#10;e-XML#24;                               | <nullObject /> |
	| xml-xml-xml-xml-null-null           | Xml   | <root><testItem>value</testItem></root> | xml               | ./testItem            | xml                    | xml          | null              | null              | e-XML#24;e-XML#27;w-XML#26;e-TREE#11;e-TREE#12;            | <nullObject /> |
	| xml-xml-xml-xml-xml-xml             | Xml   | <root><testItem>value</testItem></root> | xml               | ./testItem            | xml                    | xml          | xml               | xml               | e-XML#24;e-XML#27;w-XML#26;e-XML#29;w-XML#7;w-XML#4;       | <nullObject /> |
	| json-json-null-null-null-null       | Json  | { "testItem": [ {"item": "value"} ]}    | json              |                       | null                   | null         | null              | null              | e-TREE#9;e-TREE#10;e-JSON#26;                              | {}             |
	| json-json-json-json-null-null       | Json  | { "testItem": [ {"item": "value"} ]}    | json              | .testItem             | json                   | json         | null              | null              | e-JSON#26;w-JSON#24;e-JSON#1;e-TREE#11;e-TREE#12;          | {}             |
	| json-json-json-json-json-json       | Json  | { "testItem": [ {"item": "value"} ]}    | json              | .testItem             | json                   | json         | json              | json              | e-JSON#26;w-JSON#24;e-JSON#1;e-JSON#6;e-JSON#19;w-JSON#11; | {}             |
	| model-model-null-null-null-null     | Model | { "Items": [{ "Items": []}]}            | model             |                       | null                   | null         | null              | null              | e-TREE#9;e-TREE#10;e-MODEL#25;                             | {}             |
	| model-model-model-model-null-null   | Model | { "Items": [{ "Items": []}]}            | model             | /Items                | model                  | model        | null              | null              | e-MODEL#25;e-TREE#11;e-TREE#12;w-MODEL#2;w-MODEL#9;        | {}             |
	| model-model-model-model-model-model | Model | { "Items": [{ "Items": []}]}            | model             | /Items                | model                  | model        | model             | model             | e-MODEL#25;w-MODEL#2;w-MODEL#9;w-MODEL#9;w-MODEL#9;        | {}             |