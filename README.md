# AdaptableMapper

A package for any to any mapping without hardcoding.
The implementation is set up with storage in mind through serialization. I use newtonsoft for serializing the composite for storage (see object JsonSerializer)
But ofcourse you can use your own methods for storage!

### I'd like your help!

Simply put, do what you want with my code, help me build it, test it, and help it grow, my goal is to spread this idea, and hopefully make the lives of programmers in companies that are build around API intergrations allot easier!
I've worked for a few companies that all struggle with integrations. I think i've found a simple yet useful concept that elevates the configuration of mapping from hardcoding to softcoding.
This package is only an abstract representation of that idea. Storage of configuration and giving it actual meaning is up to you. And I hope with you and your experience this idea can grow!

So please, ask questions, give feedback, make comments, create pullrequests!
Copy my code, install my packages, use it for yourself and help it grow. :)

### Roadmap

Completed plans:
 * Create an algorithm flexible enough to support any to any conversion
 * Soft error handling (through observer)
 * Implement Xml mapping
 * Implement Model mapping
 * Implement Json mapping
 * Add null checks for the composite tree
 * Write documentation
 * Add 100% code coverage for Model Mapping
 * Add 100% code coverage for Xml Mapping
 * Add 100% code coverage for Json Mapping
 * Simplify some error messaging (some errors cascade into other errors) 
 * Isolate everything regarding traversal functionality, so that new functionalities can be added (conditionals and Formatting)
 * Add formatting for Dates, included as a formatter for SetValue implementations
 * Add formatting for Numbers, included as a formatter for SetValue implementations
 * Extend MappingScopeComposite with a slot for Condition
 * GetValueStaticTraversal (Softcodes a value to get)
 * Add DictionaryReplaceValueMutation (Softcodes a list of values to replace in a value)

Current plans:
 
 
Future plans:
 * Introduce new module : Conditions
    * FirstNotEmptyCondition (abstract generic implementation, if Condition, invoke A else B. A and B or of generic type)
    * XXXGetValueFirstNotEmptyCondition (implements FirstNotEmptyCondition for type GetValue)

    * NotEqualsCondition (GetValue != GetValue)
    * NotEmptyCondition (GetValue != string.empty)
    * IfElseCondition (implements condition, If condition is true, do A, else B)
    * IfOrCondition (implements condition, If 'if' or 'or' is true, return true)
    * ListOfConditions (List of condition)
	
    * XXXIfAnyInPathCondition (has a path, if path results in any hits returns true)
	
 * GetScopeStayHere
 * GetTemplateStayHere
 
 * Refactor test suite

### All release notes

[ReleaseNotes.txt](ReleaseNotes.txt)

### Prerequisites

As long as I'm in alpha, Ill only build the package for .NET framework.
After I feel confident the package is stable I will research how to deploy for .NET standard (I think that includes .NET Core or visa versa)

### Installing

See installation instructions on nuget.org where the package is hosted:
```
https://www.nuget.org/packages/AdaptableMapper/
```

### How to use

Mock up of structure and flow:

![UML](Mapping.jpg)

See unit tests for examples on mappingConfiguration

### How to use searches

Example target Json
```
{ 
   "root":{ 
      "people":[ 
         { 
            "person":{ 
               "professionName":""
            }
         }
      ]
   }
}
```
* Documentation
    * SearchPath : This path should have use for the placeholder {{searchValue}}, so that traversal is possible outside of this scope with values of inside this scope
    * SearchPathValue : {{searchValue}} will be replaced with the result of this path
```
<root>
    <people>
        <person>
            <professionId>123</professionId>
        </person>
        <person>
            <professionId>123</professionId>
        </person>
    </people>
    <professions>
        <profession id="123">
            <name>Programmer</name>
        </profession>
    </professions>
</root>
```
* Xml
    * Example Scope : //people/person
    * Example JsonTraversal : $.root.people
    * SearchPath : ../../professions/profession[@id='{{searchValue}}']/name
    * SearchPathValue : ./professionId
    * Example JsonSetValue : .person.professionName
    
```
{ 
   "root":{ 
      "people":[ 
         { 
            "person":{ 
               "professionId":"123"
            }
         },
         { 
            "person":{ 
               "professionId":"123"
            }
         }
      ],
      "profession":[ 
         { 
            "id":"123",
            "name":"Programmer"
         }
      ]
   }
}
```
* Json
    * Example Scope : $.root.people[*]
    * Example JsonTraversal : $.root.people
    * SearchPath : ../../../.profession[?(@.id=='{{searchValue}}')].name
    * SearchPathValue : .person.professionId
    * Example JsonSetValue : .person.professionName
```
{ 
   "root":{ 
      "people":[ 
         { 
            "person":{ 
               "professionId":"123"
            }
         },
         { 
            "person":{ 
               "professionId":"123"
            }
         }
      ],
      "profession":[ 
         { 
            "id":"123",
            "name":"Programmer"
         }
      ]
   }
}
```
* Model
    * Example Scope : /root/people
    * Example JsonTraversal : $.root.people
    * SearchPath : ../../profession{'PropertyName':'id','Value':'{{searchValue}}'}/name
    * SearchPathValue : professionId
    * Example JsonSetValue : .person.professionName

How to use ModelTargetInitiatorSource
```
{
  "$type": "AdaptableMapper.Model.ModelTargetInstantiatorSource, AdaptableMapper",
  "AssemblyFullName": "Hercules.ModelObjects, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
  "TypeFullName": "Hercules.ModelObjects.Hardwares.Root"
}
```

### Versioning

I use [SemVer](http://semver.org/) for versioning. (I've got no clue on what that says for alpha releases, so when I reach 1.0.0 Ill start using SemVer, for now 0.X.0 will be used for breaking changes, 0.0.X will be used for bugfixes or non-breaking changes)

### Authors

* **Davey van Tilburg**

### License

See the [License.txt](AdaptableMapper/License.txt) file for details.
Do what ever the f*** you want! (But please respect eachother in the process)

### Acknowledgments

* Newtonsoft for json serialization
