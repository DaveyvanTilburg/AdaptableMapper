Feature: ConfigurationValidations

Scenario: Empty argument mappingConfiguration
	Given I create a mappingConfiguration
	When I run Map with a null parameter
	Then the result should contain the following errors 'TREE#1;'
	Then result should be null

Scenario: Empty contextFactory mappingConfiguration
	Given I create a mappingConfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors 'TREE#2;TREE#5;TREE#6;'
	Then result should be null

Scenario: Empty scoperoot empty factory nullconverter mappingConfiguration
	Given I create a mappingConfiguration
	Given I add a contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a 'Null' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors 'TREE#3;TREE#4;'
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
	| TestName         | ContextFactoryObjectConverter | ContextFactoryTargetInitiator | ObjectConverter | InformationCodes  | Result         |
	| Model-Xml-Xml    | ModelBase                     | Xml                           | Xml             | XML#24;MODEL#17;  | <nullObject /> |
	| Json-Model-Model | Json                          | Model                         | Model           | JSON#13;MODEL#25; | {}             |
	| Xml-Json-Json    | Xml                           | Json                          | Json            | XML#19;JSON#26;   | {}             |

Scenario Outline: Mapping
	Given I create a mappingConfiguration
	Given I add a contextFactory
	Given I add a '<Type>' ObjectConverter to the contextFactory
	Given I add a '<Type>' TargetInitiator to the contextFactory
	Given I add a '<Type>' ObjectConverter for mappingConfiguration
	Given I add a MappingScopeRoot with an empty list
	Given I add a Scope to the root
	| GetScopeTraversal   | GetScopeTraversalPath   | Traversal   | TraversalToGetTemplate   | ChildCreator   |
	| <GetScopeTraversal> | <GetScopeTraversalPath> | <Traversal> | <TraversalToGetTemplate> | <ChildCreator> |
	Given I add a mapping to the scope
	| GetValueTraversal   | SetValueTraversal   |
	| <GetValueTraversal> | <SetValueTraversal> |
	When I run Map with a string parameter '<Source>'
	Then the result should contain the following errors '<InformationCodes>'
	Then result should be '<Result>'

	Examples:
	| TestName                            | Type  | Source                                  | GetScopeTraversal | GetScopeTraversalPath | Traversal | TraversalToGetTemplate | ChildCreator | GetValueTraversal | SetValueTraversal | InformationCodes                                                          | Result         |
	| All null                            | Xml   | <root><testItem>value</testItem></root> | null              |                       | null      | null                   | null         | null              | null              | TREE#7;TREE#8;TREE#9;TREE#10;XML#24;                                      | <nullObject /> |
	| xml-xml-null-null-null-null         | Xml   | <root><testItem>value</testItem></root> | xml               |                       | xml       | null                   | null         | null              | null              | TREE#9;TREE#10;XML#24;                                                    | <nullObject /> |
	| xml-xml-xml-xml-null-null           | Xml   | <root><testItem>value</testItem></root> | xml               | ./testItem            | xml       | xml                    | xml          | null              | null              | XML#24;XML#27;XML#27;XML#26;XML#11;TREE#11;TREE#12;                       | <nullObject /> |
	| xml-xml-xml-xml-xml-xml             | Xml   | <root><testItem>value</testItem></root> | xml               | ./testItem            | xml       | xml                    | xml          | xml               | xml               | XML#24;XML#27;XML#27;XML#26;XML#11;XML#29;XML#7;                          | <nullObject /> |
	| json-json-null-null-null-null       | Json  | { "testItem": [ {"item": "value"} ]}    | json              |                       | json      | null                   | null         | null              | null              | TREE#9;TREE#10;JSON#26;                                                   | {}             |
	| json-json-json-json-null-null       | Json  | { "testItem": [ {"item": "value"} ]}    | json              | .testItem             | json      | json                   | json         | null              | null              | JSON#26;JSON#22;JSON#23;JSON#1;TREE#11;TREE#12;                           | {}             |
	| json-json-json-json-json-json       | Json  | { "testItem": [ {"item": "value"} ]}    | json              | .testItem             | json      | json                   | json         | json              | json              | JSON#26;JSON#22;JSON#23;JSON#1;JSON#6;JSON#11;JSON#19;                    | {}             |
	| model-model-null-null-null-null     | Model | { "Items": [{ "Items": []}]}            | model             |                       | model     | null                   | null         | null              | null              | TREE#9;TREE#10;MODEL#25;                                                  | {}             |
	| model-model-model-model-null-null   | Model | { "Items": [{ "Items": []}]}            | model             | /Items                | model     | model                  | model        | null              | null              | MODEL#25;MODEL#9;MODEL#7;MODEL#8;MODEL#5;MODEL#9;MODEL#2;TREE#11;TREE#12; | {}             |
	| model-model-model-model-model-model | Model | { "Items": [{ "Items": []}]}            | model             | /Items                | model     | model                  | model        | model             | model             | MODEL#25;MODEL#9;MODEL#7;MODEL#8;MODEL#5;MODEL#9;MODEL#2;MODEL#9;MODEL#9; | {}             |