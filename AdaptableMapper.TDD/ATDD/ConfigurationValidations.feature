﻿Feature: ConfigurationValidations

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

Scenario: Empty scope model to xml mappingConfiguration input nullobject
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

Scenario: Empty scope json to model mappingConfiguration input nullobject
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a JsonObjectConverter to the contextFactory
	Given I add a ModelTargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a ModelToStringObjectConverter for mappingConfiguration
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| Type  | Message                                                                                                                                                |
	| error | JSON#13; Source could not be parsed to JToken; objects:["","JsonReaderException","Error reading JToken from JsonReader. Path '', line 0, position 0."] |
	| error | MODEL#24; assembly and typename could not be instantiated; objects:["","","ArgumentException","String cannot have zero length."]                       |
	Then result should be a '{}'

Scenario: Empty scope xml to json mappingConfiguration input nullobject
	Given I create a mappingconfiguration
	Given I add a contextFactory
	Given I add a XmlObjectConverter to the contextFactory
	Given I add a JsonTargetInitiator with an empty string to the contextFactory
	Given I add a MappingScopeRoot with an empty list
	Given I add a JtokenToStringObjectConverter for mappingConfiguration
	When I run Map with a string parameter ''
	Then the result should contain the following errors
	| Type  | Message                                                                                                                                               |
	| error | XML#19; input could not be parsed to XElement; objects:["","XmlException","Root element is missing."]                                                 |
	| error | JSON#20; Template could not be parsed to JToken; objects:["JsonReaderException","Error reading JToken from JsonReader. Path '', line 0, position 0."] |
	Then result should be a '{}'