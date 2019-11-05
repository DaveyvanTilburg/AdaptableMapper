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

Scenario: Empty scoperoot empty factory nullconverter mappingConfiguration
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a 'Null' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter 'test'
	Then the result should contain the following errors
	| Type  | Message                                               |
	| error | TREE#3; ObjectConverter cannot be null; objects:[]    |
	| error | TREE#4; TargetInstantiator cannot be null; objects:[] |
	Then result should be null

Scenario: Empty scoperoot model to xml mappingConfiguration input empty string
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a 'Model' ObjectConverter to the contextFactory
	Given I add a 'Xml' TargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a 'Xml' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| Type  | Message                                                                               |
	| error | XML#6; Template is not valid Xml; objects:["XmlException","Root element is missing."] |
	| error | MODEL#17; source is not of expected type Model; objects:[""]                          |
	Then result should be a '<nullObject />'

Scenario: Empty scoperoot json to model mappingConfiguration input empty string
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a 'Json' ObjectConverter to the contextFactory
	Given I add a 'Model' TargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a 'Model' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| Type  | Message                                                                                                                                                |
	| error | JSON#13; Source could not be parsed to JToken; objects:["","JsonReaderException","Error reading JToken from JsonReader. Path '', line 0, position 0."] |
	| error | MODEL#24; assembly and typename could not be instantiated; objects:["","","ArgumentException","String cannot have zero length."]                       |
	Then result should be a '{}'

Scenario: Empty scoperoot xml to json mappingConfiguration input empty string
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a 'Xml' ObjectConverter to the contextFactory
	Given I add a 'Json' TargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a 'Json' ObjectConverter for mappingConfiguration
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| Type  | Message                                                                                                                                               |
	| error | XML#19; input could not be parsed to XElement; objects:["","XmlException","Root element is missing."]                                                 |
	| error | JSON#20; Template could not be parsed to JToken; objects:["JsonReaderException","Error reading JToken from JsonReader. Path '', line 0, position 0."] |
	Then result should be a '{}'

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
	| Type  | Message                                                                                                                                               |
	| error | XML#19; input could not be parsed to XElement; objects:["","XmlException","Root element is missing."]                                                 |
	| error | JSON#20; Template could not be parsed to JToken; objects:["JsonReaderException","Error reading JToken from JsonReader. Path '', line 0, position 0."] |
	Then result should be a '{}'