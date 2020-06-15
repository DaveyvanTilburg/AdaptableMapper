# MappingFramework

A package for any to any mapping without hardcoding.
The implementation is set up with storage in mind through serialization. I use newtonsoft for serializing the composite for storage (see object JsonSerializer)
But ofcourse you can use your own methods for storage!

### I'd like your help!

Simply put, do what you want with my code, help me build it, test it, and help it grow, my goal is to spread this idea, and hopefully make the lives of programmers in companies that are build around API intergrations allot easier!

So please, ask questions, give feedback, make comments, create pullrequests!
Copy my code, install my packages, use it for yourself and help it grow. :)

###Future plans:
 * Add new concept: Compositions (building blocks (get/set) composed out of leafs (xml get/set)

### All release notes

[ReleaseNotes.txt](ReleaseNotes.txt)

### Installing

See installation instructions on nuget.org where the package is hosted:
```
link here
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

See the [License.txt](MappingFramework/License.txt) file for details.
Do what ever the f*** you want! (But please respect eachother in the process)

### Acknowledgments

* Newtonsoft for json serialization
